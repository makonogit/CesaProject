//=========================================
// �S���F�����V�S
// ���e�F�{�X�G���A�̔���
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea_CaveBoss : MonoBehaviour
{
    //===========================
    // �ϐ��錾                  
    //===========================

    [Header("�Փˏ��")]
    public bool hit = false;

    //============================================================
    // *** �Փ˔��� ***                                           
    //============================================================

    void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[���͈͓��ɂ���Ȃ�true�ɂ���
        if (collision.gameObject.tag == "Player")
        {
            hit = true;
        }
    }
}
