using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmountOfResource : MonoBehaviour
{
    [SerializeField] ResourceManager resourceManager;
    [SerializeField] TextMeshProUGUI textUI;
    public resourseType myResouce;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textUI.text = resourceManager.GetResouceAmount(myResouce).ToString();
    }
}
