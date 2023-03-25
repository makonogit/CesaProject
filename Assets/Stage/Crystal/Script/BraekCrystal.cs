//--------------------------------
//�S���F�����S
//���e�F�N���X�^���̔j��
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraekCrystal : MonoBehaviour
{
    StageStatas stageStatas;    //�X�e�[�W�̃X�e�[�^�X�Ǘ�

    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------
        //�X�e�[�W�̃X�e�[�^�X���擾
        stageStatas = transform.root.gameObject.GetComponent<StageStatas>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------
        // �B���Ђт��Փ˂����玩�g��j��
        if(collision.gameObject.tag == "UsedNail" || collision.gameObject.tag == "Crack")
        {
            //----------------------------------------------
            //�@�X�e�[�W�̃N���X�^������1����
            stageStatas.SetStageCrystal(stageStatas.GetStageCrystal() - 1);
            Destroy(this.gameObject);
        }
    }

}
