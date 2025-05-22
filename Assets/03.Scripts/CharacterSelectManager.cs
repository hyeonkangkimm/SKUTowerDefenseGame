using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public int maxSelectCount = 3;

    private List<CharacterButton> selectedButtons = new List<CharacterButton>();

    // 버튼이 클릭될 때 호출
    public void OnCharacterClicked(CharacterButton button)
    {
        if (button.IsSelected)
        {
            // 이미 선택되어 있다면 → 해제
            selectedButtons.Remove(button);
            button.SetSelected(false);
        }
        else
        {
            // 선택하려는데 3개 초과면 막기
            if (selectedButtons.Count >= maxSelectCount)
                return;

            selectedButtons.Add(button);
            button.SetSelected(true);
        }
    }
}
