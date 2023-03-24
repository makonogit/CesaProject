//----------------------------------
//担当：菅眞心
//内容：ボタンの点滅処理
//----------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // 点滅させる対象
    private float nextTime;

    // 点滅周期[s]
    public  float interval = 0.5f;

    // 時間を経過する関数
    public float time = 0.0f;

    //点滅させるオブジェクト
    private GameObject ButtonPushUI;

    //点滅させるオブジェクトのSpriteRender
    private SpriteRenderer renderer;

    //透明度
    private float Alpha = 1.0f;

    //点滅フラグ
    public bool Flash = false;

    // Start is called before the first frame update
    void Start()
    {
        //点滅させるオブジェクトの情報を取得
        ButtonPushUI = GameObject.Find("Button");
        renderer = ButtonPushUI.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //時間計測
        time += Time.deltaTime;
        renderer.color = new Color(1.0f, 1.0f, 1.0f, Alpha);

        //0.5秒経過毎に点滅フラグを変更
        if(Alpha >= 1.0f)
        {
            Flash = false;
        }
        if (Alpha <= 0.0f)
        {
            Flash = true;
        }


        if (Flash)
        {
            Alpha += 1.0f * Time.deltaTime;
        }
        else
        {
            Alpha -= 1.0f * Time.deltaTime;
        }
    }
}
