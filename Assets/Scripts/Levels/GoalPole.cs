using UnityEngine;

public class GoalPole : MonoBehaviour
{
    //public GameObject pointPrefab;
    public Transform flag;
    public Transform bottom;
    public float flagVelocity = 5f;

    bool downFlag;
    Mover mover;
    private bool triggered = false;
    public GameObject floatPointsPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        Mario mario = collision.GetComponent<Mario>();
        if (mario != null)
        {
            triggered = true;
            mario.Goal();
            downFlag = true;

            mover = collision.GetComponent<Mover>();
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            //Instantiate(pointPrefab, contactPoint, Quaternion.identity);

            // Calcula la altura desde donde tocó el palo
            CalculateHeight(contactPoint.y);
        }
    }

    private void FixedUpdate()
    {
        if (downFlag)
        {
            if (flag.position.y > bottom.position.y)
            {
                flag.position = new Vector2(
                    flag.position.x,
                    flag.position.y - (flagVelocity * Time.fixedDeltaTime)
                );
            }
            else
            {
                mover.isFlagDown = true;
            }
        }
    }

    void CalculateHeight(float marioPosition)
    {
        float size = GetComponent<BoxCollider2D>().bounds.size.y;
        //Debug.Log("Total size: " + size);

        float minPosition1 = transform.position.y + (size - size / 5f); // 5000
        //Debug.Log("Min Position 1: " + minPosition1);

        float minPosition2 = transform.position.y + (size - 2 * size / 5f); // 2000
        //Debug.Log("Min Position 2: " + minPosition2);

        float minPosition3 = transform.position.y + (size - 3 * size / 5f); // 800
        //Debug.Log("Min Position 3: " + minPosition3);

        float minPosition4 = transform.position.y + (size - 4 * size / 5f); // 400
        int numPoints = 0;
        if (marioPosition >= minPosition1)
        {
            numPoints = 5000;
            //ScoreManager.Instance.SumarPuntos(5000);
        }
        else if (marioPosition >= minPosition2)
        {
            numPoints = 2000;
            //ScoreManager.Instance.SumarPuntos(2000);
        }
        else if (marioPosition >= minPosition3)
        {
            numPoints = 800;
            //ScoreManager.Instance.SumarPuntos(800);
        }
        else if (marioPosition >= minPosition4)
        {
            numPoints = 400;
            //ScoreManager.Instance.SumarPuntos(400);
        }
        else
        {
            numPoints = 100;
            //ScoreManager.Instance.SumarPuntos(100);
        }
        ScoreManager.Instance.SumarPuntos(numPoints);

        Vector2 positionFloatPoints = new Vector2(transform.position.x + 0.65f, bottom.position.y);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, positionFloatPoints, Quaternion.identity);
        FloatPoints floatPoints = newFloatPoints.GetComponent<FloatPoints>();
        floatPoints.numPoints = numPoints;
        floatPoints.speed = flagVelocity;
        floatPoints.distance = flag.position.y - bottom.position.y;
        floatPoints.destroy = false;
    } 
}
