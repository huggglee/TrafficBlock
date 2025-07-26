using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public GameObject cubePrefab;
    //public Material[] materials;
    private float scale = 1.2f;
    private int color;
    public void Spawn(Vector3[] cells,Material material,int color)
    {
        foreach (var pos in cells)
        {
            GameObject cube = Instantiate(cubePrefab, transform);
            Vector3 size = cube.GetComponent<MeshFilter>().sharedMesh.bounds.size;
            cube.transform.localPosition = new Vector3(pos.x * BoardManager.Instance.widthCell / size.x, pos.y * BoardManager.Instance.heigthCell / size.y, 0);
            cube.transform.localScale = new Vector3(BoardManager.Instance.widthCell / size.x, BoardManager.Instance.heigthCell / size.y, 1);
            cube.GetComponent<Renderer>().material = material;
            this.color = color;
        }
    }

    public int getColor()
    {
        return color;
    }
}
