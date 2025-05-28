using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("rcode만추가하면됨")]
    public string CardModelRcode;
    private Image cardImage;
    public GameObject cardModelPrefab;   // 3D 모델 소환용 프리팹
    public float manaCost = 2f;
    public float cooldownDuration = 3f;

    [SerializeField] private ManaUUI manaUI;          // 마나 관리 스크립트
    [SerializeField] private RectTransform manaBG;    // 드래그 취소 허용 영역 (manaBG 이미지의 RectTransform)

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
        cardImage = GetComponent<Image>();
        cardImage.fillAmount = 1f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isCooldown) return;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isCooldown || !isDragging) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        dragObject.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isCooldown || !isDragging)
        {
            ResetPosition();
            return;
        }

        // 마우스가 manaBG 영역 안에 있으면 드래그 취소
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
            if (manaUI != null && manaUI.UseMana(manaCost))
            {
                GameFlowManagerUII.Instance.CurrentTIle.PlaceCharacter(CardModelRcode);
                //Instantiate(cardModelPrefab, worldPosition, Quaternion.identity);
                StartCoroutine(StartCooldown());
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
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    private IEnumerator StartCooldown()
    {
        isCooldown = true;
        float elapsed = 0f;

        // 자신 포함 하위 모든 Image 컴포넌트 가져오기
        Image[] allImages = GetComponentsInChildren<Image>();

        foreach (var img in allImages)
        {
            img.fillAmount = 0f;
        }

        while (elapsed < cooldownDuration)
        {
            elapsed += Time.deltaTime;
            float fillValue = elapsed / cooldownDuration;

            // 모든 이미지 fillAmount 동기화
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
}
