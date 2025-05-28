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

    private List<string> availableRcodes = new List<string>() { "C001", "C002", "C003", "C004", "C005" };

    public void RollCards()
    {
        for (int i = 0; i < cardSlots.Length; i++)
        {
            var slot = cardSlots[i];

            if (i < maxAvailableCards)
            {
                string rcode = GetRandomRcode();
                string name = GetCardNameByRcode(rcode);
                string desc = GetCardDescByRcode(rcode);

                slot.cardImage.enabled = true;
                slot.SetCard(rcode, name, desc);
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

    private string GetRandomRcode()
    {
        int index = Random.Range(0, availableRcodes.Count);
        return availableRcodes[index];
    }

    private string GetCardNameByRcode(string rcode)
    {
        switch (rcode)
        {
            case "C001": return "Archer";
            case "C002": return "Knight";
            case "C003": return "Mage";
            case "C004": return "Healer";
            case "C005": return "Rogue";
            default: return "???";
        }
    }

    private string GetCardDescByRcode(string rcode)
    {
        switch (rcode)
        {
            case "C001": return "Shoots arrows at enemies.";
            case "C002": return "Tanky melee unit.";
            case "C003": return "Casts powerful spells.";
            case "C004": return "Heals allies over time.";
            case "C005": return "Fast attacker from the shadows.";
            default: return "Unknown skill.";
        }
    }

    public void PurchaseCard(StoreCardSlot slot)
    {
        if (slot.isPurchased) return;

        slot.SetEmpty();
    }
}
