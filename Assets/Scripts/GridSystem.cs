using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//stores the current selected gem
struct Move
{
    public Gem currentGemSelection;
    public Gem previousGemSelection;
}

//Organizes all gems and keep the current move
public class GridSystem : MonoBehaviour
{
    public int height;
    public int width;

    public Cell CellPrefab;
    public Gem[] Gems;

    public Gem currentGemSelection;

    public Cell[,] Grid { private set; get; }

    public UnityEvent OnGridChanged = new UnityEvent(); //something has changed in the Grid - pieces were switched

    // Start is called before the first frame update
    void Start()
    {
        //currentMove = new Move();

        Grid = new Cell[width, height];

        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Cell newCell = GameObject.Instantiate(CellPrefab);
                newCell.SetWorldPositionInGrid(i, j, transform.position);
                Gem g = GameObject.Instantiate(Gems[UnityEngine.Random.Range(0, Gems.Length)]);
                g.OnClickedOnGem.AddListener(ClickedOnGem);
                newCell.SetGem(g);
                Grid[i, j] = newCell;
            }
        }
    }

    private void ClickedOnGem(Gem g)
    {
        if (currentGemSelection)
        {
            //check if they are adjacents

            //swap
            SwapPieces(g);
        }
        else
        {
            currentGemSelection = g;
        }
    }

    private void SwapPieces(Gem g)
    {
        currentGemSelection.MoveTo(g.transform.position);
        g.MoveTo(currentGemSelection.transform.position);

        OnGridChanged?.Invoke(); //notifiy the listeners the Grid has new pieces in place
    }

    private void FixedUpdate()
    {
        DrawGrid();
    }

    public void DrawGrid()
    {
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                if(j + 1 < Grid.GetLength(1) && i + 1 < Grid.GetLength(0))
                {
                    Debug.DrawLine(GetCell(i, j).GetWorldPosition(), GetCell(i, j + 1).GetWorldPosition(), Color.red);
                    Debug.DrawLine(GetCell(i, j).GetWorldPosition(), GetCell(i + 1, j).GetWorldPosition(), Color.red);
                }
            }
        }

        if (Grid != null && Grid.GetLength(0) > 0)
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
        return Grid[x, y];
    }
}
