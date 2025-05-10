using UnityEngine;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public AudioClip background;
    public AudioClip background2;

    public void Start()
    {
        audioSource.clip = background;
        audioSource.Play();
    }

}
