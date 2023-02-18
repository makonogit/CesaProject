//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤーの体力が0になった時にゲームオーバーにシーン遷移する
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    public int HP = 5; //体力
    public int maxHp = 5; //マックるHP

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------
        //HPが0以下になったら
        if (HP <= 0)
        {
            //---------------------------------------------------------
            // "GameOver"シーンに遷移
            SceneManager.LoadScene("GameOver");
        }
    }
}
