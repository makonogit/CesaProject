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
    private HaveCrystal crystal; // HaveCrystalを保持

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // プレイヤー見つける
        player = GameObject.Find("player");

        // HaveCrystalを取得
        crystal = player.GetComponent<HaveCrystal>();

        TextCrystalNum = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // 前フレームの釘の数から変動があれば
        if (oldCrystalNum != crystal.CrystalNum)
        {
            //---------------------------------------------------------
            // プレイヤーが所持する釘の数を再描画
            TextCrystalNum.text = string.Format("×{0:00}", crystal.CrystalNum);
        }

        // 古い釘更新
        oldCrystalNum = crystal.CrystalNum;
    }
}
