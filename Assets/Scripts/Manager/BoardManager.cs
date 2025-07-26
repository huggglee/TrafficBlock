using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    [SerializeField] private GameObject boardPrefab;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject camera_object;
    // grid
    public (GameObject, int, GameObject)[,] grid;
    // length
    public int rows = 8;
    public int cols = 8;
    // size cell
    public float widthCell;
    public float heigthCell;
    public float gapCell = 0.1f;

    private List<(int, int)> highlightedCells = new List<(int, int)>();
    private Color currentColor;

    void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        SetSize();
        SetGrid();
        currentColor = grid[0, 0].Item1.GetComponent<Renderer>().material.color;
    }

    void SetSize()
    {
        Vector3 sizePlane = boardPrefab.GetComponent<Renderer>().bounds.size;
        //Debug.Log(sizePlane);

        widthCell = sizePlane.x / rows;
        heigthCell = sizePlane.y / cols;
        boardPrefab.transform.position = new Vector3(sizePlane.x / 2 - widthCell / 2, sizePlane.y / 2 - heigthCell / 2, 10);
        camera_object.transform.position = new Vector3(sizePlane.x / 2 - widthCell / 2, sizePlane.y / 2 - heigthCell / 2, -10);
    }

    void SetGrid()
    {
        grid = new (GameObject, int, GameObject)[rows, cols];

        for (int i = 0; i < grid.GetLength(0); ++i)
        {
            for (int j = 0; j < grid.GetLength(1); ++j)
            {
                GameObject cellItem = Instantiate(cellPrefab, new Vector3(i * widthCell, j * heigthCell, 10), Quaternion.identity);
                Vector3 size = cellItem.GetComponent<MeshFilter>().sharedMesh.bounds.size;
                cellItem.transform.localScale = new Vector3(widthCell / size.x - gapCell, heigthCell / size.y - gapCell, 0.2f);
                grid[i,j] = (cellItem, 0, null);
            }
        }
    } //


    public void HighlightCells(List<(int, int)> cells)
    {
        ClearHighlight();
        foreach (var (i, j) in cells)
        {
            grid[i, j].Item1.GetComponent<Renderer>().material.color = Color.yellow;
            highlightedCells.Add((i, j));
        }
    }

    public void ClearHighlight()
    {
        foreach (var (i, j) in highlightedCells)
        {
            grid[i, j].Item1.GetComponent<Renderer>().material.color = currentColor;
        }
        highlightedCells.Clear();
    }

    public int[,] GetArrayGridByColor(int color)
    {
        int[,] arr_block = new int[grid.GetLength(0), grid.GetLength(1)];

        for (int i = 0; i < grid.GetLength(0); ++i)
        {
            for (int j = 0; j < grid.GetLength(1); ++j)
            {
                if (grid[i,j].Item2 == color)
                {
                    arr_block[grid.GetLength(1) - 1 - j, i] = 1;
                } else
                {
                    arr_block[grid.GetLength(1) - 1 - j, i] = 0;
                }
            }
        }

        return arr_block;
    }

    public void UpdateBoard(List<(int, int)> move)
    {
        if (move.Count > 0)
        {
            Debug.Log("Line is destroy");
            for (int i = 0; i < move.Count; ++i)
            {
                //
                int m = move[i].Item1;
                int n = move[i].Item2;
                int k = n;
                int h = grid.GetLength(1) - 1 - m;
                Debug.Log(k + " " + h);
                if (grid[k, h].Item3 != null)
                {
                    Debug.Log("Destroy");
                    GameObject gameTemp = null;
                    gameTemp = grid[k, h].Item3;
                    Destroy(gameTemp);
                }
                var old = grid[k, h];
                grid[k, h] = (old.Item1, 0, null);
            }
        } else
        {
            Debug.Log("Line is non destroy");
        }
    }
}
