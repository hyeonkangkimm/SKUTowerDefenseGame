using UnityEngine;

public class PlaySoundOnClick : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLightningSound()
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
