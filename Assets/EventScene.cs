using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScene : MonoBehaviour
{
    public static EventScene Instance { get; private set; }

    [SerializeField] private GameObject randomEventUI;
    [SerializeField] private GameObject event1;
    [SerializeField] private GameObject eventafter1;
    [SerializeField] private GameObject eventafter2;
    [SerializeField] private GameObject event2;
    [SerializeField] private GameObject event3;
    [SerializeField] private GameObject event4;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) ActivateRandomEvent();
    }

    public void ActivateRandomEvent()
    {
        randomEventUI.SetActive(true);
        Time.timeScale = 0f;
        int random = Random.Range(1, 5);
        
        if(random == 1)
        {
            event1.SetActive(true);
        }
        else if(random == 2)
        {
            event2.SetActive(true);
            ResourceManager.Instance.AddResource(10,30,10,5);
        }
        else if (random == 3)
        {
            event3.SetActive(true);
            ResourceManager.Instance.PlusModier();
        }
        else if (random == 4)
        {
            event4.SetActive(true);
            ResourceManager.Instance.MinusMofier();
        }
    }

    public void TryResist()
    {
        if(ResourceManager.Instance.HaveEnoughResource(40, 15, 0))
        {
            event1.SetActive(false);
            eventafter1.SetActive(true);
        }
    }

    public void GiveUp()
    {
        event1.SetActive(false);
        eventafter2.SetActive(true);

        int woodMax = ResourceManager.Instance.GetResouceAmount(resourseType.wood);
        int stoneMax = ResourceManager.Instance.GetResouceAmount(resourseType.stone);
        int ironMax = ResourceManager.Instance.GetResouceAmount(resourseType.iron);

        ResourceManager.Instance.HaveEnoughResource((int)(woodMax*0.8), (int)(stoneMax * 0.6), (int)(ironMax * 0.5));
    }

    public void EndEvent()
    {
        eventafter1.SetActive(false);
        eventafter2.SetActive(false);
        event2.SetActive(false);
        event3.SetActive(false);
        event4.SetActive(false);
        Time.timeScale = 1f;
        randomEventUI.SetActive(false);
    }
}
