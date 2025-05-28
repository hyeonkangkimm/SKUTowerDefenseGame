using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject uiImage;  // 보여줄 이미지 오브젝트

    public void ToggleImage()
    {
        bool isActive = uiImage.activeSelf;
        uiImage.SetActive(!isActive);
    }
}
