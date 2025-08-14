using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int puntos;
    public int MaxPuntos;

    public static ScoreManager Instance;

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

    // Start is called before the first frame update
    void Start()
    {
        puntos = 0;
        MaxPuntos = PlayerPrefs.GetInt("Puntos", 0);
    }
    public void NewGame()
    {
        puntos = 0;
    }
    public void GameOver()
    {
        if (puntos > MaxPuntos)
        {
            MaxPuntos = puntos;
            PlayerPrefs.SetInt("Puntos", MaxPuntos);
        }
        
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
    }
}
