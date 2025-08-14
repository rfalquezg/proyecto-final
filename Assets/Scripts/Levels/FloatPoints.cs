using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPoints : MonoBehaviour
{
    // public Sprite[] sprites;
    // public int[] puntos;
    public points[] points;
    public int numPoints = 0;
    public float distance = 2f;
    public float speed = 2f;
    public bool destroy =true;

    float targetPos;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Show(numPoints);
        targetPos = transform.position.y + distance;
    }
    private void Update()
    {
        if (transform.position.y < targetPos)
        {
            transform.position = new Vector2(transform.position.x,
            transform.position.y + (speed * Time.deltaTime));
        }
        else if (destroy)
        {
            Destroy(gameObject);
        }
    }


    void Show(int points)
{
    //int index = 0;
    //for (int i = 0; i < puntos.Length; i++)
    //{
    //    if (puntos[i] == points)
    //    {
    //        index = i;
    //        break;
    //    }
    //}

    //spriteRenderer.sprite = sprites[index];

    for (int i = 0; i < this.points.Length; i++)
    {
            if (this.points[i].numPoints == points)
            {
                spriteRenderer.sprite = this.points[i].sprite;
                break;
            
            }
    }
}

}
[Serializable]
public class points
{
    public int numPoints;
    public Sprite sprite;
}
