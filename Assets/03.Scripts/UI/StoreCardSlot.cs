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
    [HideInInspector] public string rcode;
    [HideInInspector] public bool isPurchased = false;

    public void SetCard(string newRcode, string name, string description)
    {
        rcode = newRcode;
        isPurchased = false;

        cardNameText.text = name;
        cardInfo.cardName = name;
        cardInfo.cardDescription = description;

        gameObject.SetActive(true);
    }

    public void SetEmpty()
    {
        isPurchased = true;
        cardNameText.text = "";
        rcode = "";
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
