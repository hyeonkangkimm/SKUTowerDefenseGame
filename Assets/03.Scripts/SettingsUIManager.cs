using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider bgmSlider;

    void Start()
    {
        // 슬라이더 초기값 설정
        float savedVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        bgmSlider.value = savedVolume;

        // 슬라이더 값 변경 시 연결
        //bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
    }

    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    void OnBGMSliderChanged(float value)
    {
        SceneAudioManager.Instance.SetBGMVolume(value);
    }
}
