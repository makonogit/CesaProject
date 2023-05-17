//---------------------------------------------------------
//担当者：二宮怜
//内容　：プラント場の敵がひびに当たったら死ぬ
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemyCrackHit : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    private string CrackTag = "Crack";

    // 外部取得
    private CrackCreater order = null;

    [SerializeField] private PlantEnemyMove enemyMove; // EnemyMoveスクリプト取得用変数
    private GameObject EnemySE;
    private PlayEnemySound enemyse; //死んだ音用

    private void Start()
    {
        //// 敵の基本AI処理スクリプト取得
        //enemyMove = ParentEnemy.GetComponent<PlantEnemyMove>();

        EnemySE = GameObject.Find("EnemySE");
        enemyse = EnemySE.GetComponent<PlayEnemySound>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // 当たったものがひびなら
        if (collision.gameObject.tag == CrackTag)
        {
            // 初めの一回のみ入る
            if (enemyMove.EnemyAI != PlantEnemyMove.AIState.Death)
            {
                // 当たったひびのCrackOrderを取得
                order = collision.gameObject.GetComponent<CrackCreater>();

                //生成中なら
                if (order != null)
                {
                    if (order.State == CrackCreater.CrackCreaterState.CREATING || order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
                    {
                        if (enemyMove.EnemyAI == PlantEnemyMove.AIState.Attack || enemyMove.EnemyAI == PlantEnemyMove.AIState.Confusion || enemyMove.EnemyAI == PlantEnemyMove.AIState.Rage)
                        {
                            //SE再生
                            enemyse.PlayEnemySE(PlayEnemySound.EnemySoundList.Destroy);
                            // 死亡状態にする
                            enemyMove.EnemyAI = PlantEnemyMove.AIState.Death;
                        }
                    }
                }
            }
        }
    }
}
