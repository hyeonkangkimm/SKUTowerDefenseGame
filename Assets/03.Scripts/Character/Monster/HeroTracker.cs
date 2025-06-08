using UnityEngine;

public class HeroTracker : MonoBehaviour
{
    private Hero hero;
    private Vector2Int lastCell;
    
    private void Awake()
    {
       
    }
    //void Start()
    //{
    //    hero = GetComponent<Hero>();
    //    CharacterManager.Instance.Register(hero);
    //    lastCell = CharacterManager.Instance.GetCell(hero.transform.position);
    //}

    //void Update()
    //{
    //    var current = CharacterManager.Instance.GetCell(hero.transform.position);
    //    if (current != lastCell)
    //    {
    //        CharacterManager.Instance.UpdateHeroPosition(hero);
    //        lastCell = current;
    //    }
    //}

}
