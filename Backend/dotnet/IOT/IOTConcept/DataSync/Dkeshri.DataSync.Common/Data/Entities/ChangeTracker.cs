namespace Dkeshri.DataSync.Common.Data.Entities
{
    public class ChangeTracker
    {
        public string TableName { get; set; } = string.Empty;
        public long ChangeVersion { get; set; }
    }
}
