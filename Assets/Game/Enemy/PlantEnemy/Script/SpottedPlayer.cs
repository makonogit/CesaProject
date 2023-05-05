//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤーを発見したら情報を送る
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpottedPlayer : MonoBehaviour
{
    // 変数宣言
    private string playerTag = "Player"; // 文字列Enemyを持つ変数

    // 外部取得
    private PlantEnemyMove plantEnemyMove;
    private GameObject Parent; // 親オブジェクト
    private GameObject PlantEnemy;

    // Start is called before the first frame update
    void Start()
    {
        // 親オブジェクト取得
        Parent = transform.parent.gameObject;

        // 子オブジェクト取得
        PlantEnemy = Parent.transform.GetChild(2).gameObject;

        plantEnemyMove = PlantEnemy.GetComponent<PlantEnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Playerタグかどうか
        if (collision.gameObject.tag == playerTag)
        {
            // 当たっている情報を送る
            plantEnemyMove.PlayerHit = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Playerタグかどうか
        if (collision.gameObject.tag == playerTag)
        {
            // 当たっている情報を送る
            plantEnemyMove.PlayerHit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // Playerタグかどうか
        if (collision.gameObject.tag == playerTag)
        {
            // 索敵範囲から外れた情報を送る
            plantEnemyMove.PlayerHit = false;
        }
    }
}
