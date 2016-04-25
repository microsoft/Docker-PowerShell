namespace Tar
{
    internal static class TarHeader
    {
        public struct HeaderField
        {
            public int Offset;
            public int Length;
            public string PaxAttribute;
            public HeaderField WithoutPax => new HeaderField { Offset = Offset, Length = Length };
        }

        public static HeaderField Field(int offset, int size, string paxAttribute = null)
        {
            return new HeaderField { Offset = offset, Length = size, PaxAttribute = paxAttribute };
        }

        public static readonly HeaderField FullHeader = Field(0, TarCommon.BlockSize);

        public static readonly HeaderField Name = Field(0, 100, "path");
        public static readonly HeaderField Mode = Field(100, 8);
        public static readonly HeaderField UserID = Field(108, 8, "uid");
        public static readonly HeaderField GroupID = Field(116, 8, "gid");
        public static readonly HeaderField Length = Field(124, 12, "size");
        public static readonly HeaderField ModifiedTime = Field(136, 12, "mtime");
        public static readonly HeaderField Checksum = Field(148, 7);
        public static readonly HeaderField ChecksumSpace = Field(155, 1);

        public static readonly HeaderField TypeFlag = Field(156, 1);
        public static readonly HeaderField LinkTarget = Field(157, 100, "linkpath");
        public static readonly HeaderField FullMagic = Field(257, 8);
        public static readonly HeaderField Magic = Field(257, 6);
        public static readonly HeaderField Version = Field(263, 2);
        public static readonly HeaderField UserName = Field(265, 32, "uname");
        public static readonly HeaderField GroupName = Field(297, 32, "gname");

        public static readonly HeaderField DeviceMajor = Field(329, 8, "SCHILY.devmajor");
        public static readonly HeaderField DeviceMinor = Field(337, 8, "SCHILY.devminor");

        public static readonly HeaderField Prefix = Field(345, 155);
    }
}