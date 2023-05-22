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
        startPos = objTransform.localPosition;
    }

    //============================================================
    // - �X�V���� -

    void Update()
    {
        //--------------------------------------------------------
        // �������̋����𐧌䂷��

        // ��]������
        startRot.z += 2 * Time.deltaTime;

        // ���̃I�u�W�F�N�g��Transform���擾
        Transform objTransform = this.transform;

        // ���W�̎w��
        Vector3 pos = objTransform.localPosition;
        // �^�񒆂��E�Ȃ�E�Ɉړ�����
        if(pos.x > 2.0f)
        {
            pos.x += 1.0f * Time.deltaTime;
        }
        // �^�񒆂�荶�Ȃ獶�Ɉړ�����
        if (pos.x < -2.0f)
        {
            pos.x -= 1.0f * Time.deltaTime;
        }
        // �ŏ��͏�����Ɉړ�������
        if (startPos.y > pos.y - 2.0f)
        {
         pos.y += 3.0f * Time.deltaTime;
        }

        // ��]���̎w��
        Vector3 rot;
        rot.x = 1.0f;
        rot.y = 1.0f;
        rot.z = startRot.z;

        // ���W�A��]����G�p����
        objTransform.eulerAngles = rot;
        objTransform.localPosition = pos;

        // ��ʊO�Ȃ炱�̃I�u�W�F�N�g����������
        if ((pos.y < -8.0f)|| (pos.y > 8.0f))
        {
            Destroy(this.gameObject);
        }
    }
    //============================================================

}
