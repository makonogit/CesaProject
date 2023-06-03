//---------------------------------------------------------
//担当者：二宮怜
//内容　：ステージクリア後、ステージの最初の部分から壊れていき、それを映すカメラの移動
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectingBreakStage : MonoBehaviour
{
    // 変数宣言
    public bool startInit = false; // 始まって一度だけ呼び出し
    public bool startBreak = false; // 演出開始

    [Header("カメラ移動時のスピード")]public float MoveSpeed = 5.0f; // カメラ移動時のスピード
    private float timer = 0; // 演出が始まってからの経過時間
    private Transform thisTransform; // 自身の座標
    private Vector3 InitPos; // 初期位置
    private float FinalySpeed; // 計算後の最終的なスピード
    [Header("割れる時のスピード")]public float BreakSpeed = 1.1f;

    [Header("カメラ移動時に加速していくか")]public bool isAxel = true;

    private float FaderRate = 1f; // 最大値から最小値の間を変化させる

    [Header("破壊するクリスタルが横に何枚並んでいるか"),SerializeField]
    private int CrystalNum_X; // 破壊するクリスタルが横に何枚並んでいるか

    private bool BreakStage; // ステージ破壊演出が終わったらtrue
    [SerializeField] private float StartFadeOutTime = 5f;
    private bool fadeOrder = false; // フェードアウトの命令を出していたらtrue

    // クリアのフェードアウトが始まってからの経過時間
    private ResultManager _resultManager;

    // 外部取得
    private GameObject StageData; // 必要となるオブジェクトの親オブジェクト

    private GameObject StagePrefab; // ステージ情報プレハブ
    private CameraZoom sc_cameraZoom; // スクリプト

    private GameObject MainCamera;
    private CameraControl2 control;
    private GameObject player;
    private Transform _playerTransform;
    private PlayBgm bgm;

    [Header("各ステージごとに用意されたBorderLineマテリアルをセット")]private Material Mat;

    public GameObject particle; // 壊れた破片が落ちてくるパーティクル
    private GameObject ParticleObj; // 作成したパーティクルを持つ変数

    [SerializeField, Header("割れるエフェクト")]
    private ScreenBreak _ScreenBreak;

    [SerializeField, Header("背景クリスタル")]
    private List<GameObject> Crystal;

    private BGMFadeManager _BGMFadeMana;
    private AudioSource SpecialBGM; // 特殊BGM
    private bool ClearBGMflg = false;
    private Fade _fade;

    GameObject Core;
    [SerializeField, Header("ヒビ入りクリスタル管理")]
    private CrystalCrackManager _crystalmanager;

    // Start is called before the first frame update
    void Start()
    {
        // メインカメラ取得
        MainCamera = GameObject.Find("Main Camera");
        // カメラ追従スクリプト取得
        control = MainCamera.GetComponent<CameraControl2>();
        thisTransform = GetComponent<Transform>();
        InitPos = thisTransform.position;

        player = GameObject.Find("player");

        startInit = false;
        startBreak = false;
        BreakStage = false;

        _BGMFadeMana = MainCamera.GetComponent<BGMFadeManager>();
        SpecialBGM = MainCamera.transform.Find("SpecialBGM").gameObject.GetComponent<AudioSource>();
        bgm = MainCamera.GetComponent<PlayBgm>();

        _fade = GameObject.Find("SceneManager").GetComponent<Fade>();

        _playerTransform = player.GetComponent<Transform>();

        _resultManager = GameObject.Find("Result_StageClear").GetComponent<ResultManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(startInit == false)
        {
            // 目標オブジェクト取得

            // ステージの情報を持つオブジェクト取得
            StageData = GameObject.Find("StageData");
            //Debug.Log(StageData);

            // ステージプレハブ　例)Stage1-1,Stage2-1
            StagePrefab = StageData.transform.GetChild(0).gameObject;
            //Debug.Log(StagePrefab);

            // カメラズームスクリプト取得
            sc_cameraZoom = StagePrefab.GetComponent<CameraZoom>();
            //Debug.Log(sc_cameraZoom);

            // 初期化
            //Mat.SetFloat("_Fader", 1f);  // 初期値
            //Mat.SetFloat("_Width", -9f); // 固定

            startInit = true;
        }

        if(startBreak == true && BreakStage == false)
        {
            // カメラ追従ターゲットを変更
            if(control.GetTarget() != this.gameObject)
            {
                // カメラ追従ターゲットをステージの最初の方にある自身に設定
                control.SetTarget(this.gameObject);
                //Debug.Log(control.GetTarget());

                // このスクリプトを持つオブジェクトの子オブジェクトに
                // ParticleObj = Instantiate(particle, this.gameObject.transform.GetChild(0).gameObject.transform);

                _BGMFadeMana.SmallStageBGM();
                _BGMFadeMana.SmallBossBGM();
            }

            // カメラ追従ターゲットの位置を目標地点まで加速しながら移動させる
            if (thisTransform.position.x < _playerTransform.position.x)
            {
                bgm.CrackLoop();
                // 座標を目標オブジェクトの方まで移動させる
                // 後になればなるほど速くなる
                // 加速するなら
                if (isAxel == true)
                {
                    FinalySpeed = MoveSpeed + MoveSpeed * timer / 2f;
                }
                // 等速で移動させる
                else
                {
                    FinalySpeed = MoveSpeed;
                }

               thisTransform.Translate(FinalySpeed * Time.deltaTime, 0f, 0f);
            }
            else
            {
                thisTransform.position = new Vector3(_playerTransform.position.x,
                    thisTransform.position.y, 
                    thisTransform.position.z);
            }

            // 背景クリスタルを消す割合
            FaderRate -= Time.deltaTime * BreakSpeed;
            // 背景クリスタルマテリアルの閾値を最大値から最小値に下げる
            //Mat.SetFloat("_Fader", FaderRate);

            if (FaderRate <= 1f - CrystalNum_X * 2f)
            {
                if (!_ScreenBreak.enabled)
                {
                    // コアを取得、消去
                    Core = GameObject.Find("Core").transform.GetChild(0).gameObject;
                    Destroy(Core);

                    for (int i = 0; i < Crystal.Count; i++)
                    {
                        Destroy(Crystal[i]);
                    }


                    // 初めの一回のみ入る
                    if (ClearBGMflg == false)
                    {
                        // ステージクリアBGM再生開始
                        _BGMFadeMana.StageClear();

                        ClearBGMflg = true;
                    }

                    // Destroy(ParticleObj);
                    _ScreenBreak.enabled = true;
                }
            }

            if(ClearBGMflg == true)
            {
                if(SpecialBGM.time > 2.5f) // ←最低な決め打ちです
                {
                    // ステージ破壊完了フラグ
                    BreakStage = true;

                    ClearBGMflg = false;

                    Debug.Log("ステージ破壊完了");
                }
            }

            // カウント
            timer += Time.deltaTime;
        }
        else
        {
            // 演出が始まってないと入らない 演出の途中でセレクトに戻ってしまうバグ解消
            if (startBreak == true)
            {
                // 一定時間経過した最初のフレームのみ入る
                if (SpecialBGM.time > StartFadeOutTime && fadeOrder == false)
                {
                    // 画面暗くしていく
                    _fade.FadeOut();

                    // クリアBGM小さくしていく
                    _BGMFadeMana.smallSpecialBGM();

                    fadeOrder = true;
                }

                // フェードアウトが終了していたらセレクトに遷移するための関数呼び出し
                if (_fade.GetFadeState() == Fade.FadeState.FadeOut_Finish)
                {
                    _resultManager.GoSelectScene();
                }

                thisTransform.position = InitPos;
                control.SetTarget(player);
                timer = 0f;

                FaderRate = 1f;
                //Mat.SetFloat("_Fader", FaderRate);
            }
        }
    }

    public bool GetBreakStage()
    {
        return BreakStage;
    }

    public void StartBreak()
    {
        _crystalmanager.clear = true;
        startBreak = true;
    }
}
