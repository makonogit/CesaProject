//---------------------------------------------
// �S���F�����S
// ���e�F�e�X�e�[�W�̏������X�N���v�g
//---------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageChild
{

    public GameObject StageObj;   // �X�e�[�W�Ǘ�
    public Vector2 PlayerPos;     // �v���C���[�������W

    
    public StageChild(GameObject _stageobj,Vector2 _playerpos)
    {
        StageObj = _stageobj;
        PlayerPos = _playerpos;
    }


}

[System.Serializable]
public class Stage
{
    public List<StageChild> stage;

    public Stage(List<StageChild> _stage)
    {
        stage = _stage;
    }
}

public class StageManager : MonoBehaviour
{

    [Header("�X�e�[�W�Ǘ�")]
    public List<Stage> stage;

    //-----------------------------------------
    //�@�O���擾
    private GameObject player;
    private Transform PlayerTrans;  //Player��Transform

    //----------------------------------------------
    //�@�X�e�[�W�𐶐�����֐�
    //  �����F�Ȃ�
    //�@�߂�l�F�Ȃ�
    public void CreateStage()
    {
        if (StageData.StageNum > -1 && StageData.AreaNum > -1 &&
            StageData.StageNum < 5 && StageData.AreaNum < 5)
        {
           GameObject obj = Instantiate(stage[StageData.AreaNum].stage[StageData.StageNum].StageObj);
           obj.transform.parent = this.transform;                  //�q�I�u�W�F�N�g�ɂ���

        }
        else
        {
            Debug.Log("�͈͊O�̃X�e�[�W�ԍ�������U���Ă��܂�");
        }

        //---------------------------------
        // �v���C���[�̏������W�w��
        player = GameObject.Find("player");
        PlayerTrans = player.transform;
        PlayerTrans.localPosition = new Vector3(stage[StageData.AreaNum].stage[StageData.StageNum].PlayerPos.x,
            stage[StageData.AreaNum].stage[StageData.StageNum].PlayerPos.y, 1.0f);
    }

    // ��{�ǉ�
    // �����@�F�G���A�ԍ��ƃX�e�[�W�ԍ�
    // �߂�l�F�v���C���[�̏����ʒu
    public Vector3 GetInitPlayerPos(int _areaNum, int _stageNum)
    {
        Vector3 pos = new Vector3(stage[_areaNum].stage[_stageNum].PlayerPos.x,
            stage[_areaNum].stage[_stageNum].PlayerPos.y,
            1.0f);

        return pos;
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

    //----------------------------------------------
    //�@�X�e�[�W�ԍ����l������֐�
    //  �����F�Ȃ�   
    //�@�߂�l�F�X�e�[�W�ԍ�
    public int GetStageNum()
    {
        return StageData.StageNum;
    }

    //----------------------------------------------
    //�@�G���A�ԍ����l������֐�
    //  �����F�Ȃ�   
    //�@�߂�l�F�G���A�ԍ�
    public int GetAreaNum()
    {
        return StageData.AreaNum;
    }

}