//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�{�X�Ē��펞�̏�����
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBossInit : MonoBehaviour
{
    // �ϐ��錾

    // �{�X�̏������֐�������X�N���v�g���蓖��
    [Header("�X�e�[�W�ɑ��݂���{�X�̏������֐������X�N���v�g�̂݊��蓖��")]
    [SerializeField,Header("���{�X")] private TownBossMove _townMove;               // ���{�X
    [SerializeField,Header("�X�{�X")] private IceBoss _iceMove;                     // �X�{�X
    [SerializeField,Header("�����{�X")] private DesertBossMove _desertMove;         // �����{�X
    [SerializeField,Header("���A�{�X")] private StateManager_CaveBoss _caveMove;    // ���A�{�X
    [SerializeField,Header("�H��{�X")] private PlantBossInit _plantInit;           // �H��{�X

    // ���݂���X�e�[�W���擾���邽��
    private SetStage setStage = new SetStage();

    public void RespawnInit()
    {
        var AreaNum = setStage.GetAreaNum();
        var StageNum = setStage.GetStageNum();

        switch (AreaNum)
        {
            // ��
            case 0:
                // �{�X�X�e�[�W�Ȃ�
                if(StageNum == 4)
                {
                    if (_townMove != null)
                    {
                        // ���{�X������
                        _townMove.Init();
                    }
                    else
                    {
                        Debug.Log("������������X�N���v�g���Z�b�g����Ă��܂���(��)");
                    }
                }
                break;

            // �X
            case 1:
                // �{�X�X�e�[�W�Ȃ�
                if (StageNum == 4)
                {
                    if (_iceMove != null)
                    {
                        // �X�{�X������
                        _iceMove.Init();
                    }
                    else
                    {
                        Debug.Log("������������X�N���v�g���Z�b�g����Ă��܂���(�X)");
                    }
                }
                break;

            // ����
            case 2:
                // �{�X�X�e�[�W�Ȃ�
                if (StageNum == 4)
                {

                    if (_desertMove != null)
                    {
                        // �����{�X������
                        _desertMove.DesertBossInit();
                    }
                    else
                    {
                        Debug.Log("������������X�N���v�g���Z�b�g����Ă��܂���(����)");
                    }

                }
                break;

            // ���A
            case 3:
                // �{�X�X�e�[�W�Ȃ�
                if (StageNum == 4)
                {

                    if (_caveMove != null)
                    {
                        // ���A�{�X������
                        _caveMove.Init();
                    }
                    else
                    {
                        Debug.Log("������������X�N���v�g���Z�b�g����Ă��܂���(���A)");
                    }

                }
                break;

            // �H��
            case 4:
                // �{�X�X�e�[�W�Ȃ�
                if (StageNum == 4)
                {

                    if (_plantInit != null)
                    {
                        // �H��{�X������
                        _plantInit.init();
                    }
                    else
                    {
                        Debug.Log("������������X�N���v�g���Z�b�g����Ă��܂���(�H��)");
                    }

                }
                break;
        }
    }
}
