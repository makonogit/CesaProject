//----------------------------------------------------------------
// 担当者：藤原昂祐
// 内容　：集めたボーナスアイテムの数を表示する
//----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    //=================================================
    // - 変数 -

    [Header("表示するオブジェクト")]
    public GameObject[] bonus = new GameObject[3];// 表示するオブジェクト

    int bunusScore = 3;// 集めたボーナスの数（スコア）

    //=================================================
    // - 初期化処理 -

    void Start()
    {
        //--------------------------------------
        // ボーナスを全て非表示にする

        for (int i = 0; i < 3; i++)
        {
            bonus[i].SetActive(false);
        }
        //--------------------------------------

    }

    //=================================================
    // - 更新処理 -

    void Update()
    {
        //--------------------------------------
        // 集めた数だけボーナスを表示する

        for (int i = 0; i < bunusScore; i++)
        {
            bonus[i].SetActive(true);
        }
        //--------------------------------------
    }

    //=================================================
}
