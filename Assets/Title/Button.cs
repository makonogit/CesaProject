using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // 点滅させる対象
    private float nextTime;

    // 点滅周期[s]
    public  float interval = 0.5f;

    // 時間を経過する関数
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        nextTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>nextTime)
        {
            nextTime += interval;
        }

    }
}
