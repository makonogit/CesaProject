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

    //private float maxWallHp = 1.0f; // 壁の最大体力
    //private float nowWallHp; // 現在の壁の最大体力
    //private float Baseline1 = 0.2f / 3 * 2; // 壁のスプライトを変更する基準値
    //private float Baseline2 = 0.2f / 3 * 1; // 壁のスプライトを変更する基準値

    // 外部取得
    //private GameObject wallSystem;
    //private Wall_HP_System_Script wallHpSystem;

    //// HPによって状態が変わる
    //// 例：HPが5の時のHIGH、MIDDLE、LOW
    //// 全15段階まで表記わけできる
    //public enum SPRITESTATUS
    //{
    //    HIGH,   // 初期状態(0.2)～0.1333...
    //    MIDDLE, // 0.13333....～0.06666...
    //    LOW     // 0.06666....～0.0
    //}

    //private SPRITESTATUS spriteStatus = SPRITESTATUS.HIGH;

    //―追加担当者：中川直登―//
    [SerializeField, Header("パーティクル")]
    private ParticleSystem _particle;
    [SerializeField]
    private float _creatTime;
    private ParticleSystem _createdParticle;

    private bool _isGameOver;// フラグ
    [SerializeField, Header("終わり待ち時間")]
    private float _waitTime;
    private float _nowTime;

    private SceneChange _scene;
    private GameObject cam;

    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
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
        //SceneChangeの取得
        _scene = GameObject.Find("SceneManager").GetComponent<SceneChange>();
        if (_scene == null) Debug.LogError("SceneChangeのコンポーネントを取得できませんでした。");
        cam = GameObject.Find("Main Camera");
        if (cam == null) Debug.LogError("Main Cameraが見つかりませんでした。");
        _goalArea = GameObject.Find("GoalArea");
        

        _playerMove = GetComponent<PlayerMove>();
        if (_playerMove == null) Debug.LogError("PlayerMoveのコンポーネントを取得できませんでした。");
        _playerJump = GetComponent<PlayerJump>();
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



        //---------------------------------------------------------
        //HPが0以下になったら
        if (HP <= 0)
        {
            //―追加担当者：中川直登―//

            Deactivate();
            _isGameOver = true;

            // パーティクルが生成されていないなら
            if (_createdParticle == null && _nowTime>_creatTime) 
            {
                Vector3 pos = cam.transform.position;
                pos = new Vector3(pos.x, pos.y, 0);
                _createdParticle = Instantiate(_particle, pos, Quaternion.Euler(-90, 0, 0), cam.transform);
                _createdParticle.Play();
            }
           
            // 一定時間経過したら
            if (_nowTime >_waitTime)
            {
                //---------------------------------------------------------
                // "GameOver"シーンに遷移
                //SceneManager.LoadScene("GameOver");
                _scene.LoadScene("newSelectScene");
            }
            _nowTime += Time.deltaTime;
            //――――――――――――//
           
        }

        //------------------------------------------
        //　奈落に落ちたらリロード
        if(transform.position.y < -15)
        {
            _scene.LoadScene("MainScene");
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
    //――――――――――――//
    //public SPRITESTATUS GetSpriteStatus()
    //{
    //    return spriteStatus;
    //}
}
