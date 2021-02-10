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
        Validate();
    }

    void Validate()
    {
        //check vertically
        for (int i = 0; i < gridSystem.grid.GetLength(0); i++)
        {
            for (int j = 0; j < gridSystem.grid.GetLength(1); j++)
            {
                Gem gem01 = gridSystem.GetCell(i, j).currentGem;
                Gem gem02 = gridSystem.GetCell(i, j + 1).currentGem;

                if (CheckMatch(gem01, gem02))
                {
                    matchesList.Add(gem01);
                    matchesList.Add(gem02);
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
