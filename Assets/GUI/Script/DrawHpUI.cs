//---------------------------------------------------------
//担当者：二宮怜
//内容　：GUIオブジェクトからHPをとってきて、描画する
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawHpUI : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    private int NowHp; // 現在のHPを代入

    // 外部取得
    private GameObject player; // HPを持っているゲームオブジェクト
    private GameOver gameover; // HPを持っているスクリプト
    private GameObject parent; // 親となるゲームオブジェクト
    private RectTransform parentTransform; // 親となるゲームオブジェクトの座標
    public GameObject chirdren; // 生成するオブジェクト

    // アニメーション用
    private Image img; // 画像を変更するための変数
    [SerializeField] Sprite[] sprites; // 画像名を入れておく
    private int NowHPAnimationNumber = 0; // spritesの添え字用変数
    private bool isHPUIAnimation = false; // HPUIのアニメーションをするか
    public float NextImageTime = 0.1f; // 次の画像に変わるまでのタイマー
    private float ChangeImageTimer = 0f; // 前の画像に変わってからの経過時間

    GameObject[] objs;

    //private GameOver.SPRITESTATUS oldSpriteStatus; // 状態をとってきてそれに応じてスプライトを変更

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // ゲームオブジェクト探す
        parent = GameObject.Find("Health");
        // 親の座標
        parentTransform = parent.GetComponent<RectTransform>();

        // GUI探す
        player = GameObject.Find("player");
        // スクリプト取得
        gameover = player.GetComponent<GameOver>();

        objs = new GameObject[gameover.maxHp];

        // 最大HP分hpUIオブジェクト作る
        for (int i = 0; i < gameover.maxHp; i++)
        {
            objs[i] = Instantiate(chirdren, new Vector3(i * 1.0f, 0.0f, 0.0f), Quaternion.identity,parentTransform);
            objs[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(100 * i, 0);
        }

        // 表示するHp用変数に最大HP入れて初期化
        //NowHp = gameover.maxHp;

        // 一番右端にあるHPのImageコンポーネントを取得
        img = objs[gameover.maxHp - 1].GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ////--------------------------------------------------------
        //// 前の状態と現在の状態が違ったらスプライトを変更する
        //if (oldSpriteStatus != SS)
        //{
        //    // このifの中に入る = UIの個数が減った
        //    // スプライトをいじるHPUIのImageコンポーネントを再取得する
        //    if (SS == GameOver.SPRITESTATUS.HIGH)
        //    {
        //        img = objs[gameover.HP - 1].GetComponent<Image>();
        //        //Debug.Log(img);
        //    }
        //    else
        //    {
        //        // 配列spritesの 2 or 3 個目のスプライトに変更
        //        img.sprite = sprites[(int)SS];
        //    }
        //}

        //// 比較変数更新
        //oldSpriteStatus = SS;

        // アニメーション指示が出ていたら
        if(isHPUIAnimation == true)
        {
            HPUI_Animation();
        }

        //---------------------------------------------------------
        // 現在のHP分ハートを表示する
        
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
    }

    // HPUIのアニメーション用関数
    public void HPUI_Animation()
    {
        // 一定時間ごとに画像を変更してアニメーションさせる

        // 一定時間経過したら
        if(ChangeImageTimer >= NextImageTime)
        {
            // 次の画像
            NowHPAnimationNumber++;
            if (NowHPAnimationNumber <= 4)
            {
                // 次の画像に切り替え
                img.sprite = sprites[NowHPAnimationNumber];
            }

            // 初期化
            ChangeImageTimer = 0f;
        }

        // 最後のアニメーション画像になってから一定時間経過したら
        if (NowHPAnimationNumber >= 5) 
        {
            // HP減らす
            gameover.DecreaseHP(1f);

            // 次のアニメーション対象設定
            img = objs[gameover.HP - 1].GetComponent<Image>();

            // アニメーション終了
            isHPUIAnimation = false;

            // 初期化
            NowHPAnimationNumber = 0;
        }

        ChangeImageTimer += Time.deltaTime;
    }

    public void Set_HPAnim(bool _set)
    {
        isHPUIAnimation = _set;
    }
}


