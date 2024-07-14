using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.Basic
{
    public class SingleTon
    {
        private static SingleTon instance;
        private List<string> ThreadStringList;
        // for thread safty we use lock
        private static readonly object padlock = new object();
        private SingleTon()
        {
            ThreadStringList = new List<string>();
        }
        // in c# we can do it by both way 
        // one by Method defination 
        // second by property definagtion 
        public static SingleTon getInstance()
        {
            if (instance == null)
                lock (padlock)
                    if (instance == null)
                        instance = new SingleTon();
            return instance;
        }
        public static SingleTon GetInstance
        {
            get
            {
                if (instance == null)
                    lock (padlock)
                        if (instance == null)
                            instance = new SingleTon();
                return instance;
            }

        }
        public List<string> getThreadStringList()
        {
            return ThreadStringList;
        }
    }
}
