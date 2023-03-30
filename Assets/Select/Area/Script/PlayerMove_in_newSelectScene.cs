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
    }

    // Update is called once per frame
    void Update()
    {
        if(State == PlayerState_in_newSelectScene.WALK) 
        {
            float move = _inputValue.x * _moveSpeed * Time.deltaTime;
            _rigid.velocity = new Vector2(move, _rigid.velocity.y);
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
            _praticle.SetActive(value == PlayerState_in_newSelectScene.CRACK);
            // プレイヤーの表示
            _sprite.enabled = (value == PlayerState_in_newSelectScene.WALK);
        }
    }

}