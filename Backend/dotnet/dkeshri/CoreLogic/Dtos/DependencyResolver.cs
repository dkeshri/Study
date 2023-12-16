using CoreLogic.Extensibility.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Composition;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic.Dtos
{
    public class DependencyResolver
    {
        public void LoadDependencies(string dependencyAssembly)
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            if (!string.IsNullOrEmpty(codeBase))
            {
                var currentAssemblyPath = Path.GetDirectoryName(new Uri(codeBase).LocalPath);
                if (currentAssemblyPath != null)
                {
                    LoadDependencies(currentAssemblyPath, dependencyAssembly);
                }
                
            }
        }

        private void LoadDependencies(string path, string dependancyAssemblyFileName)
        {
            var directoryCatalog = new DirectoryCatalog(path, dependancyAssemblyFileName);
            var importDef = new System.ComponentModel.Composition.Primitives.ImportDefinition(
                                def => true,
                                typeof(IDependencyResolver).FullName,
                                ImportCardinality.ZeroOrMore,
                                false,
                                false
                );
            try
            {
                using var aggregateCatalog = new AggregateCatalog();
                aggregateCatalog.Catalogs.Add(directoryCatalog);
                using var compositionContainer = new CompositionContainer(aggregateCatalog);
                IEnumerable<Export> exports = compositionContainer.GetExports(importDef);
                IEnumerable<IDependencyResolver> modules =
                    exports.Select(export => export.Value as IDependencyResolver).Where(m => m != null);
                foreach (IDependencyResolver module in modules)
                {
                    module.SetUp();
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
