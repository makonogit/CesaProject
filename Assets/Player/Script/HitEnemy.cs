//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤーの無敵時間を管理
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    public float NoDamageTime = 2f; //無敵時間
    [SerializeField] private float HitTime = 0.0f; // 前回ダメージを受けた時からの経過時間

    [SerializeField] private GameOver gameOver; // ゲームオーバー画面遷移用スクリプト取得用変数
    [SerializeField] private KnockBack knocback; // ノックバックスクリプト取得用変数
    [SerializeField] private RenderOnOff _renderer; // 点滅スクリプト取得用変数
    [SerializeField] private PostEffectManager _postEffectMana; // 被ダメ時のvignette用

    [SerializeField] private PlayEnemySound enemyse; //敵のSE

    [SerializeField]
    private DamageEffectSystem _effectSystem;

    private void Start()
    {
        // 始まった時無敵時間なのを防ぐため初期化
        HitTime = NoDamageTime;
    }

    void Update()
    {
        HitTime += Time.deltaTime;
    }

    // 主にHitColliderスクリプトから呼び出し
    public void HitPlayer(Transform _trans)
    {
        // 接触時間が無敵時間より大きいならHP減らす
        if (HitTime > NoDamageTime)
        {
            //---------------------------------------------------------
            //　SEを再生
            enemyse.PlayEnemySE(PlayEnemySound.EnemySoundList.Attack);

            //---------------------------------------------------------
            

            // ノックバック
            knocback.KnockBack_Func(_trans);

            // 点滅
            _renderer.SetFlash(true);

            // 画面効果
            _postEffectMana.Damage();

            // ハンマーが飛ぶ
            _effectSystem.Creat();

            //---------------------------------------------------------
            // 接触時間リセット
            HitTime = 0.0f;
        }
    }
}
