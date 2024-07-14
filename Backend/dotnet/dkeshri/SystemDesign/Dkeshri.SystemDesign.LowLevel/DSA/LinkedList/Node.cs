using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.DSA.LinkedList
{
    internal class Node
    {
        public int val;
        public Node next;
        public Node(int val = 0, Node node = null)
        {
            this.val = val;
            this.next = node;
        }
    }
}
