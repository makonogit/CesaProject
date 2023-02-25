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

    [Header("現在のHP")]
    public int HP = 5; //体力

    [Header("最大HP")]
    public int maxHp = 5; //マックスHP

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

    public void DecreaseHP(float _hp)
    {
        HP = HP - (int)_hp;
    }
}
