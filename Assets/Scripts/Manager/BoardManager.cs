using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject boardPrefab;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject camera_object;

    // grid
    public (GameObject, int, GameObject)[,] grid;

    // length
    public int rows = 8;
    public int cols = 8;

    // size cell
    private float widthCell;
    private float heigthCell;
    private float gapCell = 0.1f;

    void Start()
    {
        SetSize();
        SetGrid();
    }

    void SetSize()
    {
        Vector3 sizePlane = boardPrefab.GetComponent<Renderer>().bounds.size;
        Debug.Log(sizePlane);

        widthCell = sizePlane.x / rows;
        heigthCell = sizePlane.y / cols;

        boardPrefab.transform.position = new Vector3(sizePlane.x / 2 - widthCell / 2, sizePlane.y / 2 - heigthCell / 2, 0);
        camera_object.transform.position = new Vector3(sizePlane.x / 2 - widthCell / 2, sizePlane.y / 2 - heigthCell / 2, -10);
    }

    void SetGrid()
    {
        grid = new (GameObject, int, GameObject)[rows, cols];

        for (int i = 0; i < grid.GetLength(0); ++i)
        {
            for (int j = 0; j < grid.GetLength(1); ++j)
            {
                grid[i,j].Item2 = 0;
                GameObject cellItem = Instantiate(cellPrefab, new Vector3(i * widthCell, j * heigthCell, 0), Quaternion.identity);
                Vector3 size = cellItem.GetComponent<Renderer>().bounds.size;
                cellItem.transform.localScale = new Vector3(widthCell / size.x - gapCell, heigthCell / size.y - gapCell, 0.2f);
                grid[i, j].Item1 = cellItem;
            }
        }
    }
}
