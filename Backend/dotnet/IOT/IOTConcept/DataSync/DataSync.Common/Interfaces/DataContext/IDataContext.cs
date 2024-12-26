using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Interfaces.DataContext
{
    public interface IDataContext
    {
        DbContext DbContext { get; }
    }
}
