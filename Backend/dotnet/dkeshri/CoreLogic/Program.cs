// See https://aka.ms/new-console-template for more information
using CoreLogic.Dtos;
using System.Reflection;
using System.Security.Policy;

Console.WriteLine("Main Console App");
AppDomain currentDomain = AppDomain.CurrentDomain;
//Provide the current application domain evidence for the assembly.


//Make an array for the list of assemblies.
Assembly[] assems = currentDomain.GetAssemblies();

//List the assemblies in the current application domain.
Console.WriteLine("List of assemblies loaded in current appdomain:");
foreach (Assembly assem in assems)
    Console.WriteLine(assem.ToString());

DependencyResolver resolver = new DependencyResolver();
resolver.LoadDependencies("CoreLogic.Data.Logic.dll");
resolver.LoadDependencies("CoreLogic.Service.Logic.dll");