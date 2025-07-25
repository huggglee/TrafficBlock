﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject blockPrefab; 
    public GameObject cubePrefab;
    public Dictionary<int, Material> materials = new Dictionary<int, Material>();
    public List<GameObject> listBlock = new List<GameObject>();

    private static BlockManager instance;

    public static BlockManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BlockManager>();
                if (instance == null)
                {
                    Debug.LogError("Không tìm thấy BlockManager trong scene!");
                }
            }
            return instance;
        }
    }

    void Start()
    {
        LoadData();
        //SpawnVertical(); 
        //SpawnSquare();   
        //SpawnLShape();
        SpawnRandom();
    }
    void LoadData()
    {
        materials[1] = Resources.Load<Material>("Materials/Blue_mat");
        materials[2] = Resources.Load<Material>("Materials/Red_mat");
        materials[3] = Resources.Load<Material>("Materials/Yellow_mat");
    }

    void SpawnRandom()
    {
        Vector3[] cells_1 = Spawn_Block.Instance.GetListVectorRandom();
        Vector3[] cells_2 = Spawn_Block.Instance.GetListVectorRandom();
        Vector3[] cells_3 = Spawn_Block.Instance.GetListVectorRandom();

        //for (int i = 0; i < cells_1.Length; ++i)
        //{
        //    Debug.Log(cells_1[i]);
        //}

        SpawnBlock(cells_1, new Vector3(0.5f, -4f, 10), materials[1],1);
        SpawnBlock(cells_2, new Vector3(4f, -4f, 10), materials[1],1);
        SpawnBlock(cells_3, new Vector3(7.5f, -4f, 10), materials[1],1);
    }

    void SpawnVertical()
    {
        Vector3[] cells = {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(0,2,0),
            new Vector3(1,0,0)
        };
        SpawnBlock(cells, new Vector3(0.5f, -4f, 10), materials[1],1);
    }

    void SpawnSquare()
    {
        Vector3[] cells = {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0)
        };
        SpawnBlock(cells, new Vector3(4f,-4f,10), materials[3],3);
    }

    void SpawnLShape()
    {
        Vector3[] cells = {
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,0,0)
        };
        SpawnBlock(cells, new Vector3(7.5f, -4f, 10), materials[3],3);
    }

    void SpawnBlock(Vector3[] cells, Vector3 position,Material material,int color)
    {
        GameObject blockGO = Instantiate(blockPrefab, position, Quaternion.identity);
        listBlock.Add(blockGO);
        Block block = blockGO.GetComponent<Block>();
        block.cubePrefab = cubePrefab;
        block.Spawn(cells, material,color);
    }

    public void DeleteBlock(GameObject block)
    {
        listBlock.Remove(block);
        GameObject.Destroy(block.gameObject);
        CheckAndCreateNewBlock();
    }

    public void CheckAndCreateNewBlock()
    {
        if (listBlock.Count == 0)
        {
            SpawnRandom();
        }
    }
}
