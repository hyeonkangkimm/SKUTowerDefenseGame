using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    public StoreCardSlot[] cardSlots;
    public float animationDuration = 0.5f;
    public Vector2 animationOffset = new Vector2(0f, 100f);
    public int maxAvailableCards = 5;

    public CardLevelManager cardLevelManager;
    [SerializeField]private List<HeroSO> availableCards = new List<HeroSO>();

    public void RollCards()
    {
        maxAvailableCards = availableCards.Count;
        for (int i = 0; i < cardSlots.Length; i++)
        {
            var slot = cardSlots[i];

            if (i < maxAvailableCards)
            {
                int random = Random.Range(0, maxAvailableCards);
                HeroSO hero = availableCards[random];
                string price = Random.Range(1, 3).ToString();

                slot.cardImage.enabled = true;
                slot.SetCard(hero, price);
            }
        }

        StopAllCoroutines();
        StartCoroutine(AnimateCardsIn());
    }

    private IEnumerator AnimateCardsIn()
    {
        foreach (var slot in cardSlots)
        {
            if (!slot.gameObject.activeSelf || slot.isPurchased) continue;

            RectTransform rt = slot.cardTransform;
            CanvasGroup cg = slot.canvasGroup;

            Vector2 end = rt.anchoredPosition;
            Vector2 start = end + animationOffset;
            rt.anchoredPosition = start;
            cg.alpha = 0f;

            float elapsed = 0f;
            while (elapsed < animationDuration)
            {
                float t = elapsed / animationDuration;
                rt.anchoredPosition = Vector2.Lerp(start, end, t);
                cg.alpha = t;
                elapsed += Time.deltaTime;
                yield return null;
            }

            rt.anchoredPosition = end;
            cg.alpha = 1f;
        }
    }



    public void PurchaseCard(StoreCardSlot slot)
    {
        if (slot.isPurchased) return;

        if (cardLevelManager != null && slot.heroSO != null)
        {
            cardLevelManager.LevelUp(slot.heroSO.hid);
        }

        slot.SetEmpty();
    }
}
