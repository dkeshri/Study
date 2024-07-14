

using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace Dkeshri.SystemDesign.LowLevel.Basic
{
    public class Parent
    {
        public void display()
        {
            Console.WriteLine("Base class method called..");
        }
    }
    public class Driverd : Parent
    {
        // hide the display method of parent class.
        public new void display()
        {
            Console.WriteLine("Drived class method called");
        }

    }

    public class MethodHidingEx : IExecute
    {
        public void run()
        {
           Driverd driverd = new Driverd();
           //driverd.display();
           Parent parent = new Driverd();
           parent.display(); // base class method called. new key word hide the method of drived class.
           //Note: if you need to call the drived class method then use method overridding. not hiding. 
        }
    }
}