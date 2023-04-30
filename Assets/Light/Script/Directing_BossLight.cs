//---------------------------------------------------------
//担当者：二宮怜
//内容　：ボス撃破後の演出
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;  // Light2D用のusing

public class Directing_BossLight : MonoBehaviour
{
    // 変数宣言

    public int LightNum = 5; // 演出に使うライトの数 
    public bool FlashStart = false; // trueで光る演出が始まる
    public bool BreakStart = false; // trueで壊れる演出始まる
    private float DirectingTimer = 0f; // 演出開始からの経過時間
    private int FlashNum = 0; // いくつ光っているか

    public GameObject PieceOfBoss; // 爆散するパーティクル

    public List<float> FlashTiming = new List<float>(); // float型のリストを定義

    // ゲームオブジェクト格納配列
    private List<GameObject> BossLight = new List<GameObject>(); //GameObject型のリストを定義

    // Light2D格納配列
    private List<Light2D> sc_light = new List<Light2D>(); // Light2D型のリストを定義

    private Transform thisTransform;

    // Start is called before the first frame update
    void Start()
    {
        // リストに要素追加
        for(int i = 0; i < LightNum; i++)
        {
            // ゲームオブジェクト取得
            BossLight.Add(transform.GetChild(i).gameObject);

            // Light2D取得
            sc_light.Add(BossLight[i].GetComponent<Light2D>());

            //Debug.Log("BossLight" + BossLight[i]);
            //Debug.Log("sc_light" + sc_light[i]);
        }

        thisTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // 演出開始
        if(FlashStart == true)
        {
            // ライトの数ぶん光るまで
            if (FlashNum < LightNum)
            {
                // 演出開始してからの経過時間がライトを光らせるタイミングを過ぎたら
                if (DirectingTimer > FlashTiming[FlashNum])
                {
                    // ライトを光らせる
                    sc_light[FlashNum].intensity = 1;

                    // 光らせたライトの数を加算
                    FlashNum++;
                }
            }
            else
            {
                if (DirectingTimer > FlashTiming[LightNum - 1] + 0.5f)
                {
                    BreakStart = true;
                }
            }

            DirectingTimer += Time.deltaTime;
        }
        else
        {
            // 初期化
            DirectingTimer = 0f;
            FlashNum = 0;

            for(int i = 0;i < LightNum; i++)
            {
                sc_light[i].intensity = 0;
            }
        }

        // 爆散する
        if (BreakStart)
        {
            // パーティクル生成
            var Obj = Instantiate(PieceOfBoss);
            Obj.transform.position = thisTransform.position;

            // ボス消す
            Destroy(transform.parent.gameObject);
        }
    }

    // 他のスクリプトで呼び出して演出開始
    public void Flash()
    {
        FlashStart = true;
    }

    public void Break()
    {
        BreakStart = true;
    }
}
