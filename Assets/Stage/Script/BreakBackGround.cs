//----------------------------------
// 担当：菅眞心
// 内容：崩れていく背景の描画
//----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBackGround : MonoBehaviour
{
    //-------------------------
    // 変数宣言

    private List<GameObject> BackCrystal = new List<GameObject>();   // 背景のクリスタル
    private float Alpha;                    // 背景クリスタルのα値

    // Start is called before the first frame update
    void Start()
    {

        if(BackCrystal.Count > 0)
        {
            BackCrystal.Clear();
        }

        // 背景クリスタルを取得
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.tag == "Crystal")
            {
                BackCrystal.Add(transform.GetChild(i).gameObject);
            }
        }

        // 初期α値を保持
        Alpha = BackCrystal[0].GetComponent<SpriteRenderer>().color.a;

    }

    //--------------------
    //　背景が壊れる関数
    //  引数：なし
    //  戻り値：なし
    public void BreakBack()
    {
        Alpha += (1.0f / 6);

        for (int i = 0; i < BackCrystal.Count; i++)
        {
            BackCrystal[i].GetComponent<SpriteRenderer>().color =
                new Color(1.0f, 1.0f, 1.0f, Alpha);
        }
    }
}
