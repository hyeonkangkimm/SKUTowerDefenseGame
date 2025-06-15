using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("카드 데이터 연결")]
    public HeroSO heroData;
    public TMP_Text manaCostText;

    [Header("하위 캐릭터 이미지에만 아이콘 적용")]
    [SerializeField] public Image characterImage;

    [SerializeField] private ManaUUI manaUI;
    [SerializeField] private RectTransform manaBG;
    
    private RectTransform dragObject;
    private Canvas canvas;
    private Camera mainCamera;

    private Vector2 originalPosition;
    private bool isCooldown = false;
    private bool isDragging = false;
    
    void Start()
    {
        dragObject = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;

        originalPosition = dragObject.anchoredPosition;

        // 아이콘 설정
        UpdateCardVisual();

        if (heroData != null)
        {
            Debug.Log($"[CardDragHandler] {heroData.heroName} 마나: {heroData.manacost}, 쿨다운: {heroData.cooldownDuration}");
        }
    }
    void Update()
    {
        if (manaUI != null && heroData != null)
        {
            float currentMana = manaUI.GetCurrentMana();  
            bool canUse = currentMana >= heroData.manacost;
            SetGrayscale(!canUse);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isCooldown) return;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isCooldown || !isDragging) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint))
        {
            dragObject.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 1. 드래그 초기 조건 확인

        if (GameManager.Instance.CurrentTile == null)
        {
            Debug.Log("CurrentTile이 null이므로 드래그 취소 및 위치 초기화");
            ResetPosition();
            isDragging = false;
            return;
        }

        if (isCooldown || !isDragging)
        {
            ResetPosition();
            isDragging = false;
            return;
        }

        // 2. manaBG에 드래그한 경우 → 드래그 취소
        if (RectTransformUtility.RectangleContainsScreenPoint(
            manaBG,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera))
        {
            Debug.Log("manaBG 영역 내 → 드래그 취소");
            CancelDrag();
            isDragging = false;
            return;
        }

        // 3. 월드 좌표 가져오기
        Vector3 worldPosition = GetMouseWorldPosition();

        if (worldPosition != Vector3.zero)
        {
            if (manaUI != null && manaUI.UseMana(heroData.manacost))
            {
                bool placedSuccessfully = false;

                if (heroData.RCode == "npcrock")
                {
                    // 벽 배치
                    placedSuccessfully = GameManager.Instance.CurrentTile.OnPlaceWall(heroData.RCode);
                }
                else
                {
                    // 캐릭터 배치
                    placedSuccessfully = GameManager.Instance.CurrentTile.OnPlaceCharacter(heroData.RCode);
                }

                if (!placedSuccessfully)
                {
                    Debug.Log("배치 실패 → 마나 복구 및 카드 원위치");
                    manaUI.UseMana(-heroData.manacost); // 마나 복구
                    CancelDrag();
                    ResetPosition();
                    isDragging = false;
                    return;
                }

                // 배치 성공 → 쿨타임 시작
                StartCoroutine(StartCooldown(heroData.cooldownDuration));
            }
            else
            {
                Debug.Log("마나 부족 → 카드 되돌림");
                ResetPosition();
                isDragging = false;
                return;
            }
        }
        else
        {
            Debug.Log("필드에 놓지 않음 → 카드 되돌림");
            ResetPosition();
            isDragging = false;
            return;
        }

        // 4. 정상 종료 시 처리
        ResetPosition();
        isDragging = false;
    }


    private void CancelDrag()
    {
        ResetPosition();
        isDragging = false;
    }

    private void ResetPosition()
    {
        dragObject.anchoredPosition = originalPosition;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private IEnumerator StartCooldown(float duration)
    {
        isCooldown = true;
        float elapsed = 0f;

        Image[] allImages = GetComponentsInChildren<Image>();

        foreach (var img in allImages)
        {
            img.fillAmount = 0f;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float fillValue = elapsed / duration;

            foreach (var img in allImages)
            {
                img.fillAmount = fillValue;
            }

            yield return null;
        }

        foreach (var img in allImages)
        {
            img.fillAmount = 1f;
        }

        isCooldown = false;
    }

    public void UpdateCardVisual()
    {
        if (heroData != null && characterImage != null)
        {
            characterImage.sprite = heroData.icon;
        }
        if (heroData != null && manaCostText != null)
        {
            manaCostText.text = heroData.manacost.ToString();
        }
    }
    private void SetGrayscale(bool isGray)
    {
        if (characterImage != null)
        {
            if (isGray)
                characterImage.color = Color.gray;  // 회색으로
            else
                characterImage.color = Color.white; // 원래대로
        }
    }

}