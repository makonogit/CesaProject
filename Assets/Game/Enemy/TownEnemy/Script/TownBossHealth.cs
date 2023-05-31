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
    [SerializeField] private TownBossMove bossMove;
    [SerializeField] private RenderOnOff _renderOnOff;

    // SE関係
    private GameObject SE;
    private PlayEnemySound enemySE; 

    private void Start()
    {
        //// ボス探す
        //Boss = GameObject.Find("TownBoss");
        //// ボスの行動スクリプト取得
        //bossMove = Boss.GetComponent<TownBossMove>();

        BossHealth = MaxBossHealth;
        //BossHealth = 2;
        //BossHealth = 1;

        SE = GameObject.Find("EnemySE");
        enemySE = SE.GetComponent<PlayEnemySound>();
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
            if (order != null)
            {
                if (order.State != CrackCreater.CrackCreaterState.CRAETED)
                {
                    // プレイヤー未発見状態でないなら
                    if (bossMove.EnemyAI != TownBossMove.AIState.None)
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
                            enemySE.KillBossSet();
                        }
                        else
                        {
                            // SEならす
                            enemySE.PlayEnemySE(PlayEnemySound.EnemySoundList.Destroy);

                            // 点滅させる
                            // 第二引数：点滅時間
                            _renderOnOff.SetFlash(true,1.5f);
                        }

                        if (bossMove.EnemyAI == TownBossMove.AIState.Ramming)
                        {
                            bossMove.EnemyAI = TownBossMove.AIState.Walk;
                        }
                    }
                }
            }
        }
    }
}
