using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utlis;

namespace Task4
{
    class Node<T>
    {
		public int key;
		public T data;

		public HarrisReference<Node<T>> nextNode;

		public bool IsMarked()
        {
			if (nextNode == null)
			{
				return false;
			}
			else
			{
				return nextNode.IsMarked;
			}
        }

		public Node(int key, T data = default(T))
		{
			this.key = key;
			this.data = data;
		}
	}
}
