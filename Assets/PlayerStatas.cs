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
