using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CarSFXHandler : MonoBehaviour
{
    [Header("Mixers")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]
    public AudioSource tiresScreechingAudioSource;
    public AudioSource engineAudioSource;
    public AudioSource carHitAudioSource;

    // Local Variable
    float desiredEnginePitch = 0.5f;
    float tireScreechPitch = 0.5f;

    // Components
    TopDownCarController topDownCarController;

    void Awake()
    {
        topDownCarController = GetComponentInParent<TopDownCarController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("SFXVolume", 0.5f); // Set the volume of "SFX Volume" in the audio mixers when the game starts.
    }

    // Update is called once per frame
    void Update()
    {
         UpdateEngineSFX();
         UpdateTiresScreechingSFX();
    }

    void UpdateEngineSFX()
    {
        // Handle Engine SFX
        float velocityMagnitude = topDownCarController.GetVelocityMagnitude();

        // Increase the engine volume as the car goes faster and faster.
        float desiredEngineVolume = velocityMagnitude * 0.05f;

        // Keep the volume at a minimum level so it keeps playing regardless. Also adding a max level.
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.2f, 1.0f);

        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, desiredEngineVolume, Time.deltaTime * 10);

        // To make the engine sound have more variation, the pitch is adjusted.
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.5f, 1.8f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);
    }

    void UpdateTiresScreechingSFX()
    {
        // Handle tire screeching SFX
        if (topDownCarController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            // If car is braking screech should be louder.
            if (isBraking)
            {
                tiresScreechingAudioSource.volume = Mathf.Lerp(tiresScreechingAudioSource.volume, 1.0f, Time.deltaTime * 10);
                tireScreechPitch = Mathf.Lerp(tireScreechPitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                // If not braking. Play the screech sound when drifting.
                tiresScreechingAudioSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireScreechPitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        // Fade out the tire screech SFX if no screeching is happening.
        else tiresScreechingAudioSource.volume = Mathf.Lerp(tiresScreechingAudioSource.volume, 0, Time.deltaTime * 10);
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        // Get the relative velocity of the collision
        float relativeVelocity = collision2D.relativeVelocity.magnitude;

        float volume = relativeVelocity * 0.1f;

        carHitAudioSource.pitch = Random.Range(0.95f, 1.05f);
        carHitAudioSource.volume = volume;

        if (!carHitAudioSource.isPlaying)
            carHitAudioSource.Play();
    }
}