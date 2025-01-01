using DataSync.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DbChangeReceiver.Interfaces
{
    internal interface IDbChangeApplyService
    {
        void ApplyTableChanges(TableChanges tableChanges);
    }
}
