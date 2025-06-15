using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum resourseType {
    money,
    wood,
    stone,
    iron
}

public class ResourceManager : Singleton<ResourceManager>
{
    private int resourceSkillLevel = 0;

    public int money;//카드구매
    //특성강화
    public int wood;
    public int stone;
    public int iron;

    // Start is called before the first frame update
    void Start()
    {
        //money = 0;
        //wood = 0;  
        //stone = 0;
        //iron = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GainResource(resourseType resouce,int amount)
    {
        switch (resouce)
        {
            case resourseType.money:
                money+=amount;
                break;
            case resourseType.wood:
                wood+=amount;
                break;
            case resourseType.stone:
                stone += amount;
                break;
            case resourseType.iron:
                iron += amount;
                break;
        }
    }
    public int GetResouceAmount(resourseType resouce)//UI표시
    {
        int amount = 0;
        switch(resouce)
        {
            case resourseType.money:
                amount =  money;
                break;
            case resourseType.wood:
                amount = wood;
                break;
            case resourseType.stone:
                amount = stone;
                break;
            case resourseType.iron:
                amount = iron;
                break;
        }
        return amount;
    }

    public bool HaveEnoughMoney(int price)
    {
        if (price <= money)
        {
            money -= price;
            return true;
        }
        else return false;
    }
    public bool HaveEnoughResource(int woodAmount,int stoneAmount,int ironAmount)
    {
        if (woodAmount <= wood && stoneAmount <= stone && ironAmount <= iron) {
            wood -= woodAmount;
            stone -= stoneAmount;
            iron -= ironAmount;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AddResource(int moneyAmount, int woodAmount, int stoneAmount, int ironAmount)
    {
        money += moneyAmount;
        wood += woodAmount;
        stone += stoneAmount;
        iron += ironAmount;
    }

    public void UpdateLevel()
    {
        resourceSkillLevel++;
    }

    public void WaveOver()
    {
        if(resourceSkillLevel == 1)
        {
            AddResource(0, 10, 0, 0);
        }
        if (resourceSkillLevel == 2)
        {
            AddResource(0, 10, 5, 0);
        }
        if (resourceSkillLevel == 3)
        {
            AddResource(0, 25, 5, 0);
        }
        if (resourceSkillLevel == 4)
        {
            AddResource(0, 25, 15, 0);
        }
        if (resourceSkillLevel == 5)
        {
            AddResource(0, 25, 15, 5);
        }
        if (resourceSkillLevel >= 6)
        {
            AddResource(0, 25, 15, 10);
        }
    }
}
