//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�FInputSystem�ɂ�����
//�@�@�@�FPlayerInputManager�ɂ��ꂼ��̓��͂̏�ԓn��
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    // �ړ��p
    public Vector2 L_move;     // ���X�e�B�b�N�̓��͗ʂ��擾����ϐ�
    // �Ђїp
    public Vector2 R_Push;     // �E�X�e�B�b�N�̓��͗ʂ��擾����ϐ�(Press)
    public Vector2 R_move;     // �E�X�e�B�b�N�̓��͗ʂ��擾����ϐ�
   
    // �O���擾
    private GameObject PlayerInputManager; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�

    //----------------------------------------------------------------------------------------------------------
    // - ���������� -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManager��T��
        PlayerInputManager = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
    }

    //----------------------------------------------------------------------------------------------------------
    // - �X�V���� -

    //----------------------------------------------------------------------------------------------------------
    // ����context�F�g�p��
    //if (context.phase == InputActionPhase.Started) �ݒ肵�����͂��������u�Ԃ̏��
    //if (context.phase == InputActionPhase.Performed) �ݒ肵�����͂������Ă�����
    //if (context.phase == InputActionPhase.Canceled) �ݒ肵�����͂���������u�Ԃ̏��

    //----------------------------------------------------------------
    //�߂�l�F����
    //�����@�F���̗͂l�X�ȃp�����[�^�[�����ϐ�
    public void OnMove(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        // ���͗ʂ��擾
        L_move = context.ReadValue<Vector2>();

        //---------------------------------------------------------------
        //PlayerInputManager�ɓ��͗ʂ��Z�b�g
        ScriptPIManager.SetMovement(L_move);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            //---------------------------------------------------------------
            //���������̓��ڈȍ~�̃W�����v�𕕂���
            ScriptPIManager.SetJumpTrigger(true);

            //---------------------------------------------------------------
            //�W�����v�����͂���Ă����ԂɃZ�b�g����
            ScriptPIManager.SetJump(true);
        }

        //---------------------------------------------------------------
        //�W�����v���͂��I�������u��
        if(context.phase == InputActionPhase.Canceled)
        {
            //---------------------------------------------------------------
            //PlayerInputManager�̃����o�ϐ�IsJump��true�Ȃ�
            if (ScriptPIManager.GetJump() == true)
            {
                //---------------------------------------------------------------
                //�W�����v�����͂���Ă��Ȃ���ԂɃZ�b�g����
                ScriptPIManager.SetJump(false);
            }
        }
    }

    public void OnCreateCrack(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        // ���͗ʂ��擾
        R_Push = context.ReadValue<Vector2>();

       // Debug.Log(R_move);
        
        ScriptPIManager.SetCrackPower(R_Push);
    }

    public void OnCrackMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (ScriptPIManager.GetCrackMove() == false)
            {
                ScriptPIManager.SetCrackMove(true);
            }
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            if (ScriptPIManager.GetCrackMove() == true)
            {
                ScriptPIManager.SetCrackMove(false);
            }
        }
    }

    public void OnRightMove(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        // ���͗ʂ��擾
        R_move = context.ReadValue<Vector2>();

        ScriptPIManager.SetRmove(R_move);
    }

}
