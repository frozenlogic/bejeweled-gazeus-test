using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//stores the current selected gem
public class Move
{
    public Gem currentGemSelection;
    public Gem previousGemSelection;

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

//Organizes all gems and keep the current move
public class GridSystem : MonoBehaviour
{
    public MoveValidator moveValidator;

    public int height;
    public int width;

    public Cell CellPrefab;
    public Gem[] Gems;

    public Gem currentGemSelection;

    public Move currentMove;

    public Cell[,] Grid { private set; get; }

    public UnityEvent OnGridChanged = new UnityEvent(); //something has changed in the Grid - pieces were swaped

    // Start is called before the first frame update
    void Start()
    {

        Grid = new Cell[width, height];

        currentMove = new Move();
        currentMove.OnMoveEnd.AddListener(MoveEnd);

        moveValidator.OnMoveValidated.AddListener(AfterMoveValidated);

        Fill();
    }

    private void MoveEnd()
    {
        OnGridChanged?.Invoke(); //notifiy the listeners the Grid has new pieces in place
    }

    private void AfterMoveValidated(List<Gem> matchesList)
    {
        if (matchesList.Count > 0)
        {
            foreach (Gem g in matchesList)
            {
                Debug.Log(g);
                g.gameObject.SetActive(false);
            }
        }
        else
        {
            currentMove.Swap();
        }
    }

    void Fill()
    {
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                if(!GetCell(i, j))
                {
                    Cell newCell = GameObject.Instantiate(CellPrefab);
                    newCell.SetWorldPositionInGrid(i, j, transform.position);
                    Gem g = GameObject.Instantiate(Gems[UnityEngine.Random.Range(0, Gems.Length)]);
                    g.OnClickedOnGem.AddListener(ClickedOnGem);
                    newCell.SetGem(g);
                    g.SetCell(newCell);
                    Grid[i, j] = newCell;
                }
            }
        }
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

    public Cell GetCell(int x, int y)
    {
        return Grid[x, y];
    }
}
