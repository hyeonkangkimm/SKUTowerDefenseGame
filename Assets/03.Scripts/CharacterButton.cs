using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public int cardIndex;
    public HeroSO heroData;
    public CharacterSelectManager selectManager;
    public GameObject checkMark; // 체크 마크 오브젝트
    public bool isOldCard = false; // 기존 카드인지 여부

  

    // 새로 추가: 카드 이미지 표시용 Image 컴포넌트
    public Image characterImage;

    private bool isSelected = false;
    public bool IsSelected => isSelected;
    private void Awake()
    {
        // 카드 이미지 컴포넌트는 여기서 캐싱해두기 (Image 오브젝트가 자식에 있다고 가정)
        Transform imageTransform = transform.Find("Image");
        if (imageTransform != null)
            characterImage = imageTransform.GetComponent<Image>();
    }

    public void SetHeroData(HeroSO hero)
    {
        heroData = hero;
        if (characterImage != null && heroData != null)
        {
            characterImage.sprite = heroData.icon;
        }
    }
    private void Start()
    {
        if (checkMark != null)
            checkMark.SetActive(false);

        RefreshUI(); // 시작할 때 UI 갱신
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
            selectManager.OnOldCardClicked(this);
        }
        else
        {
            selectManager.OnNewCardClicked(this);
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
        SetSelected(false);
    }

    public void ShowTooltip()
    {
        if (heroData != null)
        {
            Debug.Log("이름: " + heroData.heroName);
            Debug.Log("설명: " + heroData.heroDescription);
        }
    }

    // 새로 추가: UI 이미지 갱신 메서드
    public void RefreshUI()
    {
        if (characterImage != null && heroData != null)
        {
            characterImage.sprite = heroData.icon;
        }
    }

  
   
}
