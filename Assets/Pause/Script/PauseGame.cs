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

    public bool Clear = false;          //ステージクリアしたか
    public bool IsPause = false; // ポーズ状態かどうか
    public int CursorY = 0; // Y方向の移動をするカーソルの番号
    const int CursorMax = 3; // カーソルの一番下
    public bool magnification = false; // 拡大用変数
    public bool reduction = false;     // 縮小用変数
    [Header("1 / ChangeSpeed 秒でサイズが変わりきる")]public float ChangeSpeed = 1.0f; // 
    private bool manual = false;

    private bool retry_Out = false; // リトライが選択されたらtrue
    private bool retry_In = false;  // シーンが読み込まれたらtrue

    private bool select_Out = false; // セレクトが選択されたらtrue

    [Header("フェードアウトとフェードインの間隔")] public float OutInTime = 0.5f;
    private float WaitTimer = 0f;
    private bool fadeout = false;
    private bool fadein = false;

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
    private float InitPosX; // カーソルが最初にいる位置を保存しておく変数
    private float InitPosY; // カーソルが最初にいる位置を保存しておく変数

    private GameObject Manual;
    private RectTransform manualTransform;
    private Image manualImage;
    private GameObject black;
    private Image blackImage;

    [SerializeField] PauseSnap snap;    //ポーズ用に音声変換するためのスクリプト

    //private GameObject player;
    //private Transform playerTransform;
    //private PlayerStatas playerStatus;

    // BGM
    [SerializeField] private BGMFadeManager _BGMFadeMana;

    // se関係
    private GameObject se;
    private SEManager_Pause seMana;

    // フェード関係
    [Header("SceneManager"),SerializeField]private GameObject sceneManager;
    private Fade fade;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        // 非表示にする
        //this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
        transform.localScale = new Vector3(0f, 0f, 0f);

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
        InitPosX = cursorTransform.anchoredPosition.x;
        InitPosY = cursorTransform.anchoredPosition.y;

        Manual = GameObject.Find("Manual");
        //manualTransform = Manual.GetComponent<RectTransform>();
        //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        manualImage = Manual.GetComponent<Image>();
        manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        // 背景暗くする
        black = GameObject.Find("Black");
        blackImage = black.GetComponent<Image>();
        blackImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        // サウンド関係
        se = GameObject.Find("SE");
        // Seコンポーネント取得
        seMana = se.GetComponent<SEManager_Pause>();

        // リスポーン関係
        //player = GameObject.Find("player");
        //playerTransform = player.GetComponent<Transform>();
        //playerStatus = player.GetComponent<PlayerStatas>();



        // フェード関係
        fade = sceneManager.GetComponent<Fade>();

        if(FadeAlpha.black == true)
        {
            retry_In = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // ポーズボタンが押されたなら(ステージクリアしていなかったら)
        if (!Clear)
        {
            if (ScriptPIManager.GetPause() == true)
            {
                //TimeOperate();

                if (IsPause == false)
                {
                    magnification = true;

                    snap.PauseSnapChange(); //音を籠らせる

                }
                else
                {
                    manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    // 子オブジェクトのα値を全て更新
                    for (int i = 0; i < Manual.transform.childCount; i++)
                    {
                        Manual.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    }
                    reduction = true;

                    snap.NormalSnapChange(); // 元に戻す
                }

                ScriptPIManager.SetPause(false);

                seMana.PlaySE_OK();
            }
        }

        if (magnification)
        {
            // ポーズメニューを拡大
            Magnification();
        }

        if (reduction)
        {
            // ポーズメニューを縮小
            Reduction();
        }

        // リトライフェードアウト込み
        if(retry_Out)
        {
            Retry_FadeOut();
        }

        // リトライフェードイン
        if (retry_In)
        {
            Retry_FadeIn();
        }

        // セレクトフェードアウト
        if (select_Out)
        {
            Select_FadeOut();
        }

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
                    cursorTransform.anchoredPosition = new Vector2(InitPosX,targetTransform.anchoredPosition.y);

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
                    // 子オブジェクトのα値を全て更新
                    for(int i =0;i< Manual.transform.childCount; i++)
                    {
                        Manual.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    }

                    cursorImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
                    //TimeOperate();
                    reduction = true;

                    snap.NormalSnapChange();    //元の音に戻す
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
                            snap.NormalSnapChange();    //元の音に戻す
                            // ポーズ終了
                            reduction = true;
                            break;

                        // リトライ
                        case 1:
                            //reduction = true;

                            retry_Out = true;
                            
                            break;

                        // 操作方法へ
                        case 2:
                            manual = true;
                            //manualTransform.localScale = new Vector3(ManualSizeX, ManualSizeY, 0.0f);
                            manualImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                            // 子オブジェクトのα値を全て更新
                            for (int i = 0; i < Manual.transform.childCount; i++)
                            {
                                Manual.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                            }
                            cursorImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                            break;

                        //セレクトへ
                        case 3:
                            select_Out = true;

                            
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
                cursorTransform.anchoredPosition = new Vector2(InitPosX, InitPosY);
                //cursorTransform.position = InitTransform.position;
                CursorY = 0;

                // ターゲット更新
                Target = GameObject.Find(PauseObj[CursorY]);
                // ターゲット座標更新
                targetTransform = Target.GetComponent<RectTransform>();

                //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                cursorImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
            //this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
            blackImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

            IsPause = false;
        }
        // 通常ならポーズ状態にする
        else
        {
            // 一時停止する
            Time.timeScale = 0f;
            //this.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            blackImage.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            IsPause = true;
        }
    }

    // ポーズメニュー拡大演出
    void Magnification()
    {
        // スケールを徐々に大きくしていく
        float scale = transform.localScale.x + ChangeSpeed * Time.unscaledDeltaTime;

        // 上限
        if(scale > 1f)
        {
            scale = 1f;
        }

        // 計算結果をスケールに代入
        if (transform.localScale.x < 1.0f) 
        {
            transform.localScale = new Vector3(scale, scale, 0);
        }
        else
        {
            // 拡大しきったら
            magnification = false;

            TimeOperate();
        }
    }

    // ポーズメニュー縮小演出
    void Reduction()
    {
        // スケールを徐々に小さくしていく
        float scale = transform.localScale.x - ChangeSpeed * Time.unscaledDeltaTime;

        // 下限
        if (scale < 0f)
        {
            scale = 0f;
        }

        // 計算結果をスケールに代入
        if (transform.localScale.x > 0.0f)
        {
            transform.localScale = new Vector3(scale, scale, 0);
        }
        else
        {
            // 縮小しきったら
            reduction = false;

            TimeOperate();
        }
    }

    private void Retry_FadeOut()
    {
        Fade.FadeState fadestate = fade.GetFadeState();

        // フェードアウトする初回のみ入る
        if (fadestate != Fade.FadeState.FadeOut && fadeout == false)
        {
            // フェードアウト開始
            fade.FadeOut();

            // BGMフェード開始
            _BGMFadeMana.SmallStageBGM();
            _BGMFadeMana.SmallBossBGM();

            fadeout = true;
        }

        if(fadestate == Fade.FadeState.FadeOut_Finish)
        {
            WaitTimer += Time.unscaledDeltaTime;

            if(WaitTimer > OutInTime)
            {
                //FadeAlpha.black = true;

                //Debug.Log("シーンをロード");

                // 今いるシーンをロードしなおす
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void Retry_FadeIn()
    {
        // 画面のフェードイン
        fade.FadeIn();
        // BGMフェードイン(シーンのロードをするので必要はない)
        //_BGMFadeMana.BigStageBGM();

        retry_In = false;

        // 通常再生にする
        Time.timeScale = 1f;
    }

    private void Select_FadeOut()
    {
        Fade.FadeState fadestate = fade.GetFadeState();

        // フェードアウトする初回のみ入る
        if (fadestate != Fade.FadeState.FadeOut && fadeout == false)
        {
            // フェードアウト開始
            fade.FadeOut();

            // BGMフェード開始
            _BGMFadeMana.SmallStageBGM();

            fadeout = true;
        }

        if (fadestate == Fade.FadeState.FadeOut_Finish)
        {
            WaitTimer += Time.unscaledDeltaTime;

            if (WaitTimer > OutInTime)
            {
                //FadeAlpha.black = true;

                TimeOperate();

                InMainScene.inMainScene = false;

                // ステージセレクトに行く
                SceneManager.LoadScene("newSelectScene");
            }
        }
    }
}
