using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Utlis;

namespace Task2
{
    public class Node<T>
    {
        public T value;

        public int Key { get; }

        public HarrisReference<Node<T>>[] Next { get; }

        public int TopLevel { get; }

        static public Dictionary<Thread, Random> randomGens = new Dictionary<Thread, Random>();

        public Node(int key)
        {
            value = (default(T));
            Key = key;
            Next = new HarrisReference<Node<T>>[SkipList<T>.TopLvl + 1];
            for (var i = 0; i < Next.Length; ++i)
            {
                Next[i] = new HarrisReference<Node<T>>(null, false);
            }

            TopLevel = SkipList<T>.TopLvl;
        }

        public Node(T value)
        {
            this.value = value;
            Key = value.GetHashCode();
            var height = getRandomLevel();
            Next = new HarrisReference<Node<T>>[height + 1];
            for (var i = 0; i < Next.Length; ++i)
            {
                Next[i] = new HarrisReference<Node<T>>(null, false);
            }

            TopLevel = height;
        }

        private int getRandomLevel()
        {
            return new Random().Next(0, SkipList<T>.TopLvl);
        }
    }
}
