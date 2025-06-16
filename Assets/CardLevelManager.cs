using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardLevelManager : MonoBehaviour
{
    public static CardLevelManager Instance { get; private set; }

    [SerializeField]private int[] cardLevels = new int[10];
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            cardLevels[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LevelUp(int hid)
    {
        if (hid >= 0 && hid < 10)
        {
            cardLevels[hid]++;
            PoolManager.Instance.HeroUpdateeInPool(hid,true);
        }
        else
        {
            Debug.LogWarning($"카드 이름 '{hid}'을 찾을 수 없습니다.");
        }
    }

    public void LevelDown(int hid)
    {
        if (hid >= 0 && hid < 10)
        {
            if (cardLevels[hid] > 0)
            {
                cardLevels[hid]--;
                PoolManager.Instance.HeroUpdateeInPool(hid, false);

            }
            else
            {
                Debug.Log($"카드 '{hid}'의 레벨은 이미 0입니다.");
            }
        }
        else
        {
            Debug.LogWarning($"카드 이름 '{hid}'을 찾을 수 없습니다.");
        }
    }

    public int GetLevel(int hid)
    {
        if (hid >= 0 && hid < 10 && cardLevels[hid] > 0)
        {
            return cardLevels[hid];
        }

        return -1; // 없는 카드일 경우
    }

}