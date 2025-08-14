using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutCamera : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool hasBeenVisible;
    public float minDistance = 0;
    public GameObject parent;

    public bool OnlyBack; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spriteRenderer.isVisible)
        {
            hasBeenVisible = true;
        }
        else
        {
            if (hasBeenVisible)
            {
                // if (OnlyBack && minDistance == 0)
                // {
                //     if (transform.position.x > Camera.main.transform.position.x)
                //     {
                //         return; 
                //     }
                // }
                if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > minDistance)
                {
                    if (OnlyBack)
                    {
                        if (transform.position.x > Camera.main.transform.position.x)
                        {
                            return;
                        }
                    }

                    if (parent == null)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(parent);
                    }
                }

                
            }
        }
    }
}
