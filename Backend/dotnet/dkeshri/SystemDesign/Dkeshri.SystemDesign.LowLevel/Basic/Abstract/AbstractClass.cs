using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace Dkeshri.SystemDesign.LowLevel.Basic.Abstract
{
    abstract class AbstractClass
    {
        // variable can not be abstract nither be virtual. but it can be static
        private string name = "deepak";
        public static string staticVariable;
        
        // only virtual Property can be there in abstract Class.
        public virtual string virtualProperty { get { return name; } set { name = value; } }

        // Abstract class can have non abstract method along with abstract method.
        public string getName()
        {
            return name;
        }
        public abstract void abstractMethod();
        // static member can also be there. But it can not be virtual, abstract and Override.
        public static void staticMethod()
        {
            Console.WriteLine("I the static Method in abstract class.");
        }
        public AbstractClass()
        {
            this.name = "Deepak keshri";
            staticVariable = "I am in non static Constructor.";
            Console.WriteLine(staticVariable);
        }
        static AbstractClass()
        {
            // static constructor called before non static constructor.
            staticVariable = "This is in the static Constructor";
            Console.WriteLine(staticVariable);


        }

    }
    class DrivedClass : AbstractClass
    {
        private string name = "keshri";
        public override string virtualProperty { get => name; set => name = value; }

        public override void abstractMethod()
        {
            Console.WriteLine("I am the abstract method in Abstract class need to implement in drived class.");

        }

        public string deepak()
        {
            virtualProperty = "shubham";
            return virtualProperty;
        }

    }
    public class AbstractClassHandler:IExecute
    {
        

        public void run()
        {
            DrivedClass drivedClass = new DrivedClass();
            Console.WriteLine(drivedClass.deepak()); 
            Console.WriteLine(drivedClass.getName());
            drivedClass.abstractMethod();
            
        }
    }


}
