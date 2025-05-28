using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ManaUUI : MonoBehaviour
{
    [Header("UI Components")]
    public Image manaFillImage;
    public TextMeshProUGUI manaText;

    [Header("Mana Settings")]
    public float maxMana = 10f;
    public float regenInterval = 1f;
    public float transitionSpeed = 2f;  // 애니메이션 속도 조절

    private float currentMana;

    void Start()
    {
        currentMana = maxMana;
        UpdateUI(currentMana);
        StartCoroutine(ManaRegenRoutine()); // 마나 회복 루틴 시작
    }

    // 마나 회복 루틴 (주기적으로 마나 회복)
    IEnumerator ManaRegenRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenInterval);

            if (currentMana < maxMana)
            {
                float target = Mathf.Min(currentMana + 1, maxMana);
                // 마나 회복 애니메이션 실행
                StartCoroutine(SmoothChangeMana(currentMana, target));
                currentMana = target;
            }
        }
    }

    // 마나 소모 요청 (외부에서 호출)
    public bool UseMana(float cost)
    {
        if (currentMana >= cost)
        {
            float start = currentMana;
            float target = Mathf.Max(currentMana - cost, 0);

            Debug.Log($"[ManaUI] 마나 사용 요청: {cost} | 이전 마나: {start} → 이후 마나: {target}");

            // 마나 변화 애니메이션 실행
            StartCoroutine(SmoothChangeMana(start, target));
            currentMana = target;
            return true;
        }
        else
        {
            Debug.LogWarning("[ManaUI] 마나가 부족합니다!");
            return false;
        }
    }

    // 마나 변화 애니메이션 (Smooth하게 변화)
    IEnumerator SmoothChangeMana(float start, float end)
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed; // 애니메이션 속도
            float value = Mathf.Lerp(start, end, t); // 선형 보간
            UpdateUI(value); // UI 업데이트
            yield return null;
        }

        UpdateUI(end); // 애니메이션 완료 후 최종 값으로 업데이트
    }

    // UI 업데이트 (마나 바 및 텍스트)
    void UpdateUI(float value)
    {
        if (manaFillImage != null)
        {
            manaFillImage.fillAmount = value / maxMana;  // 마나 바 채우기
        }

        if (manaText != null)
        {
            manaText.text = $"{Mathf.FloorToInt(value)} / {Mathf.FloorToInt(maxMana)}";  // 마나 텍스트 표시
        }
    }

    // 현재 마나 조회
    public float GetCurrentMana() => currentMana;
}
