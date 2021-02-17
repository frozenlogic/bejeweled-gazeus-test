using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Gem currentGem;
    public float size;
    public float padding;
    public Sprite spriteDark;
    public Sprite spriteLight;

    public Vector2 gridPosition { private set; get; }
    
    public void SetSprite(bool isDark)
    {
        GetComponent<SpriteRenderer>().sprite = isDark ? spriteDark : spriteLight;
    }

    public float GetSize()
    {
        return size + padding; 
    }

    public void SetGem(Gem g)
    {
        currentGem = g;
        if (currentGem)
        {
            currentGem.transform.position = transform.position;
        }
    }

    public void RemoveGem()
    {
        Debug.Log("Removing Gem on Cell " + gridPosition.x + ", " + gridPosition.y);
        Destroy(currentGem.gameObject);
        currentGem = null;
    }

    public void SetWorldPosition(int row, int col, Vector3 pos)
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

    public bool IsEmpty()
    {
        return currentGem == null ? true : false;
    }
}
