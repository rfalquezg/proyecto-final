using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowserFire : MonoBehaviour
{
    public float direction;
    public float speed = 10f;
    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        speed *= direction;
        transform.localScale = new Vector3(-direction, 1, 1);
        rb2D.linearVelocity = new Vector2(speed, 0);
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        while (true)
        {
            spriteRenderer.flipY = !spriteRenderer.flipY;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
