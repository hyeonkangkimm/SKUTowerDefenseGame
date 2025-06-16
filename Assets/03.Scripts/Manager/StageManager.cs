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
public class StageManager: Singleton<StageManager> 
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

    public TextMeshProUGUI SpawnLeftText;

    private WaitForSeconds SpawnInterval;
    public float SpawnIntervalTime=2f;
    Coroutine myCoroutine;


    public List<Transform> spawnPoints;
    private List<GameObject> spawnObjects;
    public Transform DestinationObject;
    private bool doubleSpawn;
    [SerializeField]public MonsetInfo[] MonsterList;
    //몬스터를 잡으면 처치보상이 있지만 스테이지를 클리어할때마다 보상이 주어져야함
    
     
    
    void Start()
    {
        SpawnInterval=new WaitForSeconds(SpawnIntervalTime);
        StartNextWave();
        spawnObjects=new List<GameObject>();
        doubleSpawn = false;
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
                GeneratingTime = 0;
                bool allDead = true;
                foreach(GameObject go in spawnObjects)
                {
                    if (go.activeSelf)
                    {
                        allDead = false;
                        break;
                    }
                }
                if (allDead)
                {
                    ResourceManager.Instance.WaveOver();
                    GameManager.Instance.CombatConditionType = ECombatConditionType.READY;
                    spawnObjects.Clear();
                }
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
                GameManager.Instance.OnNextWaveStart();
                StartNextWave();
            }
        }



        ;

    }
    public void SkipReadyTime()
    {
        TimeBetweenWaves = 0f;
    }
    public void DoubleSpawn()
    {
        doubleSpawn = !doubleSpawn;
        if (doubleSpawn)
            SpawnInterval = new WaitForSeconds(1.5f);
        else
            SpawnInterval = new WaitForSeconds(3.5f);
    }
    void StartNextWave()
    {
        //Wave변수 증가후
        CurrentWave++;
        if (CurrentWave > MaxWavesPerStage)
        {
            CurrentStage++;
            CurrentWave = 1;
            AudioManager.Instance.PlayBGM("BGM000"+CurrentStage.ToString());
        }
        //소환실행
        myCoroutine = StartCoroutine("SpawnCorotine");
        if (CurrentWave == MaxWavesPerStage)
        {
            //특수몹(신규몹) 소환 실행
            int random = UnityEngine.Random.Range(1, 3);
            if (random == 2)
            {
                EventScene.Instance.ActivateRandomEvent();
            }
        }
        //시간제한 Time 변경
        GeneratingTime = (CurrentStage * 10 + CurrentWave) * SpawnIntervalTime + 20f;
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
        monsterAI.HealthUpdate(stageMultiplier);
        monsterAI.DamageUpdate(stageMultiplier);
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
        spawnObjects.Add(monster);
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
        SpawnLeftText.text = (totalMonster).ToString();
       
        for (int i = 0; i < totalMonster; i++)
        {
            yield return SpawnInterval;
            SpawnWaveMonster();
            SpawnLeftText.text = (totalMonster - i-1).ToString();
        }

        yield return new WaitForSeconds(0f);
    }
    
    
}
