using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System;

public enum GameStates { countDown, running, raceOver };

public class GameManager : MonoBehaviour
{
    // Static instance of GameManager so other scripts can access it
    public static GameManager instance = null;

    // States
    GameStates gameState = GameStates.countDown;

    // Time
    float raceStartedTime = 0;
    float raceCompletedTime = 0;

    // Driver information
    List<DriverInfo> driverInfoList = new List<DriverInfo>();

    // Audio Mixer
    public AudioMixer audioMixer;

    // Events
    public event Action<GameManager> OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Assign the AudioMixer dynamically
            audioMixer = Resources.Load<AudioMixer>("DefaultAudioMixer");
            if (audioMixer == null)
            {
                Debug.LogError("Failed to load AudioMixer. Ensure it exists in a Resources folder and is named 'DefaultAudioMixer'.");
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Supply dummy driver information for testing purposes
        driverInfoList.Add(new DriverInfo(1, "P1", 0, false));
    }

    void LevelStart()
    {
        gameState = GameStates.countDown;

        Debug.Log("Level started");

        // Handle audio effects based on the scene
        HandleSceneAudioEffects(SceneManager.GetActiveScene().name);
    }

    public GameStates GetGameState()
    {
        return gameState;
    }

    void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;

            // Invoke game state change event
            OnGameStateChanged?.Invoke(this);
        }
    }

    public float GetRaceTime()
    {
        if (gameState == GameStates.countDown)
            return 0;
        else if (gameState == GameStates.raceOver)
            return raceCompletedTime - raceStartedTime;
        else return Time.time - raceStartedTime;
    }

    // Driver information handling
    public void ClearDriversList()
    {
        driverInfoList.Clear();
    }

    public void AddDriverToList(int playerNumber, string name, int carUniqueID, bool isAI)
    {
        driverInfoList.Add(new DriverInfo(playerNumber, name, carUniqueID, isAI));
    }

    public void SetDriversLastRacePosition(int playerNumber, int position)
    {
        DriverInfo driverInfo = FindDriverInfo(playerNumber);
        driverInfo.lastRacePosition = position;
    }

    public void AddPointsToChampionship(int playerNumber, int points)
    {
        DriverInfo driverInfo = FindDriverInfo(playerNumber);

        driverInfo.championshipPoints += points;
    }

    DriverInfo FindDriverInfo(int playerNumber)
    {
        foreach (DriverInfo driverInfo in driverInfoList)
        {
            if (playerNumber == driverInfo.playerNumber)
                return driverInfo;
        }

        Debug.LogError($"FindDriverInfoBasedOnDriverNumber failed to find driver for player number {playerNumber}");

        return null;
    }

    public List<DriverInfo> GetDriverList()
    {
        return driverInfoList;
    }

    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");

        raceStartedTime = Time.time;

        ChangeGameState(GameStates.running);
    }

    public void OnRaceCompleted()
    {
        Debug.Log("OnRaceCompleted");

        raceCompletedTime = Time.time;

        ChangeGameState(GameStates.raceOver);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }

    void HandleSceneAudioEffects(string sceneName)
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer is not assigned in GameManager.");
            return;
        }

        if (sceneName == "Menu") // Apply Lowpass effect in Scene1
        {
            audioMixer.SetFloat("LowpassCutoff", 700); // Set the cutoff frequency to a low value
            Debug.Log("Applied Lowpass effect with cutoff 700 Hz.");
        }
        else if (sceneName == "Track1") // Remove Lowpass effect in Scene2
        {
            audioMixer.SetFloat("LowpassCutoff", 22000f); // Set the cutoff frequency to max (effectively bypassing it)
            Debug.Log("Removed Lowpass effect by setting cutoff to 22000 Hz.");
        }
        else
        {
            Debug.Log($"No specific audio effect handling for scene: {sceneName}");
        }
    }
}