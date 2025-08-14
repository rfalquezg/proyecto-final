using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    public GameObject floatPointsPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.AddCoins();
        AudioManager.Instance.PlayCoin();
        ScoreManager.Instance.SumarPuntos(200);
        Vector2 postionFloatPoints = new Vector2(transform.position.x, transform.position.y + 1f);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
        FloatPoints floatPoints = newFloatPoints.GetComponent<FloatPoints>();
        floatPoints.numPoints = 200;
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
        StartCoroutine(Animation());

    }

    IEnumerator Animation()
    {
        float time = 0;
        float duration = 0.25f;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + (Vector2.up * 2f);

        // Movimiento hacia arriba
        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // Movimiento de regreso
        time = 0;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(targetPosition, startPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;

        Destroy(gameObject);
    }
}
