using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointaHit : MonoBehaviour
{
    private GameObject PlayerObj;           //プレイヤーのオブジェクト
    private HammerNail HammerNail;          //釘を打つスクリプト用変数

    private void Start()
    {
        //---------------------------------------------------
        //プレイヤーのオブジェクトから釘生成スクリプトを取得
        PlayerObj = GameObject.Find("player");
        HammerNail = PlayerObj.GetComponent<HammerNail>();

    }

    //-----------------------------------------
    //壁、床に当たっていたら当たった判定を取る
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Debug.Log("Enter");
         //   HammerNail.MousePointaHit = true;
        }
    }

    //----------------------------------------
    //壁、床からでたら当たっていない判定にする
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Debug.Log("Exit");
          //  HammerNail.MousePointaHit = false;
        }
    }

}
