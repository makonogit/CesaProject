//---------------------------------
// �S���F�����S
// ���e�F�X�e�[�W�̃X�e�[�^�X�Ǘ�
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatas : MonoBehaviour
{
    [SerializeField,Header("�󂳂Ȃ��Ƃ����Ȃ��N���X�^��")]
    private int StageCrystal;

    private CameraZoom Zoom;

    private void Start()
    {
        //-------------------------------------
        //�@�X�e�[�W��̃N���X�^�����擾
        GameObject Core = GameObject.Find("Core");
        StageCrystal = Core.transform.childCount;

        if(StageCrystal == 0)
        {
            Zoom.stagestatas = null;
        }

    }

    //--------------------------------
    // �N���X�^���̐����Z�b�g����֐�
    // �����F�N���X�^���̐�
    // �߂�l�F�Ȃ�
    //--------------------------------
    public void SetStageCrystal(int _crystalnum)
    {
        StageCrystal = _crystalnum;
    }

    //------------------------------
    // �N���X�^���̐����l������֐�
    // �����F�Ȃ�
    // �߂�l�F�N���X�^���̐�
    //------------------------------
    public int GetStageCrystal()
    {
        return StageCrystal;
    }
}
