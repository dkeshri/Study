using DataSync.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Interfaces.Repositories
{
    public interface IApplyDbChangeRepository
    {
        public void InsertUpdate(string tableName, Dictionary<string,object> data);
    }
}
