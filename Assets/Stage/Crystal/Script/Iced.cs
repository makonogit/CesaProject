//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�B��łƊ����X�̏���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iced : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -
    private string NailTag = "UsedNail";

    // �������B�ƕX�̕������ڐG������X�������(���i�K�ł͏�����)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == NailTag)
        {
            Material mat = GetComponent<SpriteRenderer>().material;

            mat.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
