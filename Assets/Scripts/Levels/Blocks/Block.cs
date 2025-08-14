using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isBreakable;

    public GameObject brickPiecePrefab;
    public int numCoins;
    public GameObject coinBlockPrefab;
    bool bouncing;
    bool isEmpty;
    public Sprite emptyBlock;
    public GameObject itemPrefab;
    public GameObject itemFlowerPrefab;
    //List<GameObject> enemiesOnBlock = new List<GameObject>();
    public LayerMask onBlockLayers;
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    public Sprite defaultBlock;
    public Sprite hitBlock;
    public Sprite hitEmptyBlock;


    //public GameObject floatPointsPrefab;
    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Start()
    {
        if (spriteRenderer.sprite == null)
        {
            boxCollider2D.enabled = false;
        }
    }
    void OnTheBlock()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider2D.bounds.center + Vector3.up * boxCollider2D.bounds.extents.y, boxCollider2D.bounds.size * 0.5f, 0, onBlockLayers);

        foreach (Collider2D c in colliders)
        {
            Enemy enemy = c.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.HitBelowBlock();
            }
            else
            {
                Item item = c.GetComponent<Item>();
                if (item != null)
                {
                    if (item.type == ItemType.Coin)
                    {
                        Instantiate(coinBlockPrefab, transform.position, Quaternion.identity);
                        Destroy(item.gameObject);
                    }
                    else
                    {
                        item.HitBelowBlock();
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if(boxCollider2D != null)
        {
            Gizmos.DrawWireCube(boxCollider2D.bounds.center + Vector3.up * boxCollider2D.bounds.extents.y, boxCollider2D.bounds.size * 0.5f);
        }
        
        
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //     {
    //         Vector2 sideContact = collision.contacts[0].normal;
    //         Vector2 topSide = new Vector2(0f, -1f);

    //         if (sideContact == topSide)
    //         {
    //             if (!enemiesOnBlock.Contains(collision.gameObject))
    //             {
    //                 enemiesOnBlock.Add(collision.gameObject);
    //             }
    //         }
    //     }
    // }
    // private void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (!bouncing)
    //     {
    //         if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //         {
    //             if (enemiesOnBlock.Contains(collision.gameObject))
    //             {
    //                 enemiesOnBlock.Remove(collision.gameObject);
    //             }
    //         }
    //     }

    // }

    //private void OnTriggerEnter2D(Collider2D collision)
    public void HeadCollision(bool marioBig)
    {
        if (spriteRenderer.sprite == null)
        {
            boxCollider2D.enabled = true;
            spriteRenderer.sprite = emptyBlock;
        }
        //if (collision.CompareTag("HeadMario"))
            //{

            //collision.transform.parent.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            if (isBreakable)
            {
                if (marioBig)
                {
                    OnTheBlock();
                    Break();
                }
                else
                {
                    Bounce();
                }

            }
            else if (!isEmpty)
            {
                if (numCoins > 0)
                {
                    if (!bouncing)
                    {
                        Instantiate(coinBlockPrefab, transform.position, Quaternion.identity);
                        numCoins--;
                        
                        if (numCoins <= 0)
                        {
                            isEmpty = true;
                        }
                        Bounce();
                    }
                }
                else if (itemPrefab != null)
                {
                if (!bouncing)
                {
                    StartCoroutine(ShowItem());

                    isEmpty = true;
                    Bounce();    
                }
                }
            }
        if (!isEmpty)
        {
            OnTheBlock();
        }
        // if (!isEmpty)
        // {
        //     foreach (GameObject enemyOnBlock in enemiesOnBlock)
        //     {
        //         enemyOnBlock.GetComponent<Enemy>().HitBelowBlock();
        //     }
        // }

        //}
    }

    public void BreakFromTop()
    {
        if (isBreakable)
        {
            Break();
        }
    }
    void Bounce()
    {
        if (!bouncing)
        {
            StartCoroutine(BounceAnimation());
        }
    }

    IEnumerator BounceAnimation()
    {
        //hit
        if (isEmpty)
        {
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if (spritesAnimation != null)
            {
                spritesAnimation.stop = true;
            }
            spriteRenderer.sprite = hitEmptyBlock;
        }
        else
        {
            if (hitBlock != null)
            {
                spriteRenderer.sprite = hitBlock;
            }
        }

        AudioManager.Instance.PlayBump();
        bouncing = true;
        float time = 0;
        float duration = 0.1f;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = (Vector2)transform.position + Vector2.up * 0.25f;

        while (time < duration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        time = 0;
        while (time < duration)
        {
            transform.position = Vector2.Lerp(targetPosition, startPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition;
        bouncing = false;
        if (isEmpty)
        {
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if (spritesAnimation != null)
            {
                spritesAnimation.stop = true;
            }

            spriteRenderer.sprite = emptyBlock;
        }
        else
        {
            SpritesAnimation spritesAnimation = GetComponent<SpritesAnimation>();
            if (spritesAnimation != null)
            {
                spritesAnimation.stop = false;
            }
            else
            {
                spriteRenderer.sprite = defaultBlock;
            }
        }


    }
    void Break()
    {
        AudioManager.Instance.PlayBreak();
        ScoreManager.Instance.SumarPuntos(50);
        GameObject brickPiece;

        // Arriba a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(6f, 12f);

        // Arriba a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-6f, 12f);

        // Abajo a la derecha
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, -90)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(6f, -8f);

        // Abajo a la izquierda
        brickPiece = Instantiate(brickPiecePrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        brickPiece.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-6f, -8f);

        Destroy(gameObject);
    }
    IEnumerator ShowItem()
    {
        AudioManager.Instance.PlayPowerUpAppear();
        GameObject newItem;
        if (itemFlowerPrefab != null && Mario.Instance.isBig())
        {
            newItem = Instantiate(itemFlowerPrefab, transform.position, Quaternion.identity);
        }
        else
        {
           newItem = Instantiate(itemPrefab, transform.position, Quaternion.identity); 
        }
        if (newItem == null) yield break; // ← VERIFICACIÓN CLAVE

        Item item = newItem.GetComponent<Item>();
        item?.WaitMove();

        float time = 0;
        float duration = 0.5f;
        Vector2 startPosition = newItem.transform.position;
        Vector2 targetPosition = startPosition + Vector2.up * 0.5f;

        while (time < duration)
        {
            if (newItem == null) yield break; // ← Verificamos que no haya sido destruido

            newItem.transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        if (newItem != null)
        {
            newItem.transform.position = targetPosition;
            item?.StartMove(); // ← Llamamos solo si item no es null
        }
    }


}
