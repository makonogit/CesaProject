//---------------------------------------
//担当者：二宮
//内容　：クリスタル獲得数を表示、リアルタイム更新
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCrystalNum : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    private Text TextCrystalNum; // ゲーム画面に描画する
    public int oldCrystalNum; // 前フレームのクリスタルの数を保存する

    // 外部取得
    private GameObject player; // プレイヤーを見つけて保持する
    //private HaveCrystal crystal; // HaveCrystalを保持

    private PlayerStatas status;

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // プレイヤー見つける
        player = GameObject.Find("player");

        // HaveCrystalを取得
        //crystal = player.GetComponent<HaveCrystal>();
        status = player.GetComponent<PlayerStatas>();

        TextCrystalNum = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // 前フレームのクリスタルの数から変動があれば
        if (oldCrystalNum != status.GetCrystal())
        {
            //---------------------------------------------------------
            // プレイヤーが所持するクリスタルの数を再描画
            TextCrystalNum.text = string.Format("×{0:00}", status.GetCrystal());
        }

        // 古いクリスタル更新
        oldCrystalNum = status.GetCrystal();
    }
}
