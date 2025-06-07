using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImageCollector : MonoBehaviour
{
    public List<Image> cardImages = new List<Image>();

    void Start()
    {
        

        // Card ~ Card5의 Image 가져오기
        for (int i = 1; i <= 5; i++)
        {
            string path = $"Card{i}/Image";
            Transform card = transform.Find(path);
            if (card != null)
            {
                Image img = card.GetComponent<Image>();
                if (img != null)
                    cardImages.Add(img);
            }
        }

        Debug.Log($"총 {cardImages.Count}개의 카드 이미지가 등록되었습니다.");
    }
}
