using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeSlot : MonoBehaviour
{
    public bool unlocked;

    [SerializeField] private int requireWoodAmount;
    [SerializeField] private int requirestoneAmount;
    [SerializeField] private int requireironAmount;
    [SerializeField] private SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private Color lockedSkillColor;

    [SerializeField] private ResourceManager resource;
    [SerializeField] private Image skillImage;
    [SerializeField] private TextMeshProUGUI woodAmount;
    [SerializeField] private TextMeshProUGUI stoneAmount;
    [SerializeField] private TextMeshProUGUI ironAmount;

    // Start is called before the first frame update
    void Start()
    {
        skillImage.color = lockedSkillColor;
        //resource = ResourceManager.Instance;
        resource = FindObjectOfType<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        woodAmount.text = requireWoodAmount.ToString();
        stoneAmount.text = requirestoneAmount.ToString();
        ironAmount.text = requireironAmount.ToString();
    }

    public void UnLockSkillSlot()
    {
        if (resource.HaveEnoughResource( requireWoodAmount, requirestoneAmount, requireironAmount) == false)
            return;

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }
}
