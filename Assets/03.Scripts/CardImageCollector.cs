using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Diagnostics;

public class CardImageCollector : MonoBehaviour
{
    public GameObject cardPrefab;             // 카드 프리팹
    public Transform parentTransform;         // 카드가 생성될 부모 오브젝트 (예: ManaBG의 Content)
    public List<GameObject> cardObjects;
    public List<Image> cardImages = new List<Image>();
    public List<HeroSO> heroDataList = new List<HeroSO>(); // ✅ 추가

    // 이미지와 HeroSO를 매칭해서 저장
    public void SetupCards(List<HeroSO> heroes)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            GameObject card = Instantiate(cardPrefab, parentTransform); // 카드 생성
            HeroSO hero = heroes[i];

            cardObjects.Add(card); // 💡 카드 오브젝트 저장

            Image img = card.GetComponentInChildren<Image>();
            cardImages.Add(img);

            CardDragHandler handler = card.GetComponent<CardDragHandler>();
            if (handler != null)
            {
                handler.heroData = hero;
                handler.characterImage = img;
                handler.UpdateCardVisual();
            }

            heroDataList.Add(hero);
        }
    }
}
