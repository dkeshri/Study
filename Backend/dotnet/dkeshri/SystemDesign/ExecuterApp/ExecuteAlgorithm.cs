using Dkeshri.SystemDesign.LowLevel.Basic;
using Dkeshri.SystemDesign.LowLevel.DSA.LinkedList.Singly;
using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace ExecuterApp
{
    internal class ExecuteAlgorithm
    {
        IExecute execute;
        public ExecuteAlgorithm()
        {
            execute = new RandomConcept();
        }
        public void Run()
        {
            execute.run();
        }
    }
}
