//---------------------------------------
//�S���ҁF��{
//���e�@�F�v���C���[�����B�̐����Ǘ�
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveNails : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -
    private string NailTag = "Nail"; //�^�O��
    private string ToolBoxTag = "ToolBox"; // �^�O��

    private int GetNailNum = 10; // ���̃I�u�W�F�N�g����l���ł���B�̐�

    // �H��֘A
    private GameObject toolbox;
    private ToolBoxManager toolboxManager;

    [Header("�B������")]
    private int NailsNum = 0; // �����Ă���B�̐�

    // �O���擾
    private GameObject player;
    private PlayerStatas status;

    private void Start()
    {
        toolbox = GameObject.Find("ToolBox");
        toolboxManager = toolbox.GetComponent<ToolBoxManager>();

        player = GameObject.Find("player");
        status = player.GetComponent<PlayerStatas>();
    }

    private void Update()
    {
        //�G���^�[�œB��������������(�f�o�b�N�p)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            NailsNum++;
        }
    }

    //�����Ă���B�ɐG���ƓB��������������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // �G�ꂽ�̂��A�C�e���Ƃ��Ă̓B�Ȃ�B������������

        // �^�O���B�Ȃ�
        if (collision.tag == NailTag)
        {
            status.SetNail(status.GetNail() + 1);
            // �A�C�e���Ƃ��Ă̓B�͏���
            Destroy(collision.gameObject);
        }

        // �^�O���H��Ȃ�
        if(collision.tag == ToolBoxTag)
        {
            // �܂��B��n���Ă��Ȃ��H��Ȃ�
            if (!toolboxManager.isPassedNails)
            {
                // �I�u�W�F�N�g���ƂɊl�����͎w�肷��i�����l��10�j
                status.SetNail(status.GetNail() + GetNailNum);

                // �g�p�ς݂ɂ���
                toolboxManager.isPassedNails = true;
            }
        }
    }
}
