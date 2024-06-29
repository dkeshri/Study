using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;

namespace Dkeshri.WebApi.Extensions
{
    public static class HostbuilderExtenstions
    {
        public static void AddSerilog(this IHostBuilder builder, IConfiguration configuration)
        {
            // uncomment below line to enable MS SQL Database logging 
            //var columnOptions = new ColumnOptions
            //{
            //    AdditionalColumns = new Collection<SqlColumn>
            //    {
            //        new SqlColumn { ColumnName = "UserName", DataType = System.Data.SqlDbType.NVarChar, DataLength = 50 },
            //        new SqlColumn { ColumnName = "UserId", DataType = System.Data.SqlDbType.Int }
            //    }
            //};
            var userEnricher = new UserEnricher("System", 123);
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .Enrich.With(userEnricher)
                    // uncomment below line to enable MS SQL Database logging 
            //        .WriteTo.MSSqlServer(
            //    connectionString: configuration.GetConnectionString("DefaultConnection"),
            //    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
            //    columnOptions: columnOptions
            //)
                    .CreateLogger();
            builder.UseSerilog();
        }
    }
}
