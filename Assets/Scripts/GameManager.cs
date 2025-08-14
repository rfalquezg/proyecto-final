using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public World[] worlds;
    public int currentWorld;
    public int currentLevel;
    public HUD hud;
    int coins;
    public Mario mario;
    public int lives;
    bool isRespawning;
    public bool isGameOver;
    public int currentPoint;
    public static GameManager Instance;

    void SaveProgress()
    {
        PlayerPrefs.SetInt("World", currentWorld);
        PlayerPrefs.SetInt("Level", currentLevel);
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveProgress();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveProgress();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        HideTimer();
        isGameOver = true;
        currentWorld = PlayerPrefs.GetInt("World", 1);
        currentLevel = PlayerPrefs.GetInt("Level", 1);
        lives = PlayerPrefs.GetInt("Lives", 3);
        if (lives < 0) lives = 0;
    }

    public void AddCoins()
    {
        coins++;
        if (coins > 99)
        {
            coins = 0;
            NewLife();
        }
        hud.UpdateCoins(coins);
    }

    public void OutOfTime()
    {
        Mario.Instance.Dead(true);
    }

    public void KillZone()
    {
        if (!isRespawning)
        {
            Mario.Instance.Dead(false);
        }
    }

    public void LoseLife()
    {
        if (!isRespawning)
        {
            if (lives > 0) lives--;
            if (lives <= 0)
            {
                lives = 0;
                isRespawning = true;
                GameOver();
            }
            else
            {
                isRespawning = true;
                StartCoroutine(Respawn());
            }
            SaveProgress();
        }
    }

    public void NewLife()
    {
        lives++;
        AudioManager.Instance.PlayOneUp();
        SaveProgress();
    }

    public void StartGame()
    {
        HideTimer();
        NewGame();
        currentWorld = 1;
        currentLevel = 1;
        currentPoint = 0;
        LoadTransition();
    }

    public void ContinueGame()
    {
        HideTimer();
        isGameOver = false;
        currentWorld = Mathf.Max(1, PlayerPrefs.GetInt("World", 1));
        currentLevel = Mathf.Max(1, PlayerPrefs.GetInt("Level", 1));
        lives = PlayerPrefs.GetInt("Lives", 3);
        if (lives <= 0) lives = 3;
        currentPoint = 0;
        LoadTransition();
    }

    void NewGame()
    {
        lives = 3;
        coins = 0;
        isGameOver = false;
        ScoreManager.Instance.NewGame();
        currentPoint = 0;
        PlayerPrefs.SetInt("World", 1);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.Save();
    }

    void GameOver()
    {
        ScoreManager.Instance.GameOver();
        isGameOver = true;
        lives = 0;
        PlayerPrefs.SetInt("Lives", 3);
        PlayerPrefs.Save();
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
        isRespawning = false;
        SceneManager.LoadScene("Transition");
        if (isGameOver)
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("StartMenu");
        }
        else
        {
            yield return new WaitForSeconds(5f);
            LoadLevel();
        }
    }

    public void LevelLoaded()
    {
        hud.UpdateWorld(currentWorld, currentLevel);
        ShowTimer();
        if (isGameOver)
        {
            NewGame();
        }
        hud.UpdateCoins(coins);
        Vector3 position = LevelManager.Instance.checkPoints[currentPoint].startPointPlayer.position;
        Mario.Instance.Respawn(position);
        LevelManager.Instance.StartLevel(currentPoint);
        LevelManager.Instance.cameraFollow.StartFollow(Mario.Instance.transform);
    }

    public void ReturnToMenuAfterFinal()
    {
        StartCoroutine(ReturnToMenuAfterFinalCo());
    }

    IEnumerator ReturnToMenuAfterFinalCo()
    {
        HideTimer();
        isGameOver = true;
        yield return new WaitForSeconds(18f); 
        SceneManager.LoadScene("StartMenu");
    }


    public void GoToLevel(string sceneName)
    {
        currentPoint = 0;
        SceneManager.LoadScene(sceneName);
    }

    public void GoToLevel(int world, int level)
    {
        currentPoint = 0;
        currentLevel = level;
        currentWorld = world;
        hud.UpdateWorld(world, level);
        SaveProgress();
        LoadTransition();
    }

    void LoadTransition()
    {
        SceneManager.LoadScene("Transition");
        Invoke("LoadLevel", 2f);
    }

    void LoadLevel()
    {
        int worldIndex = currentWorld - 1;
        int levelIndex = currentLevel - 1;
        string sceneName = worlds[worldIndex].levels[levelIndex].sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void NextLevel()
    {
        int worldIndex = currentWorld - 1;
        int levelIndex = currentLevel - 1;
        levelIndex++;
        if (levelIndex >= worlds[worldIndex].levels.Length)
        {
            worldIndex++;
            if (worldIndex >= worlds.Length)
            {
                return;
            }
            else
            {
                levelIndex = 0;
            }
        }
        currentWorld = worldIndex + 1;
        currentLevel = levelIndex + 1;
        currentPoint = 0;
        hud.UpdateWorld(currentWorld, currentLevel);
        SaveProgress();
        LoadTransition();
    }

    public void HideTimer()
    {
        hud.time.enabled = false;
    }

    public void ShowTimer()
    {
        hud.time.enabled = true;
    }
}

[System.Serializable]
public struct World
{
    public int id;
    public Level[] levels;
}

[System.Serializable]
public struct Level
{
    public int id;
    public string sceneName;
}
