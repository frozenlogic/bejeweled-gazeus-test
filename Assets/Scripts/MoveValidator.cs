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
        //check vertically
        for (int i = 0; i < gridSystem.grid.GetLength(0); i++)
        {
            for (int j = 0; j < gridSystem.grid.GetLength(1); j++)
            {
                if (j + 1 >= gridSystem.grid.GetLength(1))
                {
                    continue;
                }

                Validate(i, j, gridSystem.grid.GetLength(1));
            }
        }

        foreach (Gem g in matchesList)
        {
            Debug.Log(g);
        }
    }

    void Validate(int x, int y, int length)
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
