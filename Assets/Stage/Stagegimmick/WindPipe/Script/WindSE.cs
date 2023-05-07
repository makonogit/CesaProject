//-------------------------------------
//　担当:菅眞心
//　内容：風が出る音
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSE : MonoBehaviour
{
    SpriteRenderer CrystalRenderer; //  パイプについたクリスタルのレンダー

    private GameObject SEobj;           //SE再生用オブジェクト
    private GimmickPlaySound PlaySE;    //SE再生用スクリプト

    // Start is called before the first frame update
    void Start()
    {
        //　SE再生用
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
