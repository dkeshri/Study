using Dkeshri.SystemDesign.LowLevel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.Basic
{
    class Deepak
    {
        private static Deepak _deepak;
        int a = 0;
        private Deepak()
        {
        }
       
        public void display()
        {
            Console.WriteLine("Deepak"+ a++);
        }

        public static Deepak GetInstance()
        {
            if(_deepak == null)
            {
                _deepak = new Deepak();
            }
            return _deepak;
        }

    }
















    public class RandomConcept : IExecute
    {
        

        public void run()
        {
            Deepak deepak = Deepak.GetInstance();
            deepak.display();

            deepak = Deepak.GetInstance();
            deepak.display();

        }
    }
}
