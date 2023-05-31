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
    private GameObject ParentEnemy; // 親オブジェクトの敵;
    private EnemyMove enemyMove; // EnemyMoveスクリプト取得用変数

    private void Start()
    {
        ParentEnemy = transform.parent.gameObject;  //　親オブジェクトを取得
        //Debug.Log(ParentEnemy);
        // 敵の基本AI処理スクリプト取得
        enemyMove = ParentEnemy.GetComponent<EnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        //Debug.Log(collision.gameObject.tag);

        // 当たったものがひびなら
        if (collision.gameObject.tag == CrackTag)
        {
            if (enemyMove != null)
            {
                // 初めの一回のみ入る
                if (enemyMove.EnemyAI != EnemyMove.AIState.DEATH)
                {
                    // 当たったひびのCrackOrderを取得
                    order = collision.gameObject.GetComponent<CrackCreater>();

                    //生成中なら
                    if (order != null)
                    {
                        //if (order.State == CrackCreater.CrackCreaterState.CREATING || order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
                        //{
                        //    // 死亡状態にする
                        //    enemyMove.EnemyAI = EnemyMove.AIState.DEATH;
                        //}

                        if(order.State != CrackCreater.CrackCreaterState.CRAETED)
                        {
                            // 死亡状態にする
                            enemyMove.EnemyAI = EnemyMove.AIState.DEATH;
                        }
                    }
                }
            }
        }
    }
}
