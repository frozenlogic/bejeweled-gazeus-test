using System;
using System.Collections.Generic;
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
            selection[0].MoveTo(selection[1].transform.position, 0.3f);
            selection[1].MoveTo(selection[0].transform.position, 0.3f);

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
                    newCell.name = string.Format("{0} x {1} - Cell", i, j);
                    Gem g = CreateRandomGem();
                    g.OnClickedOnGem.AddListener(ClickedOnGem);
                    newCell.SetGem(g);
                    g.SetCell(newCell);
                    Grid[i, j] = newCell;

                    Debug.Log("Created Cell in" + "[" + i + ", " + j + "]. Created Gem.");
                }
                else
                {
                    if(cell.IsEmpty())
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

        Debug.Log("Grid Filling is Done");
    }

    void MoveCells()
    {
        bool firstToMove = true;
        bool isTopRow = false;
        foreach(Cell cell in emptyCells)//for every empty cell, we move down all gems that are above it
        {
            if (cell.gridPosition.y == 7)
            {
                isTopRow = true;
            }

            for (int i = (int)cell.gridPosition.y + 1; i < Grid.GetLength(1); i++) //iterate over Y
            {
                Cell fromCell = GetCell((int)cell.gridPosition.x, i);
                Cell targetCell = null;
                Cell c = null;
                GetNextEmptyCell((int)cell.gridPosition.x, i, out c);
                targetCell = c;

                if (fromCell.currentGem)
                {
                    fromCell.currentGem.MoveTo(targetCell.GetWorldPosition(), 1.0f);
                    if (firstToMove)
                    {
                        firstToMove = false;
                        fromCell.currentGem.OnMovementEnd.AddListener(GemMovementIsDone);
                    }
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

        emptyCells.Clear();

        Debug.Log("Is Top Row? " + isTopRow);

        if (isTopRow)
        {
            Fill();
            moveValidator.ResolveGrid();
        }
    }

    private void GemMovementIsDone(Gem gem)
    {
        gem.OnMovementEnd.RemoveAllListeners();
        Fill();
        moveValidator.ResolveGrid();
    }

    void GetNextEmptyCell(int x, int y, out Cell c)
    {
        c = null;
        if(IsCellUnderMeEmpty(x, y))
        {
            Cell nextCell = GetCell(x, y - 1);
            GetNextEmptyCell((int)nextCell.gridPosition.x, (int)nextCell.gridPosition.y, out c);
        }
        else
        {
            c = GetCell(x, y);
        }
    }

    bool IsCellAboveMeEmpty(int x, int y)
    {
        bool isEmpty = false;
        if(y + 1 < Grid.GetLength(1))
        {
            Cell c = GetCell(x, y + 1);
            isEmpty = c.IsEmpty();
        }
        return isEmpty;
    }

    bool IsCellUnderMeEmpty(int x, int y)
    {
        bool isEmpty = false;
        if (y - 1 >= 0)
        {
            Cell c = GetCell(x, y - 1);
            isEmpty = c.IsEmpty();
        }
        return isEmpty;
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
            Cell firstSelection = currentMove.selection[0].currentCell;
            Cell secondSelection = g.currentCell;

            Vector2 p = Vector2.Perpendicular((secondSelection.gridPosition - firstSelection.gridPosition).normalized);

            if (p.x == -1 || p.x == 1 || p.y == 1 || p.y == -1)
            {
                Debug.Log("Adjacent");
                //swap
                currentMove.AddSelection(g);
                currentMove.Swap();
            }
            else
            {
                currentMove.Reset();
            }
        }
        else
        {
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
