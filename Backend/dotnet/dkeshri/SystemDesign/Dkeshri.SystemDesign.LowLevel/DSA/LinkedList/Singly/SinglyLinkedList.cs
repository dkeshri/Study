using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace Dkeshri.SystemDesign.LowLevel.DSA.LinkedList.Singly
{
    internal class SinglyLinkedList
    {
        private Node head;
        public SinglyLinkedList()
        {
            head = null;
        }
        public Node GetHeadNode { get { return head; } }
        public Node GetLastNode
        {
            get
            {
                Node last = head;
                while (last.next != null)
                {
                    last = last.next;
                }
                return last;
            }
        }
        public void TraverseList(Node listNode)
        {
            Node temp = listNode;

            while (temp != null)
            {
                Console.WriteLine(temp.val);
                temp = temp.next;
            }
        }
        public void InsertAtEnd(int value)
        {
            if (head == null)
            {
                head = new Node(value);
            }
            else
            {
                Node lastNode = GetLastNode;
                lastNode.next = new Node(value);
            }

        }

        public void InsertAtBening(int value)
        {
            if (head == null)
            {
                head = new Node(value);
            }
            else
            {
                Node newNode = new Node(value);
                newNode.next = head;
                head = newNode;
            }
        }



    }

    public class SinglyLinkedListEx : IExecute
    {
        public void run()
        {
            SinglyLinkedList singlyLinkedList = new SinglyLinkedList();
            int[] data = new int[] { 89, 78, 60, 10, 24 };

            foreach (int i in data)
            {
                singlyLinkedList.InsertAtEnd(i);
            }

            singlyLinkedList.InsertAtBening(8);
            singlyLinkedList.TraverseList(singlyLinkedList.GetHeadNode);

        }
    }
}
