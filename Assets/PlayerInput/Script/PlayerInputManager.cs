//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�Fplayer�̓���ɕK�v�ȃX�e�[�^�X���Ǘ�
//�@�@�@�F�B��PlayerInput�Ƃ����ł���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    // �ړ��p
    private Vector2 movement; // ���͗ʂ��擾����ϐ�

    //�W�����v�p
    [Header("�W�����v��")]
    public bool IsJump = false; // ���݃W�����v�{�^����������Ă��邩
    [Header("")]
    [SerializeField] bool JumpTrigger = false; // �W�����v���͗����Ȃ��Ɠ��ڈȍ~�W�����v���Ȃ��悤�ɂ���

    //�Ђѐ����p
    private Vector2 CrackPower; //�Ђт����鋭��

    //�Ђт̈ړ��p
    public bool CrackMoveflg = false; //�Ђт̈ړ��J�n�t���O

    //�E�X�e�B�b�N
    private Vector2 R_move;

    //�}�E�X���W
    private Vector2 MousePos;

    //�B�ł����ݔ���
    bool Nail_Push = false;

    //�n���}�[�̓�����������
    bool Hammer_Push = false;

    //-----------------------------------------------------
    // �|�[�Y���̓��͎擾�p�ϐ�
    // �|�[�Y�{�^���������ꂽ��
    private bool Pause = false;

    // ����{�^���������ꂽ��
    private bool PressA = false;

    // �L�����Z���{�^���������ꂽ��
    private bool PressB = false;

    // �ړ�����������
    private bool CursorMoveFlg = false;
    private Vector2 CursorMove; // �|�[�Y���̓��͗�

    // �ړ����[�h�ƏƏ����[�h������
    public enum PLAYERMODE
    {
        MOVE,  //�ړ�
        AIM,   //�Ə�
    }

    private PLAYERMODE Mode = PLAYERMODE.MOVE; // �v���C���[�̌��݂̃��[�h�����ϐ�

    //----------------------------------------------------------------------------------------------------------
    // - �Q�b�^�[ �E �Z�b�^�[ -

    //----------------------------------------------------------------------------------------------------------
    // �ړ�
    //--------------------------------------------
        //�߂�l�FVector2�^�i�f�o�C�X���͂ɂ����͗ʁj
        //�����@�F����
        public Vector2 GetMovement()
        {
            return movement;
        }

        //--------------------------------------------
        //�߂�l�F����
        //�����@�FVector2�^�i�f�o�C�X���͂ɂ����͗ʁj
        public void SetMovement(Vector2 _move)
        {
            movement = _move;
        }

    //----------------------------------------------------------------------------------------------------------
    // �W�����v
    //----------------------------------------------------------------------------------------------------------
    //�߂�l�Fbool�^�i�W�����v�̓��͂�����Ă��邩�j
    //�����@�F�Ȃ�
    public bool GetJump()
    {
        return IsJump;
    }

    //�߂�l�F����
    //�����@�Fbool�^�i�W�����v�̓��͂�����Ă��邩�j
    public void SetJump(bool _TRUEorFalse)
    {
        IsJump = _TRUEorFalse;
    }

    //----------------------------------------------------------------------------------------------------------
    //�߂�l�Fbool�^�i�W�����v�̓��͂�����Ă��邩�j
    //�����@�F�Ȃ�
    public bool GetJumpTrigger()
    {
        return JumpTrigger;
    }

    public void SetJumpTrigger(bool _TRUEorFALSE)
    {
        JumpTrigger = _TRUEorFALSE;
    }

    //----------------------------------------------------------------------------------------------------------
    // �E�X�e�B�b�N�̓��͗�
    //----------------------------------------------------------------------------------------------------------

    //�߂�l�FVector2�^�i�f�o�C�X���͂ɂ����͗ʁj
    //�����@�F����
    public Vector2 GetRmove()
    {
        return R_move;
    }

    //�߂�l�F����
    //�����@�FVector2�^�i�f�o�C�X���͂ɂ����͗ʁj
    public void SetRmove(Vector2 _R_move)
    {
        R_move = _R_move;
    }

    //----------------------------------------------------------------------------------------------------------
    // �Ђ�
    //----------------------------------------------------------------------------------------------------------

    //�߂�l�FVector2�^�i�f�o�C�X���͂ɂ����͗ʁj
    //�����@�F����
    public Vector2 GetCarackPower()
    {
        return CrackPower;
    }

    //�߂�l�F����
    //�����@�FVector2�^�i�f�o�C�X���͂ɂ����͗ʁj
    public void SetCrackPower(Vector2 _CrackPower)
    {
        CrackPower = _CrackPower;
    }

    //----------------------------------------------------------------------------------------------------------
    // �Ђт̈ړ�
    //----------------------------------------------------------------------------------------------------------
    //�߂�l�Fbool�^(�Ђтɓ����Ă��邩)
    //�����@�F�Ȃ�
    public bool GetCrackMove()
    {
        return CrackMoveflg;
    }

    //�߂�l�F����
    //�����@�Fbool�^(�Ђтɓ����Ă��邩)
    public void SetCrackMove(bool _CrackMoveflg)
    {
        CrackMoveflg = _CrackMoveflg;
    }


    //----------------------------------------------------------------------------------------------------------
    // �}�E�X
    //----------------------------------------------------------------------------------------------------------

    //�߂�l�FVector2�^�i�}�E�X�̍��W�j
    //�����@�F����
    public Vector2 GetMousePos()
    {
        return MousePos;
    }

    //�߂�l�F����
    //�����@�FVector2�^�i�}�E�X�̍��W�j
    public void SetMousePos(Vector2 _MousePos)
    {
        MousePos = _MousePos;
    }

    //----------------------------------------------------------------------------------------------------------
    // �v���C���[�̃��[�h�ϐ�
    //----------------------------------------------------------------------------------------------------------
    
    //�߂�l�FPLAYERMODE�^
    //�����@�F����
    public PLAYERMODE GetPlayerMode()
    {
        return Mode;
    }

    //�߂�l�F����
    //�����@�FPLAYERMODE�^
    public void SetPlayerMode(PLAYERMODE _mode)
    {
        Mode = _mode;
    }

    //----------------------------------------------------------------------------------------------------------
    // �B�ł����ݔ���
    //----------------------------------------------------------------------------------------------------------

    //�߂�l�Fbool(�B�ł����݂����ꂽ��)
    //�����@�F�Ȃ�
    public bool GetNail()
    {
        return Nail_Push;
    }


    //�߂�l�F����
    //�����@�Fbool(�B�ł����݂����ꂽ��)
    public void SetNail(bool _NailFlg)
    {
        Nail_Push = _NailFlg;
    }

    //----------------------------------------------------------------------------------------------------------
    // �n���}�[�̓�����������
    //----------------------------------------------------------------------------------------------------------

    //�߂�l�Fbool(�n���}�[�̓��������ꂽ��)
    //�����@�F�Ȃ�
    public bool GetHammer()
    {
        return Hammer_Push;
    }


    //�߂�l�F����
    //�����@�Fbool(�n���}�[�̓��������ꂽ��)
    public void SetHammer(bool _HammerFlg)
    {
        Hammer_Push = _HammerFlg;
    }

    public bool GetPause()
    {
        return Pause;
    }

    //-----------------------------------------------------
    // �|�[�Y�ngettersetter
    public void SetPause(bool _pause)
    {
        Pause = _pause;
    }

    public bool GetPressA()
    {
        return PressA;
    }

    public void SetPressA(bool _pressA)
    {
        PressA = _pressA;
    }

    public bool GetPressB()
    {
        return PressB;
    }

    public void SetPressB(bool _pressB)
    {
        Pause = _pressB;
    }

    public bool GetCursorMoveFlg()
    {
        return CursorMoveFlg;
    }

    public void SetCursorMoveFlg(bool _cursorMoveFlg)
    {
        CursorMoveFlg = _cursorMoveFlg;
    }

    public Vector2 GetCursorMove()
    {
        return CursorMove;
    }

    public void SetCursorMove(Vector2 Value)
    {
        CursorMove = Value;
    }

    //-----------------------------------------------------

}
