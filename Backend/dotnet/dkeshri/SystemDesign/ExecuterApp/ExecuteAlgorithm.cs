using Dkeshri.SystemDesign.LowLevel.DSA.LinkedList;
using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace ExecuterApp
{
    internal class ExecuteAlgorithm
    {
        IExecute execute;
        public ExecuteAlgorithm()
        {
            execute = new SinglyLinkListEx();
        }
        public void Run()
        {
            execute.run();
        }
    }
}
