using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Hero", menuName = "HeroSO")]
public class HeroSO : ScriptableObject
{
    public int hid;
    public Sprite icon;
    public int manacost;
    public float cooldownDuration;

    public string heroName;
    public string heroDescription;

    public string RCode;

    public CharacterStat multipleStat;

    public CharacterStat PassiveStat;

    public CharacterStat gradeStatModifier;
    public CharacterStat starsStatModifier;

}