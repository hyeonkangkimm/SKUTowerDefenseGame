using UnityEngine;
using UnityEngine.UI;

public class LightningButtonHandler : MonoBehaviour
{
    private bool hasStruckInStart = false;
    private ECombatConditionType lastCombatCondition;

    public LightningSpawner lightningSpawner;
    public AudioSource lightningAudioSource;

    [SerializeField] private Button lightningButton;  // 버튼 참조
    [SerializeField] private Image buttonImage;       // 버튼 이미지 (회색 처리용)

    private Color originalColor;

    private void Start()
    {
        if (buttonImage != null)
            originalColor = buttonImage.color;
    }

    private void Update()
    {
        var currentCondition = GameManager.Instance.CombatConditionType;

        // START 상태로 새롭게 진입했을 때 초기화 및 버튼 활성화
        if (currentCondition == ECombatConditionType.START && lastCombatCondition != ECombatConditionType.START)
        {
            hasStruckInStart = false;

            if (lightningButton != null)
            {
                lightningButton.interactable = true;
            }

            if (buttonImage != null)
            {
                buttonImage.color = originalColor; // 원래 색으로 복구
            }

            Debug.Log("START 상태로 진입, 번개 사용 가능 상태로 초기화 및 버튼 활성화");
        }

        lastCombatCondition = currentCondition;
    }

    public void OnLightningButtonClicked()
    {
        if (GameManager.Instance.CombatConditionType == ECombatConditionType.START)
        {
            if (hasStruckInStart)
            {
                Debug.Log("전투 시작 상태에서 이미 번개를 한 번 쳤습니다.");
                return;
            }

            hasStruckInStart = true;
            StrikeLightning();

            if (lightningButton != null)
            {
                lightningButton.interactable = false;  // 버튼 비활성화
            }

            if (buttonImage != null)
            {
                buttonImage.color = Color.gray;        // 회색으로 변경
            }
        }
        else
        {
            Debug.Log("전투 시작 상태가 아니어서 번개를 칠 수 없습니다.");
        }
    }

    private void StrikeLightning()
    {
        if (lightningSpawner != null)
        {
            lightningSpawner.SpawnLightningOnFiveRandomMonsters();
        }

        if (lightningAudioSource != null)
        {
            lightningAudioSource.Play();
        }

        Debug.Log("번개 한 번 쳤음!");
    }
}
