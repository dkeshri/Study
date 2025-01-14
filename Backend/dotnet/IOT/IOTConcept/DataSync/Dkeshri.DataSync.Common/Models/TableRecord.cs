namespace Dkeshri.DataSync.Common.Models
{
#nullable disable
    public class TableRecord
    {
        public long ChangeVersion { get; set; }
        public string Operation { get; set; }
        public string Data { get; set; }
        public string PkKeysWithValues { get; set; }
        
    }
}
