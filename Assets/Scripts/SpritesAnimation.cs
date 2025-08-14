using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesAnimation : MonoBehaviour
{
    public Sprite[] sprites; 
    public float frameTime = 0.1f; 

    float timer = 0f; 
    int animationFrame = 0; 
    public bool stop;
    public bool loop = true;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    void Start()
    {
        
        StartCoroutine(Animation());
    }


    IEnumerator Animation()
    {
        if (loop)
        {
            while (!stop)
            {
                //Debug.Log("Animation Frame: " + animationFrame); // Mostramos en consola el índice actual
                spriteRenderer.sprite = sprites[animationFrame]; // Cambiamos el sprite
                animationFrame++; // Avanzamos al siguiente
                if (animationFrame >= sprites.Length)
                {
                    animationFrame = 0;
                }
                yield return new WaitForSeconds(frameTime);
            }
        }
        else
        {
            while (animationFrame < sprites.Length)
            {
                spriteRenderer.sprite = sprites[animationFrame];
                animationFrame++;
                yield return new WaitForSeconds(frameTime);
            }
            Destroy(gameObject);
        }
        //while (animationFrame < sprites.Length)

    }

}
