//-------------------------------------
//�S���F�����S
//�����F�͈͂ɂ���Ђт𐬒��h����
//-------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackGrow : MonoBehaviour
{

    private CrackCreater Creater;       //CrackCreater��ێ�����ϐ�
    private CircleCollider2D thiscol;   //���̃I�u�W�F�N�g�̃R���C�_�[

    // Start is called before the first frame update
    void Start()
    {
        //���̃I�u�W�F�N�g�̃R���C�_�[���擾
        thiscol = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //------------------------------
        //�Ђт̐��������擾
        if (collision.gameObject.tag == "Crack")
        {
            Creater = collision.GetComponent<CrackCreater>();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Crack")
        {
            //--------------------------------------
            //�����I�����Ă�����Ђт�ǉ�
            if (Creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
            {
                Creater.SetState(CrackCreater.CrackCreaterState.ADD_CREATE);
            }
        }

    }

}
