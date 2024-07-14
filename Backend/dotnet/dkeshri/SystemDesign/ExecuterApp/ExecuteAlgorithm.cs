using Dkeshri.SystemDesign.LowLevel.Basic.Linq;
using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace ExecuterApp
{
    internal class ExecuteAlgorithm
    {
        IExecute execute;
        public ExecuteAlgorithm()
        {
            execute = new LinqEx();
        }
        public void Run()
        {
            execute.run();
        }
    }
}
