//---------------------------------------
//担当者：二宮
//内容　：プレイヤーが持つ釘の数を管理
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveNails : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    private string NailTag = "Nail"; //タグ名

    [Header("釘所持数")]
    public int NailsNum = 0; // 持っている釘の数

    //落ちている釘に触れると釘所持数が増える
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // 触れたのがアイテムとしての釘なら釘所持数増える

        // タグが釘なら
        if (collision.tag == NailTag)
        {
            NailsNum++;
            // アイテムとしての釘は消す
            Destroy(collision.gameObject);
        }

    }
}
