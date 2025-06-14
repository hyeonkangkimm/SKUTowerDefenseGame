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
    public static ResourceManager Instance { get; private set; }
    public int money;
    public int wood;
    public int stone;
    public int iron;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetResouceAmount(resourseType resouce)
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
    public void GainResource(resourseType resouce, int amount)
    {
        switch (resouce)
        {
            case resourseType.money:
                money += amount;
                break;
            case resourseType.wood:
                wood += amount;
                break;
            case resourseType.stone:
                stone += amount;
                break;
            case resourseType.iron:
                iron += amount;
                break;
        }
    }
    public void AddResource(int moneyAmount, int woodAmount, int stoneAmount, int ironAmount)
    {
        money += moneyAmount;
        wood += woodAmount;
        stone += stoneAmount;
        iron += ironAmount;
    }
}
