//=========================================
// 担当：藤原昂祐
// 内容：ボスエリアの判定
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea_CaveBoss : MonoBehaviour
{
    //===========================
    // 変数宣言                  
    //===========================

    [Header("衝突状態")]
    public bool hit = false;

    //============================================================
    // *** 衝突判定 ***                                           
    //============================================================

    void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーが範囲内にいるならtrueにする
        if (collision.gameObject.tag == "Player")
        {
            hit = true;
        }
    }
}
