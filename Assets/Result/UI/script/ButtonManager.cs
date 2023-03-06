//----------------------------------------------------------------
// 担当者：藤原昂祐
// 内容　：リザルト画面のボタン
//----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    //=================================================
    // - 変数 -

    [Header("表示するボタン")]
    public GameObject[] button = new GameObject[3];// 表示するオブジェクト

    // ボタン用
    Vector3 scale;       // ボタンの大きさ
    int selectButton = 0;// 現在選択されているボタンの番号

    // 入力用
    private GameObject PlayerInputMana;         // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    bool stickTrigger;                          // ステックの入力状態
    int buttonTim = 0;                          // ボタンの入力時間

    //=================================================
    // - 初期化処理 -

    void Start()
    {
        //--------------------------------------
        // インプットマネージャーを取得する

        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        //--------------------------------------
    }

    //=================================================
    // - 更新処理 -

    void Update()
    {
        //-----------------------------------------------
        // Lステックの左右入力でボタンを選択する

        Vector2 inputStick = ScriptPIManager.GetMovement();

        if(stickTrigger == false)
        {
            // 右入力
            if (inputStick.x > 0)
            {
                selectButton++;

                if (selectButton > 2)
                    selectButton = 0;

                stickTrigger = true;
            }

            // 左入力
            if (inputStick.x < 0)
            {
                selectButton--;

                if (selectButton < 0)
                    selectButton = 2;

                stickTrigger = true;
            }
        }
        else
        {
            // 右の非入力
            if (inputStick.x == 0)
            {

                stickTrigger = false;
            }

            // 左の非入力
            if (inputStick.x == 0)
            {

                stickTrigger = false;
            }
        }

        //-----------------------------------------------
        // ボタンのサイズを統一する

        for (int i = 0; i < 3; i++)
        {
            scale.x = 1.0f;
            scale.y = 0.5f;
            button[i].transform.localScale = scale;
        }

        //-----------------------------------------------
        // 選択されているボタンだけ大きくする

        scale.x = 2.0f;
        scale.y = 1.0f;
        button[selectButton].transform.localScale = scale;

        //-----------------------------------------------

        //-----------------------------------------------
        // Bボタンの入力で決定する

        if (ScriptPIManager.GetJump())
        {
            if (buttonTim == 0)
            {
                switch (selectButton)
                {
                    //-----------------------------------
                    // 次のステージに進むボタンの処理
                    case 0:
                        Debug.Log("next");
                        break;
                    //-----------------------------------
                    // 今のステージをもう一度プレイするボタンの処理
                    case 1:
                        Debug.Log("retry");
                        break;
                    //-----------------------------------
                    // マップ選択画面に戻るボタンの処理
                    case 2:
                        Debug.Log("select");
                        break;
                    //-----------------------------------

                }
            }

            buttonTim++;
        }
        else
        {
            buttonTim = 0;
        }
        //-----------------------------------------------
    }
    //=================================================
}
