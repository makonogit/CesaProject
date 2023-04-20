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
    private GameObject ParentEnemy; // 親オブジェクトの敵
    private EnemyMove enemyMove; // EnemyMoveスクリプト取得用変数

    private void Start()
    {
        // 親オブジェクト取得
        ParentEnemy = transform.root.gameObject;

        // 敵の基本AI処理スクリプト取得
        enemyMove = ParentEnemy.GetComponent<EnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        //Debug.Log(collision.gameObject.tag);

        // 初めの一回のみ入る
        if (enemyMove.EnemyAI != EnemyMove.AIState.DEATH)
        {
            // 当たったものがひびなら
            if (collision.gameObject.tag == CrackTag)
            {
                // 当たったひびのCrackOrderを取得
                order = collision.gameObject.GetComponent<CrackCreater>();

                //生成中なら
                if (order.State == CrackCreater.CrackCreaterState.CREATING)
                {
                    // 死亡状態にする
                    enemyMove.EnemyAI = EnemyMove.AIState.DEATH;
                }
            }
        }
    }
}
