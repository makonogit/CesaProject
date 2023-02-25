//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�������̂Ђт�G�ɓ��Ă����ɓG������
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrackHit : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -
    private string CrackTag = "Crack";

    // �O���擾
    private CrackOrder order = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // �����������̂��ЂтȂ�
        if (collision.gameObject.tag == CrackTag)
        {
            // ���������Ђт�CrackOrder���擾
            order = collision.gameObject.GetComponent<CrackOrder>();

            // �������Ȃ�
            if (order.crackState == CrackOrder.CrackState.NowCreate)
            {
                // �G������
                Destroy(this.gameObject);
            }
        }  
    }
}