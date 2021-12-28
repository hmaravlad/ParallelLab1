using System;
using System.Collections.Generic;
using System.Text;
using Task1;

namespace Task1.Tests
{
    class Incrementer
    {

        public int count = 0;
        private MyMutex mutex;

        public Incrementer(MyMutex mutex)
        {
            this.mutex = mutex;
        }

        public void increment()
        {
            mutex.MyLock();
            count = count + 1;
            mutex.MyUnlock();
        }
    }
}
