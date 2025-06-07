using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public CharacterSelectManager selectManager;
    public GameObject checkMark; // 체크 마크 오브젝트
    public bool isOldCard = false; // 기존 카드인지 여부

    private bool isSelected = false;
    public bool IsSelected => isSelected;

    private void Start()
    {
        // 체크마크 비활성화로 초기화
        if (checkMark != null)
            checkMark.SetActive(false);
    }

    public void OnClickCharacter()
    {
        if (selectManager == null)
        {
            Debug.LogError($"selectManager가 할당되지 않았습니다! {gameObject.name}에서 확인하세요.");
            return;
        }

        if (isOldCard)
        {
            selectManager.OnOldCardClicked(this); // 기존 카드 클릭 로직
        }
        else
        {
            selectManager.OnNewCardClicked(this); // 새 카드 클릭 로직
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        if (checkMark != null)
            checkMark.SetActive(selected);
    }
    public void ResetSelection()
    {
        SetSelected(false); // 선택 해제
    }
}
