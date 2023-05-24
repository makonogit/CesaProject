//----------------------------
//�@�S���F�����S
//�@���e�F�{�XBGM�̃C���g�������Đ�
//----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIntroBGM : MonoBehaviour
{
    // �ϐ��錾

    [SerializeField] private AudioSource BossBGM_Intro;
    [SerializeField] private AudioSource BossBGM_Loop;
    [SerializeField] private BGMFadeManager _BGMfadeMana;

    private void Update()
    {
        // �C���g���Đ����Ɉ�莞�Ԍo������
        if(BossBGM_Intro.time > 2.0f && BossBGM_Intro.isPlaying == true)
        {
            // �C���g����~
            BossBGM_Intro.Stop();

            // ���[�v�����Đ��J�n
            BossBGM_Loop.Play();
        }
    }
}
