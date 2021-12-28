using System;
using System.Collections.Generic;
using System.Text;

namespace Task3
{
    public class Pointer<T>
    {
        public Node<T> ptr = null;
        public long count = 0;
        public Pointer() { }

        public Pointer(Node<T> ptr, long count)
        {
            this.ptr = ptr;
            this.count = count;
        }
    }
}
