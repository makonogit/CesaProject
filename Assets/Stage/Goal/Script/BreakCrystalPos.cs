//---------------------------------
// �j�Ђ̈ʒu�ݒ�
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCrystalPos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetStage stage = new SetStage();    //�X�e�[�W�Ǘ��N���X
        if(stage.GetStageNum() == 4)
        {
            //�{�X�X�e�[�W
            transform.position = new Vector3(102.0f, 0.0f, 0.0f);
        }
        else
        {
            // 1-1
            if (stage.GetStageNum() == 0 && stage.GetAreaNum() == 0)
            {
                transform.position = new Vector3(45.6f, 0.0f, 0.0f);
            }
            else
            {

                //�ʏ�X�e�[�W

                transform.position = new Vector3(76.6f, 0.0f, 0.0f);
            }
        }
    }

}
