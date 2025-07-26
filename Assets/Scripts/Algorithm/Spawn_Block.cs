using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

class IF_Block
{
    private Vector2 Ox;
    private Vector2 Oy;
}

public class Spawn_Block
{
    private static Spawn_Block instance;

    private Spawn_Block() { }

    public static Spawn_Block Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Spawn_Block();
            }
            return instance;
        }
    }

    public void Spawn(int[,] array_block)
    {
        int length = array_block.GetLength(0);
        int[,] arr_ana = new int[length, length];

        // enter value to analycs
        for (int i = 0; i < length; ++i)
        {
            for (int j = 0; j < length; ++j)
            {
                if (array_block[i, j] == 0)
                {
                    arr_ana[i, j] = 0;
                }
                else
                {
                    arr_ana[i, j] = 1;
                }
            }
        }
    }

    public Vector3[] GetListVectorRandom()
    {
        List<Vector3> listVector = new List<Vector3>();
        listVector.Add(new Vector3(0, 0, 0));

        // random so chieu
        int random_dimen = UnityEngine.Random.Range(0, 2);

        Debug.Log("random_dimen: " + random_dimen);

        if (random_dimen == 0)
        {
            // 2 chieu
            int random_right_left = UnityEngine.Random.Range(0, 2);
            int random_top_down = UnityEngine.Random.Range(0, 2);
            int random_number_1 = UnityEngine.Random.Range(1, 3);
            int random_number_2 = UnityEngine.Random.Range(1, 3);

            if (random_number_1 == 2)
            {
                random_number_2 = 1;
            }

            if (random_number_2 == 2)
            {
                random_number_1 = 1;
            }

            if (random_right_left == 1)
            {
                // trai
                for (int i = 1; i <= random_number_1; ++i)
                {
                    listVector.Add(new Vector3(-i, 0, 0));
                }
            } else
            {
                // phai
                for (int i = 1; i <= random_number_1; ++i)
                {
                    listVector.Add(new Vector3(i, 0, 0));
                }
            }

            if (random_top_down == 1)
            {
                // tren
                for (int i = 1; i <= random_number_2; ++i)
                {
                    listVector.Add(new Vector3(0, i, 0));
                }
            } else
            {
                // duoi
                for (int i = 1; i <= random_number_2; ++i)
                {
                    listVector.Add(new Vector3(0, -i, 0));
                }
            }
        }
        else
        {
            // 1 chieu
            int random_ver_hor = UnityEngine.Random.Range(0, 2);
            int random_number_1 = UnityEngine.Random.Range(0, 2);
            int random_number_2 = UnityEngine.Random.Range(0, 2);

            if (random_number_1 == 0 && random_number_2 == 0)
            {
                int random_number_3 = UnityEngine.Random.Range(0, 2);
                if (random_number_3 == 1)
                {
                    random_number_1 = 1;
                }
                else
                {
                    random_number_2 = 1;
                }
            }

            if (random_ver_hor == 0)
            {
                // doc
                if (random_number_1 == 1)
                {
                    listVector.Add(new Vector3(0, 1, 0));
                }
                if (random_number_2 == 1)
                {
                    listVector.Add(new Vector3(0, -1, 0));
                }
            } else
            {
                // ngang
                if (random_number_1 == 1)
                {
                    listVector.Add(new Vector3(1, 0, 0));
                }
                if (random_number_2 == 1)
                {
                    listVector.Add(new Vector3(-1, 0, 0));
                }
            }
        }

        Vector3[] result = listVector.ToArray();

        return result;
    }
}
