using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    enum State { Default = 0, Super = 1, Fire = 2 }
    State currentState = State.Default;

    public GameObject stompBox;

    public Mover mover;
    Colisiones colisiones;
    Animaciones animaciones;
    Rigidbody2D rb2D;
    public GameObject headBox;
    bool isDead;
    public GameObject fireBallPrefab;
    public Transform shootPos;
    public bool isInvincible;
    public float invincibleTime;
    float invincibleTimer;
    public bool isHurt;
    public float hurtTime;
    float hurtTimer;
    public bool isCrouched;
    public static Mario Instance;
    //public bool levelFinished;
    //public HUD hud;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            mover = GetComponent<Mover>();
            colisiones = GetComponent<Colisiones>();
            animaciones = GetComponent<Animaciones>();
            rb2D = GetComponent<Rigidbody2D>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
      
    }

    private void Update()
    {
        isCrouched = false;
        if (!isDead)
        {
            if (rb2D.linearVelocity.y < 0)
            {
                stompBox.SetActive(true);
            }
            else
            {
                if (transform.parent == null)
                {
                    stompBox.SetActive(false);
                }

                
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (colisiones.Grounded())
                {
                    isCrouched = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (isCrouched && currentState != State.Default)
                {
                    colisiones.StompBlock();
                }
                else
                {
                    Shoot();
                }

            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Hit();
            }
            if (isInvincible)
                {
                    invincibleTimer -= Time.deltaTime;
                    if (invincibleTimer <= 0)
                    {
                        isInvincible = false;
                        animaciones.InvincibleMode(false);
                    }
                }
            if (isHurt)
            {
                hurtTimer -= Time.deltaTime;
                if (hurtTimer <= 0)
                {
                    EndHurt();
                }
            }

        }
        animaciones.Crouch(isCrouched);

        if (rb2D.linearVelocity.y > 0 && !isDead)
        {
            headBox.SetActive(true);
        }
        else
        {
            headBox.SetActive(false);
        }

    }

    public void Hit()
    {
        if (!isHurt)
        {
            if (currentState == State.Default)
            {
                Dead(true);
            }
            else
            {
                AudioManager.Instance.PlayPowerDown();
                Time.timeScale = 0;
                animaciones.Hit();
                StartHurt();
            }
        }
    }
    void StartHurt()
    {
        isHurt = true;
        animaciones.Hurt(true);
        hurtTimer = hurtTime;
        colisiones.HurtCollision(true);


    }

    void EndHurt()
    {
        isHurt = false;
        animaciones.Hurt(false);
        colisiones.HurtCollision(false);
    }

    public void Dead(bool BounceUp)
    {
        if (!isDead)
        {
            AudioManager.Instance.PlayDie();
            isDead = true;
            colisiones.Dead();
            mover.Dead(BounceUp);
            animaciones.Dead();
            isInvincible = false;
            GameManager.Instance.LoseLife();
        }

    }
    public void Respawn(Vector2 pos)
    {
        if (isDead)
        {
            animaciones.Reset();
            currentState = State.Default;
        }
        isDead = false;
        colisiones.Respawn();
        mover.Respawn();
        
        transform.position = pos;
    }

    void ChangeState(int newState)
    {
        currentState = (State)newState;
        animaciones.NewState(newState);
        Time.timeScale = 1;

    }
    public void CatchItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.MagicMushroom:
                AudioManager.Instance.PlayPowerUp();
                if (currentState == State.Default)
                {

                    animaciones.PowerUp();
                    Time.timeScale = 0;
                    rb2D.linearVelocity = Vector2.zero;
                }
                break;

            case ItemType.FireFlower:
                AudioManager.Instance.PlayPowerUp();
                if (currentState != State.Fire)
                {

                    animaciones.PowerUp();
                    Time.timeScale = 0;
                    rb2D.linearVelocity = Vector2.zero;
                }
                break;

            case ItemType.Coin:
                AudioManager.Instance.PlayCoin();
                
                
                GameManager.Instance.AddCoins();
                break;

            case ItemType.Life:
                GameManager.Instance.NewLife();
                break;

            case ItemType.Star:
                AudioManager.Instance.PlayPowerUp();
                isInvincible = true;
                animaciones.InvincibleMode(true);
                invincibleTimer = invincibleTime;
                EndHurt();

                break;
        }
    }
    void Shoot()
    {
        if (currentState == State.Fire && !isCrouched)
        {
            AudioManager.Instance.PlayShoot();
            GameObject newFireBall = Instantiate(fireBallPrefab, shootPos.position, Quaternion.identity);
            newFireBall.GetComponent<Fireball>().direction = transform.localScale.x;
            animaciones.Shoot();
        }
    }
    public bool isBig()
    {
        return currentState != State.Default;
    }
   public void Goal()
    {
        AudioManager.Instance.PlayFlagPole();
        //levelFinished = true;              
        mover.DownFlagPole();             
        LevelManager.Instance.LevelFinished();
    }


}
