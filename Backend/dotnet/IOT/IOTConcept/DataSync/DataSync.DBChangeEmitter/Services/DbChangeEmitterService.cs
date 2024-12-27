﻿using DataSync.Common.Models;
using DataSync.Common.Services;
using DataSync.DBChangeEmitter.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Services
{
    internal class DbChangeEmitterService : HostedTimerService
    {
        private int testCounter = 0;
        IDatabaseChangeTrackerService DatabaseChangeTrackerService { get; }
        public DbChangeEmitterService(IDatabaseChangeTrackerService databaseChangeTrackerService):base(TimeSpan.FromSeconds(10))
        {
            DatabaseChangeTrackerService = databaseChangeTrackerService;
        }

        protected override async Task OperationToPerforme(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<TableChanges> tablechanges = await DatabaseChangeTrackerService.GetChangesOfTrackedTableAsync();
            foreach (TableChanges tablechange in tablechanges)
            {
                Console.WriteLine($"************ {tablechange.TableName} ************");
                Console.WriteLine($"Total Records: {tablechange.Records.Count}\n" );
                foreach (var record in tablechange.Records) {
                    Console.WriteLine(record.Data.ToString());
                }
                Console.WriteLine($"\n************ {tablechange.TableName} End ************\n");
            }
            
        }
    }
}
