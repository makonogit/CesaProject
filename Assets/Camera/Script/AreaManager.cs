//-----------------------------------
//�S���F�����S
//���e�F�X�e�[�W���G���A�̊Ǘ�
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [Header("�G���A�̐�")]
    public int AreaNum;

    [Header("�G���A�̃T�C�Y")]
    public float AreaSize = 10;

    private void Start()
    {
        SetStage stage = new SetStage();
        if(stage.GetStageNum() == 4)
        {
            AreaSize = 107;
        }
    }

}
