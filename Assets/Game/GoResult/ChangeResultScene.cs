//---------------------------------
//�S���F��{��
//���e�F�����𖞂������烊�U���g�V�[���Ɉڍs
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeResultScene : MonoBehaviour
{
    // - �ϐ��錾 -
    public float time = 0.0f; // �N���X�^����S���󂵂Ă���̌o�ߎ���
    private float WaitTime = 2.0f; // �V�[���J�ڂ���܂ł̑҂�����

    // �O���擾
    private GameObject player;
    private PlayerStatas playerStatus;
    private GameObject Resultobj;   // ���U���g���o�p�̃I�u�W�F�N�g
    private Result result;          // ���U���g���o�p�̃X�N���v�g

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerStatus = player.GetComponent<PlayerStatas>();

        //------------------------------------------------
        // ���U���g���o�p�̃V�X�e���擾
        //Resultobj = GameObject.Find("Result");
        //result = Resultobj.GetComponent<Result>();
    }

    // Update is called once per frame
    void Update()
    {
        // 3�̃N���X�^�����󂵂��烊�U���g��ʂɈړ�
        if (playerStatus.GetBreakCrystalNum() == 3)
        {
            //playerStatus.AddBreakCrystal();
            //���o�J�n
            //result.SetFadeFlg(true);

            // ���U���g��ʂ�
            SceneManager.LoadScene("newSelectScene");

            //// �҂����Ԃ��o�߂�����
            //if (time > WaitTime)
            //{
            //    // ���U���g��ʂ�
            //    SceneManager.LoadScene("newSelectScene");
            //}
        }
    }
}
