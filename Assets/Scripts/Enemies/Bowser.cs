using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowser : Enemy
{
    public int health = 5;
    public float fallSpeed = 5f;
    public bool isDead;
    public float speed = 2f;
    public float minJumpTime = 1f;
    public float maxJumpTime = 5f;
    public float jumpForce = 8f;

    float jumpTimer;
    float direction = -1;
    bool canMove;
    public float minDistanceToMove = 10f;

    public bool collapseBridge;

    bool pendingJump;

    public GameObject firePrefab;
    public Transform shootPos;
    public float minShotTime = 1f;
    public float maxShotTime = 5f;
    float shotTimer;
    public float minDistanceToShot = 50f;
    bool canShoot;

    protected override void Start()
    {
        base.Start();
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
        shotTimer = Random.Range(minShotTime, maxShotTime);
        canMove = false;
        canShoot = false;

        rb2D.bodyType = RigidbodyType2D.Dynamic;
        rb2D.simulated = true;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (rb2D.gravityScale <= 0f) rb2D.gravityScale = 2f;
    }

    protected override void Update()
    {
        if (!canMove && Mathf.Abs(Mario.Instance.transform.position.x - transform.position.x) <= minDistanceToMove)
            canMove = true;

        if (!canShoot && Mathf.Abs(Mario.Instance.transform.position.x - transform.position.x) <= minDistanceToShot)
            canShoot = true;

        if (!isDead && canMove)
        {
            float dx = Mario.Instance.transform.position.x - transform.position.x;
            if (dx > 0.1f) direction = 1f;
            else if (dx < -0.1f) direction = -1f;

            if (direction == 1f) transform.localScale = new Vector3(-1, 1, 1);
            else transform.localScale = Vector3.one;

            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0f)
            {
                pendingJump = true;
                jumpTimer = Random.Range(minJumpTime, maxJumpTime);
            }
        }

        if (canShoot && !isDead)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0f)
            {
                Shoot();
            }
        }
    }

    void FixedUpdate()
    {
        if (isDead || !canMove) return;

        if (rb2D.bodyType != RigidbodyType2D.Dynamic) rb2D.bodyType = RigidbodyType2D.Dynamic;
        if (!rb2D.simulated) rb2D.simulated = true;
        if (rb2D.gravityScale <= 0f) rb2D.gravityScale = 2f;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        Vector2 v = rb2D.linearVelocity;
        v.x = direction * speed;
        rb2D.linearVelocity = v;
        rb2D.MovePosition(rb2D.position + new Vector2(direction * speed, 0f) * Time.fixedDeltaTime);

        if (pendingJump)
        {
            pendingJump = false;
            rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    void Shoot()
    {
        GameObject fire = Instantiate(firePrefab, shootPos.position, Quaternion.identity);
        fire.GetComponent<BowserFire>().direction = direction;
        shotTimer = Random.Range(minShotTime, maxShotTime);
    }

    public void FallBridge()
    {
        if (isDead) return;
        isDead = true;
        FlipDie();
    }

    public override void Stomped(Transform player)
    {
        player.GetComponent<Mario>().Hit();
    }
    public override void HitRollingShell() { }
    public override void HitBelowBlock() { }

    public override void HitFireBall()
    {
        rb2D.linearVelocity = Vector2.zero;
        health--;
        if (health <= 0 && !isDead)
        {
            isDead = true;
            FlipDie();
        }
    }

    public override void HitStarman() { }
}
