using NUnit.Framework;
using System.Threading.Tasks;

namespace Task2.Test
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
            SkipList<int> skipList = new SkipList<int>();
            for (int i = 0; i < maxValue; i++)
            {
                skipList.Insert(i);
            }
            var list = skipList.ToRegularList();
            for (int i = 0; i < maxValue; i++)
            {
                Assert.IsTrue(list.Contains(i));
            }
        }

        [Test]
        public void TestOneThreadDelete()
        {
            int maxValue = 100;
            SkipList<int> skipList = new SkipList<int>();
            for (int i = 0; i < maxValue; i++)
            {
                skipList.Insert(i);
            }
            for (int i = 0; i < maxValue; i++)
            {
                if (i % 2 == 0)
                {
                    skipList.Delete(i);
                }
            }
            var list = skipList.ToRegularList();
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
            SkipList<int> skipList = new SkipList<int>();
            for (int i = 0; i < maxValue; i++)
            {
                skipList.Insert(i);
            }
            for (int i = 0; i < maxValue; i++)
            {
                if (i % 2 == 0)
                {
                    skipList.Delete(i);
                }
            }
            for (int i = 0; i < maxValue; i++)
            {
                if (i % 2 == 0)
                {
                    Assert.IsFalse(skipList.Includes(i));
                }
                else
                {
                    Assert.IsTrue(skipList.Includes(i));
                }
            }
        }

        [Test]
        public void TestMultipleThreadsInsert()
        {
            int maxValue = 1000;
            int taskNum = 10;
            SkipList<int> skipList = new SkipList<int>();
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
                        skipList.Insert(j);
                    }
                });
            }

            Task.WaitAll(tasks);

            var list = skipList.ToRegularList();
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
            SkipList<int> skipList = new SkipList<int>();
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
                        skipList.Insert(j);
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
                        skipList.Delete(j);
                    }
                });
            }


            Task.WaitAll(tasks);

            var list = skipList.ToRegularList();
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
                        //Assert.IsFalse(skipList.Includes(j));
                    }
                    else
                    {
                        Assert.IsTrue(list.Contains(j));
                        //Assert.IsTrue(skipList.Includes(j));
                    }
                }
            }
        }

        [Test]
        public void TestMultipleThreadsIncludes()
        {
            int maxValue = 1000;
            int taskNum = 10;
            SkipList<int> skipList = new SkipList<int>();
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
                        skipList.Insert(j);
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
                        skipList.Delete(j);
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
                            Assert.IsTrue(skipList.Includes(j));
                        }
                        else
                        {
                            Assert.IsFalse(skipList.Includes(j));
                        }
                    }
                });
            }
        }
    }
}