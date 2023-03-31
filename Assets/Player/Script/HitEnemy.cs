//---------------------------------------------------------
//担当者：二宮怜
//内容　：敵との当たり判定
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    private string enemyTag = "Enemy"; // 文字列Enemyを持つ変数

    public float NoDamageTime = 100.0f; //無敵時間
    [SerializeField] float HitTime = 0.0f; // 敵と接触している時間

    [Header("敵によるダメージ")]
    public int Damage = 10; // 接触時にくらうダメージ

    // 外部取得
    public GameOver gameOver; // ゲームオーバー画面遷移用スクリプト取得用変数

    void Update()
    {
        HitTime += Time.deltaTime;
    }

    //---------------------------------------------------------
    // Enemyタグを持つオブジェクトとの衝突があった時に
    // gameOverスクリプトが持つ変数 HP を-1する
    void OnTriggerEnter2D(Collider2D collision)
    {
        ////---------------------------------------------------------
        //// Enemyタグかどうか
        //if (collision.gameObject.tag == enemyTag)
        //{
        //    //---------------------------------------------------------
        //    // HP -Damage
        //    gameOver.DecreaseHP(Damage);

        //    //---------------------------------------------------------
        //    // 初期化
        //    HitTime = 0.0f;

        //    //---------------------------------------------------------
        //    // アルファ値変更
        //    //alpha.SetAlpha(gameOver.HP,gameOver.maxHp);
        //}

        //---------------------------------------------------------
        // Enemyタグかどうか
        if (collision.gameObject.tag == enemyTag)
        {
            // 接触時間が無敵時間より大きいならHP減らす
            if (HitTime > NoDamageTime)
            {
                //---------------------------------------------------------
                // HP -Damage
                gameOver.DecreaseHP(Damage);

                //---------------------------------------------------------
                // 接触時間リセット
                HitTime = 0.0f;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Enemyタグかどうか
        if (collision.gameObject.tag == enemyTag)
        {
            // 接触時間が無敵時間より大きいならHP減らす
            if (HitTime > NoDamageTime)
            {
                //---------------------------------------------------------
                // HP -Damage
                gameOver.DecreaseHP(Damage);

                //---------------------------------------------------------
                // アルファ値変更
                //alpha.SetAlpha(gameOver.HP, gameOver.maxHp);

                //---------------------------------------------------------
                // 接触時間リセット
                HitTime = 0.0f;
            }
        }
    }
}
