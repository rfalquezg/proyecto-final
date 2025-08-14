using System.Collections;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Transform enterPoint;
    public Mover.ConnectDirection enterDirection;

    public CameraFollow cam;  
    public bool cameraMove;

    void StartStage()
    {
        Mario.Instance.mover.ResetMove();
        LevelManager.Instance.levelPaused = false;

        if (cam)
        {
            cam.StartFollow(Mario.Instance.transform);
            cam.canMove = true;          
        }
    }

    public void EnterStage()
    {
        
        Mario.Instance.transform.position = enterPoint.position;
        cam.lastPos = transform.position.x;

       
        if (cam)
            cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);

        switch (enterDirection)
        {
            case Mover.ConnectDirection.Down:
                StartCoroutine(StartStageDown());
                break;

            case Mover.ConnectDirection.Up:
                StartCoroutine(StartStageUp());
                break;

            case Mover.ConnectDirection.Left:
            case Mover.ConnectDirection.Right:
                StartStage();
                break;
        }
    }

    IEnumerator StartStageDown()
    {
        yield return new WaitForSeconds(1f);
        StartStage();
    }

    IEnumerator StartStageUp()
    {
        float sizeMario = Mario.Instance.GetComponent<SpriteRenderer>().bounds.size.y;
        Mario.Instance.transform.position = enterPoint.position + Vector3.down * sizeMario;

        Mario.Instance.mover.AutoMoveConnection(enterDirection);
        while (!Mario.Instance.mover.moveConnectionCompleted)
            yield return null;

        StartStage();
    }
}


