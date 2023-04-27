//-----------------------------
//�S���F��{��
//���e�F�ЂтƐڐG����Ɖ���u���b�N
//-----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    // �ϐ��錾

    // �ЂуI�u�W�F�N�g�̃^�O��
    private string CrackTag = "Crack";

    // ���̃Q�[���I�u�W�F�N�g�̃}�e���A����ێ�����ϐ�
    private Material mat;

    // �O���擾
    private CrackCreater order = null;

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ђтɂ���������
        if (collision.gameObject.tag == CrackTag)
        {
            // ���������Ђт�CrackOrder���擾
            order = collision.gameObject.GetComponent<CrackCreater>();

            // �Ђѐ������Ȃ�
            if (order.State == CrackCreater.CrackCreaterState.CREATING ||
                order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
            {
                // ����u���b�N�̏����p�֐��Ăяo��
                Func_BreakBlock();
            }
        }
    }
    
    public void Func_BreakBlock()
    {
        // �����ɂ���
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);

        // �����蔻�������
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
