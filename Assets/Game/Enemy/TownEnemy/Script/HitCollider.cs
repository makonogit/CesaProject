//---------------------------------------------------------
//担当者：二宮怜
//内容　：敵の当たり判定関係
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    private string playerTag = "Player"; // 文字列Enemyを持つ変数

    // 外部取得
    private GameObject player;
    //private GameOver gameOver; // ゲームオーバー画面遷移用スクリプト取得用変数
    //private KnockBack knocback; // ノックバックスクリプト取得用変数
    //private RenderOnOff _renderer; // 点滅スクリプト取得用変数
    private HitEnemy _hitEnemy; // 無敵時間関係スクリプト

    private Transform thisTransform; // 自身のトランスフォーム

    //追加菅
    //private PlayEnemySound enemyse; //敵のSE

    private void Start()
    {
        player = GameObject.Find("player");

        thisTransform = GetComponent<Transform>();

        //// ゲームオーバースクリプト取得
        //// プレイヤーの被ダメージ処理用
        //gameOver = player.GetComponent<GameOver>();

        //// ノックバックスクリプト取得
        //knocback = player.GetComponent<KnockBack>();

        //// 点滅スクリプト取得
        //_renderer = player.GetComponent<RenderOnOff>();

        //// 敵のSE再生用スクリプト取得　追加:菅
        //enemyse = GameObject.Find("EnemySE").GetComponent<PlayEnemySound>();

        // プレイヤーのHitEnemy取得
        _hitEnemy = player.GetComponent<HitEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Playerタグかどうか
        if (collision.gameObject.tag == playerTag)
        {
            // 敵とプレイヤーが接触したときの処理関数呼び出し
            _hitEnemy.HitPlayer(thisTransform);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Playerタグかどうか
        if (collision.gameObject.tag == playerTag)
        {
            // 敵とプレイヤーが接触したときの処理関数呼び出し
            _hitEnemy.HitPlayer(thisTransform);

            //// 接触時間が無敵時間より大きいならHP減らす
            //if (_hitEnemy.HitTime > _hitEnemy.NoDamageTime)
            //{
            //    //---------------------------------------------------------
            //    //　SEを再生
            //    enemyse.PlayEnemySE(PlayEnemySound.EnemySoundList.Attack);

            //    //---------------------------------------------------------
            //    // HP減らすための処理
            //    gameOver.StartHPUIAnimation();

            //    // ノックバック
            //    knocback.KnockBack_Func(transform.parent.transform);

            //    // 点滅
            //    _renderer.SetFlash(true);

            //    //---------------------------------------------------------
            //    // 接触時間リセット
            //    _hitEnemy.HitTime = 0.0f;
            //}
        }
    }
}
