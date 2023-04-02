//---------------------------------------------
// �S���F�����S
// ���e�F�e�X�e�[�W�̏������X�N���v�g
//---------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    [Header("�X�e�[�W�Ǘ�")]
    public List<GameObject> StageObj;

    //----------------------------------------------
    //�@�X�e�[�W�𐶐�����֐�
    //  �����F�Ȃ�
    //�@�߂�l�F�Ȃ�
    public void CreateStage()
    {
        if (StageData.StageNum > -1 && StageData.AreaNum > -1 &&
            StageData.StageNum < 5 && StageData.AreaNum < 5)
        {
            GameObject obj = Instantiate(StageObj[StageData.AreaNum * 5 + StageData.StageNum]);
            obj.transform.parent = this.transform;                  //�q�I�u�W�F�N�g�ɂ���

        }
        else
        {
            Debug.Log("�͈͊O�̃X�e�[�W�ԍ�������U���Ă��܂�");
        }

    }
}

public static class StageData
{
  
    [Header("�G���A�ԍ�")]
    public static int AreaNum = 0;        // �G���A�̔ԍ������ϐ�
    [Header("�X�e�[�W�ԍ�")]
    public static int StageNum = 0;       // �X�e�[�W�̔ԍ������ϐ�

}

public class SetStage {

    //----------------------------------------------
    //�@�X�e�[�W�ԍ����w�肷��֐�
    //  �����F1~5�@(�G���A�ԍ�) 1~5 (�X�e�[�W�ԍ�)
    //�@�߂�l�F�Ȃ�
    public void SetStageData(int _AreaNum, int _StageNum)
    {
        StageData.AreaNum = _AreaNum - 1;
        StageData.StageNum = _StageNum - 1;
    }
}