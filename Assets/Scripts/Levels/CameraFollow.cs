using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followAhead = 2.5f;

    public Transform limitLeft;
    public Transform limitRight;

    public Transform colLeft;
    public Transform colRight;
    public bool canMove;

    float camWidth;
    float minPosX;
    float maxPosX;
    public float lastPos;
    public float lowestPoint;

    void Start()
    {
        // Ancho visible de la cámara ortográfica
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;

        // Límites de movimiento de la cámara considerando su ancho
        minPosX = limitLeft.position.x + camWidth;
        maxPosX = limitRight.position.x - camWidth;
        lastPos = minPosX;

        // Posición inicial de los colisionadores laterales
        colLeft.position = new Vector2(transform.position.x - camWidth - 0.5f, colLeft.position.y);
        colRight.position = new Vector2(transform.position.x + camWidth + 0.5f, colRight.position.y);
    }

    void Update()
    {
        if (target != null && canMove)
        {
            float newPosX = target.position.x + followAhead;
            newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX);
            transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
            lastPos = newPosX;
        }

    }
    public void StartFollow(Transform t)
    {
        target = t;
        float newPosX = target.position.x + followAhead;
        newPosX = Mathf.Clamp(newPosX, lastPos, maxPosX);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
        lastPos = newPosX;
        canMove = true;
        SearchHeightPos();
    }
    void SearchHeightPos()
    {
        bool foundPos = false;
        float checkPosition = lowestPoint;

        do
        {
            if (target.position.y < checkPosition + Camera.main.orthographicSize
                && target.position.y > checkPosition - Camera.main.orthographicSize)
            {
                transform.position = new Vector3(transform.position.x, checkPosition, transform.position.z);
                foundPos = true;
            }
            else
            {
                checkPosition += Camera.main.orthographicSize * 2;
            }
        } while (!foundPos);
    }
    public void UpdateMaxPos(float newMaxLimit)
    {
        maxPosX = newMaxLimit - camWidth;
    }
}
