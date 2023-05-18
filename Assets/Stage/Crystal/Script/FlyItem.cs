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

    [Header("���b�ň��]���邩")]
    private float rotateSecond = 10f;

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
        var rot = 360f * Time.deltaTime / rotateSecond;

        // Mathf.PingPong(����,�㉺��)
        transform.position = new Vector3(transform.position.x, initTransformY + Mathf.PingPong(Time.time * UpDownSpeed, Difference), transform.position.z);
        transform.Rotate(0f, rot, 0f);
    }
}