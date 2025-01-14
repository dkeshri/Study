namespace Dkeshri.DataSync.Common.Models
{
    public class TableChanges
    {
        public string TableName { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public IReadOnlyCollection<TableRecord> Records { get; set; } = null!;

    }
}
