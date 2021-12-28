using System;
using System.Collections.Generic;
using System.Threading;

namespace Task3
{
    public class Node<T>
    {
        public T value;
        public Pointer<T> next = new Pointer<T>();

        public Node() { }
        public Node(T value)
        {
            this.value = value;
        }
    }
    public class Queue<T>
    {
        Pointer<T> head = new Pointer<T>();
        Pointer<T> tail = new Pointer<T>();

        public Queue()
        {
            var node = new Node<T>();
            head.ptr = node;
            tail.ptr = node;
        }

        public List<T> ToRegularList()
        {
            List<T> list = new List<T>();
            var curr = head.ptr.next;
            while (curr.ptr != null)
            {
                list.Add(curr.ptr.value);
                curr = curr.ptr.next;
            }
            return list;
        }

        public void Enqueue(T value)
        {
            var node = new Node<T>();
            node.value = value;
            node.next.ptr = null;

            var currTail = tail;

            while (true)
            {
                currTail = tail;
                var next = tail.ptr.next;
                if (currTail == tail)
                {
                    if (next.ptr == null)
                    {
                        if (next == Interlocked.CompareExchange(ref tail.ptr.next, new Pointer<T>(node, next.count + 1), next))
                        {
                            break;
                        }
                    }
                    else
                    {
                        Interlocked.CompareExchange(ref tail, new Pointer<T>(next.ptr, tail.count + 1), currTail);
                    }
                }
            }
            Interlocked.CompareExchange(ref tail, new Pointer<T>(node, tail.count + 1), currTail);
        }

        public T Dequeue()
        {
            T result;
            var currHead = head;
            while (true)
            {
                currHead = head;
                var currTail = tail;
                var next = currHead.ptr.next;
                if (currHead == head)
                {
                    if (currHead.ptr == currTail.ptr)
                    {
                        if (next.ptr == null)
                        {
                            throw new InvalidOperationException("Queue is empty");
                        }
                        Interlocked.CompareExchange(ref tail, new Pointer<T>(next.ptr, tail.count + 1), currTail);
                    }
                    else
                    {
                        result = next.ptr.value;
                        if (currHead == Interlocked.CompareExchange(ref head, new Pointer<T>(next.ptr, head.count + 1), currHead))
                        {
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
