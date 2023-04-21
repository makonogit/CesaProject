//------------------------------------------------------------------------------
// �S���F�����V�S
// ���e�F���U���g�̔j��
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDebris : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - �ϐ��錾 -

    // ���W�֘A
    Vector3 startRot;       // �������̉�]��
    Vector3 startPos;       // �������̍��W
    //public Vector3 clearPos;// �e�L�X�g�������̍��W

    // �e�L�X�g�֘A
    bool isMoveFlg = false;// �ړ��J�n�t���O
    int delayCnt = 0;      // �ҋ@���Ԃ��J�E���g

    int rndX;
    int rndY;

    //============================================================
    // - ���������� -

    void Start()
    {
        //-------------------------------------------------------
        // �����ʒu��ۑ�����

        Transform objTransform = this.transform;
        startRot = objTransform.eulerAngles;
        startPos = objTransform.position;
    }

    //============================================================
    // - �X�V���� -

    void Update()
    {
        // ���̃I�u�W�F�N�g��Transform���擾
        Transform objTransform = this.transform;

        // ���W�̎w��
        Vector3 pos = objTransform.position;


        if (isMoveFlg == false)
        {

            rndX = Random.Range(-3, 3 + 1);
            rndY = Random.Range(-3, 3 + 1);

            pos.x = startPos.x + 0.01f * rndX;
            pos.y = startPos.y + 0.01f * rndY;

            isMoveFlg = true;
        }

        pos.x += 0.001f * rndX;
        pos.y += 0.001f * rndY;

        // ���W��G�p����
        objTransform.position = pos;

        Vector3 rot;
        rot.x = 1.0f;
        rot.y = 1.0f;
        rot.z = startRot.z += 0.05f;
        objTransform.eulerAngles = rot;

    }
}


