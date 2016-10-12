using System.Collections.Generic;

namespace RedMart_Question_1
{
    public class Node
    {
        public int Value { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public List<Node> Children { get; set; } = new List<Node>();
    }
}
