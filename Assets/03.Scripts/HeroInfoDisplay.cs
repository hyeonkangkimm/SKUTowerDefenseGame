using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class HeroInfoDisplay : MonoBehaviour
{
    [SerializeField] private HeroManager11 heroManager;  // 할당 필수
    [SerializeField] private TMP_Text infoText;          // 정보를 출력할 텍스트 (한 곳에 다 표시)

    private void Start()
    {
        if (heroManager == null)
            Debug.LogError("HeroManager가 할당되지 않았습니다.");

        if (infoText != null)
        {
            Color color = infoText.color;
            color.a = 0.5f;  // 알파값 0.5 (50% 투명)
            infoText.color = color;
        }
    }
    private void Update()
    {
        DisplayHeroesInfo();
    }

    private void DisplayHeroesInfo()
    {
        if (heroManager == null || infoText == null)
            return;

        Hero[] heroes = heroManager.GetPlayerHeroesInPoolBox();

        HashSet<HeroSO> seenHeroes = new HashSet<HeroSO>();
        string display = "";
        int count = 0;

        foreach (var hero in heroes)
        {
            if (seenHeroes.Contains(hero.so))
                continue;

            seenHeroes.Add(hero.so);

            string heroInfo = $"[{hero.so.heroName}] Atk:{hero.StatHandler.curStat.Atk}, Health:{hero.StatHandler.curStat.Health} ";
            display += heroInfo;

            count++;
            if (count % 2 == 0)
                display += "\n";
        }

        infoText.text = display;
    }


}
