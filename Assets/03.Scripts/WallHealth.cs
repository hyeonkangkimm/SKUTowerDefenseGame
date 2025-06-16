using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // 씬 전환에 필요
using System.Collections;

public class WallHealth : MonoBehaviour, IDamageable
{
    public int maxHP = 20;
    public int currentHP = 20;

    [Header("UI")]
    public Image hpBarImage; // UI Image (Fill 형식)

    private Coroutine hpBarCoroutine;

    private void Start()
    {
        UpdateHPBarInstant();
    }

    public void TakeDamage(int value, bool critic = false)
    {
        currentHP -= value;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log($"[벽] 데미지 {value} 받음. 현재 체력: {currentHP}");

        if (hpBarCoroutine != null)
        {
            StopCoroutine(hpBarCoroutine);
        }
        hpBarCoroutine = StartCoroutine(AnimateHPBar());

        if (currentHP <= 0)
        {
            Debug.Log("[벽] 파괴됨!");
            LoadEndScene();
        }
    }

    public void TakePhysicalDamage(int damage)
    {
        TakeDamage(damage);
    }

    private void UpdateHPBarInstant()
    {
        if (hpBarImage != null)
        {
            float fill = (float)currentHP / maxHP;
            hpBarImage.fillAmount = fill;
        }
    }

    private IEnumerator AnimateHPBar()
    {
        if (hpBarImage == null)
            yield break;

        float targetFill = (float)currentHP / maxHP;
        float startFill = hpBarImage.fillAmount;
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            hpBarImage.fillAmount = Mathf.Lerp(startFill, targetFill, elapsed / duration);
            yield return null;
        }

        hpBarImage.fillAmount = targetFill;
    }

    private void LoadEndScene()
    {
        SceneManager.LoadScene("EndScence");  // "EndScene"은 빌드 세팅에 등록된 씬 이름
    }
}
