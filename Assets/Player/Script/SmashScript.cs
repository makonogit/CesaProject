//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：短いひびを作る
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmashScript : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―公開変数―(公)

    // ナシ

    //-----------------------------------------------------------------
    //―秘匿変数―(私)

    private PlayerInputManager _playerInputManager;
    private Transform _thisTrans;
    private HammerNail _hammerNail;

    private bool _push;

    private Vector2 _moveVector;


    [Header("ひびの長さ")]
    [SerializeField]
    private float _length = 1.0f;

    [Header("ひびを作るobj")]
    [SerializeField]
    private GameObject _crackCreaterObj;
    private CrackCreater _creater;

    [SerializeField]
    private List<Vector2> _pos;

    //-----------------------------------------------------------------
    //―スタート処理―
    void Start()
    {
        // 初期化
        _push = false;

        //--------------------------------------
        // Transformのコンポーネント取得
        _thisTrans = GetComponent<Transform>();
        // エラー文
        if(_thisTrans == null) Debug.LogError("Transformのコンポーネントを取得できませんでした。");

        //--------------------------------------
        // HammerNailのコンポーネント取得
        _hammerNail = GetComponent<HammerNail>();
        //エラー文
        if (_hammerNail == null) Debug.LogError("HammerNailのコンポーネントを取得できませんでした。");

        //--------------------------------------
        // PlayerInputManagerの取得

        // PlayerInputManagerを探す
        GameObject _inputManager = GameObject.Find("PlayerInputManager");
        // エラー文
        if (_inputManager == null) Debug.LogError("PlayerInputManagerを見つけることが出来ませんでした。");
        // コンポーネントの取得
        _playerInputManager = _inputManager.GetComponent<PlayerInputManager>();
        // エラー文
        if (_playerInputManager == null) Debug.LogError("PlayerInputManagerのコンポーネントを取得できませんでした。");

        //--------------------------------------
        //CrackCreaterを取得  
        _creater = _crackCreaterObj.GetComponent<CrackCreater>();
        // エラー文
        if (_creater == null) Debug.LogError("CrackCreaterのコンポーネントを取得できませんでした。");
    }

    //-----------------------------------------------------------------
    //―更新処理―
    void Update()
    {
        InputMove();
        // 押されている間
        if (_playerInputManager.GetHammer()) 
        {
            
        }
        // ハンマーが離された時
        if (isTrigger()) 
        {
            Smashed();
        }
        _push = _playerInputManager.GetHammer();
    }

    //-----------------------------------------------------------------
    //★★公開関数★★(公)

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    //―入力保存関数―(私)
    private void InputMove() 
    {
        if(_playerInputManager.GetMovement().magnitude > 0.25f) // 触った時
        {
            _moveVector = _playerInputManager.GetMovement();
        }
    }

    //―離された瞬間の関数―(私)
    private bool isTrigger()
    { 
        return !_playerInputManager.GetHammer() && _push == true;
    }

    //―スマッシュ関数―(私)
    private void Smashed() 
    {
        // Lスティックの方向を取得
        Vector2 _vec = _moveVector.normalized;
        
        // 方向と距離でレイであたり判定
        RaycastHit2D hit = Physics2D.Raycast(_thisTrans.position, _vec, _length,3);

        Vector2 Distance = _vec * _length;

        Debug.DrawRay(_thisTrans.position, Distance, Color.red, 5, false);

        bool _create = true;

        if (hit) 
        {
            _create = CheckTag(hit);
        }
        if(_create)
        { 
            _pos.Add(new Vector2(_thisTrans.position.x, _thisTrans.position.y));
            _pos.Add(new Vector2(_thisTrans.position.x  + Distance.x, _thisTrans.position.y + Distance.y));
            _creater.SetPointList(_pos);
            GameObject obj = Instantiate(_crackCreaterObj);
            for (;0 < _pos.Count;) { _pos.RemoveAt(0); }
        }

    }
    //―チェックtタグ関数―(私)
    private bool CheckTag(RaycastHit2D _hit) 
    {
        if (_hit.collider.tag == "Ground") return false;
        if (_hit.collider.tag == "Crack") return false;
        if (_hit.collider.tag == "UsedNail")
        {
            _hammerNail.CreateCrack = true;
            return false;
        }
        return true;
    }
}