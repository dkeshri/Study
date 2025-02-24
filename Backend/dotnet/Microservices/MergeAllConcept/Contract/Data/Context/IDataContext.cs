using Microsoft.EntityFrameworkCore;
namespace Contract.Data.Context
{
    public interface IDataContext : IDisposable
    {
        DbContext DbContext { get; }
    }
}
