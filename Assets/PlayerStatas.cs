//---------------------------------
//�S���F�����S
//���e�F�v���C���[�̃X�e�[�^�X�Ǘ�
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatas : MonoBehaviour
{
    [SerializeField,Header("�B������")]
    private int HaveNail;

    [SerializeField, Header("�N���X�^��������")]
    private int HaveCrystal;

    // ��{�ǉ�
    private int BreakCrystalNum = 0; // �󂵂��N���X�^���̐�

    [SerializeField] private bool HitStop = false; // �q�b�g�X�g�b�v����]

    [SerializeField]private int NowRespawnNumber = 0; // �ŐV�̃��X�|�[���n�_�̍��W�����I�u�W�F�N�g�̌ŗL�ԍ�
    [SerializeField]private Vector3 PlayerRespawnPos; // �v���C���[�̍ŐV���X�|�[�����W��ێ�

    public void SetRespawnNum(int _num)
    {
        NowRespawnNumber = _num;
    }

    public int GetNowRespawnNum()
    {
        return NowRespawnNumber;
    }

    // ���X�|�[�����W��������
    public Vector3 GetRespawn()
    {
        return PlayerRespawnPos;
    }

    // ���X�|�[�����W��ݒ�
    public void SetRespawn(Vector3 _respawn)
    {
        PlayerRespawnPos = _respawn;
    }

    public void AddBreakCrystal()
    {
        BreakCrystalNum++;
    }

    public int GetBreakCrystalNum()
    {
        return BreakCrystalNum;
    }

    //------------------------------
    // �B�̏��������Z�b�g����֐�
    // �����F�B�̏�����
    // �߂�l�F�Ȃ�
    //------------------------------
    public void SetNail(int _nail)
    {
        HaveNail = _nail;
    }

    //------------------------------
    // �B�̏��������l������֐�
    // �����F�Ȃ�
    // �߂�l�F�B�̏�����
    //------------------------------
    public int GetNail()
    {
        return HaveNail;
    }


    //-------------------------------------
    // �N���X�^���̏��������Z�b�g����֐�
    // �����F�N���X�^���̏�����
    // �߂�l�F�Ȃ�
    //-------------------------------------
    public void SetCrystal(int _crystal)
    {
        HaveCrystal = _crystal;
    }

    //-------------------------------------
    // �N���X�^���̏��������擾����֐�
    // �����F�Ȃ�
    // �߂�l�F�N���X�^���̏�����
    //-------------------------------------
    public int GetCrystal()
    {
        return HaveCrystal;
    }

    public bool IsHitStop()
    {
        return HitStop;
    }

    public bool GetHitStop()
    {
        return HitStop;
    }

    public void SetHitStop(bool value)
    {
        HitStop = value;
    }
}
