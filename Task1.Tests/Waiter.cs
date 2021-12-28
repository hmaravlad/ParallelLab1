using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Task1.Tests
{
    public class Waiter
    {
        private MyMutex mutex;
        private bool notify = false;
        private bool notifyAll = false;
        public readonly Dictionary<Thread, bool> threadsNotified = new Dictionary<Thread, bool>();

        public Waiter(MyMutex mutex)
        {
            this.mutex = mutex;
        }

        public void Wait()
        {
            mutex.MyLock();

            threadsNotified[Thread.CurrentThread] = false;

            mutex.MyWait();

            threadsNotified[Thread.CurrentThread] = true;

            mutex.MyUnlock();
        }

        public void Notify()
        {
            mutex.MyLock();
            mutex.MyNotify();
            mutex.MyUnlock();
        }

        public void NotifyAll()
        {
            mutex.MyLock();
            mutex.MyNotifyAll();
            mutex.MyUnlock();
        }
    }
}
