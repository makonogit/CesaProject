//---------------------------------------
//担当者：二宮
//内容　：プレイヤーが取得したクリスタルの数を管理
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveCrystal : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    private string CrystalTag = "Crystal"; //タグ名

    [Header("クリスタル所持数")]
    public int CrystalNum = 0; // 持っている釘の数

    // Update is called once per frame
    void Update()
    {
        
    }

    //落ちているクリスタルに触れるとクリスタル所持数が増える
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // 触れたのがアイテムとしてのクリスタルならクリスタル所持数増える

        // タグが釘なら
        if (collision.tag == CrystalTag)
        {
            CrystalNum++;
            // アイテムとしてのクリスタルは消す
            Destroy(collision.gameObject);
        }

    }
}
