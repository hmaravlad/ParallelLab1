using NUnit.Framework;
using System.Threading.Tasks;

namespace Task4.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestOneThreadInsert()
        {
            int maxValue = 100;
            LinkedList<int> linkedList = new LinkedList<int>();
            for (int i = 0; i < maxValue; i++)
            {
                linkedList.Insert(i, i);
            }
            var list = linkedList.ToRegularList();
            for (int i = 0; i < maxValue; i++)
            {
                Assert.IsTrue(list.Contains(i));
            }
        }

        [Test]
        public void TestOneThreadDelete()
        {
            int maxValue = 100;
            LinkedList<int> linkedList = new LinkedList<int>();
            for (int i = 0; i < maxValue; i++)
            {
                linkedList.Insert(i, i);
            }
            for (int i = 0; i < maxValue; i++)
            {
                if (i % 2 == 0)
                {
                    linkedList.Delete(i);
                }
            }
            var list = linkedList.ToRegularList();
            for (int i = 0; i < maxValue; i++)
            {
                if (i % 2 == 0)
                {
                    Assert.IsFalse(list.Contains(i));
                }
                else
                {
                    Assert.IsTrue(list.Contains(i));
                }
            }
        }

        [Test]
        public void TestOneThreadIncludes()
        {
            int maxValue = 100;
            LinkedList<int> linkedList = new LinkedList<int>();
            for (int i = 0; i < maxValue; i++)
            {
                linkedList.Insert(i, i);
            }
            for (int i = 0; i < maxValue; i++)
            {
                if (i % 2 == 0)
                {
                    linkedList.Delete(i);
                }
            }
            for (int i = 0; i < maxValue; i++)
            {
                if (i % 2 == 0)
                {
                    Assert.IsFalse(linkedList.Includes(i));
                }
                else
                {
                    Assert.IsTrue(linkedList.Includes(i));
                }
            }
        }

        [Test]
        public void TestMultipleThreadsInsert()
        {
            int maxValue = 1000;
            int taskNum = 10;
            LinkedList<int> linkedList = new LinkedList<int>();
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
                        linkedList.Insert(j, j);
                    }
                });
            }

            Task.WaitAll(tasks);

            var list = linkedList.ToRegularList();
            for (int i = 0; i < maxValue; i++)
            {
                Assert.IsTrue(list.Contains(i));
            }
        }

        [Test]
        public void TestMultipleThreadsDelete()
        {
            int maxValue = 1000;
            int taskNum = 10;
            LinkedList<int> linkedList = new LinkedList<int>();
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
                        linkedList.Insert(j, j);
                    }
                });
            }

            Task.WaitAll(tasks);

            for (int i = 0; i < taskNum; i++)
            {
                if (i % 2 == 0) continue;
                var k = i;
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var a = maxValue / taskNum;
                    var b = k * (a);
                    var c = (k + 1) * (a);
                    for (int j = b; j < c; j++)
                    {
                        linkedList.Delete(j);
                    }
                });
            }


            Task.WaitAll(tasks);

            var list = linkedList.ToRegularList();
            for (int i = 0; i < taskNum; i++)
            {
                var k = i;
                var a = maxValue / taskNum;
                var b = k * (a);
                var c = (k + 1) * (a);
                for (int j = b; j < c; j++)
                {
                    if (i % 2 != 0)
                    {
                        Assert.IsFalse(list.Contains(j));
                    }
                    else
                    {
                        Assert.IsTrue(list.Contains(j));
                    }
                }
            }
        }

        [Test]
        public void TestMultipleThreadsIncludes()
        {
            int maxValue = 1000;
            int taskNum = 10;
            LinkedList<int> linkedList = new LinkedList<int>();
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
                        linkedList.Insert(j, j);
                    }
                });
            }

            Task.WaitAll(tasks);

            for (int i = 0; i < taskNum; i++)
            {
                if (i % 2 == 0) continue;
                var k = i;
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    var a = maxValue / taskNum;
                    var b = k * (a);
                    var c = (k + 1) * (a);
                    for (int j = b; j < c; j++)
                    {
                        linkedList.Delete(j);
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
                        if (i % 2 == 0)
                        {
                            Assert.IsTrue(linkedList.Includes(j));
                        }
                        else
                        {
                            Assert.IsFalse(linkedList.Includes(j));
                        }
                    }
                });
            }
        }
    }
}