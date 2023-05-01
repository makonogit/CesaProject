//-------------------------------------------
// �S���F�����V�S
// ���e�F��
//------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    //=====================================
    // *** �ϐ��錾 ***
    //=====================================

    //-------------------------------------
    // �O���Q��
    //-------------------------------------

    GameObject player;// �v���C���[

    //-------------------------------------
    // �ړ��֘A
    //-------------------------------------

    // ��]
    Vector2 center;      // ��]�̒��S���W
    float angle;         // ��]�p�x
    float radius = 0.25f;// �~�̔��a

    //-------------------------------------
    // ��Ԑ���֘A
    //-------------------------------------

    enum StateID// ���C�����ID
    {
        NULL,   // ��ԂȂ�
        NO_GET, // ���擾���
        GET,    // �擾���
        
    }
    StateID oldState = StateID.NULL;// �O�̏��
    StateID nowState = StateID.NULL; // ���݂̏��
    StateID nextState = StateID.NULL;// ���̏��

    //=====================================
    // *** ���������� ***
    //=====================================

    void Start()
    {
        //---------------------------------
        // �ϐ��̏�����
        //---------------------------------

        // ��Ԃ𖢎擾��ԂɕύX
        nowState = StateID.NO_GET;
        // ��]�̒��S���W�ɏ����ʒu��ۑ�
        center = transform.position;
        // �v���C���[�̃I�u�W�F�N�g���擾
        player = GameObject.Find("player");
    }

    //=====================================
    // *** �X�V���� ***
    //=====================================

    void Update()
    {
        //---------------------------------
        //  ���݂̏�Ԃ����̏�ԂɑJ��
        //---------------------------------

        if (nextState != StateID.NULL)
        {
            oldState = nowState;
            nowState = nextState;
            nextState = StateID.NULL;
        }

        //---------------------------------
        //  ���݂̏�Ԃɂ���ď����𕪊�
        //---------------------------------

        switch (nowState)
        {
            // ���擾���
            case StateID.NO_GET:NoGet();break;
            // �擾���
            case StateID.GET:Get();     break;
        }
    }

    //=====================================
    // *** �Փ˔��� ***
    //=====================================

    void OnTriggerEnter2D(Collider2D other)
    {
        //----------------------------------------------------
        //  ���擾��ԂŃv���C���[�ƏՓ˂�����擾��Ԃɂ���
        //----------------------------------------------------

        if(nowState == StateID.NO_GET)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                nextState = StateID.GET;
            }
        }

        //---------------------------------------------------
        //  �擾��ԂŃh�A�ɏՓ˂�����A�h�A�ƌ�����������
        //---------------------------------------------------

        if (nowState == StateID.GET)
        {
            if (other.gameObject.name == "door")
            {
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    //=====================================
    // *** ���擾��Ԃ̏��� ***
    //=====================================

    void NoGet()
    {
        //---------------------------------------------------
        //  �����㉺�ɂӂ�ӂ킳����
        //---------------------------------------------------

        // ���݂̃g�����X�t�H�[�����擾
        Vector3 pos = this.transform.position;
        // �p�x�����W�A���ɕϊ�
        float rd = -angle * Mathf.PI / 180.0f;
        // ��]��̍��W���v�Z
        //pos.x = center.x + (Mathf.Sin(rd) * radius) + radius + 0.1f;
        pos.y = center.y + (Mathf.Cos(rd) * radius) + radius + 0.1f;
        // �ύX�𔽉f
        this.transform.position = pos;
        // �p�x�����Z
        angle += 0.2f;
    }

    //=====================================
    // *** �擾��Ԃ̏��� ***
    //=====================================

    void Get()
    {
        //-------------------------------------
        // �v���C���[�̏�����Ɍ��̍��W���ړ�
        //-------------------------------------

        Vector3 pos = this.transform.position;
        pos.x = player.transform.position.x;
        pos.y = player.transform.position.y + 1.0f;
        this.transform.position = pos;
    }
}
