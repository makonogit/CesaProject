//-------------------------------
//�@�S���F�����S
//�@���e�F�X�e�[�W�̐���
//-------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStage : MonoBehaviour
{
    //-------------------------
    //�X�e�[�W�����p�̃f�[�^
  
    StageManager _stagemanager;

    // Start is called before the first frame update
    void Start()
    {
        //-----------------------------------
        //�@�X�e�[�W��������
        _stagemanager = GetComponent<StageManager>();
        _stagemanager.CreateStage();
        
    }

}
