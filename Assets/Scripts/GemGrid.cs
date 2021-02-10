using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGrid : MonoBehaviour, ISerializationCallbackReceiver
{
    public int height;
    public int width;

    int[,] grid;

    public void OnAfterDeserialize()
    {
        grid = new int[width, height];
    }

    public void OnBeforeSerialize()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
