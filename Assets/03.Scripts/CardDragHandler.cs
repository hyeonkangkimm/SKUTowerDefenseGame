using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("카드 데이터 연결")]
    public HeroSO heroData;

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
        if (heroData != null && characterImage != null)
        {
            characterImage.sprite = heroData.icon;
        }

        if (heroData != null)
        {
            Debug.Log($"[CardDragHandler] {heroData.heroName} 마나: {heroData.manacost}, 쿨다운: {heroData.cooldownDuration}");
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
        if (isCooldown || !isDragging)
        {
            ResetPosition();
            return;
        }

        if (RectTransformUtility.RectangleContainsScreenPoint(
            manaBG,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera))
        {
            Debug.Log("manaBG 영역 내 → 드래그 취소");
            CancelDrag();
            return;
        }

        Vector3 worldPosition = GetMouseWorldPosition();

        if (worldPosition != Vector3.zero)
        {
            bool mana=true;
            if (manaUI != null && manaUI.UseMana(heroData.manacost))
            {
                if (heroData.RCode == "rock")
                {
                    mana= GameManager.Instance.CurrentTIle.OnPlaceWall(heroData.RCode);
                    if(mana==false)
                    {
                        manaUI.UseMana(-heroData.manacost);
                    }
                    
                }
                else
                {
                    GameManager.Instance.CurrentTIle.OnPlaceCharacter(heroData.RCode);
                    
                }

                if(mana==false)
                {
                    CancelDrag();
                }
                else
                {
                    StartCoroutine(StartCooldown(heroData.cooldownDuration));
                }
                    
            }
            else
            {
                Debug.Log("마나 부족!");
            }
        }
        else
        {
            Debug.Log("필드에 놓지 않음");
        }

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
    }
}