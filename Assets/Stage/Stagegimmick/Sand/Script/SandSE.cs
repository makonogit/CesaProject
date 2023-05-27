//----------------------------------------
//　担当:菅眞心
//　内容：砂のSE
//------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandSE : MonoBehaviour
{
    private GameObject SEobj;           //SE再生用オブジェクト
    private GimmickPlaySound PlaySE;    //SE再生用スクリプト


    // Start is called before the first frame update
    void Start()
    {

        SEobj = GameObject.Find("GimmickSE");
        PlaySE = SEobj.GetComponent<GimmickPlaySound>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //　SEを再生
        if(collision.tag == "Player")
        {
            SEobj = GameObject.Find("GimmickSE");
            PlaySE = SEobj.GetComponent<GimmickPlaySound>();
            //　再生されていなかったら
            if (!PlaySE.IsPlay())
            {
                PlaySE.PlayerGimmickSE(GimmickPlaySound.GimmickSEList.SAND_LOOP);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //　SEを停止
        if (collision.tag == "Player")
        {
            PlaySE.Stop();
        }
    }
}

