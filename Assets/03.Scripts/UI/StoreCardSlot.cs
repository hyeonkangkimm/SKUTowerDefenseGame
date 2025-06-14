using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class StoreCardSlot : MonoBehaviour, IPointerClickHandler
{
    public RectTransform cardTransform;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI cardNameText;
    public StoreCardInfo cardInfo;
    public Image cardImage;

    [SerializeField] public GameObject store;
    [HideInInspector] public string price;
    [HideInInspector] public bool isPurchased = false;
    [HideInInspector] public HeroSO heroSO;
    public void SetCard(HeroSO hero, string price)
    {
        this.price = price;
        isPurchased = false;

        heroSO = hero; // HeroSO ¿˙¿Â

        cardNameText.text = hero.heroName;
        cardInfo.cardName = hero.heroName;
        cardInfo.cardDescription = hero.heroDescription;
        cardInfo.price = price;
        cardImage.sprite = hero.icon;

        gameObject.SetActive(true);
    }

    public void SetEmpty()
    {
        isPurchased = true;
        cardNameText.text = "";
        price = "";
        if (cardImage != null)
            cardImage.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPurchased) return;

        ShopManager shop = store.GetComponent<ShopManager>();
        if (shop != null)
        {
            shop.PurchaseCard(this);
        }
    }


}
