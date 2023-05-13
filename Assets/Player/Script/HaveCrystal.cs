//---------------------------------------
//担当者：二宮
//内容　：プレイヤーが取得したクリスタルの数を管理
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveCrystal : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    private string CrystalTag = "Crystal"; //タグ名

    [Header("クリスタル所持数")]
    private int CrystalNum = 0; // 持っている釘の数

    // 外部取得
    [SerializeField] private PlayerStatas status;

    private CrystalNum Crystal;

    [SerializeField] private Animator anim1;
    [SerializeField] private Animator anim2;

    // Update is called once per frame
    void Update()
    {
        //player = GameObject.Find("player");
        //status = player.GetComponent<PlayerStatas>();
    }

    //落ちているクリスタルに触れるとクリスタル所持数が増える
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // 触れたのがアイテムとしてのクリスタルならクリスタル所持数増える

        // タグがクリスタルなら
        if (collision.tag == CrystalTag)
        {
            Crystal = collision.GetComponent<CrystalNum>();
            //Debug.Log(Crystal.Get);

            // 取得済みでないなら
            if (Crystal.Get == false)
            {
                // クリスタル所持数を増やす
                status.SetCrystal(status.GetCrystal() + Crystal.crystalNum);
                // 取得済みフラグ
                collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;

                Crystal.Get = true;

                // クリスタル取得時のアニメーション
                anim1.SetBool("get", true);
                anim2.SetBool("get", true);

                // 既にアニメーションしていたら
                if (anim1.GetBool("get"))
                {
                    // 始めから
                    anim1.Play("AccentNumber", 0, 0);
                    anim2.Play("AccentNumber", 0, 0);
                }
            }
        }
    }
}
