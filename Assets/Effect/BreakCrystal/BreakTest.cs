using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakTest : MonoBehaviour
{
    private float Wait = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<BoxCollider2D>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Wait > 0.1f)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            Wait += Time.deltaTime;
        }
    }
}
