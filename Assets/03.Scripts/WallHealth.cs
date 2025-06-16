using UnityEngine;

public class WallHealth : MonoBehaviour, IDamageable
{
    public int hp = 100;

    public void TakeDamage(int value, bool critic = false)
    {
        hp -= value;
        Debug.Log($"[벽] 데미지 {value} 받음. 현재 체력: {hp}");

        if (hp <= 0)
        {
            Debug.Log("[벽] 파괴됨!");
        }
    }

    public void TakePhysicalDamage(int damage)
    {
        TakeDamage(damage);
    }
}
