namespace Tar
{
    public class TarEntry
    {
        private string _name;
        private long _mode;
        private int _userID;
        private int _groupID;
        private long _length;
        private DateTime _ModifiedTime;
        private DateTime? _accessTime;
        private DateTime? _changeTime;
        private TarEntryType _type = TarEntryType.File;
        private string _linkTarget;
        private string _userName;
        private string _groupName;
        private long _deviceMajor;
        private long _deviceMinor;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public long Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public int GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        public long Length
        {
            get { return _length; }
            set { _length = value; }
        }

        public DateTime ModifiedTime
        {
            get { return _ModifiedTime; }
            set { _ModifiedTime = value; }
        }

        public DateTime? AccessTime
        {
            get { return _accessTime; }
            set { _accessTime = value; }
        }

        public DateTime? ChangeTime
        {
            get { return _changeTime; }
            set { _changeTime = value; }
        }

        public TarEntryType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string LinkTarget
        {
            get { return _linkTarget; }
            set { _linkTarget = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        public long DeviceMajor
        {
            get { return _deviceMajor; }
            set { _deviceMajor = value; }
        }

        public long DeviceMinor
        {
            get { return _deviceMinor; }
            set { _deviceMinor = value; }
        }
    }
}