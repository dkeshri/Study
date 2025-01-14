using Microsoft.EntityFrameworkCore;
namespace Dkeshri.DataSync.Common.Interfaces.DataContext
{
    public interface IDataContext
    {
        DbContext DbContext { get; }
    }
}
