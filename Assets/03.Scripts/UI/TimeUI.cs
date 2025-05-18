using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    private float time = 100;
    private int min;
    private int sec;
    private float timer = 0f;
    [SerializeField] private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        min = (int)(time/60);
        sec = (int)(time % 60);
        text.text = min.ToString("D2") + " " + sec.ToString("D2");

        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            timer -= 1f;
            time--;
        }
    }

    public void setTime(float timeAmount)
    {
        this.time = timeAmount;
    }
}
