//---------------------------------------------------------
//担当者：二宮怜
//内容　：ポーズ画面
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    public bool IsPause = false; // ポーズ状態かどうか
    private bool IgnoredFlg = false; // trueの時ifの条件無視する
    public int CursorY = 0; // Y方向の移動をするカーソルの番号
    const int CursorMax = 2; // カーソルの一番下

    // メニューの数が増えるたびに追加
    private string[] PauseObj = {
        "Continue",
        "Retry",
        "Select" };

    // 外部取得
    private GameObject PlayerInputMana;
    private PlayerInputManager ScriptPIManager;
    private GameObject Cursor; // カーソル
    private RectTransform cursorTransform; // カーソルの座標
    private GameObject Target; // カーソルの位置の基準となるobj
    private RectTransform targetTransform; // Targetの座標取得
    private RectTransform InitTransform; // カーソルが最初にいる位置を保存しておく変数

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

        // カーソルの位置の基準となるobj探す
        Target = GameObject.Find(PauseObj[CursorY]);

        // 現時点でのTargetの座標取得
        targetTransform = Target.GetComponent<RectTransform>();

        // カーソル初期位置保存
        InitTransform = targetTransform;
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
        }

        // ポーズ状態の時の処理
        if (IsPause)
        {
            // スティックの入力とってくる
            float move = ScriptPIManager.GetCursorMove().y;

            // カーソルの移動
            // 左スティック or 十字ボタン
            // 入力があったなら
            if (move != 0)
            {
                // 上入力があったなら
                if(move > 0)
                {
                    // カーソルを上に
                    CursorY--;
                    if(CursorY < 0)
                    {
                        CursorY = CursorMax;
                    }
                }
                // 下入力があったなら
                else if(move < 0)
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
            }

            // キャンセルボタンが入力された
            if (ScriptPIManager.GetPressB() == true)
            {
                // ポーズ終了
                TimeOperate();

                ScriptPIManager.SetPressB(false);
            }

            // 決定ボタンが押された
            if(ScriptPIManager.GetPressA() == true)
            {
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

                    //セレクトへ
                    case 2:
                        // ステージセレクトに行く
                        SceneManager.LoadScene("SelectScene");
                        break;
                }
                ScriptPIManager.SetPressA(false);
            }
        }
        else
        {
            if (CursorY != 0)
            {
                // カーソル位置初期化
                cursorTransform.position = InitTransform.position;
                CursorY = 0;
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
