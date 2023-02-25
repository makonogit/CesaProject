//---------------------------------------------------------
//担当者：二宮怜
//内容　：GUIオブジェクトからHPをとってきて、描画する
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawHpUI : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    private int NowHp; // 現在のHPを代入

    // 外部取得
    private GameObject GUI; // HPを持っているゲームオブジェクト
    private GameOver gameover; // HPを持っているスクリプト
    private GameObject parent; // 親となるゲームオブジェクト
    private RectTransform parentTransform; // 親となるゲームオブジェクトの座標
    public GameObject chirdren; // 生成するオブジェクト

    GameObject[] objs;

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // ゲームオブジェクト探す
        parent = GameObject.Find("Health");
        // 親の座標
        parentTransform = parent.GetComponent<RectTransform>();

        // GUI探す
        GUI = GameObject.Find("GUI");
        // スクリプト取得
        gameover = GUI.GetComponent<GameOver>();

        objs = new GameObject[gameover.maxHp];

        // 最大HP分hpUIオブジェクト作る
        for (int i = 0; i < gameover.maxHp; i++)
        {
            objs[i] = Instantiate(chirdren, new Vector3(i * 1.0f, 0.0f, 0.0f), Quaternion.identity,parentTransform);
            objs[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(100 * i, 0);
        }

        // 表示するHp用変数に最大HP入れて初期化
        NowHp = gameover.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------
        // Hpに変動があった時に描画処理の更新をする
        //// HPがへった
        //if (NowHp > gameover.maxHp)
        //{
        //    // 変動がある場所を非表示
        //    objs[gameover.HP].SetActive(false);
        //}
        //// HPが増えた
        //else if(NowHp < gameover.maxHp)
        //{
        //    // 変動がある場所を表示
        //    objs[gameover.HP].SetActive(true);
        //}

        for(int i = 0;i< gameover.maxHp; i++)
        {
            if (gameover.HP >= i + 1)
            {
                objs[i].SetActive(true);
            }
            else
            {
                objs[i].SetActive(false);
            }
        }

        // HPを最新の値にする
        //NowHp = gameover.maxHp;

        //Debug.Log(parentTransform.anchoredPosition);
    }

    
}
