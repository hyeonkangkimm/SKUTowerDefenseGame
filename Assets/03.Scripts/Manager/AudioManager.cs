using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgmAudioMixer;
    [SerializeField] private AudioMixerGroup sfxAudioMixer;

    [Header("# BGM Info")]
    [SerializeField] private List<AudioClip> bgmClips;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField][Range(0f, 1f)] private float bgmVolume = 1f;
    private Dictionary<string, AudioClip> bgmDict;

    [Header("# SFX Info")]
    [SerializeField] private List<AudioClip> sfxClips;
    [SerializeField] private AudioSource[] sfxSource;
    [SerializeField][Range(0f, 1f)] private float sfxVolume = 1f;
    [SerializeField] private int channels = 16;
    private Dictionary<string, AudioClip> sfxDict;
    private int channelIdx;

    protected override void Awake()
    {
        base.Awake();
        InitAudioMixer();
        InitDictionaries();
    }

    private void Start()
    {
        PlayBGM("BGM0001");
        SetAudioMixerVolume(EAudioMixerType.Master, 1f);
    }

    private void InitDictionaries()
    {
        bgmDict = new Dictionary<string, AudioClip>();
        foreach (var clip in bgmClips)
        {
            if (clip != null)
                bgmDict[clip.name] = clip;
        }

        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var clip in sfxClips)
        {
            if (clip != null)
                sfxDict[clip.name] = clip;
        }
    }

    private void InitAudioMixer()
    {
        // BGM
        GameObject bgmObject = new GameObject("BGMPlayer");
        bgmObject.transform.parent = transform;

        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;
        bgmSource.outputAudioMixerGroup = bgmAudioMixer;

        // SFX
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;

        sfxSource = new AudioSource[channels];
        for (int i = 0; i < channels; i++)
        {
            var source = sfxObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.volume = sfxVolume;
            source.bypassListenerEffects = true;
            source.outputAudioMixerGroup = sfxAudioMixer;
            sfxSource[i] = source;
        }
    }

    public void PlayBGM(string rcode)
    {
        if (!bgmDict.TryGetValue(rcode, out var clip))
        {
            Debug.LogWarning($"BGM [{rcode}] not found.");
            return;
        }

        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(string rcode)
    {
        if (!sfxDict.TryGetValue(rcode, out var clip))
        {
            Debug.LogWarning($"SFX [{rcode}] not found.");
            return;
        }

        for (int i = 1; i < sfxSource.Length; i++)
        {
            int loopIndex = (i + channelIdx) % sfxSource.Length;
            if (sfxSource[loopIndex].isPlaying)
                continue;

            channelIdx = loopIndex;
            sfxSource[loopIndex].clip = clip;
            sfxSource[loopIndex].Play();
            break;
        }
    }

    public void SetAudioMixerVolume(EAudioMixerType type, float volume)
    {
        audioMixer.SetFloat(type.ToString(), Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
    }

    public void GetAudioMixerVolume(EAudioMixerType type, out float volume)
    {
            Debug.Log(type.ToString());
        if (audioMixer.GetFloat(type.ToString(), out float vol))
        {
            volume = Mathf.Pow(10f, vol / 20f);
        }
        else
        {
            volume = 1f;
        }
    }

    public void OnMenuClick()
    {
        PlaySFX("Click");
    }
}