using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public float frameTime = 0.1f;

    Image image;
    int animationFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        InvokeRepeating("ChangeImage", frameTime, frameTime);
    }

    void ChangeImage()
    {
        image.sprite = sprites[animationFrame];
        animationFrame++;
        if (animationFrame >= sprites.Length)
        {
            animationFrame = 0;
        }
    }
}
