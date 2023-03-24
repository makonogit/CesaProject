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

    public int GetNailNum = 10; // ���̃I�u�W�F�N�g����l���ł���B�̐�

    // �H��֘A
    private GameObject toolbox;
    private ToolBoxManager toolboxManager;

    [Header("�B������")]
    public int NailsNum = 0; // �����Ă���B�̐�

    private void Start()
    {
        toolbox = GameObject.Find("ToolBox");
        toolboxManager = toolbox.GetComponent<ToolBoxManager>();
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
            NailsNum++;
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
                NailsNum += GetNailNum;

                // �g�p�ς݂ɂ���
                toolboxManager.isPassedNails = true;
            }
        }
    }
}
