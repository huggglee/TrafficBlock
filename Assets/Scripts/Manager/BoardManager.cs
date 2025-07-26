using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public int rows = 8;
    public int cols = 10;
    void Start()
    {
        for (int i = 0; i < rows * cols; i++)
        {
            Instantiate(cellPrefab, transform);
        }
    }
}
