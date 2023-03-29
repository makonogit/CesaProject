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
    // �N���X�^���̏��������֐�����֐�
    // �����F�Ȃ�
    // �߂�l�F�N���X�^���̏�����
    //-------------------------------------
    public int GetCrystal()
    {
        return HaveCrystal;
    }

}
