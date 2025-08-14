using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeFinal : MonoBehaviour
{
    public GameObject[] bridgeParts;
    public Transform finalLimit;
    public GameObject bridge;
    public Bowser bowser;
    bool isBridgeCollapse;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!isBridgeCollapse)
            {
                bowser.collapseBridge = true;
                isBridgeCollapse = true;
                GetComponent<CircleCollider2D>().enabled = false;

                bowser.FallBridge();                
                Mario.Instance.mover.StopMove();    

                StartCoroutine(CollapseBridge());
            }
        }
    }

    IEnumerator CollapseBridge()
    {
        foreach (GameObject bridgePart in bridgeParts)
        {
            Destroy(bridgePart);
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(bridge);

        yield return new WaitForSeconds(1.25f);

        Mario.Instance.mover.AutoWalk();
        Camera.main.GetComponent<CameraFollow>().UpdateMaxPos(finalLimit.position.x);
    }
}
