//---------------------------------------------------------
//担当者：二宮怜
//内容　：パイプにひびが当たったらtrueを返す
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHitCrack : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    private string CrackTag = "Crack";

    // 外部取得
    private CrackCreater order = null;
    public GameObject EnemyObj; // plantenemymoveを持つオブジェクト;
    private PlantEnemyMove enemyMove; // PlantEnemyMoveスクリプト取得用変数
    private void Start()
    {
        // 敵の基本AI処理スクリプト取得
        enemyMove = EnemyObj.GetComponent<PlantEnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        //Debug.Log(collision.gameObject.tag);

        // 当たったものがひびなら
        if (collision.gameObject.tag == CrackTag)
        {
            // 当たったひびのCrackOrderを取得
            order = collision.gameObject.GetComponent<CrackCreater>();

             //生成中なら
             if (order.State == CrackCreater.CrackCreaterState.CREATING || order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
             {
                Destroy(collision.gameObject);

                // ひびがあたった情報を送る
                enemyMove.CrackInPipe = true;

                // ひびに当たった方のパイプの情報を送る
                enemyMove.CrackInObject = this.gameObject;
            }
        }
    }
}
