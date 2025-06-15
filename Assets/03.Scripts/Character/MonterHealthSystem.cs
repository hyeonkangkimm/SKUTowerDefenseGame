using UnityEngine;
using UnityEngine.UI;

public class MonterHealthSystem : MonoBehaviour
{
    MonsterAI mob;
    public Image HpBar;
    public GameObject HpBarPrefab;
    [field: SerializeField] public float CurHealth { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }
    private float preMaxHealth;
    private Quaternion initialRotation;
    //public StatHandler statHandler;

    //public void Start()
    //{
    //    statHandler.OnStatChanged += DetectChangeMaxHealth;
    //}
    //public void OnDisable()
    //{
    //    statHandler.OnStatChanged -= DetectChangeMaxHealth;
    //}

    void Awake()
    {
        mob = GetComponent<MonsterAI>();
        initialRotation = HpBar.transform.rotation;
    }
   
    void LateUpdate()
    {
        HpBarPrefab.transform.rotation = initialRotation; 
    }

    public  void ShowCurrentHpRate()
    {
        HpBar.fillAmount = GetCurrentHpRate();
    }

   public void HealthChanged(int hp)
    {
        MaxHealth = hp;
        CurHealth = hp;
        ShowCurrentHpRate();
    }

    public bool TakeDamage(int damage, bool isCrit = false)
    {
        CurHealth = Mathf.Clamp(CurHealth - damage, 0, CurHealth);
        ShowCurrentHpRate();
        if (CurHealth == 0) return true;
        return false;
    }

    public bool TakeHeal(float amount)
    {
        if (CurHealth == MaxHealth) return false;
        CurHealth = Mathf.Min(CurHealth + amount, MaxHealth);
        ShowCurrentHpRate();
        return true;
    }
    public float GetCurrentHpRate()
    {
        return CurHealth / MaxHealth;
    }
}
