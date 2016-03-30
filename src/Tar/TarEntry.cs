using System;
using System.IO;

namespace Tar
{
    public class TarEntry
    {
        public string Name { get; set; }
        public long Mode { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public long Length { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime? AccessTime { get; set; }
        public DateTime? ChangeTime { get; set; }
        public DateTime? CreationTime { get; set; }
        public TarEntryType Type { get; set; }
        public string LinkTarget { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public long DeviceMajor { get; set; }
        public long DeviceMinor { get; set; }
        public string SecurityDescriptor { get; set; }
        public FileAttributes? FileAttributes { get; set; }
        public bool IsMountPoint {get; set; }
    }
}