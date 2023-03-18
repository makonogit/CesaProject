//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�w�i�I�u�W�F�N�g�̐����~��
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStar : MonoBehaviour
{
    //-�ϐ��錾-

    [Header("�����~�鑬�x")]
    public float FallSpeed = 10.0f;
    [Header("���ł܂ł̎���")]
    public float DestroyTime = 3.0f;
    // �o�ߎ���
    public float time = 0.0f;

    // �O���擾
    private Transform thisTransform;

    private void Start()
    {
        // Transform�擾
        thisTransform = GetComponent<Transform>();

        // ���̍~�鑬�x�������_����
        // 10�`20
        FallSpeed = Random.Range(10.0f, 20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // ���Ԍo��
        time += Time.deltaTime;

        // ���܂�Ă����莞�Ԍo�߂��������
        if(time > DestroyTime)
        {
            Destroy(this.gameObject);
        }

        // ���������
        Vector3 fall = new Vector3(-0.5f, -1.0f, 0.0f);

        // ����������Ɍ��ݒn����ړ�������
        thisTransform.Translate(fall * FallSpeed * Time.deltaTime);
    }
}
