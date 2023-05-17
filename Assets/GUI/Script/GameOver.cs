//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤーの体力が0になった時にゲームオーバーにシーン遷移する
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    [Header("現在のHP")]
    public int HP = 5; //体力

    [Header("最大HP")]
    public int maxHp = 5; //表示するHPUIの個数

    private GameObject health; // HPUIManagerオブジェクト
    private DrawHpUI drawHpUI; // HP描画スクリプト

    // フェード関係
    [Header("フェードアウトとフェードインの間隔")]public float OutInTime = 0.5f;
    private float OutInTimer = 0; // タイマー
    private bool _fadeout = false; // フェードアウト中
    private bool WaitTime = false; // フェードインとアウトの待ち時間中
    private bool hell = false; // 奈落
    private bool death = false; // 死亡

    private Transform playerTransform;
    private PlayerStatas playerStatus;
    private Fade fade;

    // 二宮追加
    private GameObject StageData;
    private GameObject StagePrefab;
    private RespawnObjManager _respawnObjManager;

    //―追加担当者：中川直登―//
    [SerializeField, Header("パーティクル")]
    private ParticleSystem _particle;
    [SerializeField]
    private float _creatTime;
    private ParticleSystem _createdParticle;
    private bool Create = false;

    private bool _isGameOver;// フラグ
    [SerializeField, Header("終わり待ち時間")]
    private float _waitTime;
    private float _nowTime;

    private SceneChange _scene;
    private GameObject cam;

    private PlayerMove _playerMove;
    private New_PlayerJump _playerJump;
    private CrackAutoMove _crackAuto;
    private SmashScript _smashScript;
    [SerializeField,Header("ゴールエリアのオブジェ")]
    private GameObject _goalArea;
    // GameOverCameraEventと少し干渉するので止めるため
    private CameraZoom _cameraZoom;
    private CameraControl2 _cameraControl2;
    //――――――――――――//

    private void Start()
    {
        //wallSystem = GameObject.Find("Wall_Hp_Gauge");
        //wallHpSystem = wallSystem.GetComponent<Wall_HP_System_Script>();
        //―追加担当者：中川直登―//
        _isGameOver = false;
        _nowTime = 0.0f;
        //--------------------------------------

        StageData = GameObject.Find("StageData");

        //SceneChangeの取得
        _scene = GameObject.Find("SceneManager").GetComponent<SceneChange>();
        if (_scene == null) Debug.LogError("SceneChangeのコンポーネントを取得できませんでした。");
        cam = GameObject.Find("Main Camera");
        if (cam == null) Debug.LogError("Main Cameraが見つかりませんでした。");
        _goalArea = GameObject.Find("StageData").transform.GetChild(0).gameObject;
        

        _playerMove = GetComponent<PlayerMove>();
        if (_playerMove == null) Debug.LogError("PlayerMoveのコンポーネントを取得できませんでした。");
        _playerJump = GetComponent<New_PlayerJump>();
        if (_playerJump == null) Debug.LogError("PlayerJumpのコンポーネントを取得できませんでした。");
        _crackAuto = GetComponent<CrackAutoMove>();
        if (_crackAuto == null) Debug.LogError("CrackAutoMoveのコンポーネントを取得できませんでした。");

        _smashScript = GetComponent<SmashScript>();
        if (_smashScript == null) Debug.LogError("SmashScripteのコンポーネントを取得できませんでした。");

        _cameraZoom = _goalArea.GetComponent<CameraZoom>();
        if (_cameraZoom == null) Debug.LogError("CameraZoomのコンポーネントを取得できませんでした。");

        _cameraControl2 = cam.GetComponent<CameraControl2>();
        if (_cameraControl2 == null) Debug.LogError("CameraControl2のコンポーネントを取得できませんでした。");
        //――――――――――――//

        // DrawHpUIスクリプト取得
        health = GameObject.Find("Health");
        drawHpUI = health.GetComponent<DrawHpUI>();

        // リスポーン関係
        playerTransform = GetComponent<Transform>();
        playerStatus = GetComponent<PlayerStatas>();
        StagePrefab = StageData.transform.GetChild(0).gameObject;
        //Debug.Log(StagePrefab);
        _respawnObjManager = StagePrefab.GetComponent<RespawnObjManager>();
        //Debug.Log(_respawnObjManager);

        // フェード関係
        fade = GameObject.Find("SceneManager").GetComponent<Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------
        // 壁のHPに合わせてUIの数、状態を変化

        // 壁の体力を取得
        //nowWallHp = wallHpSystem.GetHp();

        // 壁の体力とUIの体力を比較して状態、個数を計算
        // maxWallHp(1.0f) を maxHP(UI) の段階(今は5段階)に分割、それ(0.2)に現在のHPUIの個数-1(最初なら4)を掛けた数(最初なら0.8)と壁のHPを比較
        //if(nowWallHp < (HP - 1) * (maxWallHp / ((float)maxHp)))
        //{
        //    // UIの数減らす
        //    HP--;

        //    //初期状態
        //    spriteStatus = SPRITESTATUS.HIGH;
        //}

        // 多分0.2から0.0000001くらいまでの値になる
        //float temp = nowWallHp - (HP - 1) * (maxWallHp / ((float)maxHp));

        // tempの値によって状態を変える
        // 0.2を三段階にわけて状態を対応付け
        //if(temp > Baseline1)
        //{
        //    spriteStatus = SPRITESTATUS.HIGH;
        //}
        //else if(temp > Baseline2)
        //{
        //    spriteStatus = SPRITESTATUS.MIDDLE;
        //}
        //else
        //{
        //    spriteStatus = SPRITESTATUS.LOW;
        //}

        // HPが0になってない時
        //if (HP != 0)
        //{
        //    // 取得してきたwallHPが0なら
        //    if (nowWallHp == 0.0f)
        //    {
        //        // 0にする
        //        HP = 0;
        //    }
        //}

        //------------------------------------------
        //奈落に落ちた
        if (transform.position.y < -15)
        {
            hell = true;
            _isGameOver = true;

        }
        // HPがなくなった
        if(HP <= 0)
        {
            death = true;
            _isGameOver = true;
        }

        //---------------------------------------------------------
        //HPが0か奈落に落ちたらリスポーン
        if (death || hell)
        {
            //―追加担当者：中川直登―//

            //Deactivate();

            // パーティクルが生成されていないなら
            if (_createdParticle == null && _nowTime>_creatTime && Create == false) 
            {
                Vector3 pos = cam.transform.position;
                pos = new Vector3(pos.x, pos.y, 0);
                _createdParticle = Instantiate(_particle, pos, Quaternion.Euler(-90, 0, 0), cam.transform);
                _createdParticle.Play();
                Create = true;
            }
           
            // 一定時間経過したら
            if (_nowTime >_waitTime)
            {
                //---------------------------------------------------------
                // "GameOver"シーンに遷移
                //SceneManager.LoadScene("GameOver");
                //_scene.LoadScene("newSelectScene");

                // フェードの状態取得
                Fade.FadeState fadestate = fade.GetFadeState();

                // フェードアウト
                if (fadestate != Fade.FadeState.FadeOut && _fadeout == false)
                {
                    // フェードアウト開始
                    fade.FadeOut();

                    _fadeout = true;
                }

                // 待ち時間中にリスポーンと巻き戻し処理(未実装)
                if(fadestate == Fade.FadeState.FadeOut_Finish || WaitTime)
                {
                    // 初回のみ
                    if(OutInTimer == 0f)
                    {
                        // リスポーン

                        // リスポーン時のステータスを取得
                        //RespawnStatus _respawnSta = playerStatus.GetRespawnStatus();

                        Debug.Log("リスポーン");

                        // 保存されているリスポーン座標をプレイヤーに代入
                        playerTransform.position = playerStatus.respawnStatus.PlayerRespawnPos;

                        // 落下速度があがりすぎて床を貫通しないようにするため
                        _playerJump.MoveY = 0f;

                        // HPUI初期化
                        drawHpUI.NowHPAnimationNumber = 0;
                        drawHpUI.InitImage();

                        //Debug.Log("SSS");

                        // 巻き戻し処理
                        _respawnObjManager.RespawnInit();

                        //Debug.Log("SSS");
                        //Debug.Log(playerStatus.respawnStatus.RespawnCrystalNum);

                        // HP回復
                        HP = maxHp;

                        WaitTime = true;
                        _isGameOver = false;
                    }

                    OutInTimer += Time.deltaTime;
                    if(OutInTimer > OutInTime)
                    {
                        // フェードイン開始
                        fade.FadeIn();

                        // 生成したパーティクル削除
                        Destroy(_createdParticle);
                        //Debug.Log(playerStatus.respawnStatus.RespawnCrystalNum);
                        // クリスタル所持数もセット
                        playerStatus.SetCrystal(playerStatus.respawnStatus.RespawnCrystalNum);

                        // 初期化
                        OutInTimer = 0f;
                        _fadeout = false;
                        WaitTime = false;
                        Create = false;
                        hell = false;
                        death = false;
                    }
                }

            }
            _nowTime += Time.deltaTime;
            //――――――――――――//
           
        }
    }

    public void StartHPUIAnimation()
    {
        drawHpUI.Set_HPAnim(true);
    }

    public void DecreaseHP(float _hp)
    {
        HP = HP - (int)_hp;
        if(HP < 0)
        {
            HP = 0;
        }


    }
    
    //―追加担当者：中川直登―//
    public bool IsGameOver // 外部閲覧用―外部からの設定変更なし
    {
        get
        {
            return _isGameOver;
        }
    }

    // プレイヤーの行動系スクリプトを非アクティブ化
    private void Deactivate() 
    {
        _playerMove.enabled = false;
        _playerJump.enabled = false;
        _crackAuto.enabled = false;
        _smashScript.enabled = false;
        _goalArea.SetActive(false);
        _cameraZoom.enabled = false;
        _cameraControl2.enabled = false;
    }

    private void activate()
    {
        _playerMove.enabled = true;
        _playerJump.enabled = true;
        _crackAuto.enabled = true;
        _smashScript.enabled = true;
        _goalArea.SetActive(true);
        _cameraZoom.enabled = true;
        _cameraControl2.enabled = true;
    }
    //――――――――――――//
    //public SPRITESTATUS GetSpriteStatus()
    //{
    //    return spriteStatus;
    //}
}
