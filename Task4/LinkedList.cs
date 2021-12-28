using System;
using System.Threading;
using Utlis;

namespace Task4
{
    public class LinkedList<T> where T : IComparable
    {
        Node<T> head;
        Node<T> tail;

        public LinkedList()
        {
            head = new Node<T>(int.MinValue);
            tail = new Node<T>(int.MaxValue);
            head.nextNode = new HarrisReference<Node<T>>(tail);
        }

        public bool Insert(T data, int key)
        {
            Node<T> newNode = new Node<T>(key, data);
            Node<T> rightNode;
            Node<T> leftNode;

            while(true)
            {
                (rightNode, leftNode) = Search(key);
                if ((rightNode != tail) && (rightNode.key.Equals(data))) return false;
                newNode.nextNode = new HarrisReference<Node<T>>(rightNode);
                if (leftNode.nextNode.CAS(rightNode, newNode)) return true;
            }
        }

        public System.Collections.Generic.List<T> ToRegularList()
        {
            var list = new System.Collections.Generic.List<T>();
            var curr = head.nextNode.Data;
            while (curr.nextNode != null)
            {
                list.Add(curr.data);
                curr = curr.nextNode.Data;
            }
            return list;
        }


        public bool Delete (int key)
        {
            Node<T> rightNode = null;
            HarrisReference<Node<T>> rightNodeNext = null;
            Node<T> leftNode = null;

            while(true)
            {
                (rightNode, leftNode) = Search(key);

                if (rightNode == tail || !rightNode.key.Equals(key)) {
                    return false;
                }
                rightNodeNext = rightNode.nextNode;
                if (!rightNode.nextNode.IsMarked)
                {
                    if (rightNodeNext == Interlocked.CompareExchange(ref rightNode.nextNode, new HarrisReference<Node<T>>(rightNodeNext.Data, true), rightNodeNext))
                    {
                        break;
                    }
                }
            }
            if (! leftNode.nextNode.CAS(rightNode, rightNodeNext.Data))
            {
                rightNode = Search(rightNode.key).Item1;
            }

            return true;
        }

        public bool Includes(int key)
        {
            Node<T> rightNode = Search(key).Item1;
            return !((rightNode == tail) || (rightNode.key != key));
        }

        (Node<T>, Node<T>) Search(int key)
        {
            Node<T> leftNode = null;
            Node<T> leftNodeNext = null;

            while (true)
            {
                Node<T> t = head;
                Node<T> tNext = head.nextNode.Data;

                do
                {
                    if (!tNext.IsMarked())
                    {
                        leftNode = t;
                        leftNodeNext = tNext;
                    }

                    t = tNext;
                    if (t == tail) break;
                    tNext = t.nextNode.Data;
                } while (tNext.IsMarked() || t.key < key);

                Node<T> rightNode = t;

                if (leftNodeNext == rightNode)
                {
                    if ((rightNode != tail) && rightNode.nextNode.IsMarked)
                    {
                        continue;
                    } 
                    else
                    {
                        return (rightNode, leftNode);
                    }

                    if (leftNode.nextNode.CAS(leftNodeNext, rightNode))
                    {
                        if ((rightNode != tail) && rightNode.nextNode.IsMarked)
                        {
                            continue;
                        }
                        else
                        {
                            return (rightNode, leftNode);
                        }
                    }
                }    
            }
        }
    }
}
