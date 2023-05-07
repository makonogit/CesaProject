//---------------------------------------------------------
//
// 担当者：中川直登
//
// 内容　：トロッコの動く処理
//
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Trolleys_Move : MonoBehaviour
{
    //======================================================
    // プライベート変数宣言
    
    [Space(10)]
    [SerializeField, Header("スクリプト設定")]
    private CheckArea _onPlayer;     // プレイヤーが乗っているかを判断するクラス
    [SerializeField]
    private GameObject _attackArea; // 攻撃範囲を含むGameObject
    [SerializeField]
    private CheckArea _hitEnemy; // 敵に当たった判定
    [SerializeField]
    private GameObject _damageArea; // ダメージ範囲を含むGameObject
    private Rigidbody2D _rb2d;
    private Hammer _hammer;     // ハンマーの状態を見るため
    private Hammer.HammerState _oldState;   // 状態が変わった瞬間を判別するため
    private PlayerInputManager _inputManager;   // 入力方向を判断するため

    [Space(10)]
    [SerializeField, Header("ステータス設定")]
    private float _normalPower =50; // 通常時の移動力
    [SerializeField]
    private float _dashPower = 50;  // 溜め技時の移動力
    [SerializeField]
    private float _attackTime;  // 攻撃判定時間
    private bool _dashFlag;     // ダッシュ中フラグ
    private float _nowTime;     // 攻撃の経過時間
    private int _direction = 1; // X軸の進行方向

    [Space(10)]
    [SerializeField, Header("タイヤ設定")]
    private GameObject _frontWheel;// タイヤ前
    [SerializeField]
    private GameObject _backWheel;// タイヤ後ろ

    private Vector3 _oldpos;            //前回の位置
    private float _movetime = 0.0f;     //移動時間
    TrolleyManager trolleyManager = null;

    //======================================================
    //
    // 関数：Start()
    //
    void Start()
    {
        ComponentProcess(); // コンポーネントの取得
        TrolleyInit();  // 初期化
    }

    //
    // 関数：TrolleyInit()
    //
    // 目的：初期化処理
    // 
    private void TrolleyInit() 
    {
        _dashFlag = false;
        _nowTime = 0.0f;
        _attackArea.SetActive(false);// 攻撃範囲を非表示
        _damageArea.SetActive(false);// ダメージ範囲を非表示
    }

    //
    // 関数：ComponentProcess()
    //
    // 目的：全部のコンポーネントを取得する処理をまとめた関数
    // 
    private void ComponentProcess() 
    {
        //-----------------------------------------------------
        // 設定されているか確認する

        //_onPlayer
        if (_onPlayer == null) Debug.LogError("_onPlayerが設定されていません。");
        //_attackArea
        if (_attackArea == null) Debug.LogError("_attackAreaが設定されていません。");
        // _hitEnemy
        if (_hitEnemy == null) Debug.LogError("_hitEnemyが設定されていません。");
        //_damageArea
        if (_damageArea == null) Debug.LogError("_damageAreaが設定されていません。");
        // wheels
        if (_frontWheel == null) Debug.LogError("タイヤが設定されていません。");
        if (_backWheel == null) Debug.LogError("タイヤが設定されていません。");

        //-----------------------------------------------------
        // Rigidbody2Dのコンポーネントを取得
        _rb2d = GetComponent<Rigidbody2D>();
        if (_rb2d == null) Debug.LogError("Rigidbody2Dのコンポーネントを取得できませんでした。");

        //-----------------------------------------------------
        // プレイヤーからHammerのコンポーネントを取得
        
        // プレイヤーを探す
        GameObject palyer = GameObject.Find("player");
        if (palyer == null) Debug.LogError("playerが見つかりません。");

        // コンポーネントを取得
        _hammer = palyer.GetComponent<Hammer>();
        if (_hammer == null) Debug.LogError("Hammerのコンポーネントを取得できませんでした。");
        //-----------------------------------------------------
        // PlayerInputManagerからPlayerInputManagerのコンポーネントを取得

        // PlayerInputManagerを探す
        GameObject playerInputManager = GameObject.Find("PlayerInputManager");
        if (playerInputManager == null) Debug.LogError("PlayerInputManagerが見つかりません。");
        
        // コンポーネントを取得
        _inputManager =playerInputManager.GetComponent<PlayerInputManager>();
        if (_inputManager == null) Debug.LogError("PlayerInputManagerのコンポーネントを取得できませんでした。");

    }

    //======================================================
    //
    // 関数： Update()
    //
    void Update()
    {
        _movetime += Time.deltaTime;

        if (isStop && trolleyManager != null) trolleyManager.SetStop(true); trolleyManager = null;

        SetDirection(); // 進行方向の設定
        DashSetting();  // ダッシュの設定
        AttackSetting();// 攻撃の設定
        DamageSystem(); // ダメージ処理
        Move();         // 移動処理
        RotateWheel();  // タイヤの回転
        
        if (_movetime > 0.3f)
        {
            _oldpos = transform.position;
            _movetime = 0.0f;
        }
        _oldState = _hammer.hammerstate;// 状態を保存
       

    }

    
    //
    // 関数： SetDirection()
    //
    // 目的：進行方向を設定する
    // 
    private void SetDirection()
    {
        // 動き始める瞬間に進行方向を設定
        if (isMoveTrigger)
        {
           _direction = GetDirection;
            trolleyManager = transform.parent.GetComponent<TrolleyManager>();
            trolleyManager.SetMoveTrolley(this);
        }
    }

    //
    // 関数：DashSetting() 
    //
    // 目的：ダッシュの設定をする
    //
    private void DashSetting() 
    {
        // ダッシュするなら
        if (isDash) 
        {
            _dashFlag = true;
            _nowTime = 0.0f;
        }
        // ダッシュ中なら
        if (_dashFlag) _nowTime += Time.deltaTime;
        // ダッシュが終わったら
        if (isEndDash)
        {
            _dashFlag = false;
            _nowTime = 0.0f;
        }
    }

    //
    // 関数： Move()
    //
    // 目的：移動処理
    //
    private void Move()
    {
        // 移動力を設定
        Vector2 move = new Vector2(_normalPower * _direction, 0);

        // ダッシュ時の移動力を設定
        if (_dashFlag) move = new Vector2(_dashPower * _direction, 0);

        // 移動する
        if (isMove) _rb2d.AddForce(move);
        
    }

    //
    // 関数：AttackSetting()
    //
    // 目的：攻撃の設定
    // 
    private void AttackSetting() 
    {
        // ダッシュ中ならアクティブ
        _attackArea.SetActive(_dashFlag);

        // 変数
        //Vector3 attackDirection = new Vector3(_direction, _attackArea.transform.localScale.y, _attackArea.transform.localScale.z);

        // 方向設定
        //_attackArea.transform.localScale = attackDirection;
    }

    //
    // 関数：DamageSystem() 
    //
    // 目的：攻撃の設定
    // 

    private void DamageSystem() 
    {
        _damageArea.SetActive(isDamage);
    }

    //
    // 関数：GetDirection
    //
    // 目的：入力が右か左か見て進む方向を返す-入力左:1    入力右:-1
    // 
    private int GetDirection 
    {
        get 
        {
            // 左スティックの入力から角度を取得する
            Vector2 LeftStick = _inputManager.GetMovement();
            // 右に入力したら－1
            if (LeftStick.x > 0) return -1;
            // 左なら 1
            return 1;
        }
    }

    //
    // 関数：RotateWheel() 
    //
    // 目的：タイヤの回転
    // 
    private void RotateWheel() 
    {
        Vector3 rotate = new Vector3(0, 0,-_rb2d.velocity.x);
        _frontWheel.transform.eulerAngles += rotate;
        _backWheel.transform.eulerAngles += rotate;
    }

    //======================================================
    // 条件式一覧

    //
    // 関数：isStateHammer 
    //
    // 目的：現在の状態が HAMMER かどうか見る
    // 
    private bool isStateHammer 
    {
        get 
        {
            // 現在の状態が HAMMERではないなら
            if (_hammer.hammerstate != Hammer.HammerState.HAMMER) return false;
            return true;
        }
    }

    //
    // 関数：isMoveTrigger 
    //
    // 目的：動き始める瞬間の判断をする
    // 
    public bool isMoveTrigger 
    {
        get 
        {
            // 現在の状態が HAMMERではないなら
            if (!isStateHammer) return false;
            // 前と同じ状態なら
            if (_hammer.hammerstate == _oldState) return false;
            return true;
        }
    }
    //
    // 関数： isDash 
    //
    // 目的：ダッシュするかどうかの判断をする
    // 
    private bool isDash 
    {
        get 
        {
            // 乗っていないなら
            if (!_onPlayer.IsEnter) return false;
            // 現在の状態が HAMMERではないなら
            if (!isStateHammer) return false;
            // 前の状態が POWER ではないなら
            if (_oldState != Hammer.HammerState.POWER) return false;
            return true;
        }
    }

    //
    // 関数：isMove 
    //
    // 目的：動く条件
    // 
    private bool isMove 
    {
        get 
        {
            // プレイヤーが乗ってないなら
            if (!_onPlayer.IsEnter) return false;
            // 現在の状態が HAMMERではないなら
            if (!isStateHammer) return false;
            return true;
        }
    }

    //
    // 関数：isAttackTimeOver 
    //
    // 目的：攻撃時間を過ぎたか
    // 
    private bool isAttackTimeOver 
    {
        get 
        {
            // 攻撃時間を過ぎてないなら
            if (_nowTime < _attackTime) return false;
            return true;
        }
    }

    //
    // 関数：isEndDash
    //
    // 目的：ダッシュが終わる時
    // 
    private bool isEndDash 
    {
        get 
        {
            // ダッシュ中ではないなら
            if (!_dashFlag) return false;
            // 移動する瞬間、ダッシュではないなら
            if (isMoveTrigger&&!isDash) return true;
            // 攻撃時間を過ぎてないなら
            if (!isAttackTimeOver) return false;
            return true;
        }
    }

    //
    // 関数：isDamage
    //
    // 目的：ダメージを受けるか
    // 
    private bool isDamage 
    {
        get
        {
            // ダッシュ中なら
            if (_dashFlag) return false;            
            // 敵に当たってないなら
            if (!_hitEnemy.IsEnter) return false;
            return true;
        }
    }

    public bool isStop
    {
        get
        {
            if (_oldpos != transform.position) return false;
            return true;
        }
    }
}