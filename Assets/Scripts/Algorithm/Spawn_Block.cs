using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class IF_Block
{
    private Vector2 Ox;
    private Vector2 Oy;
}

class Spawn_Block
{
    public void Spawn(int[,] array_block, int color_1, int color_2)
    {
        Vector2 result_1 = new Vector2();
        Vector2 result_2 = new Vector2();
        Vector2 result_3 = new Vector2();

        int length = array_block.GetLength(0);
        int[,] arr_ana = new int[length, length];

        // enter value to analycs
        for (int i = 0; i < length; ++i)
        {
            for (int j = 0; j < length; ++j)
            {
                if (array_block[i, j] == 0 || array_block[i, j] == color_1)
                {
                    arr_ana[i, j] = 1;
                }
                else
                {
                    arr_ana[i, j] = 0;
                }
            }
        }
    }
}
