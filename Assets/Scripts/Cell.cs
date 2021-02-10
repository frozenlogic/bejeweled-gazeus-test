using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Gem currentGem;
    public float size;
    public float padding;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public float GetSize()
    {
        return size + padding; 
    }

    public void SetGem(Gem g)
    {
        currentGem = g;
        g.currentCell = this;
        currentGem.transform.position = transform.position;
    }

    public void SetWorldPositionInGrid(int row, int col, Vector3 pos)
    {
        transform.position = (new Vector3(row, col, 0) * GetSize()) + pos;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}
