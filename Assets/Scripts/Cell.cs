using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Gem currentGem;
    public float size;
    public float padding;

    public Vector2 gridPosition { private set; get; }

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
        currentGem.transform.position = transform.position;
    }

    public void RemoveGem()
    {
        Destroy(currentGem.gameObject);
        currentGem = null;
    }

    public void SetWorldPositionInGrid(int row, int col, Vector3 pos)
    {
        transform.position = (new Vector3(row, col, 0) * GetSize()) + pos;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public void SetGridPosition(int x, int y)
    {
        gridPosition = new Vector2(x, y);
    }
}
