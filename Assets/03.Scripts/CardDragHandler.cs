using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject cardModelPrefab; // 소환할 3D 모델
    public float manaCost = 2f;
    public float cooldownDuration = 3f; // 쿨타임 시간

    [SerializeField]
    private ManaUUI manaUI;

    private RectTransform dragObject;
    private Canvas canvas;
    private Camera mainCamera;

    private Vector3 originalPosition;  // 카드의 원래 위치
    private bool isCooldown = false;
    private Image cardImage; // 카드 이미지 컴포넌트 (fillAmount 쿨타임 표시용)

    void Start()
    {
        dragObject = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;

        originalPosition = dragObject.position;
        cardImage = GetComponent<Image>();
        cardImage.fillAmount = 1f;  // 쿨타임 시작 전엔 꽉 찬 상태
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isCooldown) return; // 쿨타임 중이면 드래그 못 하게 막기
        dragObject.position = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isCooldown) return;
        dragObject.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isCooldown)
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

        // 카드 UI 위치 무조건 원래 자리로 되돌림
        dragObject.position = originalPosition;
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
        cardImage.fillAmount = 0f;  // 쿨타임 시작 시 빈 상태로 초기화

        while (elapsed < cooldownDuration)
        {
            elapsed += Time.deltaTime;
            cardImage.fillAmount = elapsed / cooldownDuration;  // 점점 채우기
            yield return null;
        }

        cardImage.fillAmount = 1f;  // 쿨타임 완료 후 꽉 채우기
        isCooldown = false;
    }
}
