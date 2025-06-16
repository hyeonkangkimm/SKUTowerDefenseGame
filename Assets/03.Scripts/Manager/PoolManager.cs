
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.Pool;
using UnityEditor.VersionControl;
[Serializable]
public class Pool
{
    public string rcode; // key value by rcode
    public int size; // initial size
    [NonSerialized] public Transform parentTransform; // parent object
    public GameObject prefab;
}
public class PoolManager : Singleton<PoolManager>
{
    [Header("# Pool Info")]
    [SerializeField] private List<Pool> pools = new List<Pool>();
    private Dictionary<string, List<GameObject>> poolDictionary;
    [NonSerialized] public bool IsInit;
    public GameObject PoolParent;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(InitCoroutine());
    }

    public static IEnumerator TaskAsIEnumerator(System.Threading.Tasks.Task task)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.Exception != null)
        {
            throw task.Exception;
        }
    }

    private async System.Threading.Tasks.Task InitAsync()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();
        // pools에 있는 모든 오브젝트를 탐색하고 정해놓은 size만큼 프리팹을 미리 만들어 놓음
        foreach (Pool pool in pools)
        {
           //Debug.Log(pool.rcode);
            List<GameObject> list = new List<GameObject>();
            poolDictionary.Add(pool.rcode, list);
            pool.prefab = LoadManager.Instance.GetPrefab(pool.rcode);
            pool.parentTransform = PoolParent.transform;
            AddPoolObject(pool);
            
            //var path = ResourceManager.Instance.GetPath(pool.rcode, EAddressableType.PREFAB);
            //pool.prefab = await Addressables.InstantiateAsync(path, parent: pool.parentTransform).Task;
            //yield return TaskAsIEnumerator(AddPoolObject(pool));
        }
        IsInit = true;
    }

    private IEnumerator InitCoroutine()
    {
        #region NonIntro
        // TODO : 배포 시 삭제
        if (!ResourceManagerH.Instance.isInit)
        {
            //Debug.Log(Time.time+"init_before");
            ResourceManagerH.Instance.Init();
            yield return new WaitUntil(() => ResourceManagerH.Instance.isInit);
            //Debug.Log(Time.time+"init_after");
        }
        #endregion
        yield return TaskAsIEnumerator(InitAsync());
    }

    //private IEnumerator InitCoroutine()
    //{
    //    poolDictionary = new Dictionary<string, List<GameObject>>();
    //    // pools에 있는 모든 오브젝트를 탐색하고 정해놓은 size만큼 프리팹을 미리 만들어 놓음
    //    foreach (Pool pool in pools)
    //    {
    //        Debug.Log(pool.rcode);
    //        List<GameObject> list = new List<GameObject>();
    //        poolDictionary.Add(pool.rcode, list);
    //        Task task = AddPoolObject(pool);
    //        yield return new WaitUntil(() => task.IsCompleted);
    //    }
    //    IsInit = true;
    //}

    private void AddPoolObject(Pool pool) // 프리팹 생성
    {
        GameObject gameObject = new GameObject(pool.rcode);
        gameObject.transform.parent = pool.parentTransform;
        for (int i = 0; i < pool.size; i++)
        {
            GameObject poolObj = Instantiate(pool.prefab,pool.parentTransform);
            poolObj.name = pool.rcode;
            poolObj.transform.SetParent(gameObject.transform, false);
            poolObj.SetActive(false);
    
            poolDictionary[pool.rcode].Add(poolObj);
        }
    }

    // 이미 생성된 오브젝트 풀에서 프리팹을 가져옴
    public GameObject SpawnFromPool(string rcode, bool active = true)
    {
        if (!poolDictionary.ContainsKey(rcode))
        {
            return null;
        }

        GameObject poolObject = null;

        for (int i = 0; i < poolDictionary[rcode].Count; i++)
        {
            if (!poolDictionary[rcode][i].gameObject.activeSelf) // 비활성화된 오브젝트를 찾았을 때
            {
                poolObject = poolDictionary[rcode][i];
                break;
            }

            if (i == poolDictionary[rcode].Count - 1) // 모든 오브젝트가 활성화 됐을 때 
            {
                Pool pool = pools.Find(x => x.rcode == rcode);
                AddPoolObject(pool);
                poolObject = poolDictionary[rcode][i + 1];
            }
        }
       
        poolObject.gameObject.SetActive(active); // 활성화

        return poolObject;
    }
    public void HeroUpdateeInPool(int hid,bool IsUpgrade)
    {

        string rcode = "npc000" + hid;
        if (!poolDictionary.ContainsKey(rcode))
        {
            return;
        }
        GameObject poolObject = null;

        for (int i = 0; i < poolDictionary[rcode].Count; i++)
        {
            poolObject = poolDictionary[rcode][i];
            CharacterStat Gradedata = poolObject.GetComponent<Hero>().data.gradeStatModifier;
            StatHandler statHandler = poolObject.GetComponent<StatHandler>();
            if (IsUpgrade)
                statHandler.AddStatModifier(Gradedata);
            else
                statHandler.RemoveStatModifier(Gradedata);

        }
    }
    // 이미 생성된 오브젝트 풀에서 프리팹을 가져옴
    public T SpawnFromPool<T>(string rcode) where T : MonoBehaviour
    {
        if (!poolDictionary.ContainsKey(rcode))
        {
            return null;
        }

        GameObject poolObject = null;

        for (int i = 0; i < poolDictionary[rcode].Count; i++)
        {
            if (!poolDictionary[rcode][i].gameObject.activeSelf) // 비활성화된 오브젝트를 찾았을 때
            {
                poolObject = poolDictionary[rcode][i];
                break;
            }

            if (i == poolDictionary[rcode].Count - 1) // 모든 오브젝트가 활성화 됐을 때 
            {
                Pool pool = pools.Find(x => x.rcode == rcode);
                AddPoolObject(pool);
                poolObject = poolDictionary[rcode][i + 1];
            }
        }

        poolObject.gameObject.SetActive(true); // 활성화

        return poolObject.GetComponent<T>();
    }


}