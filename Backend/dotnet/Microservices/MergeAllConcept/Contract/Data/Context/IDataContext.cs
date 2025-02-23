using Microsoft.EntityFrameworkCore;
namespace Contract.Data.Context
{
    public interface IDataContext
    {
        DbContext DbContext { get; }
    }
}
