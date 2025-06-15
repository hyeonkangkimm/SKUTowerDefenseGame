using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class MonsetInfo
{
    public string Name;
    public string Rcode;
    public string Description;
    public Sprite Icon;
    MonsetInfo(string name, Sprite icon)
    {
        Name = name; Icon = icon;
    }
}
public class StageManager : MonoBehaviour
{
    [Header("StageField")]
    public int CurrentStage = 1;
    public int CurrentWave = 0;
    public int MaxWavesPerStage = 3;

    [Header("TimeField")]
    public float TimeBetweenWaves=60f;
    public float GeneratingTime;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI WaveText;
    [NonSerialized]public TextMeshProUGUI SpawnLeftText;

    private WaitForSeconds SpawnInterval;
    public float SpawnIntervalTime=2f;
    Coroutine myCoroutine;


    public List<Transform> spawnPoints;
    public Transform DestinationObject;
    
    [SerializeField]public MonsetInfo[] MonsterList;
    //몬스터를 잡으면 처치보상이 있지만 스테이지를 클리어할때마다 보상이 주어져야함
    
     
    
    void Start()
    {
        SpawnInterval=new WaitForSeconds(SpawnIntervalTime);
        StartNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CombatConditionType == ECombatConditionType.START)
        {

            //전투중
            GeneratingTime -= Time.deltaTime;
            if (GeneratingTime <= 0)
            {
                GameManager.Instance.CombatConditionType = ECombatConditionType.READY;
            }

                int minutes = (int)(GeneratingTime / 60);
                int seconds = (int)(GeneratingTime % 60);

                TimeText.text = string.Format("{0:D2} {1:D2}", minutes, seconds);

        }
        else if(GameManager.Instance.CombatConditionType == ECombatConditionType.READY)
        {
            //다음 라운드 대기중 60초
            TimeBetweenWaves-=Time.deltaTime;
            int minutes = (int)(TimeBetweenWaves / 60);
            int seconds = (int)(TimeBetweenWaves % 60);
            TimeText.text = string.Format("{0:D2} {1:D2}", minutes, seconds);

            if (TimeBetweenWaves <= 0)
            {
                GameManager.Instance.CombatConditionType = ECombatConditionType.START;
                StartNextWave();
            }
        }



        ;

    }
    void StartNextWave()
    {
        //Wave변수 증가후
        CurrentWave++;
        if (CurrentWave > MaxWavesPerStage)
        {
            CurrentStage++;
            CurrentWave = 1;
        }
        //소환실행
        myCoroutine = StartCoroutine("SpawnCorotine");
        if (CurrentWave == MaxWavesPerStage)
        {
            //특수몹(신규몹) 소환 실행
        }
        //시간제한 Time 변경
        GeneratingTime = (CurrentStage * 10 + CurrentWave) * 2 + 40f;
        TimeBetweenWaves = 60f;
        WaveText.text = $"Stage:{CurrentStage} || Wave:{CurrentWave}/{MaxWavesPerStage}";
    }
    void SpawnWaveMonster()
    {
        //진행도에 따라서 생성할 몬스터를 선택
        GameObject monster = PoolManager.Instance.SpawnFromPool(ChooseMob(CurrentStage),false);
        
        //진행도에 따라 스탯초기화 refactor(stage를 주고 Monster에서 변경하는게 자연스러울듯)
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
        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPoints[UnityEngine.Random.Range(0, 4)].position, out hit, 2f, NavMesh.AllAreas))
            monster.transform.position = hit.position;
        monsterAI.targetDestination = DestinationObject;
        // Wave/Stage 기반 스탯 적용
        monster.SetActive(true);
    }
    string ChooseMob(int stage)
    {
        List<string> availableRcodes = MonsterList.Take(stage).Select(m => m.Rcode).ToList(); // A~E 중 stage 수만큼 선택

        // 가중치 계산 (A가 가장 높고 E가 가장 낮음)
        int totalWeight = 0;
        List<int> weights = new List<int>();
        for (int i = 0; i < availableRcodes.Count; i++)
        {
            int weight = (availableRcodes.Count - i) * 3; // A:15, B:12, C:9 ... 식
            weights.Add(weight);
            totalWeight += weight;
        }

        int rand = UnityEngine.Random.Range(0, totalWeight);
        int cumulative = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            cumulative += weights[i];
            if (rand < cumulative)
            {
                return availableRcodes[i];
            }
        }
        return availableRcodes[0]; // fallback
    }
    IEnumerator SpawnCorotine()
    {
        int totalMonster = CurrentStage * 10 + CurrentWave;
        for (int i = 0; i < totalMonster; i++)
        {
            //SpawnLeftText.text = (totalMonster - i).ToString();
            yield return SpawnInterval;
            SpawnWaveMonster();
        }

        yield return new WaitForSeconds(0f);
    }
    
    
}
