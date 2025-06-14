using DG.Tweening;
using UnityEngine;

public class DOTweenProjectile : MonoBehaviour
{
    public GameObject target;
    public float height = 2f;
    public float duration = 1f;
    public int damage = 10;

    void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned to projectile.");
            Destroy(gameObject);
            return;
        }

        Vector3 start = transform.position;
        Vector3 end = target.transform.position;

        // 경유지점 (중간에 높이 있는 지점)
        Vector3 mid = (start + end) / 2f + Vector3.up * height;

        Vector3[] path = new Vector3[] { mid, end };

        // 움직임
        transform.DOPath(path, duration, PathType.CatmullRom, PathMode.Full3D)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => {
                // 명중 시 처리
                MonsterAI ai = target.GetComponent<MonsterAI>();
                if (ai != null)
                {
                    ai.TakeDamage(damage);
                }
                Destroy(gameObject);
            });
    }
    void Update()
    {
        if (target != null)
        {
            transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear);
        }
    }
}