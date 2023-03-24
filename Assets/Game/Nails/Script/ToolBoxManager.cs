//---------------------------------------
//担当者：二宮
//内容　：工具箱の状態管理
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBoxManager : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    public bool isPassedNails = false; // 釘をプレイヤーに渡したか
    private bool Finished = false; // このスクリプトの仕事が終わったか

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // このスクリプトでやりたいことが終わってなければ
        if (!Finished)
        {
            // 一度釘を渡していれば
            if (isPassedNails)
            {
                // 半透明にする
                sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                Finished = true;
            }
        }
    }
}
