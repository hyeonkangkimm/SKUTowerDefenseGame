using TMPro;
using Unity.VisualScripting;
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
    private int heroID = 0;
    private int level;

    [SerializeField] public GameObject store;
    [HideInInspector] public int price;
     public bool isPurchased = false;

    public void SetCard(int price, string name, string description, Sprite heroImage, int heroID, int level)
    {
        this.price = price;
        isPurchased = false;

        cardNameText.text = level.ToString();
        cardInfo.cardName = name;
        cardInfo.cardDescription = description;
        cardInfo.price = price.ToString();
        cardImage.sprite = heroImage;
        this.heroID = heroID;
        this.level = level;

        gameObject.SetActive(true);
    }

    public void SetEmpty()
    {
        isPurchased = true;
        cardNameText.text = "";
        price = 0;
        cardInfo.cardName = "";
        cardInfo.cardDescription = "";
        cardInfo.price = "";
        if (cardImage != null)
            cardImage.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPurchased) return;

        ShopManager shop = store.GetComponent<ShopManager>();
        if (shop != null)
        {
            if(!ResourceManager.Instance.HaveEnoughMoney(price))
            {
                return;
            }
            
            for(int i = 0; i <= level; i++)
            {
                CardLevelManager.Instance.LevelUp(this.heroID);
            }
            shop.PurchaseCard(this);
        }
    }


}
