using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLS
{
    // A utility function to find the
    // vertex with minimum distance
    // value, from the set of vertices
    // not yet included in shortest
    // path tree

    private static FLS instance;

    private FLS() { }

    public static FLS Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FLS();
            }
            return instance;
        }
    }

    private List<(int, int)> move = new List<(int, int)>();

    public int minDistance(int[] dist, bool[] sptSet)
    {
        // Initialize min value
        int min = int.MaxValue, min_index = -1;

        for (int v = 0; v < dist.Length; v++)
            if (sptSet[v] == false && dist[v] <= min)
            {
                min = dist[v];
                min_index = v;
            }

        return min_index;
    }

    // A utility function to print
    // the constructed distance array
    public void printSolution(int[] dist, int[] parent, int n, List<(int, int)> post, int length_arr_block)
    {
        Debug.Log("Vertex Dist Path");
        for (int i = 0; i < n; i++)
        {
            // Console.Write(post[i].Item1 + "/" + (length_arr_block - 1) + " ");
            if (post[i].Item1 == length_arr_block - 1 && dist[i] != int.MaxValue)
            {
                Debug.Log(i + "\t" + (dist[i] == int.MaxValue ? "INF" : dist[i].ToString()) + "\t");
                int numberMove = CountPath(parent, i, post);
                PrintPath(parent, i, post);
                Console.WriteLine();

                if (move.Count == 0 || numberMove < move.Count)
                {
                    Debug.Log("Find move");
                    move.Clear();
                    AddPathToListMove(parent, i, post);
                }
            }
        }
    }

    public void printMoveShortest()
    {
        Console.WriteLine("This is result");
        for (int i = 0; i < move.Count; ++i)
        {
            if (i == move.Count - 1)
            {
                Console.Write(move[i]);
            }
            else
            {
                Console.Write(move[i] + " -> ");
            }
        }
    }

    // Function that implements Dijkstra's
    // single source shortest path algorithm
    // for a graph represented using adjacency
    // matrix representation
    public void dijkstra(int[,] graph, int src, List<(int, int)> post, int length_arr_block)
    {
        Debug.Log("dijkstra");
        int V = graph.GetLength(0);

        int[] dist = new int[V]; // The output array. dist[i]
        // will hold the shortest
        // distance from src to i

        // sptSet[i] will true if vertex
        // i is included in shortest path
        // tree or shortest distance from
        // src to i is finalized
        bool[] sptSet = new bool[V];

        int[] parent = new int[V];
        for (int i = 0; i < V; i++)
        {
            parent[i] = -1;
        }

        // Initialize all distances as
        // INFINITE and stpSet[] as false
        for (int i = 0; i < V; i++)
        {
            dist[i] = int.MaxValue;
            sptSet[i] = false;
        }

        // Distance of source vertex
        // from itself is always 0
        dist[src] = 0;

        // Find shortest path for all vertices
        for (int count = 0; count < V - 1; count++)
        {
            // Pick the minimum distance vertex
            // from the set of vertices not yet
            // processed. u is always equal to
            // src in first iteration.
            int u = minDistance(dist, sptSet);

            // Mark the picked vertex as processed
            sptSet[u] = true;

            // Update dist value of the adjacent
            // vertices of the picked vertex.
            for (int v = 0; v < V; v++)

                // Update dist[v] only if is not in
                // sptSet, there is an edge from u
                // to v, and total weight of path
                // from src to v through u is smaller
                // than current value of dist[v]
                if (!sptSet[v] && graph[u, v] != 0 &&
                     dist[u] != int.MaxValue && dist[u] + graph[u, v] < dist[v])
                {
                    dist[v] = dist[u] + graph[u, v];
                    parent[v] = u;
                }
        }

        // print the constructed distance array
        printSolution(dist, parent, V, post, length_arr_block);
    }

    public List<(int, int)> AlgorithmToConnectBridge(int[,] arr_block)
    {
        //Debug.Log("Algorithm");
        int[,] arr_clean = CleanArray(arr_block);

        List<(int, int)> validCells = new List<(int, int)>();

        for (int i = 0; i < arr_clean.GetLength(0); ++i)
        {
            for (int j = 0; j < arr_clean.GetLength(1); ++j)
            {
                if (arr_clean[i, j] > 0)
                {
                    validCells.Add((i, j));
                }
            }
        }

        int count = validCells.Count;

        // Bước 2: Tạo map tọa độ -> index
        Dictionary<(int, int), int> cellToIndex = new Dictionary<(int, int), int>();
        for (int i = 0; i < count; ++i)
        {
            cellToIndex[validCells[i]] = i;
        }

        // Bước 3: Tạo ma trận kề
        int[,] arr_graph = new int[count, count];

        // Các hướng kề nhau (trên, dưới, trái, phải)
        int[,] directions = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

        for (int i = 0; i < count; ++i)
        {
            var (x, y) = validCells[i];

            for (int d = 0; d < 4; ++d)
            {
                int nx = x + directions[d, 0];
                int ny = y + directions[d, 1];

                if (cellToIndex.ContainsKey((nx, ny)))
                {
                    int j = cellToIndex[(nx, ny)];
                    arr_graph[i, j] = 1;
                }
            }
        }

        // arr_graph sử dụng để áp dụng thuật toán dijkstra

        for (int i = 0; i < arr_graph.GetLength(0); ++i)
        {
            for (int j = 0; j < arr_graph.GetLength(1); ++j)
            {
                Console.Write(arr_graph[i, j] + " ");
            }
            Console.WriteLine();
        }

        //Debug.Log("This is array graph");

        for (int i = 0; i < validCells.Count; ++i)
        {
            //Debug.Log(validCells[i].Item1 + " " + validCells[i].Item2);
            if (validCells[i].Item1 == 0)
            {
                dijkstra(arr_graph, i, validCells, arr_block.GetLength(0));
            }
        }

        printMoveShortest();

        return move;
    }

    public int[,] CleanArray(int[,] arr_block)
    {
        return arr_block;
    }

    int CountPath(int[] parent, int j, List<(int, int)> post)
    {
        if (parent[j] == -1)
        {
            return 1;
        }
        return 1 + CountPath(parent, parent[j], post);
    }

    void PrintPath(int[] parent, int j, List<(int, int)> post)
    {
        if (parent[j] == -1)
        {
            Console.Write(post[j]);
            return;
        }

        PrintPath(parent, parent[j], post);
        Console.Write(" -> " + post[j]);
    }

    void AddPathToListMove(int[] parent, int j, List<(int, int)> post)
    {
        if (parent[j] == -1)
        {
            move.Add(post[j]);
            return;
        }
        AddPathToListMove(parent, parent[j], post);
        move.Add(post[j]);
    }
}
