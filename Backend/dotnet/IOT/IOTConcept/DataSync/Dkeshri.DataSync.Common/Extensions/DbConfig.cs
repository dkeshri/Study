namespace Dkeshri.DataSync.Common.Extensions
{
    public class DbConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int TransactionTimeOutInSec { get; set; } = 30;
    }
}
