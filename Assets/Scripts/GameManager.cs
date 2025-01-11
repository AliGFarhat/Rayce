using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { countDown, running, raceOver };

public class GameManager : MonoBehaviour
{
    //Static instance of GameManager so other scripts can access it
    public static GameManager instance = null;

    GameStates gameState = GameStates.countDown;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void LevelStart()
    {
        gameState = GameStates.countDown;

        Debug.Log("Level started");
    }


    public GameStates GetGameState()
    {
        return gameState;
    }

    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");

        gameState = GameStates.running;

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }

}
