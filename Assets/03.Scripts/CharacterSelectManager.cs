using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    public int maxSelectCount = 3;

    private List<CharacterButton> selectedButtons = new List<CharacterButton>();
    private List<CharacterButton> confirmedSelection = new List<CharacterButton>();

    [Header("Swap UI")]
    public GameObject swapPanel;                   // 스왑 패널 (SetActive로 제어)
    public Transform swapPanelContent;             // 카드들을 넣을 Content 오브젝트
    public GameObject cardPrefab;                  // Image 포함된 카드 프리팹
    public CardImageCollector cardImageCollector;  // 기존 카드 이미지들이 담긴 스크립트

    public void OnCharacterClicked(CharacterButton button)
    {
        if (button.IsSelected)
        {
            selectedButtons.Remove(button);
            button.SetSelected(false);
        }
        else
        {
            if (selectedButtons.Count >= maxSelectCount)
                return;

            selectedButtons.Add(button);
            button.SetSelected(true);
        }
    }


    public void OnConfirmSelection()
    {
        // ✅ 선택된 버튼 저장
        confirmedSelection.Clear();
        confirmedSelection.AddRange(selectedButtons);

        Debug.Log("선택된 캐릭터 수: " + confirmedSelection.Count);
        foreach (var btn in confirmedSelection)
        {
            Debug.Log("선택된 캐릭터: " + btn.name);
        }

        // ✅ 스왑 패널 표시
        if (swapPanel != null)
        {
            swapPanel.SetActive(true);
        }

        // ✅ 이전 카드 UI 제거
        foreach (Transform child in swapPanelContent)
        {
            Destroy(child.gameObject);
        }

        // ✅ 카드 이미지 리스트로부터 UI 생성
        foreach (Image sourceImage in cardImageCollector.cardImages)
        {
            GameObject card = Instantiate(cardPrefab, swapPanelContent);
            Image targetImage = card.GetComponent<Image>();

            if (targetImage != null && sourceImage != null)
            {
                targetImage.sprite = sourceImage.sprite;
                targetImage.color = sourceImage.color;

                // 🪵 디버그 로그로 어떤 이미지인지 출력
                string spriteName = sourceImage.sprite != null ? sourceImage.sprite.name : "null";
                Debug.Log($"스왑 카드 이미지 배치됨: {spriteName}");
            }
        }

        Debug.Log("✅ 스왑패널에 카드 이미지가 모두 배치되었습니다.");
    }


    public List<CharacterButton> GetConfirmedSelection()
    {
        return confirmedSelection;
    }
}
