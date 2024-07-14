using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace Dkeshri.SystemDesign.LowLevel.Basic
{
    public class DelegateEx : IExecute
    {
        public delegate int MyDelegate(int a, int b);
        public int test(int a, int b)
        {
            return a + b;
        }
        public void actionMethod(int a, int b)
        {
            Console.WriteLine("Action method");
        }
        public int funcMethod(int a, int b)
        {
            Console.WriteLine("function method");
            return 9;
        }
        public void run()
        {
            MyDelegate myDelegate = test;

            Console.WriteLine(myDelegate(2, 8));

            // we can do this way too.
            Console.WriteLine(new MyDelegate(test)(9, 7));

            // Action delegate
            // Action delegate defined in system namespace.
            //the Action delegate doesn't return a value.
            // In other words, an Action delegate can be used with a method that has a void return type.

            Action<int, int> action = actionMethod;
            action(9, 4);

            // Func delegate
            // similar to Action delegate but it have retun type
            // last parameter is out type in Func Delegate.

            Func<int,int, int> function_Delegate = funcMethod;

            int a = function_Delegate(9,9);
            Console.WriteLine(a);


        }
    }
}