//---------------------------------------------------------
//担当者：二宮怜
//内容　：生成中のひびを敵に当てた時に敵が死ぬ
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrackHit : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    private string CrackTag = "Crack";

    // 外部取得
    private CrackCreater order = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // 当たったものがひびなら
        if (collision.gameObject.tag == CrackTag)
        {
            // 当たったひびのCrackOrderを取得
            order = collision.gameObject.GetComponent<CrackCreater>();

            //生成中なら
            if (order.State == CrackCreater.CrackCreaterState.CREATING)
            {
                // 敵を消す
                Destroy(this.gameObject);
            }
        }  
    }
}
