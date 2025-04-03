using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    //Ted

    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        //audioMixer.SetFloat("MasterVolume", level);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20);
    }

    public void SetSoundFXVolume(float level)
    {
        //audioMixer.SetFloat("SFXVolume", level);
        audioMixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {
        //audioMixer.SetFloat("MusicVolume", level);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
    }
}
