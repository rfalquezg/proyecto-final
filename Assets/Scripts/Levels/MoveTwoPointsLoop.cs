using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTwoPointsLoop : MonoBehaviour
{
    public enum Direction
    {
        NONE,
        HORIZONTAL,
        VERTICAL
    }
    public Direction direction;
    public Transform startPoint;
    public Transform endPoint;
    public float speed;
    public bool reverse;

    Vector3 currentTarget;
    Vector3 startPos;
    Vector3 endPos;

    void Start()
    {
        //startPos = startPoint.position;
        //endPos   = endPoint.position;

        switch (direction)
        {
            case Direction.NONE:
                startPos = startPoint.position;
                endPos   = endPoint.position;
                break;

            case Direction.HORIZONTAL:
                startPos = new Vector3(startPoint.position.x, transform.position.y, transform.position.z);
                endPos   = new Vector3(endPoint.position.x,   transform.position.y, transform.position.z);
                break;

            case Direction.VERTICAL:
                startPos = new Vector3(transform.position.x,  startPoint.position.y, transform.position.z);
                endPos   = new Vector3(transform.position.x,  endPoint.position.y,   transform.position.z);
                break;
        }

        currentTarget = endPos;
    }


    void Update()
    {
        float fixedSpeed = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, fixedSpeed);

        if (transform.position == currentTarget)
        {
            if (reverse)
            {
                if (currentTarget == startPos)
                {
                    currentTarget = endPos;
                }
                else
                {
                    currentTarget = startPos;
                }
            }
            else
            {
                transform.position = startPos;
            }
        }
    }
}
