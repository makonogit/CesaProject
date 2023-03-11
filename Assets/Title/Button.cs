using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // “_–Å‚³‚¹‚é‘ÎÛ
    private float nextTime;

    // “_–ÅŽüŠú[s]
    public  float interval = 0.5f;

    // ŽžŠÔ‚ðŒo‰ß‚·‚éŠÖ”
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
