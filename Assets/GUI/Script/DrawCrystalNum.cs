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

    [SerializeField, Header("数字のフォント")]
    List<Sprite> Number;

    // 外部取得
    private GameObject player; // プレイヤーを見つけて保持する
    private PlayerStatas status;

    private Image Number_1;    // 1の位
    private Image Number_2;    // 10の位


    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // プレイヤー見つける
        player = GameObject.Find("player");

        // HaveCrystalを取得
        //crystal = player.GetComponent<HaveCrystal>();
        status = player.GetComponent<PlayerStatas>();

        Number_1 = transform.GetChild(0).GetComponent<Image>();
        Number_2 = transform.GetChild(1).GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        Number_1.sprite = Number[status.GetCrystal() % 10];
        Number_2.sprite = Number[status.GetCrystal() / 10];
    }
}
