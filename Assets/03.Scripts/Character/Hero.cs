using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class HeroData
{
    public int hid;
    public Sprite icon;
    public string heroName;
    public string heroDescription;
    public string RCode;
    public CharacterStat multipleStat = new();
    public CharacterStat PassiveStat = new();
    public CharacterStat gradeStatModifier = new();
    public CharacterStat starsStatModifier = new();
    public HeroData()
    {

    }


    public HeroData(HeroSO so)
    {
        this.hid = so.hid;
        this.icon = so.icon;
        this.heroName = so.heroName;
        this.heroDescription = so.heroDescription;
        this.RCode = so.RCode;
        this.multipleStat = so.multipleStat;
        this.PassiveStat = so.PassiveStat;
        this.gradeStatModifier = so.gradeStatModifier;
        this.starsStatModifier = so.starsStatModifier;

    }
}

public class Hero : Character
{
    public HeroData data;
    public HeroSO so;
    public ERarityType rarityType;
    public int GradeLevel;
    public int StarsLevel;
    public System.Random random;
    /*public CharacterStat gradeStatModifier;
    public CharacterStat starsStatModifier;*/
    //List<Monster> monsters = new List<Monster>();

    public int UpgradeDefaultCost = 100;
    public int UpgradeIncreaseCost = 50;

    public int UpstarDefaultCost = 5;
    public int UpstarIncreaseCost = 10;


    protected override void Awake()
    {
        base.Awake();
        data = new(so);
        random = new System.Random();
        
    }

    protected override void Start()
    {
        base.Start();

        //StatHandler.baseStat = StatManager.Instance.statHandler.curStat;
        StatHandler.RemoveStatModifier(data.multipleStat);
        StatHandler.AddStatModifier(data.multipleStat);
        //Health.InitHealth(StatHandler.curStat.GetCurHealth());
        //Target = GameManager.Instance.Monsters[0].transform;

    }


    public void ChangeStat()
    {
        StatHandler.UpdateStatModifier();
    }

    public override void FindTarget()
    {
        targetList.Clear();
        
        SetTarget();
        return;
    }
    public override void SetTarget()
    {
        if (targetList.Count > 0)
        {
            int randomIndex = random.Next(targetList.Count);
            Target = targetList[randomIndex].gameObject.transform;
        }
        else
        {
            Target = null;
        }
    }
}

//    /// <summary>
//    /// 매개변수로 스킬 오브젝트까지 사라지게할지 설정가능, defalut =true;
//    /// </summary>
//    /// <param name="includeChild"></param>
//    public void ShutDown(bool includeChild =true )
//    {
//        if (includeChild)
//            HskillController.ShutDownSkill(HskillController.SkillList);//채널링끊기+스킬 오브젝트까지 날리기
//        else
//            HskillController.InterruptChaneeling();//채널링만 끊기
//    }


//    public IEnumerator ShutDownCo()
//    {
//        yield return null;
//        HskillController.ShutDownSkill(HskillController.SkillList);
//    }
//}