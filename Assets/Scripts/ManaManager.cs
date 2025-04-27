using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{
    public int MaxMana { get; private set; } = 10; //최대마다 로직
    public int CurrentMana { get; private set; }

    private float regenInterval = 1f; //1초에 한번씩 마나 수급
    private float timer;// 시간을 재는 시계 변수

    void Start()
    {
        CurrentMana = 0; //초기 마나 세팅 0 변경가능
    }

    void Update()
    {
        timer += Time.deltaTime; //매 시간 더해짐

        if (timer >= regenInterval) //1초가 지나면, 마나를 1만큼 올리고, timer를 다시 0
        {
            timer = 0f;
            RegenerateMana(1);
        }
    }

    public bool UseMana(int amount)
    {
        if (CurrentMana < amount)
        {
            Debug.Log("마나가 부족합니다.");
            return false;
        }

        CurrentMana -= amount;
        return true;
    }

    public void RegenerateMana(int amount)
    {
        CurrentMana = Mathf.Min(CurrentMana + amount, MaxMana);
    }
}