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
    public Cell currentCell { private set; get; }

    bool isMoving = false;
    Vector3 moveToPosition;
    float distance;
    float startTime;

    Sprite sprite;

    public UnityEvent<Gem> OnClickedOnGem = new UnityEvent<Gem>();
    public UnityEvent<Gem> OnMovementEnd = new UnityEvent<Gem>();

    private void Awake()
    {
        sprite = GetComponent<Sprite>();
    }

    // Start is called before the first frame update
    void Update()
    {
        if (isMoving)
        {
            float t = ((Time.time - startTime) * Speed) / distance;
            if(t < 1.0f)
            {
                transform.position = Vector3.Lerp(transform.position, moveToPosition, t);
            }
            else
            {
                isMoving = false;
                OnMovementEnd?.Invoke(this);
            }
        }
    }

    public void MoveTo(Vector3 position)
    {
        isMoving = true;
        distance = Vector3.Distance(transform.position, position);
        startTime = Time.time;
        moveToPosition = position;
    }

    public Vector3 GetSize()
    {
        return sprite.bounds.extents;
    }

    private void OnMouseUp()
    {
        OnClickedOnGem?.Invoke(this);

        //play "selected" animation 
    }

    public void SetCell(Cell cell)
    {
        currentCell = cell;
    }
}
