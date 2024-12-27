﻿using DataSync.Common.Interfaces.Repositories;
using DataSync.DBChangeEmitter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync.DBChangeEmitter.Services
{
    internal class DatabaseChangeTrackerService : IDatabaseChangeTrackerService
    {
        IChangeTrackerRepository ChangeTrackerRepository { get; set; }
        public DatabaseChangeTrackerService(IChangeTrackerRepository changeTrackerRepository)
        {
            ChangeTrackerRepository = changeTrackerRepository;
        }
        public long GetTableChangeVersion(string tableName)
        {
            return ChangeTrackerRepository.GetTableChangeVersion(tableName);
        }

        public long GetDbChangeTrackingCurrentVersion()
        {
            return ChangeTrackerRepository.GetDbChangeTrackingCurrentVersion();
        }

        public async Task<long> GetDbChangeTrackingCurrentVersionAsync()
        {
            return await ChangeTrackerRepository.GetDbChangeTrackingCurrentVersionAsync();
        }

        public List<string> GetPrimaryKeys(string tableName)
        {
            return ChangeTrackerRepository.GetPrimaryKeys(tableName);
        }
    }
}
