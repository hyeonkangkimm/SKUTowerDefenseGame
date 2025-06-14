using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int CurrentStage = 1;
    public int CurrentWave = 0;

    public int MaxWavesPerStage = 5;
    public float TimeBetweenWaves = 5f;

    public List<Transform> spawnPoints;
    //Wave별로 소환할 몬스터들의 리스트와 Icon관리
    Dictionary<string, Sprite> MonsterList=new();
    public Sprite[] IconList;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartNextWave()
    {
        CurrentWave++;

        if (CurrentWave > MaxWavesPerStage)
        {
            CurrentStage++;
            CurrentWave = 1;
        }

        SpawnWaveMonsters();
    }
    void SpawnWaveMonsters()
    {
        bool isFinalWave = (CurrentWave == MaxWavesPerStage);

        foreach (var spawnPoint in spawnPoints)
        {
            GameObject monster = PoolManager.Instance.SpawnFromPool("");
            MonsterAI monsterAI = monster.GetComponent<MonsterAI>();
            float stageMultiplier = 1 + (CurrentStage - 1) * 0.2f;
            float waveMultiplier = 1 + (CurrentWave - 1) * 0.1f;
            monsterAI.Health = (int)(monsterAI.BaseHp * stageMultiplier * waveMultiplier);
            monsterAI.Damage = (int)(monsterAI.BaseDamage * stageMultiplier * waveMultiplier);
            monsterAI.OnWaveChanged();
            for (int i = 0; i < monsterAI.DropItems.Length; i++)
            {
                DropItem drop = monsterAI.DropItems[i];
                drop.amount = (int)(stageMultiplier * waveMultiplier);
                monsterAI.DropItems[i] = drop;  
            }
            monster.transform.position = spawnPoint.position;

            // Wave/Stage 기반 스탯 적용
            
        }
    }
}
