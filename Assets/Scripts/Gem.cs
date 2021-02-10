using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float GetSize()
    {
        return sprite.bounds.size.x; //considering Gem sprite is always square
    }
}
