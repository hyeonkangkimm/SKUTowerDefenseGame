using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject cardModelPrefab;
    public float manaCost = 2f;
    public float cooldownDuration = 3f;

    [SerializeField] private ManaUUI manaUI;

    private RectTransform dragObject;
    private Canvas canvas;
    private Camera mainCamera;

    private Vector3 originalPosition;
    private bool isCooldown = false;
    private bool isDragging = false;
    private Image cardImage;

    void Start()
    {
        dragObject = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;

        originalPosition = dragObject.position;
        cardImage = GetComponent<Image>();
        cardImage.fillAmount = 1f;
    }

    void Update()
    {
        // 우클릭 취소는 여기서 감지!
        if (isDragging && Input.GetMouseButtonDown(1))
        {
            CancelDrag();
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
        dragObject.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isCooldown || !isDragging)
        {
            dragObject.position = originalPosition;
            return;
        }

        Vector3 worldPosition = GetMouseWorldPosition();

        if (worldPosition != Vector3.zero)
        {
            if (manaUI != null && manaUI.UseMana(manaCost))
            {
                Instantiate(cardModelPrefab, worldPosition, Quaternion.identity);
                StartCoroutine(StartCooldown());
            }
            else
            {
                Debug.Log("Not enough mana!");
            }
        }
        else
        {
            Debug.Log("드래그가 지면에 닿지 않음");
        }

        dragObject.position = originalPosition;
        isDragging = false;
    }

    private void CancelDrag()
    {
        Debug.Log("우클릭으로 드래그 취소");
        dragObject.position = originalPosition;
        isDragging = false;
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
        cardImage.fillAmount = 0f;

        while (elapsed < cooldownDuration)
        {
            elapsed += Time.deltaTime;
            cardImage.fillAmount = elapsed / cooldownDuration;
            yield return null;
        }

        cardImage.fillAmount = 1f;
        isCooldown = false;
    }
}
