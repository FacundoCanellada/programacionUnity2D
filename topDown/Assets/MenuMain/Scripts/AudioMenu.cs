using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioMenu : MonoBehaviour
{
    public static AudioMenu Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("UI")]
    public Slider volumeSlider;

    [Header("Clips de Música")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    [Header("Clips de Efectos")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip stepClip;
    [SerializeField] private AudioClip pickupHealthClip;
    [SerializeField] private AudioClip pickupXPClip;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip buttonClickClip;

    private const string MusicVolumeKey = "volume";
    private const string MixerParamName = "volume";
    private const string MuteKey = "muted";
    private const string SFXMixerParam = "sfx";

    private bool isMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (sfxSource != null && sfxMixerGroup != null)
        {
            sfxSource.outputAudioMixerGroup = sfxMixerGroup;
        }

        if (audioSource != null)
        {
            if (!audioSource.isPlaying || audioSource.clip != menuMusic)
            {
                audioSource.clip = menuMusic;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        LoadVolume();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "MainMenu")
        {
            PlayMenuMusic();
        }
        else
        {
            PlayGameplayMusic();
        }

        FindAndSetupVolumeSlider();
    }

    private void FindAndSetupVolumeSlider()
    {
        Slider[] allSliders = FindObjectsByType<Slider>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (allSliders.Length > 0)
        {
            foreach (Slider s in allSliders)
            {
                if (s.gameObject.name == "Volume slider")
                {
                    volumeSlider = s;
                    break;
                }
            }

            if (volumeSlider != null)
            {
                volumeSlider.onValueChanged.RemoveAllListeners();
                volumeSlider.onValueChanged.AddListener(SetMusicVolume);

                LoadVolume();
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();

        if (isMuted)
            return;
 

        if (masterMixer != null)
        {
            float mixerVolume = (volume == 0) ? -80f : Mathf.Log10(volume) * 20;
            masterMixer.SetFloat(MixerParamName, mixerVolume);
            masterMixer.SetFloat(SFXMixerParam, mixerVolume);
        }
    }


    private void LoadVolume()
    {
        float savedVolume = 0.75f;

        if (PlayerPrefs.HasKey(MusicVolumeKey))
        {
            savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
        }
        else
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, savedVolume);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey(MuteKey))
        {
            isMuted = PlayerPrefs.GetInt(MuteKey) == 1;
        }

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        if (isMuted)
        {
            masterMixer.SetFloat(MixerParamName, -80f);
            masterMixer.SetFloat(SFXMixerParam, -80f);
        }
        else
        {
            SetMusicVolume(savedVolume);
        }
    }

    public void PlayGameplayMusic()
    {
        if (audioSource != null && audioSource.clip != gameplayMusic)
        {
            audioSource.Stop();
            audioSource.clip = gameplayMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayMenuMusic()
    {
        if (audioSource != null && audioSource.clip != menuMusic)
        {
            audioSource.Stop();
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            masterMixer.SetFloat(MixerParamName, -80f);
            masterMixer.SetFloat(SFXMixerParam, -80f);
        }
        else
        {
            float volume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
            float mixerVolume = (volume == 0) ? -80f : Mathf.Log10(volume) * 20f;
            masterMixer.SetFloat(MixerParamName, mixerVolume);
            masterMixer.SetFloat(SFXMixerParam, mixerVolume);
        }

        PlayerPrefs.SetInt(MuteKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayShootSound() => PlaySFX(shootClip);
    public void PlayStepSound() => PlaySFX(stepClip, 0.5f);
    public void PlayPickupHealth() => PlaySFX(pickupHealthClip);
    public void PlayPickupXP() => PlaySFX(pickupXPClip);
    public void PlayEnemyHit() => PlaySFX(enemyHitClip);
    public void PlayButtonClick() => PlaySFX(buttonClickClip);
}