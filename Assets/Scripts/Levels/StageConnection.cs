using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageConnection : MonoBehaviour
{
    public Mover.ConnectDirection exitDirection;
    public CameraFollow cam;

    bool connectionStarted;
    bool stayConnection;
    public Stage stage;
    public bool auto;

    private void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) && exitDirection == Mover.ConnectDirection.Down)
        {
            if (stayConnection && !connectionStarted)
                StartCoroutine(StartConnection());
        }

        if (Input.GetKey(KeyCode.RightArrow) && exitDirection == Mover.ConnectDirection.Right)
        {
            if (stayConnection && !connectionStarted)
                StartCoroutine(StartConnection());
        }
    }

    IEnumerator StartConnection()
    {
        connectionStarted = true;
        LevelManager.Instance.levelPaused = true;
        if (cam) cam.canMove = false;

        Mario.Instance.mover.AutoMoveConnection(exitDirection);   
        while (!Mario.Instance.mover.moveConnectionCompleted)
            yield return null;

        stage.EnterStage();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!connectionStarted && auto)
            {
                StartCoroutine(StartConnection());
            }
            stayConnection = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        stayConnection = false;
    }
}