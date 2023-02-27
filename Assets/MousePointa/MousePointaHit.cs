using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointaHit : MonoBehaviour
{
    private GameObject PlayerObj;           //�v���C���[�̃I�u�W�F�N�g
    private HammerNail HammerNail;          //�B��łX�N���v�g�p�ϐ�

    private void Start()
    {
        //---------------------------------------------------
        //�v���C���[�̃I�u�W�F�N�g����B�����X�N���v�g���擾
        PlayerObj = GameObject.Find("player");
        HammerNail = PlayerObj.GetComponent<HammerNail>();

    }

    //-----------------------------------------
    //�ǁA���ɓ������Ă����瓖��������������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Debug.Log("Enter");
         //   HammerNail.MousePointaHit = true;
        }
    }

    //----------------------------------------
    //�ǁA������ł��瓖�����Ă��Ȃ�����ɂ���
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Debug.Log("Exit");
          //  HammerNail.MousePointaHit = false;
        }
    }

}
