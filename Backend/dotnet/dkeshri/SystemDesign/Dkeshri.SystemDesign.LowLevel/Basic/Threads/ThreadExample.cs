using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace Dkeshri.SystemDesign.LowLevel.Basic.Threads
{
    public class ThreadExample : WorkerThread.ThreadJobListner,IExecute
    {
        public void Job(object obj)
        {
            ObjectData objectData = (ObjectData)obj;
            objectData.StringList.Add("deepak" + objectData.Counter);
            Console.WriteLine("deepak" + objectData.Counter);
        }

        public void Job()
        {
            throw new NotImplementedException();
        }

        public void run()
        {
            SingleTon singleTon = SingleTon.GetInstance;
            List<string> stringList = singleTon.getThreadStringList();
            for (int i=0;i<10; i++)
            {
                ObjectData objectData = new ObjectData()
                {
                    StringList = stringList,
                    Counter = i
                };
                new WorkerThread(true, this, objectData);
                
            }
            while (true)
            {
                if (stringList.Count == 10)
                {
                    break;
                }
                else
                {
                    Console.WriteLine(stringList.Count);
                }
            }
            foreach (string s in stringList)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("End of therad Process.");


        }
    }
}
