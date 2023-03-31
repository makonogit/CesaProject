//---------------------------------------------------------
//担当者：二宮怜
//内容　：ポーズ画面
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    public bool IsPause = false; // ポーズ状態かどうか
    public int CursorY = 0; // Y方向の移動をするカーソルの番号
    const int CursorMax = 3; // カーソルの一番下

    //private float ManualSizeX = 3.8f;
    //private float ManualSizeY = 10.2f;
    public bool manual = false;

    // メニューの数が増えるたびに追加
    private string[] PauseObj = {
        "Continue",
        "Retry",
        "HowTo",
        "Select" };

    // 外部取得
    private GameObject PlayerInputMana;
    private PlayerInputManager ScriptPIManager;
    private GameObject Cursor; // カーソル
    private RectTransform cursorTransform; // カーソルの座標
    private Image cursorImage;
    private GameObject Target; // カーソルの位置の基準となるobj
    private RectTransform targetTransform; // Targetの座標取得
    private RectTransform InitTransform; // カーソルが最初にいる位置を保存しておく変数

    private GameObject Manual;
    //private RectTransform manualTransform;
    private Image manualImage;

    // se関係
    private GameObject se;
    private SEManager_Pause seMana;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        // 非表示にする
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;

        // カーソル探す
        Cursor = GameObject.Find("Cursor");

        // カーソルの座標取得
        cursorTransform = Cursor.GetComponent<RectTransform>();

        // カーソルのイメージコンポーネント取得
        cursorImage = Cursor.GetComponent<Image>();

        // カーソルの位置の基準となるobj探す
        Target = GameObject.Find(PauseObj[CursorY]);

        // 現時点でのTargetの座標取得
        targetTransform = Target.GetComponent<RectTransform>();

        // カーソル初期位置保存
        InitTransform = targetTransform;

        Manual = GameObject.Find("Manual");
        //manualTransform = Manual.GetComponent<RectTransform>();
        //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        manualImage = Manual.GetComponent<Image>();
        manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        // サウンド関係
        se = GameObject.Find("SE");
        // Seコンポーネント取得
        seMana = se.GetComponent<SEManager_Pause>();

    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // ポーズボタンが押されたなら
        if (ScriptPIManager.GetPause() == true)
        {
            TimeOperate();

            ScriptPIManager.SetPause(false);

            seMana.PlaySE_OK();
        }

        //Debug.Log(manual);

        // ポーズ状態の時の処理
        if (IsPause)
        {
            // スティックの入力とってくる
            float move = ScriptPIManager.GetCursorMove().y;

            if (manual == false)
            {
                // カーソルの移動
                // 左スティック or 十字ボタン
                // 入力があったなら
                if (move != 0)
                {
                    // 上入力があったなら
                    if (move > 0)
                    {
                        // カーソルを上に
                        CursorY--;
                        if (CursorY < 0)
                        {
                            CursorY = CursorMax;
                        }
                    }
                    // 下入力があったなら
                    else if (move < 0)
                    {
                        // カーソルを下に
                        CursorY++;
                        if (CursorY > CursorMax)
                        {
                            CursorY = 0;
                        }
                    }

                    // ターゲット更新
                    Target = GameObject.Find(PauseObj[CursorY]);
                    // ターゲット座標更新
                    targetTransform = Target.GetComponent<RectTransform>();

                    // 位置を移動
                    cursorTransform.position = targetTransform.position;

                    ScriptPIManager.SetCursorMove(Vector2.zero);

                    seMana.PlaySE_Select();
                }
            }

            if (manual == true)
            {
                // キャンセルボタンが入力された
                if (ScriptPIManager.GetPressB() == true)
                {
                    //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    cursorImage.color = new Color(0.0f, 0.0f, 0.0f, 0.31f);
                    manual = false;

                    ScriptPIManager.SetPressB(false);
                    ScriptPIManager.SetPressA(false);

                    seMana.PlaySE_Cansel();
                }
            }
            else
            {
                // キャンセルボタンが入力された
                if (ScriptPIManager.GetPressB() == true)
                {
                    // ポーズ終了
                    TimeOperate();

                    ScriptPIManager.SetPressB(false);

                    seMana.PlaySE_Cansel();

                }
            }

            if (manual == false)
            {
                // 決定ボタンが押された
                if (ScriptPIManager.GetPressA() == true)
                {
                    seMana.PlaySE_OK();

                    // カーソルの位置によって処理変わる
                    switch (CursorY)
                    {
                        // 続ける
                        case 0:
                            // ポーズ終了
                            TimeOperate();
                            break;

                        // リトライ
                        case 1:
                            TimeOperate();

                            // 今いるシーンをロードしなおす
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                            break;

                        // 操作方法へ
                        case 2:
                            manual = true;
                            //manualTransform.localScale = new Vector3(ManualSizeX, ManualSizeY, 0.0f);
                            manualImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                            cursorImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                            break;

                        //セレクトへ
                        case 3:
                            TimeOperate();

                            // ステージセレクトに行く
                            SceneManager.LoadScene("newSelectScene");
                            break;
                    }
                    ScriptPIManager.SetPressA(false);
                }
            }
        }
        else
        {
            if (CursorY != 0)
            {
                // カーソル位置初期化
                cursorTransform.position = InitTransform.position;
                CursorY = 0;

                //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                cursorImage.color = new Color(0.0f, 0.0f, 0.0f, 0.31f);
                manual = false;
            }
        }
    }

    // 呼び出すことで逆の状態にする関数
    //時間止めたり動かしたりする
    void TimeOperate()
    {
        // ポーズ状態なら通常再生に戻す
        if (IsPause)
        {
            // 通常再生にする
            Time.timeScale = 1f;
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;

            IsPause = false;
        }
        // 通常ならポーズ状態にする
        else
        {
            // 一時停止する
            Time.timeScale = 0f;
            this.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;

            IsPause = true;
        }
    }
}
