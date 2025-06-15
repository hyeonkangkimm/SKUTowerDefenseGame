using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    
   

    [Header("기존 카드 관련")]
    public GameObject swapPanel;
    public Transform swapPanelContent;
    public GameObject cardPrefab;
    public CardImageCollector cardImageCollector;
    private List<CharacterButton> selectedOldCards = new List<CharacterButton>();
    private List<CharacterButton> confirmedOldSelection = new List<CharacterButton>();
    public int maxOldCardSelectCount = 2;

    [Header("새 카드 관련")]
    public GameObject newCardPrefab;
    public Transform newCardParent;
    public List<HeroSO> newHeroList;
    private List<CharacterButton> newCardButtons = new List<CharacterButton>();
    private List<CharacterButton> selectedNewCards = new List<CharacterButton>();
    private List<CharacterButton> confirmedNewSelection = new List<CharacterButton>();

    public int maxNewCardSelectCount = 2;

    [Header("새 카드 전체 UI")]
    public GameObject newCardUIParent;

    void Start()
    {
        GenerateNewCards();
    }

    // ✅ 기존 카드 클릭 처리
    public void OnOldCardClicked(CharacterButton button)
    {
        if (button.IsSelected)
        {
            selectedOldCards.Remove(button);
            button.SetSelected(false);
        }
        else
        {
            if (selectedOldCards.Count >= maxOldCardSelectCount)
                return;

            selectedOldCards.Add(button);
            button.SetSelected(true);
        }
    }

    // ✅ 새 카드 클릭 처리
    public void OnNewCardClicked(CharacterButton button)
    {
        if (button.IsSelected)
        {
            selectedNewCards.Remove(button);
            button.SetSelected(false);
        }
        else
        {
            if (selectedNewCards.Count >= maxNewCardSelectCount)
                return;

            selectedNewCards.Add(button);
            button.SetSelected(true);
        }
    }

    // ✅ 새 카드 생성
    public void GenerateNewCards()
    {
        newCardButtons.Clear();

        for (int i = 0; i < newHeroList.Count; i++)
        {
            HeroSO hero = newHeroList[i];

            GameObject cardObj = Instantiate(newCardPrefab, newCardParent, false);
            CharacterButton cb = cardObj.GetComponent<CharacterButton>();

            if (cb != null)
            {
                cb.selectManager = this;
                cb.SetSelected(false);
                cb.isOldCard = false;

                // HeroSO 연결
                cb.heroData = hero;

                // 새 카드 리스트 내 인덱스 저장
                cb.cardIndex = i;

                // 카드 이미지 설정
                Transform imageTransform = cardObj.transform.Find("Image");
                if (imageTransform != null)
                {
                    Image image = imageTransform.GetComponent<Image>();
                    if (image != null)
                    {
                        image.sprite = hero.icon;
                    }
                }

                newCardButtons.Add(cb);
            }
        }
        Debug.Log($"✅ 새 카드 {newCardButtons.Count}장 생성 완료");
    }




    // ✅ 기존 카드들을 스왑 패널에 출력
    public void OnConfirmOldCardSelection()
    {
        if (swapPanel != null)
            swapPanel.SetActive(true);

        foreach (Transform child in swapPanelContent)
        {
            Destroy(child.gameObject);
        }

        Debug.Log($"[OnConfirmOldCardSelection] cardImages.Count: {cardImageCollector.cardImages.Count}");
        Debug.Log($"[OnConfirmOldCardSelection] heroDataList.Count: {cardImageCollector.heroDataList.Count}");

        for (int i = 0; i < cardImageCollector.cardImages.Count; i++)
        {
            Image sourceImage = cardImageCollector.cardImages[i];
            HeroSO heroData = cardImageCollector.heroDataList[i];

            Debug.Log($"Creating card {i}: heroName={heroData.heroName}");

            GameObject card = Instantiate(cardPrefab, swapPanelContent);

            CharacterButton cb = card.GetComponent<CharacterButton>();
            if (cb != null)
            {
                cb.selectManager = this;
                cb.isOldCard = true;
                cb.SetSelected(false);
                cb.heroData = heroData;

                // **카드 리스트 내 인덱스 할당**
                cb.cardIndex = i;
            }

            Transform imageTransform = card.transform.Find("Image");
            if (imageTransform != null)
            {
                Image targetImage = imageTransform.GetComponent<Image>();
                if (targetImage != null && sourceImage != null)
                {
                    targetImage.sprite = sourceImage.sprite;
                    targetImage.color = sourceImage.color;
                }
            }

            Transform checkMark = card.transform.Find("CheckMark");
            if (checkMark != null)
                checkMark.gameObject.SetActive(false);
        }

        Debug.Log("기존 카드가 스왑 패널에 출력되었습니다.");
    }




    // ✅ 새 카드 선택 확정

    public void OnConfirmNewCardSelection()
    {
        Debug.Log("✅ 새 카드 선택 완료");

        // 선택된 새 카드 확정 리스트에 복사
        confirmedNewSelection.Clear();
        confirmedNewSelection.AddRange(selectedNewCards);

        foreach (var btn in confirmedNewSelection)
        {
            Debug.Log("선택된 새 카드: " + btn.name);
        }

        // 스왑 UI 켜기
        if (swapPanel != null)
            swapPanel.SetActive(true);

        // 기존 카드 리스트 출력 함수 호출
        OnConfirmOldCardSelection();
    }
    public void OnConfirmOldCardSelectionFinal()
    {
        // 선택된 기존카드를 최종 확정 리스트에 복사
        confirmedOldSelection.Clear();
        confirmedOldSelection.AddRange(selectedOldCards);

        Debug.Log("✅ 기존 카드 최종 선택 완료");
        foreach (var btn in confirmedOldSelection)
        {
            Debug.Log("선택된 기존 카드: " + btn.name);
        }
        if (newCardUIParent != null)
            newCardUIParent.SetActive(false);
        // 새카드와 기존카드 스왑 실행
        SwapSelectedCards();
    }

    // 새카드와 기존카드 스왑 함수
    private void SwapSelectedCards()
    {
        if (confirmedOldSelection.Count != confirmedNewSelection.Count)
        {
            Debug.LogWarning("기존카드와 새카드 선택 개수가 맞지 않습니다.");
            WarningPopupManager.Instance.ShowWarning("기존카드와 새카드 선택 개수가 맞지 않습니다.");
            return;
        }

        if (newHeroList == null || newHeroList.Count < confirmedOldSelection.Count)
        {
            newHeroList = new List<HeroSO>(new HeroSO[confirmedOldSelection.Count]);
        }

        Debug.Log("🌀 카드 스왑 시작");

        for (int i = 0; i < confirmedOldSelection.Count; i++)
        {
            CharacterButton oldBtn = confirmedOldSelection[i];
            CharacterButton newBtn = confirmedNewSelection[i];

            HeroSO newHeroData = newBtn.heroData;

            // 기존 카드 오브젝트 인덱스 찾기
            int matchingIndex = cardImageCollector.heroDataList.FindIndex(h => h == oldBtn.heroData);
            if (matchingIndex < 0 || matchingIndex >= cardImageCollector.cardObjects.Count)
            {
                Debug.LogWarning($"⚠️ cardObjects에서 인덱스 {matchingIndex}가 유효하지 않음");
                continue;
            }

            GameObject realCardObj = cardImageCollector.cardObjects[matchingIndex];
            CardDragHandler dragHandler = realCardObj.GetComponent<CardDragHandler>();

            if (dragHandler != null)
            {
                // 기존 카드 데이터를 새 카드 데이터로 교체
                dragHandler.heroData = newHeroData;
                dragHandler.characterImage.sprite = newHeroData.icon;
                dragHandler.UpdateCardVisual();

                Debug.Log($"✅ HeroData 교체됨: {dragHandler.heroData.heroName}");

                // CardImageCollector 내부 리스트도 갱신
                cardImageCollector.heroDataList[matchingIndex] = newHeroData;
                cardImageCollector.cardImages[matchingIndex].sprite = newHeroData.icon;

                // 스왑 후 새 카드 UI에 표시할 데이터로 기존 카드 데이터 넣기 (정확한 인덱스 사용)
                newHeroList[newBtn.cardIndex] = oldBtn.heroData;

                // 기존 카드 버튼에도 새 카드 데이터와 UI 갱신
                oldBtn.SetHeroData(newHeroData);
                oldBtn.SetSelected(false);
            }
            else
            {
                Debug.LogError($"❌ CardDragHandler가 없습니다: {realCardObj.name}");
            }
        }

        // 새 카드 버튼들에 '기존 카드 데이터(newHeroList)'로 UI 갱신
        for (int i = 0; i < newCardButtons.Count; i++)
        {
            CharacterButton btn = newCardButtons[i];
            if (i < newHeroList.Count)
            {
                btn.SetHeroData(newHeroList[i]);  // 기존 카드 데이터로 세팅
                btn.SetSelected(false);
            }
        }

        // 선택 초기화
        foreach (var btn in newCardButtons)
        {
            btn.ResetSelection();
        }

        Debug.Log("✅ 카드 스왑 완료");

        if (swapPanel != null)
            swapPanel.SetActive(false);

        selectedOldCards.Clear();
        selectedNewCards.Clear();
        confirmedOldSelection.Clear();
        confirmedNewSelection.Clear();
    }









    // ✅ 외부에서 선택된 카드 목록 접근
    public List<CharacterButton> GetSelectedOldCards() => selectedOldCards;
    public List<CharacterButton> GetSelectedNewCards() => selectedNewCards;
}
public static class TransformExtensions
{
    public static int GetSiblingIndexOfChild(this Transform parent, GameObject child)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).gameObject == child)
                return i;
        }
        return -1;
    }
}
