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

    public float NoDamageTime = 1.0f; //無敵時間
    [SerializeField] float HitTime = 0.0f; // 敵と接触している時間

    [Header("敵によるダメージ")]
    public int Damage = 1; // 接触時にくらうダメージ

    // 外部取得
    private GameObject player;
    private GameOver gameOver; // ゲームオーバー画面遷移用スクリプト取得用変数

    private void Start()
    {
        player = GameObject.Find("player");
        gameOver = player.GetComponent<GameOver>();
    }

    void Update()
    {
        HitTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Playerタグかどうか
        if (collision.gameObject.tag == playerTag)
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
        // Playerタグかどうか
        if (collision.gameObject.tag == playerTag)
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
}
