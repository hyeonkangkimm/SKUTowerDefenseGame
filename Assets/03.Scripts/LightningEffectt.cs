using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    public int damage = 2;  // 양수로 바꿈
    public float radius = 1.5f;
    public LayerMask enemyLayer;

    void Start()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        foreach (Collider hit in hits)
        {
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Debug.Log($"번개로 데미지 줌: {damage}");
            }
        }

        Destroy(gameObject, 1.5f);
    }
}
