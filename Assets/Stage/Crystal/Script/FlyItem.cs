//---------------------------------
//�S���F��{��
//���e�F�A�C�e���𕂗V������
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyItem : MonoBehaviour
{
    // �ϐ��錾

    [Header("�ǂ̂��炢�̑�����")]
    public float UpDownSpeed = 1.0f;
    [Header("�ǂ̂��炢�㉺�����邩")]
    public float Difference = 0.3f;

    private float initTransformY;

    // Start is called before the first frame update
    void Start()
    {
        // Y�̏������W��ێ�
        initTransformY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Mathf.PingPong(����,�㉺��)
        transform.position = new Vector3(transform.position.x, initTransformY + Mathf.PingPong(Time.time * UpDownSpeed, Difference), transform.position.z);
    }
}