using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task3.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestOneThreadEnqueue()
        {
            int maxValue = 100;
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < maxValue; i++)
            {
                queue.Enqueue(i);
            }
            var list = queue.ToRegularList();
            for (int i = 0; i < maxValue; i++)
            {
                Assert.IsTrue(list.Contains(i));
            }
        }

        [Test]
        public void TestOneThreadDequeue()
        {
            int maxValue = 100;
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < maxValue; i++)
            {
                queue.Enqueue(i);
            }
            for (int i = 0; i < maxValue; i++)
            {
                Assert.AreEqual(i, queue.Dequeue());
            }
        }

        [Test]
        public void TestMultipleThreadsEnqueue()
        {
            int maxValue = 100;
            int taskNum = 10;
            Queue<int> queue = new Queue<int>();
            Task[] tasks = new Task[taskNum];
            for (int i = 0; i < taskNum; i++)
            {
                var k = i;
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var a = maxValue / taskNum;
                    var b = k * (a);
                    var c = (k + 1) * (a);
                    for (int j = b; j < c; j++)
                    {
                        queue.Enqueue(j);
                    }
                });
            }

            Task.WaitAll(tasks);
            var list = queue.ToRegularList();
            for (int i = 0; i < maxValue; i++)
            {
                Assert.IsTrue(list.Contains(i));
            }
        }

        [Test]
        public void TestMultipleThreadsDequeue()
        {
            int maxValue = 100;
            int taskNum = 10;
            Queue<int> queue = new Queue<int>();
            Task[] tasks = new Task[taskNum];
            for (int i = 0; i < taskNum; i++)
            {
                var k = i;
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var a = maxValue / taskNum;
                    var b = k * (a);
                    var c = (k + 1) * (a);
                    for (int j = b; j < c; j++)
                    {
                        queue.Enqueue(j);
                    }
                });
            }

            Task.WaitAll(tasks);
            for (int i = 0; i < taskNum; i++)
            {
                var k = i;
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var a = maxValue / taskNum;
                    var b = k * (a);
                    var c = (k + 1) * (a);
                    for (int j = b; j < c; j++)
                    {
                        queue.Dequeue();
                    }
                });
            }
            Task.WaitAll(tasks);

            var queueList = queue.ToRegularList();
            Assert.AreEqual(0, queueList.Count);
        }

        [Test]
        public void TestEmptyQueueException()
        {
            Queue<int> queue = new Queue<int>();
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue(), "Queue is empty");
        }
    }
}