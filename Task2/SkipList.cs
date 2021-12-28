
using System;
using System.Collections.Generic;
using Utlis;

namespace Task2
{
    public class SkipList<T>
    {
        public const int BottomLvl = 0;
        public const int TopLvl = 16;

        private Node<T> head = new Node<T>(int.MinValue);
        private Node<T> tail = new Node<T>(int.MaxValue);

        public SkipList()
        {
            for (var i = 0; i < head.Next.Length; ++i)
            {
                head.Next[i] = new HarrisReference<Node<T>>(tail, false);
            }
        }

        public List<T> ToRegularList()
        {
            var list = new List<T>();
            var current = head.Next[BottomLvl].Data;
            while (current != tail)
            {
                list.Add(current.value);
                current = current.Next[BottomLvl].Data;
            }
            return list;
        }

        public bool Insert(T value)
        {
            
            var successors = new Node<T>[TopLvl + 1];
            var predecessors = new Node<T>[TopLvl + 1];

            while (true)
            {
                if (Find(value, ref predecessors, ref successors))
                    return false;

                var node = new Node<T>(value);

                for (var level = BottomLvl; level <= node.TopLevel; level++)
                {
                    node.Next[level] = new HarrisReference<Node<T>>(successors[level], false);
                }

                var pred = predecessors[BottomLvl];
                var succ = successors[BottomLvl];
                node.Next[BottomLvl] = new HarrisReference<Node<T>>(succ, false);

                if (!pred.Next[BottomLvl].CAS(node, false, succ, false))
                    continue;

                for (var level = BottomLvl + 1; level <= node.TopLevel; level++)
                {
                    while (true)
                    {
                        pred = predecessors[level];
                        succ = successors[level];

                        if (pred.Next[level].CAS(node, false, succ, false))
                        {
                            break;
                        }
                        Find(value, ref predecessors, ref successors);
                    }
                }
                return true;
            }
        }

        public bool Delete(T value)
        {
            var successors = new Node<T>[TopLvl + 1];
            var predecessors = new Node<T>[TopLvl + 1];
            Node<T> succ;

            while (true)
            {
                if (!Find(value, ref predecessors, ref successors))
                {
                    return false;
                }

                Node<T> node = successors[BottomLvl];
                for (var level = node.TopLevel; level > BottomLvl; level--)
                {
                    var isMarked = node.Next[level].IsMarked;
                    succ = node.Next[level].Data;

                    while (!isMarked)
                    {
                        node.Next[level].CAS(succ, true, succ, false);
                        isMarked = node.Next[level].IsMarked;
                        succ = node.Next[level].Data;
                    }
                }

                var marked = node.Next[BottomLvl].IsMarked;
                succ = node.Next[BottomLvl].Data;

                while (true)
                {
                    var iMarkedIt = node.Next[BottomLvl].CAS(succ, true, succ, false);
                    marked = successors[BottomLvl].Next[BottomLvl].IsMarked; //?1
                    succ = successors[BottomLvl].Next[BottomLvl].Data;

                    if (iMarkedIt)
                    {
                        Find(value, ref predecessors, ref successors);
                        return true;
                    }

                    if (marked)
                    {
                        return false;
                    }
                }
            }
        }

        public bool Find(T value, ref Node<T>[] preds, ref Node<T>[] succs)
        {
            Node<T> curr = null;
            int key = value.GetHashCode();
            bool marked = false;

            retry:
            while (true)
            {
                var pred = head;
                for (var level = TopLvl; level >= BottomLvl; level--)
                {
                    curr = pred.Next[level].Data;

                    while (true)
                    {
                        marked = curr.Next[level].IsMarked;
                        var succ = curr.Next[level].Data;
                        while (marked)
                        {
                            if (!pred.Next[level].CAS(succ, false, curr, false)) 
                            {
                                goto retry;
                            }

                            curr = pred.Next[level].Data;
                            marked = curr.Next[level].IsMarked;
                            succ = curr.Next[level].Data;
                        }

                        if (curr.Key < key)
                        {
                            pred = curr;
                            curr = succ;
                        }
                        else
                        {
                            break;
                        }
                    }


                    preds[level] = pred;
                    succs[level] = curr;
                }
                return curr != null && (curr.Key == key);
            }
        }

        public bool Includes(T value)
        {
            int key = value.GetHashCode();
            bool marked = false;
            Node<T> curr = null;
            Node<T> pred = head;

            for (var level = TopLvl; level >= BottomLvl; level--)
            {
                curr = pred.Next[level].Data;

                while (true)
                {
                    marked = curr.Next[level].IsMarked;
                    var succ = curr.Next[level].Data;
                    while (marked)
                    {
                        curr = pred.Next[level].Data;
                        marked = curr.Next[level].IsMarked;
                        succ = curr.Next[level].Data;
                    }

                    if (curr.Key < key)
                    {
                        pred = curr;
                        curr = succ;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            return curr != null && (curr.Key == key);
        }
    }
}