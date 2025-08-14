using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public float speed = 1f;
    bool movementPaused;

    Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;
    Vector2 lastVelocity;
    Vector2 currentDirection;
    float defaultSpeed;
    Collider2D col2D;
    public bool avoidFall;
    public LayerMask groundLayer;
    public bool isFall;
    public bool useWaysPoints;
    public Transform[] waypoints;
    int targetWayPoint = 0;


    public bool flipSprite = true;
    float timer = 0;
    bool hasBeenVisible;
    public AutoMovement partner;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        // rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
        defaultSpeed = Mathf.Abs(speed);
        rb2D.isKinematic = true;
        movementPaused = true;
    }

    public void Activate()
    {
        hasBeenVisible = true;
        if (!useWaysPoints)
        {
            rb2D.isKinematic = false;
            rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
        }
        movementPaused = false;
        if (partner != null)
        {
            partner.Activate();
        }
    }

    private void Update()
    {
        if (spriteRenderer.isVisible && !hasBeenVisible)
        {
            Activate();
        }
    }

    private void FixedUpdate()
    {
        if (!movementPaused)
        {
            if (useWaysPoints)
            {
                Vector3 direction = waypoints[targetWayPoint].position - transform.position;
                rb2D.linearVelocity = Mathf.Abs(speed) * direction.normalized;

                float distanceToTarget = Vector2.Distance(transform.position, waypoints[targetWayPoint].position);
                if (distanceToTarget < 0.1f)
                {
                    targetWayPoint++;
                    if (targetWayPoint >= waypoints.Length)
                    {
                        targetWayPoint = 0;
                    }
                }
            }
            else
            {
                if (isFall)
                {
                    if (CheckGrounded())
                    {
                        isFall = false;
                    }
                }
                else
                {
                    if (CheckSideCollision())
                    {
                        ChangeDirection();
                    }
                    else if (avoidFall && !CheckGrounded())
                    {
                        ChangeDirection();
                    }
                    else
                    {
                        CheckTimeStopped();
                    }
                    rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
                }
            }
           
            if (flipSprite)
            {
                if (rb2D.linearVelocity.x > 0)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
        }
    }

    public void PauseMovement()
    {
        if (!movementPaused)
        {
            if (!useWaysPoints)
            {
                currentDirection = rb2D.linearVelocity.normalized;
                lastVelocity = rb2D.linearVelocity;
            }
           
            movementPaused = true;
            rb2D.linearVelocity = new Vector2(0, 0);
        }
    }

    public void ContinueMovement()
    {
        if (movementPaused)
        {
            if (!useWaysPoints)
            {
                speed = defaultSpeed * currentDirection.x;
                rb2D.linearVelocity = new Vector2(speed, lastVelocity.y);
            }
            
            movementPaused = false;
        }
    }

    public void ContinueMovement(Vector2 newVelocity)
    {
        if (movementPaused)
        {
            if (!useWaysPoints)
            {
                rb2D.linearVelocity = newVelocity;
            }
            
            movementPaused = false;
        }
    }

    public void ChangeDirection()
    {
        speed = -speed;
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
        timer = 0;
    }

    bool CheckGrounded()
    {
        if (isFall)
        {
            Vector2 center = new Vector2(col2D.bounds.center.x, col2D.bounds.center.y);

            if (Physics2D.Raycast(center, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (speed > 0)
        {
            Vector2 footRight = new Vector2(col2D.bounds.center.x + col2D.bounds.extents.x, col2D.bounds.center.y);
            if (Physics2D.Raycast(footRight, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (speed < 0)
        {
            // FIX: para la izquierda debe ser center.x - extents.x (antes estaba con '+')
            Vector2 footLeft = new Vector2(col2D.bounds.center.x - col2D.bounds.extents.x, col2D.bounds.center.y);
            if (Physics2D.Raycast(footLeft, Vector2.down, col2D.bounds.extents.y * 1.25f, groundLayer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    bool CheckSideCollision()
    {
        Vector3 direction = Vector3.right * speed;
        return Physics2D.OverlapBox(
            col2D.bounds.center + direction.normalized * col2D.bounds.extents.x,
            col2D.bounds.size * 0.2f,
            0,
            groundLayer
        );
    }

    void CheckTimeStopped()
    {
        if (Math.Abs(rb2D.linearVelocity.x) < 0.1f)
        {
            if (timer > 0.05f)
            {
                ChangeDirection();
            }
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }
}
