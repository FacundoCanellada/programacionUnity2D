using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public Slider musicSilder;
    public void SetMusicVolume ()
    {
        float volume = musicSilder.value;
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
}
