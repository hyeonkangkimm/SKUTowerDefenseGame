using System.Collections.Generic;
using UnityEngine;

public class HeroManager11 : MonoBehaviour
{
    [SerializeField] private GameObject poolBox;

    // poolBox 안에 있는 "Player" 태그 가진 Hero들만 찾아서 리턴하는 함수
    public Hero[] GetPlayerHeroesInPoolBox()
    {
        List<Hero> playerHeroes = new List<Hero>();

        // poolBox 하위에 있는 모든 Hero 컴포넌트(비활성 포함) 가져오기
        Hero[] allHeroes = poolBox.GetComponentsInChildren<Hero>(true);

        // 하나씩 돌면서 태그가 "Player"인 것만 골라냄
        foreach (var hero in allHeroes)
        {
            if (hero.gameObject.CompareTag("Player"))
            {
                playerHeroes.Add(hero);
            }
        }

        return playerHeroes.ToArray();
    }
}
