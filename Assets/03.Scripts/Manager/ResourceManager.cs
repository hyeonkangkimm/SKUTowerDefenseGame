using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum resourseType {
    money,
    wood,
    stone,
    iron
}

public class ResourceManager : Singleton<ResourceManager>
{
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
}
