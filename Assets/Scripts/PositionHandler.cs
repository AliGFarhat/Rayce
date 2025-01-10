using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();

    void Start()
    {
        // Initialize existing cars
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        foreach (CarLapCounter lapCounter in carLapCounterArray)
        {
            RegisterCar(lapCounter);
        }
    }

    public void RegisterCar(CarLapCounter newCar)
    {
        if (!carLapCounters.Contains(newCar))
        {
            carLapCounters.Add(newCar);
            newCar.OnPassCheckpoint += OnPassCheckpoint;
        }
    }

    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        // Sort cars by checkpoints and time
        carLapCounters = carLapCounters
            .OrderByDescending(s => s.GetNumberOfCheckpointsPassed())
            .ThenBy(s => s.GetTimeAtLastCheckPoint())
            .ToList();

        // Update positions
        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;
        carLapCounter.SetCarPosition(carPosition);
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        carLapCounters.Clear();
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        foreach (CarLapCounter lapCounter in carLapCounterArray)
        {
            RegisterCar(lapCounter);
        }
    }
}