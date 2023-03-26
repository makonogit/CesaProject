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

    //-----------------------------
    // ��{�ǉ�
    private float SubX;    // ���߂�X���W�̍���ێ�����ϐ�
    private float SubY;    // ���߂�Y���W�̍���ێ�����ϐ�
    private float Distace; // ���߂�������ێ�����ϐ�
    private float judgeDistance = 2.0f; // ������Ƃ�͈�

    // �O���擾
    private GameObject player;
    private Transform playerTransform;
    private Transform thisTransform;

    private GameObject PlayerInputMana;
    private PlayerInputManager ScriptPIManager;

    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------
        //�X�e�[�W�̃X�e�[�^�X���擾
        stageStatas = transform.root.gameObject.GetComponent<StageStatas>();

        // �v���C���[�T��
        player = GameObject.Find("player");
        // �R���|�[�l���g�擾
        playerTransform = player.GetComponent<Transform>();
        thisTransform = GetComponent<Transform>();

        // PlayerInputManager�T��
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //-----------------------------------------------------------
        // �v���C���[�����͈͓��ŃX�}�b�V��������N���X�^��������

        // �v���C���[�Ƃ̋��������Ƃ߂�
        SubX = thisTransform.position.x - playerTransform.position.x; // x��
        SubY = thisTransform.position.y - playerTransform.position.y; // y��

        // �O�����̒藝
        Distace = SubX * SubX + SubY * SubY; // �v���C���[�ƃN���X�^���̋��������܂���

        // ��苗�����Ƀv���C���[������
        if(Distace < judgeDistance)
        {
            // �����������ꂽ
            if(ScriptPIManager.GetNail_Left() && ScriptPIManager.GetNail_Right())
            {
                stageStatas.SetStageCrystal(stageStatas.GetStageCrystal() - 1);
                Destroy(this.gameObject);
            }
        }


        //--------------------------------------------------
        // �B���Ђт��Փ˂����玩�g��j��
        //if(collision.gameObject.tag == "UsedNail" || collision.gameObject.tag == "Crack")
        //{
        //    //----------------------------------------------
        //    //�@�X�e�[�W�̃N���X�^������1����
        //    stageStatas.SetStageCrystal(stageStatas.GetStageCrystal() - 1);
        //    Destroy(this.gameObject);
        //}
    }

}
