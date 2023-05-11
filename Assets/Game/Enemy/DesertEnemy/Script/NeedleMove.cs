//-----------------------------------------
//�@�S���F�����S
//�@���e�F�j�̈ړ�
//-----------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleMove : MonoBehaviour
{
    //---------------------------------------
    //�@�ϐ��錾
    
    //---------------------------------------
    // �O���擾
    GameOver hpsystem;  //HP�V�X�e���̃X�N���v�g

    Transform ThisTrans;    // ���g��Transform

    [SerializeField, Header("�ړ��X�s�[�h")]
    private float MoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ThisTrans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------
        //�@�����Ă�������Ɉړ�
        Vector3 verocity = ThisTrans.rotation * new Vector3(MoveSpeed, 0);
        ThisTrans.position += verocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //---------------------------------------------------------
        //�@�v���C���[�ƏՓ˂�����HP�����炷
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<GameOver>().DecreaseHP(1);
            collision.gameObject.GetComponent<KnockBack>().KnockBack_Func(transform);
            collision.gameObject.GetComponent<RenderOnOff>().SetFlash(true);

        }

        //--------------------------------
        //�@�T�{�e���ȊO�ƂԂ����������
        if(collision.gameObject.name != "cactus")
        {
            Destroy(this.gameObject);
        }
    }
  

}
