﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//stores the current selected gem
public class Move
{
    public List<Gem> selection;

    int numOfMovingGems = 2;

    public UnityEvent OnMoveEnd = new UnityEvent();

    public Move()
    {
        selection = new List<Gem>();
    }

    public void AddSelection(Gem g)
    {
        g.OnMovementEnd.AddListener(OnGemMovementEnd);
        selection.Add(g);
    }

    void OnGemMovementEnd(Gem g)
    {
        g.OnMovementEnd.RemoveAllListeners();

        numOfMovingGems--;
        if (numOfMovingGems == 0)
        {
            numOfMovingGems = 2;
            OnMoveEnd?.Invoke();
        }

    }

    public bool IsFirstSelection()
    {
        return selection.Count > 0 ? false : true;
    }

    public void Swap()
    {
        if(selection.Count > 0)
        {
            selection[0].MoveTo(selection[1].transform.position);
            selection[1].MoveTo(selection[0].transform.position);

            //setting gem to his new cell
            Cell aux;
            aux = selection[0].currentCell;
            selection[0].SetCell(selection[1].currentCell);
            selection[0].currentCell.SetGem(selection[0]);
            selection[1].SetCell(aux);
            selection[1].currentCell.SetGem(selection[1]);
        }
    }

    public void Reset()
    {
        numOfMovingGems = 2;
        selection.Clear();
    }
}

//Organizes all gems and keep the current move
public class GridSystem : MonoBehaviour
{
    public MoveValidator moveValidator;

    public int height;
    public int width;

    public Cell CellPrefab;
    public Gem[] Gems;
    public List<Cell> emptyCells;

    public Move currentMove;

    public Cell[,] Grid { private set; get; }

    bool isStartingGrid;

    public UnityEvent OnGridChanged = new UnityEvent(); //something has changed in the Grid - pieces were swaped

    private void Awake()
    {
        Grid = new Cell[width, height];

        emptyCells = new List<Cell>();

        currentMove = new Move();
        currentMove.OnMoveEnd.AddListener(MoveEnd);
    }

    // Start is called before the first frame update
    void Start()
    {
        moveValidator.OnMoveValidated.AddListener(AfterMoveValidated);

        isStartingGrid = true;
        Fill();
        StartGridWithNoMatches();
    }

    void StartGridWithNoMatches()
    {
        if (moveValidator.ResolveGrid())
        {
            Fill();
            StartGridWithNoMatches();
        }

        isStartingGrid = false;
        emptyCells.Clear();
    }

    private void MoveEnd()
    {
        OnGridChanged?.Invoke(); //notifiy the listeners the Grid has new pieces in place
    }

    private void AfterMoveValidated(List<Gem> matchesList)
    {
        RemoveMatches(matchesList);
    }

    void RemoveMatches(List<Gem> matchesList)
    {
        if (matchesList.Count > 0)
        {
            for (int i = 0; i < matchesList.Count; i++)
            {
                Gem g = matchesList[i];
                g.gameObject.SetActive(false);
                RemoveGemFromCell(g.currentCell);
                emptyCells.Add(g.currentCell);
            }
        }
        else
        {
            currentMove.Swap();
        }

        currentMove.Reset();

        if (!isStartingGrid)
        {
            MoveCells();
            //Fill();
        }
    }

    void Fill()
    {
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Cell cell = GetCell(i, j);
                if (cell == null)
                {
                    Cell newCell = GameObject.Instantiate(CellPrefab);
                    newCell.SetWorldPositionInGrid(i, j, transform.position);
                    newCell.SetGridPosition(i, j);
                    Gem g = CreateRandomGem();
                    g.OnClickedOnGem.AddListener(ClickedOnGem);
                    newCell.SetGem(g);
                    g.SetCell(newCell);
                    Grid[i, j] = newCell;

                    Debug.Log("Created Cell in" + "[" + i + ", " + j + "]. Created Gem.");
                }
                else
                {
                    if(cell.currentGem == null)
                    {
                        Gem g = CreateRandomGem();
                        g.OnClickedOnGem.AddListener(ClickedOnGem);
                        cell.SetGem(g);
                        g.SetCell(cell);

                        Debug.Log("Created Gem on Cell " + "[" + i + ", " + j + "]");
                    }
                }
            }
        }
    }

    void MoveCells()
    {
        /*
        foreach(Cell cell in emptyCells)//for every empty cell, we move down all gems that are above it
        {
            for (int i = (int)cell.gridPosition.y + 1; i < Grid.GetLength(1); i++) //iterate over Y
            {
                Cell fromCell = GetCell((int)cell.gridPosition.x, i);
                Cell targetCell = GetCell((int)cell.gridPosition.x, i - 1);
                if (fromCell.currentGem)
                {
                    fromCell.currentGem.MoveTo(targetCell.GetWorldPosition());
                    targetCell.SetGem(fromCell.currentGem);
                    targetCell.currentGem.SetCell(targetCell);
                    fromCell.SetGem(null);
                }
                else
                {
                    break;
                }
            }
        }
        */

        Cell targetCell;
        for (int x = Grid.GetLength(0) - 1; x > 0; x--)
        {
            for (int y = Grid.GetLength(1) - 1; y > 0; y--)
            {
                targetCell = GetNextEmptyCell(x, y);
            }
        }
    }

    Cell GetNextEmptyCell(int x, int y)
    {
        Cell targetCell = null;
        if(IsCellEmptyUnderMe(x, y))
        {
            Cell nextCell = GetCell(x, y - 1);
            targetCell = GetNextEmptyCell((int)nextCell.gridPosition.x, (int)nextCell.gridPosition.y);
        }

        return targetCell;
    }

    bool IsCellEmptyUnderMe(int x, int y)
    {
        Cell c = GetCell(x, y - 1);
        return c.IsEmpty();
    }

    Gem CreateRandomGem()
    {
        return GameObject.Instantiate(Gems[UnityEngine.Random.Range(0, Gems.Length)]);
    }

    private void ClickedOnGem(Gem g)
    {
        if (!currentMove.IsFirstSelection())
        {
            //check if they are adjacents

            //swap
            currentMove.AddSelection(g);
            currentMove.Swap();
        }
        else
        {
            //currentGemSelection = g;
            currentMove.AddSelection(g);
        }
    }

    void Update()
    {
        DrawGrid();
    }

    void DrawGrid()
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
        if(GetCell(x, y))
        {
            GetCell(x, y).SetGem(g);
        }
        else
        {
            Debug.Log("Couldn't add Gem to Cell " + x + " " + y + " Cell is Null");
        }
    }

    public void RemoveGemFromCell(int x, int y)
    {
        Cell cell = GetCell(x, y);

        if (cell)
        {
            cell.RemoveGem();
        }
    }

    public void RemoveGemFromCell(Cell cell)
    {
        RemoveGemFromCell((int)cell.gridPosition.x, (int)cell.gridPosition.y);
    }

    public Cell GetCell(int x, int y)
    {
        return Grid[x, y];
    }
}
