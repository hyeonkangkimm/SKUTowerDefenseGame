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

    public int priceRange = 3;

    [SerializeField]private List<HeroSO> availableCards = new List<HeroSO>();

    private int[] isRolled = new int[10];
    private bool firstRolled = false;
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
        maxAvailableCards = SkillManager.Instance.CheckStoreLevel() + 4;
        priceRange = SkillManager.Instance.CheckStorePrice();
        if (firstRolled)    CheckNotRolled();
    }

    private void CheckNotRolled()
    {
        for(int i = 0; i< maxAvailableCards; i++)
        {
            if (isRolled[i] != 1)
            {
                RollTarget(i);
            }
        }
    }

    private void RollTarget(int target)
    {
        var slot = cardSlots[target];
        int random = Random.Range(1, 9);
        int price = Random.Range(1, priceRange);
        string name = availableCards[random].heroName;
        string desc = availableCards[random].heroDescription;
        Sprite heroImage = availableCards[random].icon;
        int heroID = availableCards[random].hid;

        slot.cardImage.enabled = true;
        slot.SetCard(price, name, desc, heroImage, heroID, price);

        isRolled[target] = 1;
    }

    public void RollCards()
    {
        for (int i = 0; i < maxAvailableCards; i++)
        {
            firstRolled = true;
            var slot = cardSlots[i];

            if (i < 10)
            {
                int random = Random.Range(1, 9);
                int price = Random.Range(1, priceRange);
                string name = availableCards[random].heroName;
                string desc = availableCards[random].heroDescription;
                Sprite heroImage = availableCards[random].icon;
                int heroID = availableCards[random].hid;

                slot.cardImage.enabled = true;
                slot.SetCard(price, name, desc, heroImage, heroID, price);

                isRolled[i] = 1;
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
        for (int i = 0; i < maxAvailableCards; i++)
        {
            var slot = cardSlots[i];

            if (i < 10)
            {
                int random = Random.Range(1, 9);
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
}
