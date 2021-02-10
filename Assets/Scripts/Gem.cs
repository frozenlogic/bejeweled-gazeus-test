using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public float Speed;
    public GemType gemType;
    public Cell currentCell;

    bool isMoving = false;
    Vector3 moveToPosition;
    float distance;
    float startTime;

    Sprite sprite;

    public UnityEvent<Gem> OnClickedOnGem = new UnityEvent<Gem>();

    private void Awake()
    {
        sprite = GetComponent<Sprite>();
    }

    // Start is called before the first frame update
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, moveToPosition, (((Time.time - startTime) * Speed )/ distance));
        }
    }

    public void MoveTo(Vector3 position)
    {
        isMoving = true;
        distance = Vector3.Distance(transform.position, position);
        startTime = Time.time;
        moveToPosition = position;
    }

    public float GetSize()
    {
        return sprite.bounds.size.x; //considering Gem sprite is always square
    }

    private void OnMouseUp()
    {
        OnClickedOnGem?.Invoke(this);

        //play "selected" animation 
    }
}
