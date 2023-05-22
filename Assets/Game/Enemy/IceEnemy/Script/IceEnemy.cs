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
        DEATH,
    }

    //-----------------------------------------------------------------
    //―秘匿変数―(私)
    [SerializeField]
    private StateID _state;// 状態変数

    [SerializeField,Header("チェッカー")]
    private CheckArea _playerChecker;
    [SerializeField]
    private CheckArea _dropChecker;
    [SerializeField]
    private CheckArea _floorChecker;
    private Animator _anim;

    [SerializeField,Header("歩く速さ")]
    private float _moveSpeed;
    private Vector2 _pos;

    private Rigidbody2D _rb;
    private bool _direction;
    private Transform _trans;

    [SerializeField]
    private EdgeCollider2D _edge;
    private int _max;
    [SerializeField]
    private int _num;

    private Vector3 StartPos;

    [SerializeField]
    private GameObject Particle;

    // Use this for initialization
    void Start()
    {
        StartPos = transform.position;
        //--------------------------------------
        // 設定されているかの確認
        if (_playerChecker == null) Debug.LogError("PlayerChecker設定されていません。");
        if (_dropChecker == null) Debug.LogError("DropChecker設定されていません。");
        if (_floorChecker == null) Debug.LogError("FloorChecker設定されていません。");
        if(_edge ==null) Debug.LogError("Edge設定されていません。");
        
        //--------------------------------------
        // Animatorのコンポーネントを取得
        _anim = GetComponent<Animator>();
        if (_anim == null) Debug.LogError("Animatorのコンポーネントを取得できませんでした。");

       
        //--------------------------------------
        // 重力無効にする
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) Debug.LogError("Rigidbody2Dのコンポーネントを取得できませんでした。");
        //_rb.velocity = new Vector2(0, 0);
        _trans = GetComponent<Transform>();
        if(_trans ==null) Debug.LogError("Transformのコンポーネントを取得できませんでした。");
        _max = _edge.pointCount;

        Init();
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
            case StateID.DEATH:
                Death();
                break;
        }
    }

    //-----------------------------------------------------------------
    //★★公開関数★★(公)
    public void Init()
    {
        transform.position = StartPos;
        _direction = false;
        _rb.simulated = false;
        //--------------------------------------
        // レイを飛ばして天井を探す
        // 上方向
        Vector2 direction = new Vector2(0, 1);

        // 距離
        float distance = 10000.0f;

        // レイヤーマスク( 0:Default だけ反応させる)
        int LayerMask = 1 << 10;

        RaycastHit2D _hit = Physics2D.Raycast(transform.position, direction, distance, LayerMask);
        // 当たった場所に設定する
        if (_hit&&_hit.transform.tag=="Ground")
        {
            //Debug.Log(_hit.transform.name);
            // めり込み防止用
            Vector2 _offset = new Vector2(0, -1.25f * 0.25f);
            _pos = _hit.point + _offset;
            // 位置を設定
            transform.position = _pos;

            _state = StateID.CLING;
        }
    }
    public StateID State 
    {
        get 
        {
            return _state;
        }
        set 
        {
            if (value == StateID.DEATH)
            { _state = value; }
        }
    }
    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    private bool Direction
    {
        get   
        {
            Vector3 Scale = _trans.localScale;
            return _direction == (Scale.x < 0);
        }
    }

    private bool SetDirection 
    {
        get 
        {
            return (_edge.points[_num].x + _edge.offset.x) - _trans.position.x > 0;
        }
    }
    private bool Nexst
    {
        get 
        {
            float distans = (_edge.points[_num].x + _edge.offset.x) - _trans.position.x;
            if (Mathf.Abs(distans) < 0.1f) return true;
            return false;
        }
    }

    private void Fell()
    {
        if (Nexst) 
        {
            _num++;
            _num = _num % _max;
            _direction = SetDirection;
        }
        
        float _move = _moveSpeed * Time.deltaTime;
        Vector3 Scale = _trans.localScale;
        if (!_direction) _move *= -1f;
        if (!Direction) Scale = new Vector3(Scale.x*-1, Scale.y, Scale.z);
        Vector3 moveTrans = new Vector3(_move, 0, 0);
        _trans.localScale = Scale;
        _trans.Translate(moveTrans);

    }

    private void Cling() 
    {
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
            _num = Random.Range(0, _edge.pointCount - 1);
            _direction = SetDirection;
        }
    }

    private void Death() 
    {
        //if(_rb.simulated == false) 
        //{
        //    _rb.simulated = true;
        //}

        //if (transform.childCount > 3)
        //{
        //    Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        //}

        Instantiate(Particle, transform.position,Quaternion.identity);   //　割れる演出をする
        Destroy(gameObject);    //自分を消す
    }

    private void Flag() 
    {
        _anim.SetBool("isOldFlag", true);
    }

}