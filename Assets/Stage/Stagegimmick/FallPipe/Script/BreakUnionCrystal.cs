//---------------------------------------------------------
//担当者：二宮怜
//内容　：接合部のクリスタルにひびを当てるとクリスタルが壊れる
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakUnionCrystal : MonoBehaviour
{
    // 変数宣言

    // ひびオブジェクトのタグ名
    private string CrackTag = "Crack";

    // 壊されたか
    private bool Break = false;

    // このゲームオブジェクトのマテリアルを保持する変数
    private Material mat;

    // 外部取得
    private CrackCreater order = null;

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ひびにあたったら
        if (collision.gameObject.tag == CrackTag)
        {
            // 当たったひびのCrackOrderを取得
            order = collision.gameObject.GetComponent<CrackCreater>();

            // ひび生成中なら
            if (order.State == CrackCreater.CrackCreaterState.CREATING ||
                order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
            {
                // 壊れるブロックの処理用関数呼び出し
                Func_BreakBlock();
            }
        }
    }

    public void Func_BreakBlock()
    {
        // 透明にする
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);

        // 当たり判定を消す
        GetComponent<CapsuleCollider2D>().enabled = false;

        Break = true;
    }

    public bool GetBreak()
    {
        return Break;
    }
}
