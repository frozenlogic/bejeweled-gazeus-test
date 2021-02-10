using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GemType
{
    Blue,
    Green,
    Purple,
    Red,
    Yellow
}

public class Gem : MonoBehaviour
{
    public int ScoreValue;
    public GemType gemType;
    public Cell currentCell;

    Sprite sprite;

    private void Awake()
    {
        sprite = GetComponent<Sprite>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float GetSize()
    {
        return sprite.bounds.size.x; //considering Gem sprite is always square
    }

    private void OnMouseUp()
    {
        Debug.Log("UP");
    }
}
