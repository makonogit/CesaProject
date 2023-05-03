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
    public StageStatas stagestatas;    // ステージのステータスを取得
    private ChangeResultScene resultScene;

    private bool First = false;

    public bool ZoomEnd = false;

    // ステージ破壊演出関係変数
    private GameObject lastCameraTarget;
    private DirectingBreakStage breakstage;
    public bool SetBreak = false; // 破壊演出呼び出ししたか


    // Start is called before the first frame update
    void Start()
    {
        // MainCamera探す
        Camera = GameObject.Find("Main Camera");
        // カメラスクリプトを取得
        Cam = Camera.GetComponent<Camera>();

        player = GameObject.Find("player");
        playertans = player.transform;

        stagestatas = GetComponent<StageStatas>();
        resultScene = GameObject.Find("ChageResultScene").GetComponent<ChangeResultScene>();

        lastCameraTarget = GameObject.Find("CameraTarget_Start");
        breakstage = lastCameraTarget.GetComponent<DirectingBreakStage>();
        SetBreak = false;
    }

    // Update is called once per frame
    void Update()
    {
        //　全て破壊されたら
        if ((!resultScene.BossStage || (resultScene.BossStage && GameObject.Find("BossEnemy").transform.childCount == 0 && resultScene.WaitFlame > 0.2f))
            && stagestatas.GetStageCrystal() == 0)
        {
            // 演出開始させていなければ
            if(SetBreak == false)
            {
                Debug.Log("背景クリスタル破壊開始");
                // 破壊演出開始
                breakstage.StartBreak();

                // フラグ
                SetBreak = true;
            }

            if (breakstage.GetBreakStage() == true)
            {
                Debug.Log("zoom");
                // ズーム後のカメラ描画サイズになるまで徐々にズームインしていく
                if (NowCameraSize > ZoomCameraSize)
                {
                    // 描画サイズ計算
                    NowCameraSize -= ChangeVolume * Time.deltaTime;
                }
                else
                {
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
}
