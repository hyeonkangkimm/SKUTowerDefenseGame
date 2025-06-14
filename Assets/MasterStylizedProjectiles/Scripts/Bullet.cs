using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MasterStylizedProjectile
{
    public class Bullet:MonoBehaviour
    {
        
        public ParticleSystem OnHitEffect;
        public AudioClip bulletClip;
        public AudioClip onHitClip;
        public float Speed;
        public bool isTargeting;
        public Transform target;
        public float rotSpeed = 0;
        private void Start()
        {
            if (bulletClip != null)
            {
                var audio = gameObject.AddComponent<AudioSource>();
                audio.clip = bulletClip;
                audio.Play();
            }
        }
        private void OnTriggerEnter(Collider other)
        {

            if (OnHitEffect != null)
            {
                var onHitObj = Instantiate(OnHitEffect, transform.position, Quaternion.identity);
                var onHit = onHitObj.gameObject.AddComponent<AudioTrigger>();
                if (onHitClip != null)
                {
                    onHit.onClip = onHitClip;
                }
                
            }
            Destroy(gameObject);
        }

    }
}
