using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public TextMeshProUGUI topPoints;
    public Vector3 marioStartPos;
    public GameObject buttonNewGame;
    public GameObject buttonContinue;

    // Start is called before the first frame update
    void Start()
    {
        int points = PlayerPrefs.GetInt("Puntos");
        topPoints.text = "TOP-" + points.ToString("D6");
        Mario.Instance.Respawn(marioStartPos);

        EventSystem.current.SetSelectedGameObject(buttonNewGame);

        int savedWorld = PlayerPrefs.GetInt("World", 1);
    int savedLevel = PlayerPrefs.GetInt("Level", 1);
    if (savedWorld == 1 && savedLevel == 1)
    {
        buttonContinue.GetComponent<Button>().interactable = false;
        buttonContinue.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.5f);

    }

    }

    public void ClickNewGame()
    {
        GameManager.Instance.StartGame();
    }

    public void ClickContinue()
    {
        GameManager.Instance.ContinueGame();
    }
}
