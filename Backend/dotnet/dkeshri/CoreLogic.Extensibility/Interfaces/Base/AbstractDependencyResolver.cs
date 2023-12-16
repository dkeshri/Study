using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic.Extensibility.Interfaces.Base
{
    public abstract class AbstractDependencyResolver : IDependencyResolver
    {
        public void SetUp()
        {
            OnSetUp();
        }
        protected abstract void OnSetUp();
        protected void AddServiceDescriptors(string interfaceNameSpace)
        {
            var typesInAssembly = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
            var typesInAssemblyArray = typesInAssembly as Type[] ?? typesInAssembly.ToArray();

            var interfacesFound = typesInAssemblyArray.Where(selectedType => (selectedType != null) && (selectedType.IsInterface &&
                ((selectedType.Namespace != null) && selectedType.Namespace.Contains(interfaceNameSpace))));

            foreach (var interfaceFound in interfacesFound)
            {
                if (interfaceFound.IsGenericType)
                    continue;
                Console.WriteLine("Interface Name: " + interfaceFound.ToString());
                var interfaceImplementations = typesInAssemblyArray.Where(selectedType => !selectedType.IsInterface &&
                    !selectedType.IsAbstract &&
                    interfaceFound.IsAssignableFrom(selectedType));
                Console.WriteLine("Implementing class: ");
                foreach (var interfaceImplementing in interfaceImplementations)
                {
                    Console.WriteLine(interfaceImplementing.ToString());
                    
                }

            }

        }
    }
}
