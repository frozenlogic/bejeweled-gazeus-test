using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int height;
    public int width;

    public Cell CellPrefab;
    public Gem[] Gems;

    Cell[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Cell[width, height];

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                Cell newCell = GameObject.Instantiate(CellPrefab);
                newCell.SetWorldPositionInGrid(i, j, transform.position);
                newCell.SetGem(GameObject.Instantiate(Gems[Random.Range(0, Gems.Length)]));
                grid[i, j] = newCell;
            }
        }
    }

    private void FixedUpdate()
    {
        DrawGrid();
    }

    public void DrawGrid()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if(j + 1 < grid.GetLength(1) && i + 1 < grid.GetLength(0))
                {
                    Debug.DrawLine(GetCell(i, j).GetWorldPosition(), GetCell(i, j + 1).GetWorldPosition(), Color.red);
                    Debug.DrawLine(GetCell(i, j).GetWorldPosition(), GetCell(i + 1, j).GetWorldPosition(), Color.red);
                }
            }
        }

        if (grid != null && grid.GetLength(0) > 0)
        {
            Debug.DrawLine(GetCell(0, height - 1).GetWorldPosition(), GetCell(width - 1, height - 1).GetWorldPosition(), Color.red);
            Debug.DrawLine(GetCell(width - 1, 0).GetWorldPosition(), GetCell(width - 1, height - 1).GetWorldPosition(), Color.red);
        }
    }

    public void AddGem(int x, int y, Gem g)
    {
        GetCell(x, y).SetGem(g);
    }

    public Cell GetCell(int x, int y)
    {
        return grid[x, y];
    }
}
