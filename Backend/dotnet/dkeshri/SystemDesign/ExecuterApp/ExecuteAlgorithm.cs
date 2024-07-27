using Dkeshri.SystemDesign.LowLevel.DSA.LinkedList.Singly;
using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace ExecuterApp
{
    internal class ExecuteAlgorithm
    {
        IExecute execute;
        public ExecuteAlgorithm()
        {
            execute = new SinglyLinkedListEx();
        }
        public void Run()
        {
            execute.run();
        }
    }
}
