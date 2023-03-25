//---------------------------------------
//担当者：二宮
//内容　：釘所持数をUIとして表示
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawNailNum : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    private Text TextNailNum; // ゲーム画面に描画する
    public int oldNailsNum; // 前フレームの釘の数を保存する

    // 外部取得
    private GameObject player; // プレイヤーを見つけて保持する
    //private HaveNails nails; // HaveNailsを保持
    private PlayerStatas status;

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // プレイヤー見つける
        player = GameObject.Find("player");

        // HaveNailsを取得
        //nails = player.GetComponent<HaveNails>();
        status = player.GetComponent<PlayerStatas>();

        TextNailNum = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // 前フレームの釘の数から変動があれば
        if(oldNailsNum != status.GetNail())
        {
            //---------------------------------------------------------
            // プレイヤーが所持する釘の数を再描画
            TextNailNum.text = string.Format("×{0:00}", status.GetNail());
        }

        // 古い釘更新
        oldNailsNum = status.GetNail();
    }
}
