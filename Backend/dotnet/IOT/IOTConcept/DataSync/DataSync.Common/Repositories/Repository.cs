using DataSync.Common.Interfaces.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.Common.Repositories
{
    internal abstract class Repository
    {
        protected IDataContext DataContext { get; }
        protected Repository(IDataContext dataContext)
        {
            DataContext = dataContext;
        }
        protected IEnumerable<string> GetPrimaryKeys(string tableName)
        {
            List<string> keys = new List<string>();
            try
            {
                string query = $@"
                    SELECT 
                        c.COLUMN_NAME 
                    FROM 
                        INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE c 
                            ON tc.CONSTRAINT_NAME = c.CONSTRAINT_NAME
                            AND tc.TABLE_NAME = c.TABLE_NAME
                    WHERE 
                        tc.TABLE_NAME = '{tableName}'
                        AND tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
                    ";
                keys = DataContext.DbContext.Database.SqlQueryRaw<string>(query).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: While getting primary keys!");
                Console.WriteLine(ex.Message);
            }

            return keys;
        }
    }
}
