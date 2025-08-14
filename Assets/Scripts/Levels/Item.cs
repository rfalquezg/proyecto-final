using System.Collections;
using UnityEngine;

public enum ItemType { MagicMushroom, FireFlower, Coin, Life, Star }

public class Item : MonoBehaviour
{
    public int points;
    public ItemType type;
    bool isCatched = false;
    public Vector2 startVelocity;

    private AutoMovement autoMovement;
    private Collider2D myCollider;
    public GameObject floatPointsPrefab;

    private void Awake()
    {
        autoMovement = GetComponent<AutoMovement>();
        myCollider = GetComponent<Collider2D>();
        if (type == ItemType.Star && myCollider != null)
        {
            myCollider.enabled = false;
        }
        else
        {
            myCollider.enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCatched) return;

        Mario mario = collision.collider.GetComponent<Mario>();
        if (mario != null)
        {
            isCatched = true;
            mario.CatchItem(type);
            //Destroy(gameObject);
            CatchItem();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCatched) return;
        if (myCollider != null && !myCollider.enabled) return;

        Mario mario = collision.GetComponent<Mario>();
        if (mario != null)
        {
            isCatched = true;
            mario.CatchItem(type);
            //Destroy(gameObject);
            CatchItem();
        }
    }

    public void WaitMove()
    {
        if (autoMovement != null)
            autoMovement.enabled = false;

        if (type == ItemType.Star && myCollider != null)
            myCollider.enabled = false;
    }

    public void StartMove()
    {
        if (autoMovement != null)
        {
            autoMovement.enabled = true;
        }
        else if (startVelocity != Vector2.zero)
        {
            GetComponent<Rigidbody2D>().linearVelocity = startVelocity;
        }

        // ✅ Activar collider solo si es estrella
        if (type == ItemType.Star && myCollider != null)
            myCollider.enabled = true;

    }
    public void HitBelowBlock()
    {
        if (autoMovement != null && autoMovement.enabled)
        {
            autoMovement.ChangeDirection();
        }
    }
    void CatchItem()
    {

        if (floatPointsPrefab != null)
        {
            ScoreManager.Instance.SumarPuntos(points);
            GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
            FloatPoints floatPoints = newFloatPoints.GetComponent<FloatPoints>();
            floatPoints.numPoints = points;
        }
        Destroy(gameObject);
    }
}
