using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    public PositionHandler positionHandler; // Reference to the PositionHandler

    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        // Load the car data
        CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i].transform;

            int playerSelectedCarID = PlayerPrefs.GetInt($"P{i + 1}SelectedCarID");

            // Find the player cars prefab
            foreach (CarData cardata in carDatas)
            {
                // We found the car data for the player
                if (cardata.CarUniqueID == playerSelectedCarID)
                {
                    // Spawn the car at the spawn point
                    GameObject playerCar = Instantiate(cardata.CarPrefab, spawnPoint.position, spawnPoint.rotation);

                    // Assign the player number
                    playerCar.GetComponent<CarInputHandler>().playerNumber = i + 1;

                    // Register the car with the PositionHandler
                    CarLapCounter carLapCounter = playerCar.GetComponent<CarLapCounter>();
                    if (positionHandler != null && carLapCounter != null)
                    {
                        positionHandler.RegisterCar(carLapCounter);
                    }
                    else
                    {
                        Debug.LogError("PositionHandler or CarLapCounter is missing!");
                    }

                    break;
                }
            }
        }
    }
}