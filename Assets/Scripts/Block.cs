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
            cube.transform.localPosition = pos* scale;
            cube.GetComponent<Renderer>().material = material;
            this.color = color;
        }
    }

    public int getColor()
    {
        return color;
    }
}
