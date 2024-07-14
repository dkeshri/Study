using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace Dkeshri.SystemDesign.LowLevel.DesignPrincipal.SOLID
{

    // Interface segregation Principal

    public interface IPrinterDriver
    {
        void PrinterDriver();
    }
    public interface IMouseDriver
    {
        void MouseDriver();
    }
    public interface IDisplayDriver
    {
        void DisplayDriver();
    }
    public class Mouse1 : IMouseDriver
    {
        public void MouseDriver()
        {
            Console.WriteLine("Install Mouse Driver");
        }
    }
    public class Printer : IPrinterDriver
    {
        public void PrinterDriver()
        {
            Console.WriteLine("Install Printer Driver");
        }
    }
    public class Screen : IDisplayDriver
    {
        public void DisplayDriver()
        {
            Console.WriteLine("Install the Display Driver");
        }
    }
    public class ISPDemo : IExecute
    {
        public void run()
        {
            Mouse1 mouse = new Mouse1();
            mouse.MouseDriver();
            Screen screen = new Screen();
            screen.DisplayDriver();
            Printer printer = new Printer();
            printer.PrinterDriver();
        }
    }
}