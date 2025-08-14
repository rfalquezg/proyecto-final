using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int points;
    protected Animator animator;
    protected AutoMovement autoMovement;

    protected Rigidbody2D rb2D;
    public bool isRolling;
    public GameObject floatPointsPrefab;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        autoMovement = GetComponent<AutoMovement>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start() { }
    protected virtual void Update() { }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == gameObject.layer && autoMovement != null)
        {
            autoMovement.ChangeDirection();
        }
    }

    public virtual void HitFireBall()
    {
        FlipDie();
    }

    public virtual void HitStarman()
    {
        FlipDie();
    }

    public virtual void Stomped(Transform player) { }
    public virtual void HitBelowBlock() { FlipDie(); }
    public virtual void HitRollingShell() { FlipDie(); }

    protected void FlipDie()
    {
        AudioManager.Instance.PlayFlipDie();
        animator.SetTrigger("Flip");

        rb2D.linearVelocity = Vector2.zero;
        rb2D.AddForce(Vector2.up * 6f, ForceMode2D.Impulse);

        if (autoMovement != null) autoMovement.enabled = false;

        var cols = GetComponentsInChildren<Collider2D>(true);
        for (int i = 0; i < cols.Length; i++) cols[i].enabled = false;

        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.simulated = true;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (rb2D.gravityScale <= 0f) rb2D.gravityScale = 2f;

        Dead();
    }

    protected void Dead()
    {
        ScoreManager.Instance.SumarPuntos(points);
        GameObject newFloatPoints = Instantiate(floatPointsPrefab, transform.position, Quaternion.identity);
        FloatPoints floatPoints = newFloatPoints.GetComponent<FloatPoints>();
        floatPoints.numPoints = points;
    }
}
