using UnityEngine;
using UnityEngine.UI;
//마나 UI<수정 필요>
public class ManaUI : MonoBehaviour
{
    public Text manaText;
    private ManaManager manaManager;

    void Start()
    {
        manaManager = FindObjectOfType<ManaManager>();
    }

    void Update()
    {
        manaText.text = $"마나: {manaManager.CurrentMana} /            {manaManager.MaxMana}";
    }
}