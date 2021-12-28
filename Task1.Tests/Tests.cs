using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Task1;

namespace Task1.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLock()
        {
            for (int j = 0; j < 10; j++)
            {
                int taskNum = 1000;
                MyMutex mutex = new MyMutex();
                Incrementer inc = new Incrementer(mutex);
                Task[] tasks = new Task[taskNum];
                for (int i = 0; i < taskNum; i++)
                {
                    tasks[i] = Task.Factory.StartNew(() => inc.increment()); ;
                }
                Task.WaitAll(tasks);
                Assert.AreEqual(taskNum, inc.count);
            }
        }

        [Test]
        public void TestNotify()
        {
            int taskNum = 8;
            MyMutex mutex = new MyMutex();
            Waiter waiter = new Waiter(mutex);
            Task[] tasks = new Task[taskNum];
            for (int i = 0; i < taskNum; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => waiter.Wait());
            }
            Thread.Sleep(100);
            Task task = Task.Factory.StartNew(() => waiter.Notify());
            Task.WaitAny(tasks);

            int notifiedSum = 0;
            foreach (bool notified in waiter.threadsNotified.Values)
            {
                if (notified) notifiedSum++;
            }
            Assert.AreEqual(1, notifiedSum);
        }

        [Test]
        public void TestNotifyAll()
        {
            int taskNum = 8;
            MyMutex mutex = new MyMutex();
            Waiter waiter = new Waiter(mutex);
            Task[] tasks = new Task[taskNum];
            for (int i = 0; i < taskNum; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => waiter.Wait());
            }
            Thread.Sleep(100);
            Task task = Task.Factory.StartNew(() => waiter.NotifyAll());
            Task.WaitAll(tasks);

            int notifiedSum = 0;
            foreach (bool notified in waiter.threadsNotified.Values)
            {
                if (notified) notifiedSum++;
            }
            Assert.AreEqual(taskNum, notifiedSum);
        }

        [Test]
        public void TestWaitException()
        {
            var mutex = new MyMutex();
            Assert.Throws<SynchronizationLockException>(() => mutex.MyWait());
        }

        [Test]
        public void TestNotifyException()
        {
            var mutex = new MyMutex();
            Assert.Throws<SynchronizationLockException>(() => mutex.MyNotify());
        }

        [Test]
        public void TestNotifyAllException()
        {
            var mutex = new MyMutex();
            Assert.Throws<SynchronizationLockException>(() => mutex.MyNotifyAll());
        }

        [Test]
        public void TestUnlockException()
        {
            var mutex = new MyMutex();
            Assert.Throws<SynchronizationLockException>(() => mutex.MyUnlock());
        }
    }
}