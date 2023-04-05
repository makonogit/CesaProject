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
    private CrackCreater order = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        Debug.Log(collision.gameObject.tag);

        // �����������̂��ЂтȂ�
        if (collision.gameObject.tag == CrackTag)
        {
            // ���������Ђт�CrackOrder���擾
            order = collision.gameObject.GetComponent<CrackCreater>();

            //�������Ȃ�
            if (order.State == CrackCreater.CrackCreaterState.CREATING)
            {
                // �G������
                Destroy(this.gameObject.transform.parent.gameObject);
            }
        }  
    }
}
