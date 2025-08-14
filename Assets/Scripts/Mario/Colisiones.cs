using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colisiones : MonoBehaviour
{
    [Header("Ground")]
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.25f;
    public LayerMask groundLayer;

    [Header("Side Collisions (detecta por ÁNGULO, no por capa)")]
    [Tooltip("Distancia del cast lateral (corto)")]
    [SerializeField] float sideCastDistance = 0.06f;
    [Tooltip("Umbral horizontal de la normal para considerar pared")]
    [SerializeField] float horizontalNormalThreshold = 0.9f;
    [Tooltip("Si estás en el piso, ignora contactos con normal y>=")]
    [SerializeField] float groundNormalYIgnore = 0.6f; 

    BoxCollider2D col2D;
    Mario mario;
    Mover mover;

    // Para el Cast
    ContactFilter2D sideFilter;                    
    readonly RaycastHit2D[] castHits = new RaycastHit2D[8];

    void Awake()
    {
        col2D = GetComponent<BoxCollider2D>();
        mario  = GetComponent<Mario>();
        mover  = GetComponent<Mover>();

    
        sideFilter = new ContactFilter2D
        {
            useTriggers = false,
            useLayerMask = false,                  
            useDepth = false
        };
    }

    public bool Grounded() => isGrounded;

    void FixedUpdate()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
        else
            isGrounded = false;
    }

    public bool CheckCollision(int direction)
    {
        if (direction == 0 || col2D == null) return false;

        Vector2 dir = (direction > 0) ? Vector2.right : Vector2.left;

        int hits = col2D.Cast(dir, sideFilter, castHits, sideCastDistance);
        for (int i = 0; i < hits; i++)
        {
            Vector2 n = castHits[i].normal;


            if (isGrounded && n.y >= groundNormalYIgnore) continue;
            bool esHorizontal = Mathf.Abs(n.x) >= horizontalNormalThreshold;
            bool contraMarcha = Mathf.Sign(-n.x) == Mathf.Sign(direction);
            if (esHorizontal && contraMarcha) return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (col2D == null) return;

        // Línea indicativa de cast (derecha/izquierda) a media altura
        Vector2 c = col2D.bounds.center;
        float  h = col2D.bounds.extents.y * 0.4f;
        Vector2 r0 = new Vector2(c.x + col2D.bounds.extents.x - 0.001f, c.y + h);
        Vector2 l0 = new Vector2(c.x - col2D.bounds.extents.x + 0.001f, c.y + h);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(r0, r0 + Vector2.right * sideCastDistance);
        Gizmos.DrawLine(l0, l0 + Vector2.left  * sideCastDistance);

        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (mario.isInvincible)
                collision.gameObject.GetComponent<Enemy>()?.HitStarman();
            else
                mario.Hit();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Lava"))
        {
            if (!mario.isInvincible)
            {
                mario.Hit();
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("DamagePlayer"))
        {
            if (!mario.isInvincible)
            {
                mario.Hit();
            }
        }
    }

    public void Dead()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");
        foreach (Transform t in transform)
            t.gameObject.layer = LayerMask.NameToLayer("PlayerDead");
    }
    public void Respawn()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        foreach (Transform t in transform)
        {
            t.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }


    public void HurtCollision(bool activate)
    {
        if (activate)
        {
            gameObject.layer = LayerMask.NameToLayer("OnlyGround");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
            transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (mario.isInvincible) enemy.HitStarman();
            else if (collision.CompareTag("Plant")) mario.Hit();
            else { enemy.Stomped(transform); mover.BounceUp(); }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform") && isGrounded)
        {
            transform.parent = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
    }



    public void StompBlock()
    {
        Collider2D c = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (c != null && c.gameObject.CompareTag("Block"))
            c.gameObject.GetComponent<Block>().BreakFromTop();
    }
}
