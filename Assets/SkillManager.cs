using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
    public int storeSlotLevel = 0;
    public int storePrice = 3;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSlot()
    {
        storeSlotLevel++;
    }

    public void PriceUp()
    {
        storePrice++;
    }

    public int CheckStoreLevel()
    {
        return storeSlotLevel;
    }

    public int CheckStorePrice()
    {
        return storePrice;
    }
}
