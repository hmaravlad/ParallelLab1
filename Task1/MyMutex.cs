using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Task1
{
    public class MyMutex
    {
        volatile int locked = 0;
        Thread lockedThread = null;
        volatile int waitingCount = 0;
        volatile int requiredNumber = 0;
        volatile int notified = 0;

        public void MyLock()
        {
            if (lockedThread == Thread.CurrentThread)
            {
                return;
            }

            int curr = 0;
            int value = 1;
            

            while (Interlocked.CompareExchange(ref locked, value, curr) != 0)
            {
                Thread.Yield();
            }
            lockedThread = Thread.CurrentThread;
        }

        public void MyUnlock()
        {
            if (lockedThread != Thread.CurrentThread)
            {
                throw new SynchronizationLockException();
            }
            lockedThread = null;
            locked = 0;
        }

        public void MyWait()
        {
            if (lockedThread != Thread.CurrentThread)
            {
                throw new SynchronizationLockException();
            }
            MyUnlock();
            Interlocked.Increment(ref waitingCount);
            int currWaitingCount = waitingCount;
            while (true)
            {
                if (notified == 1 && waitingCount > requiredNumber)
                {
                    if (currWaitingCount == Interlocked.CompareExchange(ref waitingCount, currWaitingCount - 1, currWaitingCount))
                    {
                        break;
                    }
                }
                Thread.Yield();
                currWaitingCount = waitingCount;
            }
            MyLock();
        }

        public void MyNotify()
        {
            if (lockedThread != Thread.CurrentThread)
            {
                throw new SynchronizationLockException();
            }
            NotifyCount(1);
        }

        public void MyNotifyAll()
        {
            if (lockedThread != Thread.CurrentThread)
            {
                throw new SynchronizationLockException();
            }
            NotifyCount(waitingCount);
        }

        private void NotifyCount(int count)
        {
            requiredNumber = Math.Max(waitingCount - count, 0);
            notified = 1;
            while (waitingCount > requiredNumber) {
                Thread.Yield();
            }
            notified = 0;
        }
    }
}
