using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SceneAudioManager : MonoBehaviour
{
    public static SceneAudioManager Instance;

    public AudioClip bgmClip;
    [Range(0f, 1f)] public float bgmVolume = 1f;

    private AudioSource audioSource;

    void Awake()
    {
        // 싱글톤 패턴으로 전역 접근 가능하게
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // 저장된 볼륨 불러오기
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        audioSource.volume = bgmVolume;

        audioSource.Play();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        audioSource.volume = bgmVolume;
        PlayerPrefs.SetFloat("BGM_VOLUME", bgmVolume);
    }
}
