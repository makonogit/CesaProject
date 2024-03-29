//---------------------------------------------------------
//担当者：二宮怜
//内容　：カメラのズーム
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    
   [Header("一秒あたりの変化量")]
    public float ChangeVolume = 0.1f; // カメラサイズの変化量

    [Header("ズーム後の描画サイズ")]
    public float ZoomCameraSize = 2.5f;

    [Header("通常時の描画サイズ")]
    public float DefaultCameraSize = 5.0f; //通常時のカメラサイズ

    [Header("現在のカメラサイズ")]
    public float NowCameraSize = 5.0f; // 現在のカメラサイズ

    // 外部取得
    private GameObject Camera; // ゲームオブジェクトMainCamera
    Camera Cam; // カメラスクリプトを取得

    private Transform cameraTransform; // カメラの座標

    private GameObject player;      // プレイヤー
    private Transform playertans;   // プレイヤーのTransform
    private Animator PlayerAnim;    // プレイヤーのAnimator
    public StageStatas stagestatas;    // ステージのステータスを取得
    private ChangeResultScene resultScene;
    private PauseGame Pause;    //ポーズを開けないようにする

    private bool First = false;
  
    public bool ZoomEnd = false;

    private float Wait = 0.0f;

    // ステージ破壊演出関係変数
    private GameObject lastCameraTarget;
    private DirectingBreakStage breakstage;
    public bool SetBreak = false; // 破壊演出呼び出ししたか
   
    private Hammer hammer;        //　ハンマー
    private PolygonCollider2D camerarea;  //　カメラの移動制限

    // Start is called before the first frame update
    void Start()
    {
        // MainCamera探す
        Camera = GameObject.Find("Main Camera");
        // カメラスクリプトを取得
        Cam = Camera.GetComponent<Camera>();

        // カメラの制限用コライダー
        camerarea = GameObject.Find("CameraArea").GetComponent<PolygonCollider2D>();

        Pause = GameObject.Find("PausePanel").GetComponent<PauseGame>();    //ポーズスクリプト取得

        player = GameObject.Find("player");
        playertans = player.transform;
        PlayerAnim = player.GetComponent<Animator>();
        hammer = player.GetComponent<Hammer>();

        stagestatas = GetComponent<StageStatas>();
        resultScene = GameObject.Find("ChageResultScene").GetComponent<ChangeResultScene>();

        lastCameraTarget = GameObject.Find("CameraTarget_Start");
        breakstage = lastCameraTarget.GetComponent<DirectingBreakStage>();
        SetBreak = false;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (Wait > 0.5f)
        {
            //　全て破壊されたら
            if ((!resultScene.BossStage || (resultScene.BossStage && GameObject.Find("BossEnemy").transform.childCount == 0 && resultScene.WaitFlame > 0.2f))
                && stagestatas.GetStageCrystal() == 0)
            {
                // 演出開始させていなければ
                if (SetBreak == false)
                {
                    // コライダーの初期化
                    Vector2[] points = camerarea.points;
                    points[1].x = -28.7f;
                    points[2].x = -28.7f;
                    camerarea.SetPath(0, points);

                    Debug.Log("背景クリスタル破壊開始");
                    // 破壊演出開始
                    breakstage.StartBreak();

                    hammer.hammerstate = Hammer.HammerState.NONE;   //強制的に状態を修正
                    if (PlayerAnim.GetBool("angle")) PlayerAnim.SetBool("angle", false);
                    if (PlayerAnim.GetBool("crack")) PlayerAnim.SetBool("crack", false);
                    if (PlayerAnim.GetBool("accumulate")) PlayerAnim.SetBool("accumulate", false);

                    Pause.Clear = true;　//クリア状態をセットしてポーズを開けないようにする
                    // フラグ
                    SetBreak = true;
                }

                if (breakstage.GetBreakStage() == true)
                {
                    //Debug.Log("zoom");
                    // ズーム後のカメラ描画サイズになるまで徐々にズームインしていく
                    if (NowCameraSize > ZoomCameraSize)
                    {
                        // 描画サイズ計算
                        NowCameraSize -= ChangeVolume * Time.deltaTime;
                    }
                    else
                    {
                        PlayerAnim.SetBool("Clear", true);
                        NowCameraSize = ZoomCameraSize;
                        ZoomEnd = true;
                    }

                    // カメラの位置はプレイヤーの中心に固定
                    //cameraTransform.position = playertans.position;

                    //カメラサイズ変更
                    Cam.orthographicSize = NowCameraSize;
                }
            }
        }
        Wait += Time.deltaTime; //なぜかステージ開始時に始まってしまう
    }

    public void RespawnInit()
    {
        SetBreak = false;
    }
}
