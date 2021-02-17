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
    Vector3 targetPosition;
    Vector3 startPosition;
    float lerpDuration = 2.0f;//0.3f;
    float timeElapsed;

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
            if (timeElapsed < 1.0f)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed);
                timeElapsed += Time.deltaTime / lerpDuration;
            }
            else
            {
                transform.position = targetPosition;
                isMoving = false;
                timeElapsed = 0.0f;
                OnMovementEnd?.Invoke(this);
            }
        }
    }

    public void MoveTo(Vector3 targetPos)
    {
        isMoving = true;
        startPosition = transform.position;
        targetPosition = targetPos;
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
