//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�̗̑͂�0�ɂȂ������ɃQ�[���I�[�o�[�ɃV�[���J�ڂ���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    public int HP = 5; //�̗�
    public int maxHp = 5; //�}�b�N��HP

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------
        //HP��0�ȉ��ɂȂ�����
        if (HP <= 0)
        {
            //---------------------------------------------------------
            // "GameOver"�V�[���ɑJ��
            SceneManager.LoadScene("GameOver");
        }
    }
}
