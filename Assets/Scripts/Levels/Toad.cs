using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toad : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;

    bool triggered;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!triggered && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            triggered = true;
            Mario.Instance.mover.StopMove();
            StartCoroutine(ShowFinalTexts());
        }
    }

    IEnumerator ShowFinalTexts()
    {
        yield return new WaitForSeconds(1f);
        text1.SetActive(true);
        yield return new WaitForSeconds(1f);
        text2.SetActive(true);
        LevelManager.Instance.LevelFinished();
        GameManager.Instance.ReturnToMenuAfterFinal();
    }
}
