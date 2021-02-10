using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Gem currentGem;
    public float size = 128;

    private void Awake()
    {
        //size = currentGem.GetSize(); //cell size = gem sprite size
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public float GetSize()
    {
        return size; 
    }

    public void SetWorldPositionInGrid(int row, int col, Vector3 pos)
    {
        transform.position = (new Vector3(row, col, 0) * size) + pos;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
}
