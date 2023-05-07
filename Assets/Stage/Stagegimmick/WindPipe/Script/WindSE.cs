//-------------------------------------
//�@�S��:�����S
//�@���e�F�����o�鉹
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSE : MonoBehaviour
{
    SpriteRenderer CrystalRenderer; //  �p�C�v�ɂ����N���X�^���̃����_�[

    private GameObject SEobj;           //SE�Đ��p�I�u�W�F�N�g
    private GimmickPlaySound PlaySE;    //SE�Đ��p�X�N���v�g

    // Start is called before the first frame update
    void Start()
    {
        //�@SE�Đ��p
        SEobj = GameObject.Find("GimmickSE");
        PlaySE = SEobj.GetComponent<GimmickPlaySound>();

        CrystalRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && CrystalRenderer.color.a < 0.1f)
        {
            PlaySE.PlayerGimmickSE(GimmickPlaySound.GimmickSEList.PIPEWIND);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && CrystalRenderer.color.a < 0.1f)
        {
            PlaySE.Stop();
        }
    }

}
