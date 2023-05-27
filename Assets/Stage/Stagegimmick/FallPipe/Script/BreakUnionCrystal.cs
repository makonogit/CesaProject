//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�ڍ����̃N���X�^���ɂЂт𓖂Ă�ƃN���X�^��������
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakUnionCrystal : MonoBehaviour
{
    // �ϐ��錾

    // �ЂуI�u�W�F�N�g�̃^�O��
    private string CrackTag = "Crack";

    // �󂳂ꂽ��
    private bool Break = false;

    // ���̃Q�[���I�u�W�F�N�g�̃}�e���A����ێ�����ϐ�
    private Material mat;

    private Transform thisTransform;

    // �O���擾
    private CrackCreater order = null;

    // �N���X�^���󂵂��Ƃ��̃p�[�e�B�N���v���n�u
    public GameObject PipeCrystalParticle;
    // ���������I�u�W�F�N�g������
    private GameObject Obj;

    private void Start()
    {
        // �}�e���A���擾
        mat = GetComponent<SpriteRenderer>().material;

        // ���̃I�u�W�F�N�g�̍��W�擾
        thisTransform = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ђтɂ���������
        if (collision.gameObject.tag == CrackTag)
        {
            // ���������Ђт�CrackOrder���擾
            order = collision.gameObject.GetComponent<CrackCreater>();

            if (order != null)
            {
                // �Ђѐ������Ȃ�
                if (order.State == CrackCreater.CrackCreaterState.CREATING ||
                    order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
                {
                    // ����u���b�N�̏����p�֐��Ăяo��
                    Func_BreakBlock();
                }
            }

            Debug.Log("order��null");
        }
    }

    public void Func_BreakBlock()
    {
        // �����ɂ���
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);

        Obj = Instantiate(PipeCrystalParticle);
        Obj.transform.position = thisTransform.position;

        // �����蔻�������
        Destroy(GetComponent<BoxCollider2D>());

        Break = true;
    }

    public bool GetBreak()
    {
        return Break;
    }

    private void OnDestroy()
    {
        // �p�[�e�B�N���̍Đ����I����������
        Destroy(Obj);
        Destroy(this.gameObject);
;    }
}
