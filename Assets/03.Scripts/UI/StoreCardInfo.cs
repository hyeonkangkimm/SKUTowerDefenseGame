using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoreCardInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Vector3 originalScale;
    public float hoverScale = 1.2f;
    [SerializeField] private TextMeshProUGUI cardPrice;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI heroCardName;

    public string cardDescription;
    public string cardName;
    public string price;
    
    public void ShowToolTip(string _skillDescription, string _skillName, string _cardPrice)
    {
        heroCardName.text = _skillName;
        skillDescription.text = _skillDescription;
        cardPrice.text = _cardPrice;
        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * hoverScale;
        ShowToolTip(cardDescription, cardName, price);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
