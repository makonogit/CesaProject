//--------------------------------
//�S���F��{��
//���e�F�{�X�̒ʘH�ɂ��邩�ǂ�����`����
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingSmallerBGM : MonoBehaviour
{
    // �ϐ��錾
    private string playerTag = "Player";

    private bool InArea = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�̂�
        if(collision.gameObject.tag == playerTag)
        {
            InArea = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // �v���C���[�̂�
        if (collision.gameObject.tag == playerTag)
        {
            InArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �v���C���[�̂�
        if (collision.gameObject.tag == playerTag)
        {
            InArea = false;
        }
    }

    public bool GetInPassageArea()
    {
        return InArea;
    }
}
