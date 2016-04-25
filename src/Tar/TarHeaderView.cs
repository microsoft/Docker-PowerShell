using System;
using System.Collections.Generic;

namespace Tar
{
    internal struct TarHeaderView
    {
        private readonly byte[] _buffer;
        private readonly int _offset;
        public readonly Dictionary<string, string> PaxAttributes;

        public TarHeaderView(byte[] buffer, int offset, Dictionary<string, string> paxAttributes)
        {
            _buffer = buffer;
            _offset = offset;
            PaxAttributes = paxAttributes;
        }

        public ArraySegment<byte> Field(TarHeader.HeaderField field)
        {
            return new ArraySegment<byte>(_buffer, _offset + field.Offset, field.Length);
        }

        public byte this[int i]
        {
            get { return _buffer[_offset + i]; }
            set { _buffer[_offset + i] = value; }
        }

        public void PutBytes(ArraySegment<byte> bytes, TarHeader.HeaderField field)
        {
            int i;
            for (i = 0; i < bytes.Count && i < field.Length; i++)
            {
                this[field.Offset + i] = bytes.Array[bytes.Offset + i];
            }

            PutNul(field.Offset + i, field.Length - i);
        }

        public bool TryPutString(string str, TarHeader.HeaderField field)
        {
            if (str == null)
            {
                PutNul(field);
                return true;
            }

            if (TarCommon.IsASCII(str))
            {
                if (str.Length <= field.Length)
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        this[field.Offset + i] = (byte)str[i];
                    }

                    PutNul(field.Offset + str.Length, field.Length - str.Length);
                    return true;
                }
            }

            if (field.PaxAttribute != null && PaxAttributes != null)
            {
                PaxAttributes[field.PaxAttribute] = str;
            }

            PutNul(field);
            return false;
        }

        public bool TryPutOctal(long value, TarHeader.HeaderField field)
        {
            if (value >= 0)
            {
                var str = Convert.ToString(value, 8);
                if (str.Length < field.Length)
                {
                    int leadingZeroes =field.Length - str.Length - 1;
                    for (int i = 0; i < leadingZeroes; i++)
                    {
                        this[field.Offset + i] = (byte)'0';
                    }

                    for (int i = 0; i < str.Length; i++)
                    {
                        this[field.Offset + leadingZeroes + i] = (byte)str[i];
                    }

                    this[field.Offset + field.Length - 1] = 0;
                    return true;
                }
            }

            if (field.PaxAttribute != null && PaxAttributes != null)
            {
                PaxAttributes[field.PaxAttribute] = value.ToString();
            }

            PutNul(field);
            return false;
        }

        public bool TryPutTime(DateTime time, TarHeader.HeaderField field)
        {
            uint nanoseconds = 0;
            long unixTime = 0;
            if (time.Ticks != 0)
            {
                unixTime = TarTime.ToUnixTime(time, out nanoseconds);
            }

            if (TryPutOctal(unixTime, field.WithoutPax) && nanoseconds == 0)
            {
                return true;
            }

            if (field.PaxAttribute != null && PaxAttributes != null)
            {
                PaxAttributes[field.PaxAttribute] = TarTime.ToPaxTime(time);
            }

            return false;
        }

        private void PutNul(int offset, int n)
        {
            for (int i = 0; i < n; i++)
            {
                this[offset + i] = 0;
            }
        }

        public void PutNul(TarHeader.HeaderField field)
        {
            PutNul(field.Offset, field.Length);
        }

        public string GetPaxValue(string paxKey)
        {
            string paxValue;
            if (paxKey != null && PaxAttributes != null && PaxAttributes.TryGetValue(paxKey, out paxValue))
            {
                return paxValue;
            }

            return null;
        }

        public string GetString(TarHeader.HeaderField field)
        {
            string paxValue = GetPaxValue(field.PaxAttribute);
            if (paxValue != null)
            {
                return paxValue;
            }

            int i;
            for (i = 0; i < field.Length; i++)
            {
                var b = this[field.Offset + i];
                if (b == 0)
                {
                    break;
                }
            }

            // Assume UTF-8 encoding. This may not be correct always, but only PAX rigorously
            // defines the encoding of strings.
            return TarCommon.UTF8.GetString(_buffer, _offset + field.Offset, i);
        }

        private static readonly char[] nulAndSpace = new char[] { ' ', '\x00' };

        public long GetOctalLong(TarHeader.HeaderField field)
        {
            string paxValue = GetPaxValue(field.PaxAttribute);
            if (paxValue != null)
            {
                return Convert.ToInt64(paxValue);
            }

            var firstByte = this[field.Offset];
            if ((firstByte & 0x80) != 0)
            {
                // Tar extension: value is encoded as MSB (ignoring the high bit of the first byte).
                if ((firstByte & 0x40) == 0)
                {
                    // The result is positive, so clear the sign bit.
                    firstByte &= 0x7f;
                }

                long result = (sbyte)firstByte;
                for (int i = 1; i < field.Length; i++)
                {
                    result <<= 8;
                    result |= this[field.Offset + i];
                }

                return result;
            }
            else
            {
                var str = GetString(field.WithoutPax).Trim(nulAndSpace);
                if (str.Length == 0)
                {
                    return 0;
                }
                return Convert.ToInt64(str, 8);
            }
        }

        public int GetOctal(TarHeader.HeaderField field)
        {
            long value = GetOctalLong(field);
            if ((int)value != value)
            {
                throw new TarParseException("value too large");
            }

            return (int)value;
        }

        public DateTime GetTime(TarHeader.HeaderField field)
        {
            string paxValue = GetPaxValue(field.PaxAttribute);
            if (paxValue != null)
            {
                return TarTime.FromPaxTime(paxValue);
            }

            long unixTime = GetOctalLong(field.WithoutPax);
            if (unixTime < TarTime.MinUnixTime)
            {
                unixTime = TarTime.MinUnixTime;
            }
            else if (unixTime > TarTime.MaxUnixTime)
            {
                unixTime = TarTime.MaxUnixTime;
            }

            return TarTime.FromUnixTime(unixTime, 0);
        }
    }
}