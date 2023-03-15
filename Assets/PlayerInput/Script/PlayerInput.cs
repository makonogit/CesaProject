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

    // �J�[�\���p
    public Vector2 CL_move; // �J�[�\���p���X�e�B�b�N���͗ʂ��擾����ϐ�

    // �}�E�X�̍��W
    [SerializeField]
    private Vector2 MousePos;  //�}�E�X���W��ێ�����ϐ�

    // �O���擾
    private GameObject PlayerInputMane; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�

    private GameObject pause;
    private PauseGame pausegame;

    //----------------------------------------------------------------------------------------------------------
    // - ���������� -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManager��T��
        PlayerInputMane = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputMane.GetComponent<PlayerInputManager>();

        pause = GameObject.Find("PausePanel");
        pausegame = pause.GetComponent<PauseGame>();
    }

    //----------------------------------------------------------------------------------------------------------
    // - �X�V���� -

    //----------------------------------------------------------------------------------------------------------
    // ����context�F�g�p��
    //if (context.phase == InputActionPhase.Started) �ݒ肵�����͂��������u�Ԃ̏��
    //if (context.phase == InputActionPhase.Performed) �ݒ肵�����͂������Ă�����
    //if (context.phase == InputActionPhase.Canceled) �ݒ肵�����͂���������u�Ԃ̏��

    public void OnCursorMove(InputAction.CallbackContext context)
    {
        // �|�[�Y��Ԃ̎��̓���
        if (pausegame.IsPause == true)
        {
            if (context.phase == InputActionPhase.Started)
            {
                CL_move = context.ReadValue<Vector2>();

                // ���͂���x�Ȃ��Ȃ����玟�̓��͂��Ƃ�
                if (ScriptPIManager.GetCursorMoveFlg() == false)
                {
                    ScriptPIManager.SetCursorMove(CL_move);

                    // y���͂�0�ɂȂ�܂Ŏ��̓��͂��Ƃ�Ȃ�
                    ScriptPIManager.SetCursorMoveFlg(true);
                }
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                ScriptPIManager.SetCursorMoveFlg(false);
            }
        }
    }

    //----------------------------------------------------------------
    //�߂�l�F����
    //�����@�F���̗͂l�X�ȃp�����[�^�[�����ϐ�
    public void OnMove(InputAction.CallbackContext context)
    {
        // �|�[�Y��Ԃ���Ȃ����̓���
        if (pausegame.IsPause == false)
        {
            //---------------------------------------------------------------
            // ���͗ʂ��擾
            L_move = context.ReadValue<Vector2>();

            //---------------------------------------------------------------
            //PlayerInputManager�ɓ��͗ʂ��Z�b�g
            ScriptPIManager.SetMovement(L_move);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // �|�[�Y��Ԃ���Ȃ�����A�{�^������
        if (pausegame.IsPause == false)
        {
            if (context.phase == InputActionPhase.Started)
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
            if (context.phase == InputActionPhase.Canceled)
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
        // �|�[�Y��Ԃ̎�
        else
        {
            // �����ꂽ�ŏ��̃t���[��
            if (context.phase == InputActionPhase.Started)
            {
                // ����{�^���̓��͂�������
                ScriptPIManager.SetPressA(true);
            }
        }
    }

    //------------------------------------------------------
    //�E�X�e�B�b�N�̓��͗ʂ��擾����@�S���F�����S
    public void OnCreateCrack(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        // ���͗ʂ��擾
        R_Push = context.ReadValue<Vector2>();

        //���͂������
        if (R_Push.x < 0 || R_Push.x > 0 || R_Push.y < 0 || R_Push.y > 0)
        {
            // �v���C���[���ړ����[�h�Ȃ�
            if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.MOVE)
            {
                // �Ə����[�h�ɐ؂�ւ���
                ScriptPIManager.SetPlayerMode(PlayerInputManager.PLAYERMODE.AIM);
            }

        }
        else
        {
            // �v���C���[���Ə����[�h�Ȃ�
            if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.AIM)
            {
                // �ړ����[�h�ɐ؂�ւ���
                ScriptPIManager.SetPlayerMode(PlayerInputManager.PLAYERMODE.MOVE);
            }
        }

        // Debug.Log(R_move);

        ScriptPIManager.SetCrackPower(R_Push);
    }


    //------------------------------------------------------
    //B�{�^�����͔���(���͒��g�Ȃ�)�@�S���F�����S
    public void OnCrackMove(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        // �����ꂽ�������Z�b�g
        if (context.phase == InputActionPhase.Started)
        {
            //// �v���C���[���ړ����[�h�Ȃ�
            //if(ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.MOVE)
            //{
            //    // �Ə����[�h�ɐ؂�ւ���
            //    ScriptPIManager.SetPlayerMode(PlayerInputManager.PLAYERMODE.AIM);
            //}
            //// �v���C���[���Ə����[�h�Ȃ�
            //else if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.AIM)
            //{
            //    // �ړ����[�h�ɐ؂�ւ���
            //    ScriptPIManager.SetPlayerMode(PlayerInputManager.PLAYERMODE.MOVE);
            //}

            Debug.Log(ScriptPIManager.GetPlayerMode());

            if (pausegame.IsPause == true)
            {
                // �|�[�Y��Ԃ���Ȃ�����B�{�^������
                if (ScriptPIManager.GetPressB() == false)
                {
                    // �L�����Z���{�^�����̓Z�b�g
                    ScriptPIManager.SetPressB(true);
                }
            }

            //������ver.�Ђтɂ͂��鎞�̏���
            //if (context.phase == InputActionPhase.Started)
            //{

            //    if (ScriptPIManager.GetCrackMove() == false)
            //    {
            //        ScriptPIManager.SetCrackMove(true);
            //    }
            //}

            //if(context.phase == InputActionPhase.Performed)
            //{

            //    if (ScriptPIManager.GetCrackMove() == false)
            //    {
            //        ScriptPIManager.SetCrackMove(true);
            //    }
            //}

            //if(context.phase == InputActionPhase.Canceled)
            //{
            //    if (ScriptPIManager.GetCrackMove() == true)
            //    {
            //        ScriptPIManager.SetCrackMove(false);
            //    }
            //}
        }
    }


    //------------------------------------------------------
    //�E�X�e�B�b�N�̓��͗ʂ��擾����(�{�^����)�@�S���F�����S
    public void OnRightMove(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        // ���͗ʂ��擾
        R_move = context.ReadValue<Vector2>();

        ScriptPIManager.SetRmove(R_move);
    }


    //------------------------------------------------------
    //�}�E�X���W�擾�@�S���F�����S
    public void OnMousePos(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        //�}�E�X�̍��W���擾
        MousePos = context.ReadValue<Vector2>();

        ScriptPIManager.SetMousePos(MousePos);

    }

    //------------------------------------------------------
    //�B�ł������@�S���F�����S
    public void OnHammerNailLeft(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started)
        {
            ScriptPIManager.SetNail_Left(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            if (ScriptPIManager.GetNail_Left() == true)
            {
                ScriptPIManager.SetNail_Left(false);
            }
        }

    }

    public void OnHammerNailRight(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started)
        {
            ScriptPIManager.SetNail_Right(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            if (ScriptPIManager.GetNail_Right() == true)
            {
                ScriptPIManager.SetNail_Right(false);
            }
        }

    }



    //------------------------------------------------------
    //�n���}�[�����i���������j�S���F�����S
    public void OnHammer(InputAction.CallbackContext context)
    {

        //--------------------------------------------
        //�����Ɖ�����Ă�Ԃ�����True����...
        if (context.phase == InputActionPhase.Started)
        {
            ScriptPIManager.SetHammer(true);
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            if(ScriptPIManager.GetHammer() == true)
            {
                ScriptPIManager.SetHammer(false);
            }
        }

    }

    public void OnPause(InputAction.CallbackContext context)
    {
        // �|�[�Y�{�^���������ꂽ�ŏ��̃t���[����true
        if(context.phase == InputActionPhase.Started)
        {
            ScriptPIManager.SetPause(true);
        }
    }

}
