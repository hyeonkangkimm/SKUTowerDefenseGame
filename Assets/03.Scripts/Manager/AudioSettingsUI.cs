using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public GameObject UIPanel;
    private void Start()
    {
        float value;
        DontDestroyOnLoad(UIPanel); 
        // 슬라이더 초기값 설정
        AudioManager.Instance.GetAudioMixerVolume(EAudioMixerType.Master, out value);
        masterSlider.SetValueWithoutNotify(value);

        AudioManager.Instance.GetAudioMixerVolume(EAudioMixerType.BGM, out value);
        bgmSlider.SetValueWithoutNotify(value);

        AudioManager.Instance.GetAudioMixerVolume(EAudioMixerType.SFX, out value);
        sfxSlider.SetValueWithoutNotify(value);

        // 이벤트 연결
        masterSlider.onValueChanged.AddListener((v) =>
        {
            AudioManager.Instance.SetAudioMixerVolume(EAudioMixerType.Master, v);
        });

        bgmSlider.onValueChanged.AddListener((v) =>
        {
            AudioManager.Instance.SetAudioMixerVolume(EAudioMixerType.BGM, v);
        });

        sfxSlider.onValueChanged.AddListener((v) =>
        {
            AudioManager.Instance.SetAudioMixerVolume(EAudioMixerType.SFX, v);
        });
    }
}