//------------------------------------
//�S���F��{��
//���e�F������\�����邩���Ȃ���
//------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSandStorm : MonoBehaviour
{
    // �ϐ��錾

    // ���݂̃X�e�[�W�A�G���A���擾�ł���N���X
    SetStage setstage = new SetStage();

    [SerializeField] private SpriteRenderer _spriteRenderer_l; // �����I�u�W�F�̃����_���[�i���j
    [SerializeField] private SpriteRenderer _spriteRenderer_r; // �����I�u�W�F�̃����_���[�i�E�j 

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(setstage.GetAreaNum());

        // ����G���A�������Ȃ獻���\��
        if(setstage.GetAreaNum() == 2)
        {
            _spriteRenderer_l.enabled = true;
            _spriteRenderer_r.enabled = true;
        }
        // �����ȊO�Ȃ獻����\��
        else
        {
            _spriteRenderer_l.enabled = false;
            _spriteRenderer_r.enabled = false;
        }
    }
}
