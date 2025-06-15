using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonsONOFF : MonoBehaviour
{
    [SerializeField] private GameObject turnOn;
    [SerializeField] private GameObject[] turnOff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnAndOff()
    {
        if((GameManager.Instance.CombatConditionType == ECombatConditionType.START))
        {
            return;
        }

        if (this.GetComponent<AudioSource>() != null)
        {
            this.GetComponent<AudioSource>().Play();
        }

        turnOn.SetActive(true);
        for(int i = 0; i < turnOff.Length; i++)
        {
            turnOff[i].SetActive(false);
        }
    }
}
