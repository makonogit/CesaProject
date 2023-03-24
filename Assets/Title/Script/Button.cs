//----------------------------------
//�S���F�����S
//���e�F�{�^���̓_�ŏ���
//----------------------------------

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
    public float time = 0.0f;

    //�_�ł�����I�u�W�F�N�g
    private GameObject ButtonPushUI;

    //�_�ł�����I�u�W�F�N�g��SpriteRender
    private SpriteRenderer renderer;

    //�����x
    private float Alpha = 1.0f;

    //�_�Ńt���O
    public bool Flash = false;

    // Start is called before the first frame update
    void Start()
    {
        //�_�ł�����I�u�W�F�N�g�̏����擾
        ButtonPushUI = GameObject.Find("Button");
        renderer = ButtonPushUI.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //���Ԍv��
        time += Time.deltaTime;
        renderer.color = new Color(1.0f, 1.0f, 1.0f, Alpha);

        //0.5�b�o�ߖ��ɓ_�Ńt���O��ύX
        if(Alpha >= 1.0f)
        {
            Flash = false;
        }
        if (Alpha <= 0.0f)
        {
            Flash = true;
        }


        if (Flash)
        {
            Alpha += 1.0f * Time.deltaTime;
        }
        else
        {
            Alpha -= 1.0f * Time.deltaTime;
        }
    }
}
