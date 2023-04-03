//--------------------------------
//�S���F�����S
//���e�F�N���X�^���̔j��
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraekCrystal : MonoBehaviour
{
    GameObject StageObj;
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

    private PlayerStatas playerStatus;

    private GameObject PlayerInputMana;
    private PlayerInputManager ScriptPIManager;

    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------
        //�X�e�[�W�̃X�e�[�^�X���擾
        StageObj = transform.root.gameObject;
        stageStatas = StageObj.transform.GetChild(0).GetComponent<StageStatas>();

        // �v���C���[�T��
        player = GameObject.Find("player");
        // �R���|�[�l���g�擾
        playerTransform = player.GetComponent<Transform>();
        thisTransform = GetComponent<Transform>();
        playerStatus = player.GetComponent<PlayerStatas>();

        // PlayerInputManager�T��
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
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
            if(/* ScriptPIManager.GetNail_Left() && ScriptPIManager.GetNail_Right() */
                collision.tag == "Crack")
            {
                stageStatas.SetStageCrystal(stageStatas.GetStageCrystal() - 1);
                Destroy(this.gameObject);
                // �N���X�^���j�󐔑���
                playerStatus.AddBreakCrystal();
            }
        }
    }


}
