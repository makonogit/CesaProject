//---------------------------------------------------------
//担当者：二宮怜
//内容　：1面ボスの体力管理
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBossHealth : MonoBehaviour
{
    // 変数宣言
    private string CrackTag = "Crack";

    // ボスの体力
    [System.NonSerialized]public int BossHealth;
    public int MaxBossHealth = 3;

    private CrackCreater order = null;

    // 外部取得
    private GameObject Boss;
    private TownBossMove bossMove;

    private void Start()
    {
        // ボス探す
        Boss = GameObject.Find("TownBoss");
        // ボスの行動スクリプト取得
        bossMove = Boss.GetComponent<TownBossMove>();

        BossHealth = MaxBossHealth;
        //BossHealth = 2;
        //BossHealth = 1;
    }

    private void Update()
    {
        //Debug.Log(BossHealth);
    }

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
                // 無敵状態じゃなければ
                if (bossMove.invincibility == false)
                {
                    // 体力-1
                    BossHealth--;

                    // ひび消す
                    Destroy(collision.gameObject);

                    // ボスの体力が0以下になったら
                    if (BossHealth <= 0)
                    {
                        // AIの状態を変化
                        bossMove.EnemyAI = TownBossMove.AIState.Death; // 撃破状態
                    }
                }
            }
        }
    }
}
