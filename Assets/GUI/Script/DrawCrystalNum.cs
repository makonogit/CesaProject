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

    public enum Rank
    {
        One,
        Ten,
    }

    [SerializeField] private Rank rank;

    // 外部取得
    [SerializeField] private PlayerStatas status;

    [SerializeField] private Image _number;

    [SerializeField] private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        ////---------------------------------------------------------
        //// プレイヤー見つける
        //player = GameObject.Find("player");

        //// HaveCrystalを取得
        ////crystal = player.GetComponent<HaveCrystal>();
        //status = player.GetComponent<PlayerStatas>();

        //Number_1 = transform.GetChild(0).GetComponent<Image>();
        //Number_2 = transform.GetChild(1).GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        switch (rank) {
            case Rank.One:
                _number.sprite = Number[status.GetCrystal() % 10];
                break;

            case Rank.Ten:
                _number.sprite = Number[status.GetCrystal() / 10];
                break;
        }
    }

    public void Get()
    {
        anim.SetBool("get", false);
    }
}
