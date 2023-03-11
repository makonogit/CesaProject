using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // �_�ł�����Ώ�
    private float nextTime;

    // �_�Ŏ���[s]
    public  float interval = 0.5f;

    // ���Ԃ��o�߂���֐�
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
