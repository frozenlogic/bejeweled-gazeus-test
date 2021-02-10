using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour, ISerializationCallbackReceiver
{
    public int height;
    public int width;

    public Cell CellPrefab;
    public Gem GemPrefab;

    Cell[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Cell cellObj = GameObject.Instantiate(CellPrefab);
                cellObj.SetWorldPositionInGrid(i, j, transform.position);
            }
        }

        DrawGrid();
    }

    public void DrawGrid()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Debug.DrawLine(GetCell(i, j).GetWorldPosition(), GetCell(i, j + 1).GetWorldPosition(), Color.blue);
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        return grid[x, y];
    }

    #region ISerializationCallbackReceiver

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
        grid = new Cell[width, height];
    }

    #endregion
}
