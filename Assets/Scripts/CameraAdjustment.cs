using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjustment : MonoBehaviour
{
    public SpriteRenderer bg;

    void Start()
    {
        float orthoSize = ((bg.bounds.size.x * Screen.height) / Screen.width) / 2;
        GetComponent<Camera>().orthographicSize = orthoSize;
    }
}