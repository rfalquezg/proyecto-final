using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int id;
    public Transform startPointPlayer;
    public Stage stage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //GameManager.Instance.isLevelCheckPoint = true;
            GameManager.Instance.currentPoint = id;
        }
    }
}
