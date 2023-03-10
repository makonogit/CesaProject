//----------------------------------------------------------------
// 担当者：藤原昂祐
// 内容　：残した釘の数を表示する
//----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class NailScore : MonoBehaviour
{
    //=================================================
    // - 変数 -

    [Header("残した釘の数を表示するテキスト")]
    [SerializeField] TextMeshProUGUI nailText;// 表示するテキスト

    int score;// スコア

    //=================================================
    // - 更新処理 -

    void Update()
    {
        //---------------------------------------------
        // 表示するテキストを更新する
        nailText.text = "nail:" + score;
        //---------------------------------------------
    }

    //=================================================
}