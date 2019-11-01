using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;

    public MyGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0f) * this.cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / this.cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / this.cellSize);
    }


    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return 0;
        }
    }

    public int[] GetValue(Vector3 worldPosition)
    {
        int x, y;

        GetXY(worldPosition, out x, out y);

        //return GetValue(x, y);
        Debug.Log(x + ", " + y);
        return new int[2] { x, y };
    }

    public Vector3 GetGridCellWorldPosition(int x, int y)
    {
        return GetWorldPosition(x, y);
    }
}
