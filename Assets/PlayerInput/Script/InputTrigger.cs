//----------------------------------------------------------------
// �S���ҁF�����V�S
// ���e�@�F���͂̃g���K�[����
//----------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    //================================================================
    // - �ϐ� -

    // �C���v�b�g�}�l�[�W���[
    private GameObject PlayerInputMana;         // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�

    // �L�[�̓��͂�����Ă��鎞�Ԃ����ϐ�
    int jumpPushTime   = 0; // �W�����v
    int nailPushTimeLeft   = 0;   // �B�̑ł�����(��)
    int nailPushTimeRight  = 0;   // �B�̑ł�����(�E)
    float hammerPushTime = 0; // �n���}�[

    //================================================================
    // - ���������� -

    void Start()
    {
        //------------------------------------------------------------
        // �C���v�b�g�}�l�[�W���[���擾����

        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        //------------------------------------------------------------
    }

    //================================================================
    // - �W�����v���͂̃g���K�[���� -
    //
    // �����@�F����
    // �߂�l�F�L�[��������Ă��鎞�Ԃ��J�E���g����0�b�̎��̂�true��Ԃ�

    public bool GetJumpTrigger()
    {
        //------------------------------------------------------------
        // �L�[��������Ă��鎞�Ԃ��J�E���g����0�b�̎��̂�true��Ԃ�

        // �߂�l�p�ϐ�
        bool isReturn = false;

        // ������Ă���ꍇ�̏���
        if (ScriptPIManager.GetJump() == true)
        {

            // 0�b�̎��̂�true��Ԃ�
            if (jumpPushTime == 0)
            {
                isReturn = true;
            }
            else
            {
                isReturn = false;
            }

            // �J�E���g��i�߂�
            jumpPushTime++;
        }
        // ������Ă��Ȃ��ꍇ�̏���
        else
        {
            // �J�E���g�����Z�b�g
            jumpPushTime = 0;
            isReturn = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }

    //================================================================
    // - �B�ł����ݓ��͂̃g���K�[����(���E) -
    //
    // �����@�F����
    // �߂�l�F�L�[��������Ă��鎞�Ԃ��J�E���g����0�b�̎��̂�true��Ԃ�

    public bool GetNailTrigger_Left()
    {
        //------------------------------------------------------------
        // �L�[��������Ă��鎞�Ԃ��J�E���g����0�b�̎��̂�true��Ԃ�

        // �߂�l�p�ϐ�
        bool isReturn = false;

        // ������Ă���ꍇ�̏���
        if (ScriptPIManager.GetNail_Left() == true)
        {
            // 0�b�̎��̂�true��Ԃ�
            if (nailPushTimeLeft == 0)
            {
                isReturn = true;
            }
            else
            {
                isReturn = false;
            }

            // �J�E���g��i�߂�
            nailPushTimeLeft++;
        }
        // ������Ă��Ȃ��ꍇ�̏���
        else
        {
            // �J�E���g�����Z�b�g
            nailPushTimeLeft = 0;
            isReturn = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }

    public bool GetNailTrigger_Right()
    {
        //------------------------------------------------------------
        // �L�[��������Ă��鎞�Ԃ��J�E���g����0�b�̎��̂�true��Ԃ�

        // �߂�l�p�ϐ�
        bool isReturn = false;

        // ������Ă���ꍇ�̏���
        if (ScriptPIManager.GetNail_Right() == true)
        {
            // 0�b�̎��̂�true��Ԃ�
            if (nailPushTimeRight == 0)
            {
                isReturn = true;
            }
            else
            {
                isReturn = false;
            }

            // �J�E���g��i�߂�
            nailPushTimeRight++;
        }
        // ������Ă��Ȃ��ꍇ�̏���
        else
        {
            // �J�E���g�����Z�b�g
            nailPushTimeRight = 0;
            isReturn = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }


    //================================================================
    // - �n���}�[�̃g���K�[���� -
    //
    // �����@�F����
    // �߂�l�F�L�[��������Ă��鎞�Ԃ��J�E���g����0�b�̎��̂�true��Ԃ�

    public bool GetHammerTrigger()
    {
        //------------------------------------------------------------
        // �L�[��������Ă��鎞�Ԃ��J�E���g����0�b�̎��̂�true��Ԃ�

        // �߂�l�p�ϐ�
        bool isReturn = false;

        // ������Ă���ꍇ�̏���
        if (ScriptPIManager.GetHammer() == true)
        {
            // 0�b�̎��̂�true��Ԃ�
            if (hammerPushTime == 0)
            {
                isReturn = true;
            }
            else
            {
                isReturn = false;
            }

            // �J�E���g��i�߂�
            hammerPushTime+=Time.deltaTime;
        }
        // ������Ă��Ȃ��ꍇ�̏���
        else
        {
            // �J�E���g�����Z�b�g
            hammerPushTime = 0;
            isReturn = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }
    //================================================================
}
