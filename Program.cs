using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathTreeSet
{

    class Graph
    {
        Dictionary<char, Dictionary<char, int>> vertices = new Dictionary<char, Dictionary<char, int>>();

        public void add_vertex(char name, Dictionary<char, int> edges)
        {
            vertices[name] = edges;
        }

        public List<char> shortest_path(char start, char finish)
        {
            var previous = new Dictionary<char, char>();
            var distances = new Dictionary<char, int>();
            var nodes = new List<char>();

            List<char> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key == start)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            path = new List<char>();

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);
                
                if (smallest == finish)
                {
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    path.Add(start);

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }
    }

    class Program
    {
        // display paths...

        //public static void Main(string[] args)
        //{
        //    Graph g = new Graph();
        //    g.add_vertex('A', new Dictionary<char, int>() { { 'B', 7 }, { 'C', 8 } });
        //    g.add_vertex('B', new Dictionary<char, int>() { { 'A', 7 }, { 'F', 2 } });
        //    g.add_vertex('C', new Dictionary<char, int>() { { 'A', 8 }, { 'F', 6 }, { 'G', 4 } });
        //    g.add_vertex('D', new Dictionary<char, int>() { { 'F', 8 } });
        //    g.add_vertex('E', new Dictionary<char, int>() { { 'H', 1 } });
        //    g.add_vertex('F', new Dictionary<char, int>() { { 'B', 2 }, { 'C', 6 }, { 'D', 8 }, { 'G', 9 }, { 'H', 3 } });
        //    g.add_vertex('G', new Dictionary<char, int>() { { 'C', 4 }, { 'F', 9 } });
        //    g.add_vertex('H', new Dictionary<char, int>() { { 'E', 1 }, { 'F', 3 } });

        //    var shortestPath = new List<char>();
        //    //g.shortest_path('A', 'H').ForEach(x => Console.WriteLine(x));
        //    shortestPath = g.shortest_path('A', 'H');
        //    shortestPath.Reverse();
        //    shortestPath.ForEach(x => Console.WriteLine(x));
        //    Console.ReadKey();
        //}


        // codes below just displays shortest distance from the source

        static void Main(string[] args)
        {
            int[,] graph = {
                { 0, 4, 0, 0, 0, 0, 0, 8, 0 },
                { 4, 0, 8, 0, 0, 0, 0, 11, 0 },
                { 0, 8, 0, 7, 0, 4, 0, 0, 2 },
                { 0, 0, 7, 0, 9, 14, 0, 0, 0 },
                { 0, 0, 0, 9, 0, 10, 0, 0, 0 },
                { 0, 0, 4, 0, 10, 0, 2, 0, 0 },
                { 0, 0, 0, 14, 0, 2, 0, 1, 6 },
                { 8, 11, 0, 0, 0, 0, 1, 0, 7 },
                { 0, 0, 2, 0, 0, 0, 6, 7, 0 }
            };

            Dijkstra(graph, 0, 9);
            //Dijkstra(graph, 1, 9);
            //Dijkstra(graph, 2, 9);

            //http://www.geeksforgeeks.org/?p=27455
            Console.ReadKey();
        }

        private static int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
        {
            int min = int.MaxValue;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (shortestPathTreeSet[v] == false && distance[v] <= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        private static void Print(int[] distance, int verticesCount)
        {
            Console.WriteLine("Vertex    Distance from source");

            for (int i = 0; i < verticesCount; ++i)
                Console.WriteLine("{0}\t  {1}", i, distance[i]);
        }        

        private static void PrintPath(int[] path, int verticesCount)
        {
            Console.WriteLine("Vertex Path from source");

            for (int i = 0; i < verticesCount; ++i)
                Console.WriteLine("{0}\t  {1}", i, path[i]);
        }

        private static void PrintFromTo(int[] path, int verticesCount, int start, int end)
        {
            Console.WriteLine("Vertex Path from source");
            int[] pathToBeReversed = new int[verticesCount];
            int nextVertice = end;
            int index = 0;
            while (verticesCount > index)
            {
                if (nextVertice == start)
                    break;
                //Console.WriteLine("{0}\t  {1}", nextVertice, path[nextVertice]);
                pathToBeReversed[index] = path[nextVertice];
                nextVertice = path[nextVertice];
                index++;
            }

            if (index > verticesCount)
                index = verticesCount;

            while (index > 0)
            {
                index--;
                Console.WriteLine("{0}\t", pathToBeReversed[index]);

                //for (int i = 0; i < verticesCount; ++i)
                //    Console.WriteLine("{0}\t  {1}", i, path[i]);
            }

        }

        public static void Dijkstra(int[,] graph, int source, int verticesCount)
        {
            int[] distance = new int[verticesCount];
            bool[] shortestPathTreeSet = new bool[verticesCount];
            int[] parent = new int[verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            distance[source] = 0;

            for (int count = 0; count < verticesCount - 1; ++count)
            {
                int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
                shortestPathTreeSet[u] = true;

                for (int v = 0; v < verticesCount; ++v)
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u, v]) && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                    { 
                        distance[v] = distance[u] + graph[u, v];

                        parent[v] = u;
                    }
            }

            Print(distance, verticesCount);
            PrintPath(parent, verticesCount);
           // PrintFromTo(parent, verticesCount,0, verticesCount - 1);
            //PrintFromTo(parent, verticesCount, 0, 3);
            //PrintFromTo(parent, verticesCount, 8, 0);
            //PrintFromTo(parent, verticesCount, 8, 8);
            //PrintFromTo(parent, verticesCount, 8, 3);
            //PrintFromTo(parent, verticesCount, 3, 8);
        }
    }
}
