using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DOTweenProjectile : MonoBehaviour
{
    public GameObject target;
    public string Name;
    public float height = 2f;
    public float duration = 1f;
    public int damage = 10;
    public ParticleSystem OnHitEffect;
    public AudioClip bulletClip;
    public AudioClip onHitClip;
    public void InitProj(int damage)
    {
        this.damage = damage;
    }
    void Start()
    {
        CapsuleCollider col = target.GetComponent<CapsuleCollider>();
        float heightOffset = col != null ? col.height/2 : 0f;
        if (target != null)
        {
            transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear);
        }
        Vector3 start = transform.position;
        Vector3 end = target.transform.position+Vector3.up* heightOffset;

        // 경유지점 (중간에 높이 있는 지점)
        Vector3 mid = (start + end) / 2f + Vector3.up * heightOffset*2;

        Vector3[] path = new Vector3[] { mid, end };

        // 움직임
        transform.DOPath(path, duration, PathType.CatmullRom, PathMode.Full3D)
            .SetEase(Ease.InOutSine)
            .SetLookAt(0.01f)
            .OnComplete(() => {
                // 명중 시 처리
                MonsterAI ai = target.GetComponent<MonsterAI>();
                if (ai != null)
                {
                    ai.TakeDamage(damage);
                }
                gameObject.SetActive(false);
            });
    }
    void Update()
    {
       
        if (target == null|| !target.activeSelf)
        {
            this.gameObject.SetActive(false);
            return;
        }
        if (target != null)
        {
            transform.DOMove(target.transform.position, 0.5f).SetEase(Ease.Linear);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (OnHitEffect != null)
        {
            PoolManager.Instance.SpawnFromPool(Name);
            var onHitObj = Instantiate(OnHitEffect, transform.position, Quaternion.identity);
            var onHit = onHitObj.gameObject.AddComponent<AudioTrigger>();
            if (onHitClip != null)
            {
                onHit.onClip = onHitClip;
            }

        }
        this.gameObject.SetActive(false);
    }
}