using DataSync.Common.Data.Entities;
using DataSync.Common.Interfaces.Repositories;
using DataSync.DBChangeEmitter.Interfaces;

namespace DataSync.DBChangeEmitter.Services
{
    internal class TopologicalSorterService : ITopologicalSorterService
    {
        IChangeTrackerRepository ChangeTrackerRepository { get;}
        public TopologicalSorterService(IChangeTrackerRepository changeTrackerRepository)
        {
            ChangeTrackerRepository = changeTrackerRepository;
        }
        public IReadOnlyCollection<ChangeTracker> TopologicalSort(IReadOnlyCollection<ChangeTracker> changeTrackers)
        {
            Dictionary<string, List<string>> graph = BuildDependencyGraph(changeTrackers);
            var sorted = new List<string>();
            var visited = new HashSet<string>();
            var visiting = new HashSet<string>();

            void Visit(string table)
            {
                if (visited.Contains(table)) return;
                if (visiting.Contains(table)) throw new Exception("Cyclic dependency detected!");

                visiting.Add(table);

                foreach (var dependency in graph[table])
                {
                    Visit(dependency);
                }

                visiting.Remove(table);
                visited.Add(table);
                sorted.Add(table);
            }

            foreach (var table in graph.Keys)
            {
                Visit(table);
            }

            // Order tableChanges according to sortedTableNames
            var orderedChangeTrackers = changeTrackers
                .OrderBy(tc => sorted.IndexOf(tc.TableName))
                .ToList();
            return orderedChangeTrackers;
        }

        private Dictionary<string, List<string>> BuildDependencyGraph(IReadOnlyCollection<ChangeTracker> trackedTables)
        {
            var graph = new Dictionary<string, List<string>>();
            var tableNames = trackedTables.Select(t => t.TableName).ToHashSet();

            // Query foreign key relationships from the database
            
            var relationships = ChangeTrackerRepository.GetForeignRelationships();
            if (relationships != null)
            {
                foreach (var relationship in relationships)
                {
                    var fkTable = relationship.FkTable;
                    var pkTable = relationship.PkTable;

                    // Include only tables in the tracked list
                    if (tableNames.Contains(fkTable) && tableNames.Contains(pkTable))
                    {
                        if (!graph.ContainsKey(fkTable))
                            graph[fkTable] = new List<string>();

                        graph[fkTable].Add(pkTable);

                        if (!graph.ContainsKey(pkTable))
                            graph[pkTable] = new List<string>();
                    }
                }
            }
            return graph;
        }
    }
}
