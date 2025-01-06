using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    //Components
    TopDownCarController topDownCarController;

    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        //Send the input to the car controller.
        topDownCarController.SetInputVector(inputVector);
    }
}