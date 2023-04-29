//--------------------------------
//�S���F�����S
//���e�F�N���X�^���̔j��
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraekCrystal : MonoBehaviour
{
    //-----------------------------------------
    // �ϐ��錾

    GameObject StageObj;
    StageStatas stageStatas;    //�X�e�[�W�̃X�e�[�^�X�Ǘ�

    // �O���擾
    private GameObject player;
    private PlayerStatas playerStatus;
    private SpriteRenderer render;

    [SerializeField, Header("�Ђт̃X�v���C�g")]
    private Sprite Crack;

    GameObject BackGround;          // �w�i�I�u�W�F�N�g
    BreakBackGround BreakBack;      // �w�i�̕���X�N���v�g���Ăяo��

    private bool Break = false;       //�@�j�󂳂ꂽ���ǂ��� 

    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------
        //�X�e�[�W�̃X�e�[�^�X���擾
        StageObj = GameObject.Find("StageData");
        stageStatas = StageObj.transform.GetChild(0).GetComponent<StageStatas>();

        // �v���C���[�T��
        player = GameObject.Find("player");
        playerStatus = player.GetComponent<PlayerStatas>();

        //�@���̃I�u�W�F�N�g��spriterenderer
        render = GetComponent<SpriteRenderer>();

        // �w�i�̏��擾
        BackGround = GameObject.Find("BackGround");
        BreakBack = BackGround.GetComponent<BreakBackGround>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // �Ђтɓ���������
        if (collision.tag == "Crack" && !Break)
        {
            render.sprite = Crack;  //�@�X�v���C�g�̕ύX

            BreakBack.BreakBack();  //�@�w�i�̕���

            //Destroy(this.gameObject);
            stageStatas.SetStageCrystal(stageStatas.GetStageCrystal() - 1);

            // �N���X�^���j�󐔑���
            playerStatus.AddBreakCrystal();
            Break = true;
        }
    }


}
