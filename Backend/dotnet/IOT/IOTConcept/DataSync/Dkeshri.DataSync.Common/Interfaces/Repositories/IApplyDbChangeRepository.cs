namespace Dkeshri.DataSync.Common.Interfaces.Repositories
{
    public interface IApplyDbChangeRepository
    {
        public void InsertUpdate(string tableName, Dictionary<string,object> data);
        public void Delete(string tableName, Dictionary<string, object> pkKeysWithValues);
    }
}
