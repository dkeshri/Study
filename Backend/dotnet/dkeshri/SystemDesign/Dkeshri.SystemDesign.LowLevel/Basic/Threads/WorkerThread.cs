using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace Dkeshri.SystemDesign.LowLevel.Basic.Threads
{
    public class WorkerThread : IDisposable
    {
        protected ManualResetEvent m_ThreadHandle;
        protected Thread m_ThreadObj;
        protected bool m_EndLoop;
        protected Mutex m_EndLoopMutex;
        protected ThreadJobListner threadJobListner;
        protected Object JobDataObj;

        public Thread Thread
        {
            get
            {
                return m_ThreadObj;
            }
        }
        protected bool EndLoop
        {
            set
            {
                m_EndLoopMutex.WaitOne();
                m_EndLoop = value;
                m_EndLoopMutex.ReleaseMutex();
            }
            get
            {
                bool result = false;
                m_EndLoopMutex.WaitOne();
                result = m_EndLoop;
                m_EndLoopMutex.ReleaseMutex();
                return result;
            }
        }
        public WorkerThread()
        {
            m_EndLoop = false;
            m_ThreadObj = null;
            m_EndLoopMutex = new Mutex();
            m_ThreadHandle = new ManualResetEvent(false);

            ThreadStart threadStart = new ThreadStart(Run);
            m_ThreadObj = new Thread(threadStart);
            m_ThreadObj.Name = "Worker Thread";
        }
        public WorkerThread(bool autoStart, ThreadJobListner threadJobListner = null, Object JobDataObj = null) : this()
        {
            if (autoStart)
            {
                Start(threadJobListner, JobDataObj);
            }

        }
        public WaitHandle Handle
        {
            get
            {
                return m_ThreadHandle;
            }
        }
        public void Start(ThreadJobListner threadJobListner = null, Object JobDataObj = null)
        {
            if (threadJobListner != null)
                SetThreadJobListner(threadJobListner);
            if (JobDataObj != null)
                SetJobDataObj(JobDataObj);
            Debug.Assert(m_ThreadObj != null);
            Debug.Assert(m_ThreadObj.IsAlive == false);
            m_ThreadObj.Start();
        }

        public void Dispose()
        {
            Kill();
        }
        private void Run()
        {// here we will do our task.
            try
            {
                if (threadJobListner != null)
                    if (JobDataObj != null)
                        threadJobListner.Job(JobDataObj);
                    else
                        threadJobListner.Job();
            }
            catch (Exception)
            {

            }
            finally
            {
                m_ThreadHandle.Set();
            }
        }

        public void Kill()
        {
            //Kill is called on client thread - must use cached object
            Debug.Assert(m_ThreadObj != null);
            if (IsAlive == false)
            {
                return;
            }
            EndLoop = true;
            //Wait for thread to die
            Join();
            m_EndLoopMutex.Close();
            m_ThreadHandle.Close();
        }
        public void Join()
        {
            Debug.Assert(m_ThreadObj != null);
            if (IsAlive == false)
            {
                return;
            }
            Debug.Assert(Thread.CurrentThread.GetHashCode() !=
                                              m_ThreadObj.GetHashCode());
            m_ThreadObj.Join();
        }
        public bool Join(int millisecondsTimeout)
        {
            TimeSpan timeout;
            timeout = TimeSpan.FromMilliseconds(millisecondsTimeout);
            return Join(timeout);
        }
        public bool Join(TimeSpan timeout)
        {
            Debug.Assert(m_ThreadObj != null);
            if (IsAlive == false)
            {
                return true;
            }
            Debug.Assert(Thread.CurrentThread.GetHashCode() !=
                                              m_ThreadObj.GetHashCode());
            return m_ThreadObj.Join(timeout);
        }
        public string Name
        {
            get
            {
                return m_ThreadObj.Name;
            }
            set
            {
                m_ThreadObj.Name = value;
            }
        }
        public bool IsAlive
        {
            get
            {
                Debug.Assert(m_ThreadObj != null);
                bool isAlive = m_ThreadObj.IsAlive;
                bool handleSignaled = m_ThreadHandle.WaitOne(0, true);
                Debug.Assert(handleSignaled == !isAlive);
                return isAlive;
            }
        }
        public void SetThreadJobListner(ThreadJobListner threadJob)
        {
            this.threadJobListner = threadJob;
        }
        public void SetJobDataObj(Object obj)
        {
            this.JobDataObj = obj;
        }
        public interface ThreadJobListner
        {
            void Job(Object obj);
            void Job();
        }
    }
}