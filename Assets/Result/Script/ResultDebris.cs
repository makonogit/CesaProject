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
    public Vector3 clearPos;// �e�L�X�g�������̍��W

    // �e�L�X�g�֘A
    bool isMoveFlg = false;// �ړ��J�n�t���O
    int delayCnt = 0;      // �ҋ@���Ԃ��J�E���g

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
        //--------------------------------------------------------
        // �������̋����𐧌䂷��

        // ���̃I�u�W�F�N�g��Transform���擾
        Transform objTransform = this.transform;

        // ���W�̎w��
        Vector3 pos = objTransform.position;

        if (isMoveFlg == false)
        {
            // ����������
            if (pos.y > -5.0f)
            {
                pos.y -= 0.02f;
            }

            // ��]������
            if (pos.y > -4.0f)
            {
                startRot.z += 0.1f;
            }

            // ��ʂ̉��܂ŗ�������ҋ@
            if(pos.y < -5.0f)
            {
                delayCnt++;
               
            }

            // �ҋ@���Ԃ��I�������ړ��J�n
           if(delayCnt > 60 * 8)
            {
                isMoveFlg = true;
                delayCnt = 0;
            }
        }
        else
        {

            //--------------------------------------------------------
            // �e�L�X�g�������̋����𐧌䂷��

            if (clearPos.z != 0.0f)
            {
               
                // �傫�����e�L�X�g�p�ɏ���������
                Vector3 scale = objTransform.localScale;
                scale.x = 0.8f;
                scale.y = 0.8f;
                scale.z = 1.0f;
                objTransform.localScale = scale;

                // �e�L�X�g�̈ʒu�܂ňړ�����
                if (clearPos.x > pos.x)
                {
                    pos.x += 0.01f;
                    
                }
                if (clearPos.x < pos.x)
                {
                    pos.x -= 0.01f;
                    
                }
                if (clearPos.y > pos.y)
                {
                    pos.y += 0.01f;
                    startRot.z += 0.1f;
                }
            }

           
        }

        //--------------------------------------------------------
        // Transform��G�p����

        // ��]����G�p����
        Vector3 rot;
        rot.x = 1.0f;
        rot.y = 1.0f;
        rot.z = startRot.z;
        objTransform.eulerAngles = rot;

        // ���W��G�p����
        objTransform.position = pos;

    }
    //============================================================
}


