using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleLevel : MonoBehaviour
{
    //public string nextScene;
    //public int nextWorld;
    //public int nextLevel;
    bool marioInCastle;

    void Update()
    {
        if (marioInCastle && LevelManager.Instance.countPoint)
        {
            //GameManager.Instance.GoToLevel(nextScene);
            //GameManager.Instance.GoToLevel(nextWorld, nextLevel);
            GameManager.Instance.NextLevel();
            marioInCastle = false;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Mario mario = collision.gameObject.GetComponent<Mario>();
        if (mario != null)
        {
            mario.transform.position = new Vector3(1000, 1000, 1000);
            marioInCastle = true;
            LevelManager.Instance.MarioInCastle();
        }
    }
}
