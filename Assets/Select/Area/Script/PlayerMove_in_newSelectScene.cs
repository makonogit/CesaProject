//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：SelectSceneのプレイヤーの移動処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMove_in_newSelectScene : MonoBehaviour
{
    public enum PlayerState_in_newSelectScene
    {
        WALK,
        CRACK,
    }

    //-----------------------------------------------------------------
    //―秘匿変数―(私)
    [SerializeField,Header("移動速度")]
    private float _moveSpeed;
    private Vector2 _inputValue;
    private Rigidbody2D _rigid;
    private PlayerState_in_newSelectScene _state;
    [SerializeField,Header("パーティクルobj")]
    private GameObject _praticle;
    private SpriteRenderer _sprite;

    private int _maxArea = 5;
    private int _maxStage = 5;

    // 二宮追加
    PlayerInputManager.DIRECTION oldDire; // 前フレームの向きを入れておくための変数
    PlayerInputManager.DIRECTION Direction; // 現在の向き
    private float stopTime = 0f; // 停止時間
    private float idleStartTime = 1.0f; // アイドルモーション開始時間

    private GameObject se;
    private SEManager_Select seMana;
    private Animator anim; // アニメーターを取得するための変数

    //----------------------------------
    // 菅追加
    [SerializeField] private SelectArea _selectarea;        //エリア移動
    private Transform _playertrans;                         //プレイヤーのTransform
    [SerializeField] private EdgeCollider2D HorizonLimit;   //横制限 

    // Use this for initialization
    void Start()
    {
        //--------------------------------------
        // Rigidbody2Dのコンポーネント取得
        _rigid = GetComponent<Rigidbody2D>();
        if (_rigid == null) Debug.LogError("Rigidbody2Dのコンポーネントを取得できませんでした。");
        //--------------------------------------
        // SpriteRendererのコンポーネント取得
        _sprite = GetComponent<SpriteRenderer>();
        if (_sprite == null) Debug.LogError("SpriteRendererのコンポーネントを取得できませんでした。");
        //--------------------------------------
        // 初期化
        State = PlayerState_in_newSelectScene.WALK;

        anim = GetComponent<Animator>();

        Direction = PlayerInputManager.DIRECTION.RIGHT;
        oldDire = PlayerInputManager.DIRECTION.RIGHT;

        se = GameObject.Find("SE");
        // Seコンポーネント取得
        seMana = se.GetComponent<SEManager_Select>();

        _playertrans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(State == PlayerState_in_newSelectScene.WALK) 
        {
            float move = _inputValue.x * _moveSpeed * Time.deltaTime;
            //_rigid.velocity = new Vector2(move, _rigid.velocity.y);
            transform.Translate(move, 0, 0);
            // 二宮追加

            // なんらかのスティック入力があれば
            if (_inputValue.x != 0)
            {
                // 右入力があれば右を向かせる
                if (_inputValue.x > 0)
                {
                    if (Direction == PlayerInputManager.DIRECTION.LEFT)
                    {
                        Direction = PlayerInputManager.DIRECTION.RIGHT;
                    }
                }
                // 左入力があれば左を向かせる
                else if (_inputValue.x < 0)
                {
                    if (Direction == PlayerInputManager.DIRECTION.RIGHT)
                    {
                        Direction = PlayerInputManager.DIRECTION.LEFT;
                    }
                }

                // 足音再生開始関数呼び出し
                seMana.SetMoveStart();

                stopTime = 0f;
            }
            // 入力が無ければ
            else if (_inputValue.x == 0)
            {
                if(stopTime == 0.0f)
                {
                    // 足音停止関数呼び出し
                    seMana.SetMoveFinish();
                }

                stopTime += Time.deltaTime;
            }

            if (oldDire != Direction)
            {
                // プレイヤーの向きを今と逆にする
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            // 前フレームの状態保存
            oldDire = Direction;

            anim.SetBool("run", move != 0f); // 移動中
            anim.SetBool("frieze", move == 0f);
            if(stopTime > idleStartTime)
            {
                anim.SetBool("frieze", false);
            }
        }

        if(_playertrans.position.x > HorizonLimit.points[1].x)
        {
            _selectarea.NextArea();
        }
        if (_playertrans.position.x < HorizonLimit.points[0].x)
        {
            _selectarea.PrevArea();
        }

    }
    //-----------------------------------------------------------------
    //★★公開関数★★(公)
    public void MoveInput(InputAction.CallbackContext _context) 
    {
        _inputValue = _context.ReadValue<Vector2>();
    }

    public PlayerState_in_newSelectScene State 
    {
        get 
        {
            return _state;
        }
        set 
        {
            _state = value;

            // パーティクルの非表示
            //_praticle.SetActive(value == PlayerState_in_newSelectScene.CRACK);
            // プレイヤーの表示
            _sprite.enabled = (value == PlayerState_in_newSelectScene.WALK);
        }
    }

}