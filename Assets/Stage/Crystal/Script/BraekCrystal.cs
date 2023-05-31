//--------------------------------
//�S���F�����S
//���e�F�N���X�^���̔j��
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BraekCrystal : MonoBehaviour
{
    //-----------------------------------------
    // �ϐ��錾

    private LayerMask BlockLayer = 14;

    GameObject StageObj;
    StageStatas stageStatas;    //�X�e�[�W�̃X�e�[�^�X�Ǘ�

    // �O���擾
    private GameObject player;
    private PlayerStatas playerStatus;
    private SpriteRenderer render;

    [SerializeField, Header("�Ђт̃X�v���C�g")]
    private Sprite Crack;

    private float ParentBreakTime; // �e�I�u�W�F�N�g���j�󂳂ꂽ����
    private bool Break = false;       //�@�j�󂳂ꂽ���ǂ��� 
    private bool Add = false;

    private BreakBlock breakblock; // �X�N���v�g�擾�p�ϐ�

   
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

        //Debug.Log(transform.parent.gameObject);
        // �e�I�u�W�F�N�g�̃��C���[��Block
        if (transform.parent.gameObject.layer == BlockLayer)
        {
            breakblock = transform.parent.gameObject.GetComponent<BreakBlock>();
        }
    }

    private void Update()
    {
        //if (breakblock != null)
        //{
        //    if (breakblock.Break == true && Add == false)
        //    {
        //        ParentBreakTime = Time.time;

        //        // �R���C�_�[�ǉ�
        //        var col = this.gameObject.AddComponent<CircleCollider2D>();
        //        col.radius = 12.5f;
        //        col.isTrigger = true;

        //        Add = true;
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ђтɓ���������
        if (collision.name == "CrackCreaterObject(Clone)")
        {
            SetStage stage = new SetStage();    //�X�e�[�W���擾

            Destroy(collision.gameObject);
            if (!Break)
            {
                render.sprite = Crack;  //�@�X�v���C�g�̕ύX

                stageStatas.SetStageCrystal(stageStatas.GetStageCrystal() - 1);

                // �N���X�^���j�󐔑���
                //playerStatus.AddBreakCrystal();
                Break = true;
            }

            // ���A�X�e�[�W�Ȃ烉�C�g�𖾂邭����
            if(stage.GetAreaNum() == 3)
            {
                GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 1.0f;
            }

            //if (Time.time > ParentBreakTime)
            //{
               
            //}
        }
    }


}
