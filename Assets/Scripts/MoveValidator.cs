using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveValidator : MonoBehaviour
{
    public GridSystem gridSystem;

    List<Gem> matchesList;

    // Start is called before the first frame update
    void Start()
    {
        matchesList = new List<Gem>();

        gridSystem.OnGridChanged.AddListener(LookUpGrid);
    }

    public void LookUpGrid()
    {
        //vertical
        for (int x = 0; x < gridSystem.grid.GetLength(0); x++)
        {
            for (int y = 0; y < gridSystem.grid.GetLength(1); y++)
            {
                if (y + 1 >= gridSystem.grid.GetLength(1))
                {
                    continue;
                }

                ValidateVertical(x, y, gridSystem.grid.GetLength(1));
            }
        }

        //horizontal
        for (int y = 0; y < gridSystem.grid.GetLength(1); y++)
        {
            for (int x = 0; x < gridSystem.grid.GetLength(0); x++)
            {
                if (x + 1 >= gridSystem.grid.GetLength(0))
                {
                    continue;
                }

                ValidateHorizontal(x, y, gridSystem.grid.GetLength(0));
            }
        }

        foreach (Gem g in matchesList)
        {
            Debug.Log(g);
            g.gameObject.SetActive(false);
        }
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
                    matchesList.Add(gem01);
                    matchesList.Add(gem02);
                    matchesList.Add(g);
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
                    matchesList.Add(gem01);
                    matchesList.Add(gem02);
                    matchesList.Add(g);
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
