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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerStatus = player.GetComponent<PlayerStatas>();
    }

    // Update is called once per frame
    void Update()
    {
        // 3�̃N���X�^�����󂵂��烊�U���g��ʂɈړ�
        if (playerStatus.GetBreakCrystalNum() >= 3)
        {
            time += Time.deltaTime;

            // �҂����Ԃ��o�߂�����
            if (time > WaitTime)
            {
                // ���U���g��ʂ�
                SceneManager.LoadScene("ResultScene");
            }
        }
    }
}
