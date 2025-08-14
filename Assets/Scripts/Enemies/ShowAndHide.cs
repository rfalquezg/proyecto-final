using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAndHide : MonoBehaviour
{
    public GameObject objectToMove;
    public Transform showPoint;
    public Transform hidePoint;

    public float waitForShow;
    public float waitForHide;

    public float speedShow;
    public float speedHide;

    public LayerMask playerLayer;
    public Vector2 checkSize = new Vector2(1f, 0.5f); // Ajustable en el Inspector
    public Vector2 checkOffset = new Vector2(0f, 0.6f); // Posición del área respecto a la planta

    float timerShow;
    float timerHide;

    float speed;
    Vector2 targetPoint;

    void Start()
    {
        targetPoint = hidePoint.position;
        speed = speedHide;
        timerHide = 0;
        timerShow = 0;
    }

    void Update()
    {
        objectToMove.transform.position = Vector2.MoveTowards(objectToMove.transform.position, targetPoint, speed * Time.deltaTime);

        if (Vector2.Distance(objectToMove.transform.position, hidePoint.position) < 0.1f)
        {
            timerShow += Time.deltaTime;

            if (timerShow >= waitForShow && !Locked())
            {
                targetPoint = showPoint.position;
                speed = speedShow;
                timerHide = 0;
            }
        }
        else if (Vector2.Distance(objectToMove.transform.position, showPoint.position) < 0.1f)
        {
            timerHide += Time.deltaTime;
            if (timerHide >= waitForHide)
            {
                targetPoint = hidePoint.position;
                speed = speedHide;
                timerShow = 0;
            }
        }
    }

    bool Locked()
    {
        Vector2 checkPosition = (Vector2)transform.position + checkOffset;
        Collider2D mario = Physics2D.OverlapBox(checkPosition, checkSize, 0f, playerLayer);

        // OPCIONAL: imprime para confirmar
        // Debug.Log("Mario está encima? " + (mario != null));

        return mario != null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 checkPosition = (Vector2)transform.position + checkOffset;
        Gizmos.DrawWireCube(checkPosition, checkSize);
    }
}
