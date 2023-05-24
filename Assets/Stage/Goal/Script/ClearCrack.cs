//---------------------------------------
//  担当：菅
//　内容：クリア後にひびが入っていく演出
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCrack : MonoBehaviour
{
    [SerializeField, Header("分岐ひびなのか")]
    private bool Branch;

    [SerializeField,Header("1つ上のひび")]
    private SpriteRenderer ParentSprite;

    [SerializeField,Header("ひびのリスト")]
    private List<SpriteRenderer> CrackRender;

    [SerializeField, Header("表示されるスピード")]
    private float AnimSpeed;

    private float TimeMasure = 0.0f;   //時間計測用

    private int NowCrack = 0;          //現在表示しているヒビの番号

    // Update is called once per frame
    void Update()
    {
        // 分岐ひびじゃなければそのまま表示
        if (!Branch)
        {
            //　時間経過したらひびを表示
            if (TimeMasure > AnimSpeed && NowCrack < CrackRender.Count)
            {
                CrackRender[NowCrack].enabled = true;
                NowCrack++;         //次のひびを設定
                TimeMasure = 0.0f;  //時間の初期化
            }
            else
            {
                TimeMasure += Time.deltaTime;
            }
        }
        else
        {
            // 親が表示されたら表示開始
            if (ParentSprite.enabled)
            {
                Branch = false;
            }   
        }

    }


}
