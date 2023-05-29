//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：氷エリアのボスの処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IceBoss : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―公開変数―(公)

    public enum StateID
    {
        NULL,
        WALK,       // 移動
        RUN,        // 突進
        CONFUSE,    // 混乱
        EATING,     // 食事
        ATTACK,     // 攻撃
        DEATH,      // 死
    }

    public GameObject fish;// 魚

    //-----------------------------------------------------------------
    //―秘匿変数―(私)

    [SerializeField, Header("歩く速度")]
    private float _walkSpeed;
    [SerializeField, Header("走る速度")]
    private float _runSpeed;

    [SerializeField, Header("時間")]
    private float _coolTime;
    [SerializeField]
    private float _confuselTime;
    [SerializeField]
    private float _eatTime;
    private float _nowTime;

    [SerializeField,Header("あたり判定")]
    private CheckArea _isRun;// 発見
    [SerializeField]
    private CheckArea _isAttcked;// 攻撃
    [SerializeField]
    private CheckArea _isWall;// 壁
    [SerializeField]
    private GameObject _HitBox;
    [SerializeField]
    private CheckArea _isIceBlock;// 
    [SerializeField, Header("体力")]
    private IceBossHealth _health;


    [SerializeField, Header("状態")]
    private StateID _state;
    [SerializeField]
    private Directing_BossLight bossDirecting;
    private Animator _anim;

    private bool _direction; // true = 右　false = 左　※現段階
    private float _sizeX;

    private bool _isFish;
    private Vector3 StartPos;

    private BGMFadeManager _BGMfadeMana;

    //-----------------------------------------------------------------
    // Use this for initialization
    void Start()
    {
        StartPos = transform.position;
        _sizeX = transform.localScale.x;
        //--------------------------------------
        // 設定のチェック

        // 発見
        if (_isRun == null) Debug.LogError("IsRunが設定されてません。");
        // 攻撃
        if (_isAttcked == null) Debug.LogError("IsAttckedが設定されてません。");
        // 壁
        if (_isWall == null) Debug.LogError("IsWallが設定されてません。");

        if (_HitBox == null) Debug.LogError("HitBoxが設定されてません。");
        if (_isIceBlock == null) Debug.LogError("_isIceBlockが設定されてません。");
        if (bossDirecting ==null) Debug.LogError("bossDirectingが設定されてません。");
        //--------------------------------------
        // Animatorのコンポーネント取得
        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.LogError("Animatorのコンポーネントを取得できませんでした。");
        Init();

        _BGMfadeMana = GameObject.Find("Main Camera").GetComponent<BGMFadeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (State) 
        {
            case StateID.WALK:
                Walk();
                break;
            case StateID.RUN:
                Run();
                break;
            case StateID.CONFUSE:
                Confuse();
                break;
            case StateID.EATING:
                Eating();
                break;
            case StateID.DEATH:
                Death();
                break;
        }
       
        transform.localScale = new Vector3(_sizeX * Direction * -1, transform.localScale.y, transform.localScale.z);
    }
    //-----------------------------------------------------------------
    //★★公開関数★★(公)

    public void Init()
    {
        transform.position = StartPos;
        // 初期化処理
        _state = StateID.WALK;
        _direction = false;
        _nowTime = 0.0f;
        _health.Init();
    }
    public StateID State
    {
        get
        {
            return _state;
        }
    }

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    //
    // 関数：Walk() 
    //
    // 内容：歩いてる状態の処理
    //
    private void Walk() 
    {
        // プレイヤーが攻撃範囲に入ったら
        if (_isRun.IsEnter&&_nowTime>=_coolTime) 
        {
            // 状態変更：走る
            _state = StateID.RUN;
            // 走るアニメーションフラグ
            _anim.SetBool("isRun", true);

            // リセット
            _nowTime = 0.0f;
        }
        // 魚が落ちたら
        if (fish != null) 
        {
            // 状態変更：食べる
            _state = StateID.EATING;
            // リセット
            _nowTime = 0.0f;
        }
        // 壁に当たったら
        if (Wall)
        {
            // 反転する
            _direction = !_direction;
        }

        // 移動
        float move = _walkSpeed * Time.deltaTime * Direction;
        // 歩く
        transform.Translate(move, 0, 0);

        _nowTime += Time.deltaTime;
    }

    //
    // 関数：Run() 
    //
    // 内容：走っている状態の処理
    //
    private void Run() 
    {
        // プレイヤーに当たったら
        if (_isAttcked.IsEnter) 
        {
            // 状態変更：攻撃
            _state = StateID.ATTACK;
            
            // 走るアニメーションフラグ
            _anim.SetBool("isAttack", true);
        }

        // 壁に当たったら
        if (Wall)
        {
            // 状態変更：混乱
            _state = StateID.CONFUSE;

            // 混乱アニメーションフラグ
            _anim.SetBool("isConfuse", true);
        }

        // 移動
        float move = _runSpeed * Time.deltaTime * Direction;
        // 走る
        transform.Translate(move, 0, 0);
    }

    //
    // 関数：Confuse() 
    //
    // 内容：混乱時の処理
    //
    private void Confuse() 
    {
        // 時間経過したら
        if (_nowTime >= _confuselTime) 
        {
            _state = StateID.WALK;
            // 走るアニメーションフラグ
            _anim.SetBool("isRun", false);
            // 混乱アニメーションフラグ
            _anim.SetBool("isConfuse", false);
            // リセット
            _nowTime = 0.0f;
        }
        // 時間計算
        _nowTime += Time.deltaTime;
    }

    private void Eating() 
    {
        Vector3 vec = transform.position - fish.transform.position;
        //勝手にあたり判定が移動するので固定する。
        _HitBox.transform.localPosition = new Vector3(0, 0, 0);
        // 魚が近くにあったら
        if (_isFish)
        {
            
            // 時間経過したら
            if (_nowTime >= _eatTime) 
            {
                // オブジェクトを破壊
                Destroy(fish);
                // 食べるアニメーションフラグ
                _anim.SetBool("isEating", false);
                // 状態変更：歩く
                _state = StateID.WALK;
                _isFish = false;
                // リセット
                _nowTime = 0.0f;

                _health.SetDamageFlag();
            }
            else 
            {
                fish.SetActive(false);
                // 食べるアニメーション
                _anim.SetBool("isEating", true);
            }
            // 時間計算
            _nowTime += Time.deltaTime;
        }
        else 
        {
            // ボス視点から魚の方向に向く
            _direction = (transform.position.x < fish.transform.position.x);

            float move = _walkSpeed * Time.deltaTime * Direction;
            // 魚の所まで歩く
            transform.Translate(move, 0, 0);
        }

        // 体力がなくなったら
        if (_health.HP <= 0)
        {
            _state = StateID.DEATH;
        }
    }

    private void Death() 
    {
        // 走るアニメーションフラグ
        _anim.SetBool("isDeath", true);
        bossDirecting.Flash();
        // ボスBGMフェードアウト
        _BGMfadeMana.SmallBossBGM();
    }

    private int Direction 
    {
        get 
        {
            if (_direction) 
            {
                return 1;
            }
            else 
            {
                return -1;
            }
        }
    }

    private bool Wall 
    {
        get 
        {
            //if (_isWall) return true;
            //if (_isIceBlock) return true;

            Vector2 pos = new Vector2(transform.position.x, transform.position.y-0.8f);
            Vector2 rayDirection = new Vector2(1, 0);
            int layerMask = 1 << 10 | 1 << 14 ;    //Rayのレイヤーマスク
            RaycastHit2D hit = Physics2D.Raycast(pos, rayDirection*Direction, 1.5f, layerMask);
            Debug.DrawRay(pos, rayDirection * Direction*1.5f, Color.blue, 0.2f, false);
            if (hit) return true;
            return false;
        }
    }

    private void AttackIsEnd() 
    {
        // 状態変更：歩く
        _state = StateID.WALK;
        // 方向転換
        _direction = !_direction;
        // 走るアニメーションフラグ
        _anim.SetBool("isRun", false);
        _anim.SetBool("isAttack", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == fish) _isFish = true;
    }
}