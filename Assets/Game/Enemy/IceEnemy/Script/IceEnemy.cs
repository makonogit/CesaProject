//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：SelectSceneのプレイヤーの移動処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IceEnemy : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―公開変数―(公)

    public enum StateID
    {
        NULL,
        //MOVE,     // 移動
        FELL,       // 落ちた後
        CLING,      // 天井にくっつく
        DROP,       // 落ちる

    }

    //-----------------------------------------------------------------
    //―秘匿変数―(私)

    private StateID _state;// 状態変数

    [SerializeField,Header("チェッカー")]
    private CheckArea _playerChecker;
    [SerializeField]
    private CheckArea _dropChecker;
    [SerializeField]
    private CheckArea _floorChecker;

    private Animator _anim;

    private Vector2 _pos;

    private Rigidbody2D _rb;

    // Use this for initialization
    void Start()
    {
        //--------------------------------------
        // 設定されているかの確認
        if (_playerChecker == null) Debug.LogError("PlayerChecker設定されていません。");
        if (_dropChecker == null) Debug.LogError("DropChecker設定されていません。");
        if (_floorChecker == null) Debug.LogError("FloorChecker設定されていません。");

        //--------------------------------------
        // Animatorのコンポーネントを取得
        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.LogError("Animatorのコンポーネントを取得できませんでした。");

        //--------------------------------------
        // レイを飛ばして天井を探す
        // 上方向
        Vector2 direction = new Vector2(0, 1);

        // 距離
        float distance = 10000.0f;

        // レイヤーマスク( 0:Default だけ反応させる)
        int LayerMask = 1<<10;

        RaycastHit2D _hit = Physics2D.Raycast(transform.position, direction,distance, LayerMask);
        // 当たった場所に設定する
        if (_hit&&_hit.transform.tag == "Ground") 
        {
            //Debug.Log(_hit.transform.name);
            // めり込み防止用
            Vector2 _offset = new Vector2(0, -1.25f * 0.25f);
            _pos = _hit.point + _offset;
            // 位置を設定
            transform.position = _pos;

            _state = StateID.CLING;
        }
        //--------------------------------------
        // 重力無効にする
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.LogError("Rigidbody2Dのコンポーネントを取得できませんでした。");
        //_rb.velocity = new Vector2(0, 0);
        _rb.simulated = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        //Debug.Log("落ちる"+_dropChecker.IsEnter);
        //Debug.Log("床"+_floorChecker.IsEnter);
        //Debug.Log("プレイヤー"+_playerChecker.IsEnter);
        switch (State) 
        {
            case StateID.FELL:
                Fell();
                break;
            case StateID.CLING:
                Cling();
                break;
            case StateID.DROP:
                Drop();
                break;
        }
    }
    //-----------------------------------------------------------------
    //★★公開関数★★(公)
    public StateID State 
    {
        get 
        {
            return _state;
        }
    }
    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)
    private void Fell()
    {
        
    }
    private void Cling() 
    {
        if(_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "IE_SuprizredAnim") 
        {
            _anim.SetBool("isOldFlag", true);
        }
        
        _anim.SetBool("isSurprised", _playerChecker.IsEnter);// 気づいたアニメーションを出す。
        //_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name
        // もしdropAreaに入ったら
        // 重力を無効にする
        if (_dropChecker.IsEnter) 
        {
            _state = StateID.DROP;
            _anim.SetBool("isDrop", true);
        }
    }
    private void Drop()
    {
        // 重力を有効にする
        if(_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "IE_FallingAnim") 
        {
            _rb.simulated = true;
        }
        
        // 着地したら
        if (_floorChecker.IsEnter) 
        {
            _state = StateID.FELL;
            _anim.SetBool("isFell", true);
        }
    }
}