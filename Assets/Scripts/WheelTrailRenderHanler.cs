using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRenderHanler : MonoBehaviour
{
    // Components
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;

    void Awake()
    {
        // Get the topdowncarcontroller.
        topDownCarController = GetComponentInParent<TopDownCarController>();

        // Get the trail renderer component.
        trailRenderer = GetComponent<TrailRenderer>();

        // Set the trail renderer to not work from the start.
        trailRenderer.emitting = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If car tires are "screeching" then emit a trail.
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
            trailRenderer.emitting = true;
        else trailRenderer.emitting = false; 
    }
}
