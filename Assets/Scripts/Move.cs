using System.Collections;
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
        if (selection.Count > 0)
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
