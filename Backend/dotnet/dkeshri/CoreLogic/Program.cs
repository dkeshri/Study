
using System.Reflection;
using System.Security.Policy;

//Console.WriteLine("Main Console App");
//AppDomain currentDomain = AppDomain.CurrentDomain;
////Provide the current application domain evidence for the assembly.


////Make an array for the list of assemblies.
//Assembly[] assems = currentDomain.GetAssemblies();

////List the assemblies in the current application domain.
//Console.WriteLine("List of assemblies loaded in current appdomain:");
//foreach (Assembly assem in assems)
//    Console.WriteLine(assem.ToString());

//DependencyResolver resolver = new DependencyResolver();
//resolver.LoadDependencies("CoreLogic.Data.Logic.dll");
//resolver.LoadDependencies("CoreLogic.Service.Logic.dll");



string s = "(])";
string openChars = "({[";

Dictionary<char,char> paran = new Dictionary<char, char>()
{
    {'}','{' },
    {')','(' },
    {']','[' }
};



Stack<char> stack = new Stack<char>();

foreach (char c in s)
{
    if (openChars.Contains(c))
    {
        stack.Push(c);
    }
    else
    {
        if(stack.Count > 0)
        {
            char a = stack.Peek();
            char closeParan = paran[c];
            if (closeParan == a)
            {
                stack.Pop();
            }
        }
        else
        {
            stack.Push(c);
        }
        
    }

}
Console.WriteLine(stack.Count == 0);





