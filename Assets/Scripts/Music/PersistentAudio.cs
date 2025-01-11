using UnityEngine;
using UnityEngine.Audio;

public class PersistentAudio : MonoBehaviour
{
    public AudioMixer audioMixer;
    private static PersistentAudio instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLowpassFilter(bool enable)
    {
        if (enable)
        {
            audioMixer.SetFloat("LowpassCutoff", 3000f); // Adjust cutoff frequency for Lowpass
        }
        else
        {
            audioMixer.ClearFloat("LowpassCutoff");
        }
    }
}