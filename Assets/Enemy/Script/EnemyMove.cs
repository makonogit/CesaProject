//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�G�̈ړ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    public float MoveDistance = 1; // �ړ��͈�
    public float MoveSpeed = 0.05f; // �ړ����x
    private Vector3 StartPosition; // �G�̊J�n�ʒu
    private float StartTime = 0.0f; // �G����������Ă���̌o�ߎ���
    public bool Stop = false; // �f�o�b�O�p �G�����̏�ɂƂǂ܂�

    // �O���擾
    private Transform thisTranform; // ���̃I�u�W�F�N�g�̍��W�����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �I�u�W�F�N�g��Transform���擾
        thisTranform = GetComponent<Transform>();

        //---------------------------------------------------------
        // �G�̊J�n�ʒu���擾
        StartPosition = thisTranform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Stop == false)
        {
            //-------------------------------------------------------------------------------------------
            // �G�̊J�n�ʒu����distance * MoveSpeed�͈̔͂ō��E�ړ�
            thisTranform.position = new Vector3(StartPosition.x + Mathf.Sin(StartTime) * MoveSpeed * MoveDistance, StartPosition.y, StartPosition.z);

            //-------------------------------------------------------------------------------------------
            //���Ԍo��
            StartTime += Time.deltaTime;
        }
    }
}
