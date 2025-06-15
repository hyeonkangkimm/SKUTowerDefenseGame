using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public StoreCardSlot[] cardSlots;
    public float animationDuration = 0.5f;
    public Vector2 animationOffset = new Vector2(0f, 100f);
    public int maxAvailableCards = 5;

    [SerializeField] private GameObject[] storeUnlocks;

    public int priceRange = 2;

    [SerializeField]private List<HeroSO> availableCards = new List<HeroSO>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        RollCards();
    }

    void OnEnable()
    {
        for (int i = 0; i < SkillManager.Instance.CheckStoreLevel(); i++)
        {
            storeUnlocks[i].SetActive(true);
        }
    }
    private void Update()
    {
        maxAvailableCards = 4 + SkillManager.Instance.CheckStoreLevel();
    }

    public void RollCards()
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            var slot = cardSlots[i];

            if (i < maxAvailableCards)
            {
                int random = Random.Range(1, maxAvailableCards);
                int price = Random.Range(1, priceRange);
                string name = availableCards[random].heroName;
                string desc = availableCards[random].heroDescription;
                Sprite heroImage = availableCards[random].icon;
                int heroID = availableCards[random].hid;

                slot.cardImage.enabled = true;
                slot.SetCard(price, name, desc, heroImage, heroID, price);
            }
        }
    }

    public void RollCardsWithButton()
    {
        if (!ResourceManager.Instance.HaveEnoughMoney(1)) return;
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }
        for (int i = 0; i < cardSlots.Length; i++)
        {
            var slot = cardSlots[i];

            if (i < maxAvailableCards)
            {
                int random = Random.Range(1, maxAvailableCards);
                int price = Random.Range(1, priceRange);
                string name = availableCards[random].heroName;
                string desc = availableCards[random].heroDescription;
                Sprite heroImage = availableCards[random].icon;
                int heroID = availableCards[random].hid;

                slot.cardImage.enabled = true;
                slot.SetCard(price, name, desc, heroImage, heroID, price);
            }
        }
    }


    public void PurchaseCard(StoreCardSlot slot)
    {
        if (slot.isPurchased) return;

        slot.SetEmpty();
    }

    public void UpdatePriceRange()
    {
        priceRange++;
    }
}
