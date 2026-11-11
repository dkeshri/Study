using Dkeshri.SystemDesign.LowLevel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.Basic
{
    public class RandomConcept : IExecute
    {
        private void test(out int a)
        {
            a = 20;
            dynamic test;
            test = "deepak";
            Console.WriteLine(a);
            Console.WriteLine(test);
        }

        public void run()
        {
            // Implementation for run method


            int a = 9;
            test(out a);
            Console.WriteLine(a);
        }
    }
}
