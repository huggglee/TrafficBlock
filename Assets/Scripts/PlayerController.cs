using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    private Transform selectedBlock;
    private Vector3 offset;
    private Plane dragPlane;
    private float fixedZ;
    private Vector3 startPosition;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Ray ray = cam.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform.parent != null && hit.transform.parent.CompareTag("Block"))
                        {
                            selectedBlock = hit.transform.parent;
                            startPosition = selectedBlock.position;
                        }
                        else
                            selectedBlock = null;

                        if (selectedBlock != null)
                        {
                            fixedZ = selectedBlock.position.z;
                            dragPlane = new Plane(Vector3.forward, new Vector3(0, 0, fixedZ));

                            float distance;
                            dragPlane.Raycast(ray, out distance);
                            offset = selectedBlock.position - ray.GetPoint(distance);
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    if (selectedBlock != null)
                    {
                        Ray moveRay = cam.ScreenPointToRay(touch.position);
                        float moveDistance;
                        if (dragPlane.Raycast(moveRay, out moveDistance))
                        {
                            Vector3 newPos = moveRay.GetPoint(moveDistance) + offset;
                            newPos.z = fixedZ;
                            selectedBlock.position = newPos;

                            HighlightNearestCells(selectedBlock);
                        }
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (selectedBlock != null)
                    {
                        bool placed = SnapToGrid(selectedBlock);
                        if (!placed)
                        {
                            selectedBlock.position = startPosition;
                        }
                    }
                    BoardManager.Instance.ClearHighlight();
                    selectedBlock = null;
                    break;
            }
        }
    }

    bool SnapToGrid(Transform block)
    {
        Dictionary<(int, int), GameObject> cells = new Dictionary<(int, int), GameObject>();

        List<Transform> childList = new List<Transform>();
        foreach (Transform child in block)
        {
            childList.Add(child);
        }

        foreach (Transform child in childList)
        {
            Vector3 pos = child.position;
            int i = Mathf.RoundToInt(pos.x / BoardManager.Instance.widthCell);
            int j = Mathf.RoundToInt(pos.y / BoardManager.Instance.heigthCell);

            if (i < 0 || i >= BoardManager.Instance.rows || j < 0 || j >= BoardManager.Instance.cols)
                return false;

            if (BoardManager.Instance.grid[i, j].Item2 != 0)
                return false;

            cells.Add((i, j), child.gameObject);
            //children.Add(child.gameObject);
            //child.SetParent(null);
        }

        int pivotI = Mathf.RoundToInt(block.position.x / BoardManager.Instance.widthCell);
        int pivotJ = Mathf.RoundToInt(block.position.y / BoardManager.Instance.heigthCell);
        pivotI = Mathf.Clamp(pivotI, 0, BoardManager.Instance.rows - 1);
        pivotJ = Mathf.Clamp(pivotJ, 0, BoardManager.Instance.cols - 1);

        Vector3 pivotCellPos = BoardManager.Instance.grid[pivotI, pivotJ].Item1.transform.position;
        Vector3 offset = block.position - pivotCellPos;
        block.position -= offset;

        foreach (Transform child in childList)
        {
            child.SetParent(null);
        }

        foreach (var ((i, j), gameObject) in cells)
        {
            BoardManager.Instance.grid[i, j].Item2 = block.GetComponent<Block>().getColor();
            BoardManager.Instance.grid[i, j].Item3 = gameObject;
        }

        BlockManager.Instance.DeleteBlock(block.gameObject);

        int[,] arr_block = BoardManager.Instance.GetArrayGridByColor(1);

        List<(int, int)> move = FLS.Instance.AlgorithmToConnectBridge(arr_block);

        BoardManager.Instance.UpdateBoard(move);

        return true;
    }

    void HighlightNearestCells(Transform block)
    {
        List<(int, int)> cells = new List<(int, int)>();
        foreach (Transform child in block)
        {
            Vector3 worldPos = child.position;
            int i = Mathf.RoundToInt(worldPos.x / BoardManager.Instance.widthCell);
            int j = Mathf.RoundToInt(worldPos.y / BoardManager.Instance.heigthCell);
            i = Mathf.Clamp(i, 0, BoardManager.Instance.rows - 1);
            j = Mathf.Clamp(j, 0, BoardManager.Instance.cols - 1);

            cells.Add((i, j));
        }
        BoardManager.Instance.HighlightCells(cells);
    }
}
