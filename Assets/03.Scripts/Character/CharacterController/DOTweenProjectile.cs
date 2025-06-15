using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DOTweenProjectile : MonoBehaviour
{
    public GameObject target;
    public string Name;
    public float speed = 10f;
    public float rotateSpeed = 720f;
    public int damage = 10;
    public ParticleSystem OnHitEffect;
    public ParticleSystem OnOrbitEffect;

    public AudioClip bulletClip;
    public AudioClip onHitClip;

    private bool isLaunched = false;

    public void InitProj(int damage, GameObject target)
    {
        this.damage = damage;
        this.target = target;
        isLaunched = true;
        OnOrbitEffect.Play();
        OnHitEffect.Stop();
    }

    void Update()
    {
        if (!isLaunched || target == null || !target.activeSelf)
        {
            gameObject.SetActive(false);
            return;
        }

        // 방향 계산
        Vector3 direction = (target.transform.position - transform.position).normalized;

        // 회전 (LookAt 부드럽게)
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

        // 이동
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            MonsterAI ai = target.GetComponent<MonsterAI>();
            if (ai != null)
                ai.TakeDamage(damage);

            if (OnHitEffect != null)
            {
                var onHitObj = Instantiate(OnHitEffect, transform.position, Quaternion.identity);
                var onHit = onHitObj.gameObject.AddComponent<AudioTrigger>();
                if (onHitClip != null)
                    onHit.onClip = onHitClip;
            }

            StartCoroutine(DeactivateProjectile());
        }
    }
    private IEnumerator DeactivateProjectile()
    {
        OnOrbitEffect.Stop();
        yield return new WaitForSeconds(0.2f); // 이펙트 재생 시간에 맞춰 조절

        gameObject.SetActive(false);
    }
}