namespace Dkeshri.SystemDesign.LowLevel.DSA.LinkedList.Doubly
{
    internal class Node
    {
        public int val;
        public Node next;
        public Node prev;
        public Node(int val = 0)
        {
            this.val = val;
            next = null;
            prev = null;
        }
    }
}
