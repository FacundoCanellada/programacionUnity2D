using UnityEngine;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public AudioClip background;
    public AudioClip background2;

    private bool playingFirstClip = true;

    public void Start()
    {
        audioSource.clip = background;
        audioSource.Play();
    }

    public void Update()
    {
        if (!audioSource.isPlaying)
        {
            // Alternar entre background y background2
            if (playingFirstClip)
            {
                audioSource.clip = background2;
            }
            else
            {
                audioSource.clip = background;
            }

            playingFirstClip = !playingFirstClip;
            audioSource.Play();
        }
    }
}
