using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedMart_Question_1
{
    public class Program
    {
        public static List<Node> Nodes { get; set; } = new List<Node>();

        private const string FileName = "map.txt";

        public static void Main(string[] args)
        {
            var map = LoadMap();

            var stopwatch = Stopwatch.StartNew();

            InitializeNode(map);

            var longestPathLength = 0;
            var deepestDrop = 0;

            foreach (var node in Nodes)
            {
                var tuple = DepthSearch(node);

                if (tuple.Item1 >= longestPathLength)
                {
                    longestPathLength = tuple.Item1;

                    if (tuple.Item2 > deepestDrop)
                    {
                        deepestDrop = tuple.Item2;
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"{longestPathLength}{deepestDrop}@redmart.com");
            Console.WriteLine($"Time taken: {stopwatch.Elapsed}");
            Console.ReadLine();
        }

        private static Tuple<int, int> DepthSearch(Node rootNode)
        {
            var length = 1;
            var drop = 0;

            var nodeStack = new Stack<Tuple<int, Node>>();
            nodeStack.Push(new Tuple<int, Node>(length, rootNode));

            while (nodeStack.Count > 0)
            {
                var tuple = nodeStack.Pop();

                if (tuple.Item2.Children.Any())
                {
                    foreach (var childNode in tuple.Item2.Children)
                    {
                        nodeStack.Push(new Tuple<int, Node>(tuple.Item1 + 1, childNode));
                    }
                }
                else
                {
                    if (tuple.Item1 >= length)
                    {
                        length = tuple.Item1;

                        var tempDrop = rootNode.Value - tuple.Item2.Value;

                        if (tempDrop > drop)
                        {
                            drop = tempDrop;
                        }
                    }

                }
            }
            return new Tuple<int, int>(length, drop);
        }

        private static void InitializeNode(int[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    var rootNode = new Node { X = x, Y = y, Value = map[y, x] };

                    FindValidPath(map, rootNode);
                    Nodes.Add(rootNode);
                }
            }
        }

        private static void FindValidPath(int[,] map, Node parentNode)
        {
            var leftNode = GetLeftNode(map, parentNode.X, parentNode.Y);

            if (leftNode != null && leftNode.Value < parentNode.Value)
            {
                parentNode.Children.Add(leftNode);
                FindValidPath(map, leftNode);
            }

            var rightNode = GetRightNode(map, parentNode.X, parentNode.Y);

            if (rightNode != null && rightNode.Value < parentNode.Value)
            {
                parentNode.Children.Add(rightNode);
                FindValidPath(map, rightNode);
            }

            var topNode = GetTopNode(map, parentNode.X, parentNode.Y);

            if (topNode != null && topNode.Value < parentNode.Value)
            {
                parentNode.Children.Add(topNode);
                FindValidPath(map, topNode);
            }

            var bottomNode = GetBottomNode(map, parentNode.X, parentNode.Y);

            if (bottomNode != null && bottomNode.Value < parentNode.Value)
            {
                parentNode.Children.Add(bottomNode);
                FindValidPath(map, bottomNode);
            }
        }

        private static Node GetLeftNode(int[,] map, int x, int y)
        {
            if (x < 1)
            {
                return null;
            }
            return new Node { Y = y, X = x - 1, Value = map[y, x - 1] };
        }

        private static Node GetRightNode(int[,] map, int x, int y)
        {
            if (x == map.GetLength(1) - 1)
            {
                return null;
            }
            return new Node { Y = y, X = x + 1, Value = map[y, x + 1] };
        }

        private static Node GetTopNode(int[,] map, int x, int y)
        {
            if (y < 1)
            {
                return null;
            }
            return new Node { Y = y - 1, X = x, Value = map[y - 1, x] };
        }

        private static Node GetBottomNode(int[,] map, int x, int y)
        {
            if (y == map.GetLength(0) - 1)
            {
                return null;
            }
            return new Node { Y = y + 1, X = x, Value = map[y + 1, x] };
        }

        private static int[,] LoadMap()
        {
            var mapValues = File.ReadLines(FileName).ToArray();

            if (mapValues.Any())
            {
                var xy = mapValues.First().Split(' ').ToArray();
                var map = new int[int.Parse(xy[0]), int.Parse(xy[1])];

                for (int y = 1; y < mapValues.Length; y++)
                {
                    var rowValues = mapValues[y].Split(' ').ToArray();

                    for (int x = 0; x < rowValues.Length; x++)
                    {
                        map[y - 1, x] = int.Parse(rowValues[x]);
                    }
                }
                return map;
            }
            return null;
        }
    }
}
