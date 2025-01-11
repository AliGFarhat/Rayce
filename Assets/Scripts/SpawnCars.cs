using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        //Load the car data
        CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i].transform;

            int playerSelectedCarID = PlayerPrefs.GetInt($"P{i + 1}SelectedCarID");

            //Find the selected car
            foreach (CarData cardata in carDatas)
            {
                //We found the car data for the player
                if (cardata.CarUniqueID == playerSelectedCarID)
                {
                    //Now spawn it on the spawnpoint
                    GameObject car = Instantiate(cardata.CarPrefab, spawnPoint.position, spawnPoint.rotation);

                    int playerNumber = i + 1;

                    car.GetComponent<CarInputHandler>().playerNumber = i + 1;

                    if (PlayerPrefs.GetInt($"P{playerNumber}_IsAI") == 1)
                    {
                        car.GetComponent<CarInputHandler>().enabled = false;
                        car.tag = "AI";
                    }
                    else
                    {
                        car.GetComponent<CarAIHandler>().enabled = false;
                        car.GetComponent<AStarLite>().enabled = false;
                        car.tag = "Player";
                    }

                    break;
                }
            }
        }
    }

}