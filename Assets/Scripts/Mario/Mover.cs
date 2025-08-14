using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mover : MonoBehaviour
{
    enum Direction { Left = -1, None = 0, Right = 1 }
    Direction currentDirection = Direction.None;

    public enum ConnectDirection { Up, Down, Left, Right }

    public float speed;
    public float acceleration;
    public float maxVelocity;
    public float friction;
    float currentVelocity = 0f;

   
    public float jumpForce;
    public float maxJumpingTime = 1f;
    public bool isJumping;
    float jumpTimer = 0f;
    float defaultGravity;

 
    public bool isSkidding;


    public Rigidbody2D rb2D;
    Colisiones colisiones;

    public bool inputMoveEnabled = true;

    public GameObject headBox;
    Animaciones animaciones;

    bool isClimbingFlagPole = false;
    public float climbPoleSpeed = 5f;
    public bool isFlagDown;

  
    public bool isAutoWalk;
    public float autoWalkSpeed = 5f;

 
    SpriteRenderer spriteRenderer;
    Mario mario;

    public bool moveConnectionCompleted = true;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        colisiones = GetComponent<Colisiones>();
        animaciones = GetComponent<Animaciones>();
        mario = GetComponent<Mario>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;   
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

   
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isAutoWalk = false;
        currentVelocity = 0f;
        isClimbingFlagPole = false;
        isJumping = false;
        inputMoveEnabled = true;

        if (rb2D != null)
        {
            rb2D.isKinematic = false;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.gravityScale = defaultGravity;
        }

        animaciones?.Climb(false);
        animaciones?.Jumping(false);
        animaciones?.Skid(false);
        animaciones?.Velocity(0);
    }

    void Start()
    {
        if (rb2D != null)
        {
            defaultGravity = rb2D.gravityScale;
            if (defaultGravity <= 0f) defaultGravity = 1f;
        }
        else
        {
            defaultGravity = 1f;
        }
    }

    void Update()
    {
        bool gameOver = (GameManager.Instance != null) && GameManager.Instance.isGameOver;
        bool levelFinished = (LevelManager.Instance != null) && LevelManager.Instance.levelFinished;

        if (!gameOver)
        {
            animaciones?.Grounded(true);
        }

        bool grounded = colisiones != null && colisiones.Grounded();
        animaciones?.Grounded(grounded);

        if (!levelFinished)
        {
            if (headBox != null) headBox.SetActive(false);

            if (isJumping)
            {
                if (rb2D != null && rb2D.linearVelocity.y > 0f)
                {
                    if (headBox != null) headBox.SetActive(true);
                    jumpTimer += Time.deltaTime;

                    if (!Input.GetKey(KeyCode.Space) && jumpTimer < maxJumpingTime)
                        rb2D.gravityScale = defaultGravity * 3f;
                    else
                        rb2D.gravityScale = defaultGravity;
                }
                else
                {
                    if (rb2D != null) rb2D.gravityScale = defaultGravity;

                    if (grounded)
                    {
                        isJumping = false;
                        jumpTimer = 0;
                        animaciones?.Jumping(false);
                    }
                }
            }

            currentDirection = Direction.None;

            if (inputMoveEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (grounded) Jump();
                }

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    currentDirection = Direction.Left;

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    currentDirection = Direction.Right;
            }
        }
    }

    private void FixedUpdate()
    {
        bool gameOver = (GameManager.Instance != null) && GameManager.Instance.isGameOver;
        if (gameOver) return;

        if (isClimbingFlagPole)
        {
            if (rb2D != null)
                rb2D.MovePosition(rb2D.position + Vector2.down * climbPoleSpeed * Time.fixedDeltaTime);
            return;
        }

        bool levelFinished = (LevelManager.Instance != null) && LevelManager.Instance.levelFinished;
        if (levelFinished && !isAutoWalk) return;

        if (isAutoWalk)
        {
            if (rb2D != null)
            {
                Vector2 velocity = new Vector2(currentVelocity, rb2D.linearVelocity.y);
                rb2D.linearVelocity = velocity;
            }
            animaciones?.Velocity(Mathf.Abs(currentVelocity));
            return;
        }
        else
        {
            if (rb2D != null && !rb2D.isKinematic)
            {
                isSkidding = false;
                currentVelocity = rb2D.linearVelocity.x;

                if (colisiones != null && colisiones.CheckCollision((int)currentDirection))
                {
                    currentVelocity = 0;
                }
                else
                {
                    if (currentDirection > 0)
                    {
                        if (currentVelocity < 0f)
                        {
                            currentVelocity += (acceleration + friction) * Time.deltaTime;
                            isSkidding = true;
                        }
                        else if (currentVelocity < maxVelocity)
                        {
                            currentVelocity += acceleration * Time.deltaTime;
                            transform.localScale = new Vector2(1, 1);
                        }
                    }
                    else if (currentDirection < 0)
                    {
                        if (currentVelocity > 0f)
                        {
                            currentVelocity -= (acceleration + friction) * Time.deltaTime;
                            isSkidding = true;
                        }
                        else if (currentVelocity > -maxVelocity)
                        {
                            currentVelocity -= acceleration * Time.deltaTime;
                            transform.localScale = new Vector2(-1, 1);
                        }
                    }
                    else
                    {
                        if (currentVelocity > 1f)
                            currentVelocity -= friction * Time.deltaTime;
                        else if (currentVelocity < -1f)
                            currentVelocity += friction * Time.deltaTime;
                        else
                            currentVelocity = 0;
                    }
                }

                if (mario != null && mario.isCrouched)
                    currentVelocity = 0;

                Vector2 velocity2 = new Vector2(currentVelocity, rb2D.linearVelocity.y);
                rb2D.linearVelocity = velocity2;

                animaciones?.Velocity(currentVelocity);
                animaciones?.Skid(isSkidding);
            }
        }
    }

    void Jump()
    {
        if (!isJumping)
        {
            if (mario != null && mario.isBig()) AudioManager.Instance?.PlayBigJump();
            else AudioManager.Instance?.PlayJump();

            isJumping = true;
            if (rb2D != null)
            {
                rb2D.gravityScale = defaultGravity;
                rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
            animaciones?.Jumping(true);
        }
    }

    public void Dead(bool BounceUp)
    {
        if (BounceUp && rb2D != null)
        {
            inputMoveEnabled = false;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.gravityScale = 1;
            rb2D.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
        }
    }

    public void Respawn()
    {
        inputMoveEnabled = true;
        if (rb2D != null)
        {
            rb2D.linearVelocity = Vector2.zero;
            rb2D.gravityScale = defaultGravity;
        }
        transform.localScale = Vector2.one;
    }

    public void BounceUp()
    {
        if (rb2D != null)
        {
            rb2D.linearVelocity = Vector2.zero;
            rb2D.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
    }

    public void AutoWalk()
    {
        if (rb2D != null) rb2D.isKinematic = false;
        inputMoveEnabled = false;
        isAutoWalk = true;
        currentVelocity = autoWalkSpeed;
    }

    public void DownFlagPole()
    {
        inputMoveEnabled = false;
        if (rb2D != null)
        {
            rb2D.isKinematic = true;
            rb2D.linearVelocity = new Vector2(0, 0f);
        }
        isClimbingFlagPole = true;
        isJumping = false;
        animaciones?.Jumping(false);
        animaciones?.Climb(true);
        transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);

        StopCoroutine(nameof(WaitBottomThenJump));
        StartCoroutine(WaitBottomThenJump());
    }

    IEnumerator WaitBottomThenJump()
    {
        float t = 0f, max = 2f;
        while (!(colisiones != null && colisiones.Grounded()) && t < max)
        {
            t += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(JumpOffPole());
    }

    IEnumerator JumpOffPole()
    {
        isClimbingFlagPole = false;
        if (rb2D != null) rb2D.linearVelocity = Vector2.zero;
        animaciones?.Pause();
        yield return new WaitForSeconds(0.25f);

        float t = 0f, max = 2f;
        while (!isFlagDown && t < max)
        {
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.25f);

        animaciones?.Climb(false);
        if (rb2D != null) rb2D.isKinematic = false;
        animaciones?.Continue();
        GetComponent<SpriteRenderer>().flipX = false;

        isAutoWalk = true;
        currentVelocity = autoWalkSpeed;
    }

    public void SetFlagDown()
    {
        isFlagDown = true;
    }

    public void AutoMoveConnection(ConnectDirection direction)
    {
        moveConnectionCompleted = false;
        inputMoveEnabled = false;
        if (rb2D != null)
        {
            rb2D.isKinematic = true;
            rb2D.linearVelocity = Vector2.zero;
        }
        if (spriteRenderer != null) spriteRenderer.sortingOrder = -100;

        switch (direction)
        {
            case ConnectDirection.Up:
                StartCoroutine(AutoMoveConnectionUp());
                break;
            case ConnectDirection.Down:
                StartCoroutine(AutoMoveConnectionDown());
                break;
            case ConnectDirection.Left:
                moveConnectionCompleted = true;
                break;
            case ConnectDirection.Right:
                StartCoroutine(AutoMoveConnectionRight());
                break;
        }
    }

    public void ResetMove()
    {
        if (rb2D != null) rb2D.isKinematic = false;
        inputMoveEnabled = true;
        if (spriteRenderer != null) spriteRenderer.sortingOrder = 20;

        isAutoWalk = false;
        currentVelocity = 0f;
        if (rb2D != null) rb2D.linearVelocity = Vector2.zero;
        isClimbingFlagPole = false;
        isJumping = false;

        animaciones?.Velocity(0);
        animaciones?.Skid(false);
        animaciones?.Climb(false);
        animaciones?.Jumping(false);
    }

    IEnumerator AutoMoveConnectionDown()
    {
        float targetDown = transform.position.y - spriteRenderer.bounds.size.y;
        while (transform.position.y > targetDown)
        {
            transform.position += Vector3.down * Time.deltaTime;
            yield return null;
        }
        moveConnectionCompleted = true;
    }

    IEnumerator AutoMoveConnectionUp()
    {
        float targetUp = transform.position.y + spriteRenderer.bounds.size.y;
        while (transform.position.y < targetUp)
        {
            transform.position += Vector3.up * Time.deltaTime;
            yield return null;
        }
        moveConnectionCompleted = true;
    }

    IEnumerator AutoMoveConnectionRight()
    {
        float targetRight = transform.position.x + spriteRenderer.bounds.size.x * 2f;
        animaciones?.Velocity(1);
        while (transform.position.x < targetRight)
        {
            transform.position += Vector3.right * Time.deltaTime;
            yield return null;
        }
        animaciones?.Velocity(0);
        moveConnectionCompleted = true;
    }

    public void StopMove()
    {
        inputMoveEnabled = false;
        if (rb2D != null)
        {
            rb2D.isKinematic = true;
            rb2D.linearVelocity = Vector2.zero;
        }
        isAutoWalk = false;
        animaciones?.Velocity(0);
    }
}
