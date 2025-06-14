using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SellCardSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private HeroSO hero;
    [SerializeField]private Image cardImage;
    [SerializeField]private TextMeshProUGUI levelText;

    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button sellButton;

    public static SellCardSlot selectedSlot = null;

    private void Start()
    {
        UpdateSlot();
    }
    private void OnEnable()
    {
        UpdateSlot();
    }

    public void UpdateSlot()
    {
        int level = CardLevelManager.Instance.GetLevel(hero.hid);
        cardImage.sprite = hero.icon;

        if (level <= 0)
        {
            cardImage.color = new Color(0.4f, 0.4f, 0.4f);
            levelText.text = "";
        }
        else
        {
            cardImage.color = Color.white;
            levelText.text = $"Lv.{level}";
        }

        priceText.text = $"{GetSellPrice(level)} G";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int level = CardLevelManager.Instance.GetLevel(hero.hid);
        if (level <= 0) return;

        // 기존 선택 해제
        if (selectedSlot != null && selectedSlot != this)
        {
            selectedSlot.Unhighlight();
        }

        // 선택 토글
        if (selectedSlot == this)
        {
            Unhighlight();
            selectedSlot = null;
        }
        else
        {
            Highlight();
            selectedSlot = this;

            priceText.text = $"{GetSellPrice(level)} G";
        }
    }

    public void OnClickSell()
    {
        int level = CardLevelManager.Instance.GetLevel(hero.hid);
        if (level <= 0) return;


        ResourceManager.Instance.AddResource(GetSellPrice(level), 0, 0, 0);
        CardLevelManager.Instance.LevelDown(hero.hid);
        UpdateSlot();
        Unhighlight();
    }

    private void Highlight()
    {
        transform.localScale = Vector3.one * 1.1f;
        cardImage.color = new Color(1f, 1f, 1f, 0.8f);
    }

    private void Unhighlight()
    {
        transform.localScale = Vector3.one;
        cardImage.color = Color.white;
    }

    private int GetSellPrice(int level)
    {
        if (level >= 7) return 3;
        if (level >= 4) return 2;
        return 1;
    }

    public static void ConfirmSell()
    {
        if (selectedSlot != null)
        {
            selectedSlot.OnClickSell();
            selectedSlot = null;
        }
    }
}