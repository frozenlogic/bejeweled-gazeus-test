using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveValidator : MonoBehaviour
{
    public GridSystem gridSystem;

    public List<Gem> matchesList;

    public UnityEvent<List<Gem>> OnMoveValidated = new UnityEvent<List<Gem>>();

    // Start is called before the first frame update
    void Start()
    {
        matchesList = new List<Gem>();

        gridSystem.OnGridChanged.AddListener(GridHasChanged);
    }

    void GridHasChanged()
    {
        ResolveGrid();
    }

    public bool ResolveGrid()
    {
        matchesList.Clear();

        //vertical
        for (int x = 0; x < gridSystem.Grid.GetLength(0); x++)
        {
            for (int y = 0; y < gridSystem.Grid.GetLength(1); y++)
            {
                if (y + 1 >= gridSystem.Grid.GetLength(1))
                {
                    continue;
                }

                ValidateVertical(x, y, gridSystem.Grid.GetLength(1));
            }
        }

        //horizontal
        for (int y = 0; y < gridSystem.Grid.GetLength(1); y++)
        {
            for (int x = 0; x < gridSystem.Grid.GetLength(0); x++)
            {
                if (x + 1 >= gridSystem.Grid.GetLength(0))
                {
                    continue;
                }

                ValidateHorizontal(x, y, gridSystem.Grid.GetLength(0));
            }
        }

        Debug.Log("Matches " + matchesList.Count);

        OnMoveValidated?.Invoke(matchesList);

        return matchesList.Count > 0 ? true : false;
    }

    void ValidateHorizontal(int x, int y, int length)
    {
        Gem gem01 = gridSystem.GetCell(x, y).currentGem;
        Gem gem02 = gridSystem.GetCell(x + 1, y).currentGem;

        if (CheckMatch(gem01, gem02))
        {
            if (x + 2 < length)
            {
                Gem g = gridSystem.GetCell(x + 2, y).currentGem;
                if (CheckMatch(gem02, g))
                {
                    if (!matchesList.Contains(gem01))
                    {
                        matchesList.Add(gem01);
                    }
                    if (!matchesList.Contains(gem02))
                    {
                        matchesList.Add(gem02);
                    }
                    if (!matchesList.Contains(g))
                    {
                        matchesList.Add(g);
                    }
                }
            }
        }
    }

    void ValidateVertical(int x, int y, int length)
    {
        Gem gem01 = gridSystem.GetCell(x, y).currentGem;
        Gem gem02 = gridSystem.GetCell(x, y + 1).currentGem;

        if (CheckMatch(gem01, gem02))
        {
            if (y + 2 < length)
            {
                Gem g = gridSystem.GetCell(x, y + 2).currentGem;
                if (CheckMatch(gem02, g))
                {
                    if (!matchesList.Contains(gem01))
                    {
                        matchesList.Add(gem01);
                    }
                    if (!matchesList.Contains(gem02))
                    {
                        matchesList.Add(gem02);
                    }
                    if (!matchesList.Contains(g))
                    {
                        matchesList.Add(g);
                    }
                }
            }
        }
    }

    bool CheckMatch(Gem gem01, Gem gem02)
    {
        if(gem01.gemType == gem02.gemType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
