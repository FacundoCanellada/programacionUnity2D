using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioMenu : MonoBehaviour
{
    public static AudioMenu Instance { get; private set; }

    [SerializeField] AudioSource audioSource;
    public AudioMixer masterMixer;
    public Slider volumeSlider; 

    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    private const string MusicVolumeKey = "volume";
    private const string MixerParamName = "volume";
    private const string MuteKey = "muted";
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

        if (audioSource == null)
        {
            Debug.LogError("AudioMenu: audioSource no asignado en el Inspector.");
        }
        else
        {
            if (!audioSource.isPlaying || audioSource.clip != menuMusic)
            {
                audioSource.clip = menuMusic;
                audioSource.loop = true;
                audioSource.Play();
                Debug.Log("AudioMenu: Música del menú iniciada en Awake.");
            }
        }
    }

    void OnDestroy()
    {
        Debug.Log("AudioMenu OnDestroy: Inicio.");
        SceneManager.sceneLoaded -= OnSceneLoaded; // Corregido: Eliminar la verificación de null
        Debug.Log("AudioMenu: Desuscrito de SceneManager.sceneLoaded.");

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
            Debug.Log("AudioMenu: Listeners del slider eliminados.");
        }
        Debug.Log("AudioMenu OnDestroy: Fin.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"AudioMenu OnSceneLoaded: Escena '{scene.name}' cargada. Mode: {mode}");
        FindAndSetupVolumeSlider();
    }

    // --- CAMBIO CLAVE AQUÍ: FindObjectsByType con FindObjectsInactive.Include ---
    private void FindAndSetupVolumeSlider()
    {
        Debug.Log("AudioMenu FindAndSetupVolumeSlider: Buscando Slider (incluyendo inactivos)...");
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
                Debug.Log("AudioMenu FindAndSetupVolumeSlider: Slider encontrado y asignado.");
                volumeSlider.onValueChanged.RemoveAllListeners();
                volumeSlider.onValueChanged.AddListener(SetMusicVolume);
                Debug.Log("AudioMenu FindAndSetupVolumeSlider: Listener del slider añadido.");

                LoadVolume();
                Debug.Log("AudioMenu FindAndSetupVolumeSlider: LoadVolume llamado.");
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log($"AudioMenu SetMusicVolume: Intentando establecer volumen a {volume}.");

        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();

        if (isMuted)
        {
            Debug.Log("AudioMenu SetMusicVolume: Ignorado porque está muteado.");
            return;
        }

        if (masterMixer != null)
        {
            float mixerVolume = (volume == 0) ? -80f : Mathf.Log10(volume) * 20;
            masterMixer.SetFloat(MixerParamName, mixerVolume);
            Debug.Log($"AudioMenu SetMusicVolume: Volumen establecido en Mixer a {mixerVolume}dB.");
        }
    }


    private void LoadVolume()
    {
        Debug.Log("AudioMenu LoadVolume: Iniciando carga de volumen...");
        float savedVolume = 0.75f;

        if (PlayerPrefs.HasKey(MusicVolumeKey))
        {
            savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey);
            Debug.Log($"AudioMenu LoadVolume: Volumen encontrado en PlayerPrefs: {savedVolume}.");
        }
        else
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, savedVolume);
            PlayerPrefs.Save();
            Debug.Log("AudioMenu LoadVolume: No se encontró volumen en PlayerPrefs, usando valor por defecto.");
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
            Debug.Log("AudioMenu LoadVolume: Mute activado, volumen en -80dB.");
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
            Debug.Log("AudioMenu: Cambiando a música del juego.");
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
            Debug.Log("AudioMenu: Volviendo a música del menú.");
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            Debug.Log("AudioMenu: Música detenida.");
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            masterMixer.SetFloat(MixerParamName, -80f);
            Debug.Log("AudioMenu: Mute activado.");
        }
        else
        {
            float volume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
            float mixerVolume = (volume == 0) ? -80f : Mathf.Log10(volume) * 20f;
            masterMixer.SetFloat(MixerParamName, mixerVolume);
            Debug.Log($"AudioMenu: Mute desactivado. Volumen restaurado a {mixerVolume} dB.");
        }

        PlayerPrefs.SetInt(MuteKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}