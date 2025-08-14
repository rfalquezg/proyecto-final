using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float direction;
    public float speed;
    public float bounceForce;
    public GameObject explosionPrefab;
    bool colision;


    Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        speed *= direction;
        rb2D.linearVelocity = new Vector2(speed, 0);
    }

    void Update()
    {

      
        rb2D.linearVelocity = new Vector2(speed, rb2D.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        colision = true;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.HitFireBall();
            //Destroy(gameObject);
            Explode(collision.GetContact(0).point);
        }
        else
        {
            Vector2 sidePoint = collision.GetContact(0).normal;
            // Debug.Log("Side point: " + sidePoint); // Opcional

            if (Mathf.Abs(sidePoint.x) > 0.01f) // Colisión lateral
            {
                //Destroy(gameObject);
                Explode(collision.GetContact(0).point);
            }
            else if (sidePoint.y > 0) // Colisión por abajo
            {
                rb2D.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
            else if (sidePoint.y < 0) // Colisión por arriba
            {
                rb2D.AddForce(Vector2.down * bounceForce, ForceMode2D.Impulse);
            }
            else
            {
                //Destroy(gameObject);
                Explode(collision.GetContact(0).point);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (colision)
        {
            colision = false;
        }
        else
        {
            Explode(collision.GetContact(0).point);
        }
    }
    void Explode(Vector2 point)
    {
        AudioManager.Instance.PlayBump();
        Instantiate(explosionPrefab, point, Quaternion.identity);
        Destroy(gameObject);
    }
}
