using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public GameObject checkMark;
    public CharacterSelectManager selectManager;  // 매니저 연결

    public bool IsSelected { get; private set; } = false;

    public void OnClickCharacter()
    {
        // 매니저에게 클릭 알리기
        selectManager.OnCharacterClicked(this);
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
        checkMark.SetActive(selected);
    }
}
