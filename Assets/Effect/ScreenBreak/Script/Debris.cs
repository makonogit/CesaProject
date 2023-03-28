//------------------------------------------------------------------------------
// �S���F�����V�S
// ���e�F�j��
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - �ϐ��錾 -

    // ���W�֘A
    Vector3 startRot;// �������̉�]��
    Vector3 startPos;// �������̍��W

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

        // ��]������
        startRot.z += 0.1f;

        // ���̃I�u�W�F�N�g��Transform���擾
        Transform objTransform = this.transform;

        // ���W�̎w��
        Vector3 pos = objTransform.position;
        // �^�񒆂��E�Ȃ�E�Ɉړ�����
        if(pos.x > 2.0f)
        {
            pos.x += 0.03f;
        }
        // �^�񒆂�荶�Ȃ獶�Ɉړ�����
        if (pos.x < -2.0f)
        {
            pos.x -= 0.03f;
        }
        // �ŏ��͏�����Ɉړ�������
        if (startPos.y > pos.y - 2.0f)
        {
         pos.y += 0.01f;
        }

        // ��]���̎w��
        Vector3 rot;
        rot.x = 1.0f;
        rot.y = 1.0f;
        rot.z = startRot.z;

        // ���W�A��]����G�p����
        objTransform.eulerAngles = rot;
        objTransform.position = pos;

        // ��ʊO�Ȃ炱�̃I�u�W�F�N�g����������
        if ((pos.y < -8.0f)|| (pos.y > 8.0f))
        {
            Destroy(this.gameObject);
        }
    }
    //============================================================

}
