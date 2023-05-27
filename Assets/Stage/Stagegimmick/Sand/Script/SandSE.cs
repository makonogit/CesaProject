//----------------------------------------
//�@�S��:�����S
//�@���e�F����SE
//------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSE : MonoBehaviour
{
    private GameObject SEobj;           //SE�Đ��p�I�u�W�F�N�g
    private GimmickPlaySound PlaySE;    //SE�Đ��p�X�N���v�g


    // Start is called before the first frame update
    void Start()
    {

        SEobj = GameObject.Find("GimmickSE");
        PlaySE = SEobj.GetComponent<GimmickPlaySound>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�@SE���Đ�
        if(collision.tag == "Player")
        {
            SEobj = GameObject.Find("GimmickSE");
            PlaySE = SEobj.GetComponent<GimmickPlaySound>();
            //�@�Đ�����Ă��Ȃ�������
            if (!PlaySE.IsPlay())
            {
                PlaySE.PlayerGimmickSE(GimmickPlaySound.GimmickSEList.SAND_LOOP);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�@SE���~
        if (collision.tag == "Player")
        {
            PlaySE.Stop();
        }
    }
}

