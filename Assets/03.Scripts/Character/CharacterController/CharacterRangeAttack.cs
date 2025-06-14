using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterRangeAttack : MonoBehaviour
{
    [SerializeField] protected CharacterControllerH characterController;
    float CurAtk => characterController.character.StatHandler.curStat.Atk;

    public GameObject projectilePrefab;
    public Transform firePoint;
    private GameObject currentTarget;
    void Awake()
    {
        characterController = GetComponentInParent<CharacterControllerH>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentTarget == null||!currentTarget.activeSelf )&& characterController.enemiesInRange.Count > 0)
        {
            currentTarget = characterController.enemiesInRange[0];
        }
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            direction.y = 0; // 수평 회전만
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    public void OnDisable()
    {
       

    }
    public void OnEnable()
    {
        
    }
    /// <summary>
    /// 애니메이션 투사체 발사 시점에 출발
    /// </summary>
    public void OnShot()
    {
        if (currentTarget == null) return;
    
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        DOTweenProjectile dp = proj.GetComponent<DOTweenProjectile>();
        dp.InitProj((int)CurAtk);
        dp.target = currentTarget;
    }
}
