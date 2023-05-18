//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：ステージオブジェに当たった時の処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveScene : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―公開変数―(公)

    public enum StateID 
    {
        UNPLAYABLE, // プレイ不可能
        PLAYABLE,   // プレイ可能
        CLEAR,      // クリア済み
    }


    //-----------------------------------------------------------------
    //―秘匿変数―(私)
    [SerializeField,Header("プレイヤーObj")]
    private GameObject _player;
    private SelectMove _selectMove;// プレイヤーのセレクトムーブ
    private SelectedScene _selected;
    [SerializeField, Header("ステージシーン名")]
    private string _scene;// ステージのシーンを入れる

    [SerializeField, Header("エリア番号")]
    private int _Areanumber;    // エリアの番号

    [SerializeField, Header("ステージ番号")]
    private int _number;// ステージの番号

    private SetStage _Stagemanager;
    //private SetStage setStage;
    [SerializeField]
    private StateID _state;
    [SerializeField]
    private GameObject _crystal;
    [SerializeField,Header("クリア後画像")]
    private Sprite sprite;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _player = GameObject.Find("player");
        if(_player == null) Debug.LogError("not found");
        //_selectMove = _player.GetComponent<SelectMove>();
        //if (_selectMove == null) Debug.LogError("SelectMoveのコンポーネントを取得できませんでした。旧SelectScene用です。");
        _selected = _player.GetComponent<SelectedScene>();
        if (_selected == null) Debug.LogError("SelectedSceneのコンポーネントを取得できませんでした。newSeelctScene用です。");

        _Stagemanager = new SetStage();

        _state = StateID.UNPLAYABLE;

        
        if (_crystal == null) Debug.LogError("設定されていません。");

        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null) Debug.LogError("SpriteRendererのコンポーネントを取得できませんでした。");
        //setStage = _Stagemanager.GetComponent<SetStage>();
    }
    private void Update()
    {
        if (State == StateID.PLAYABLE)
        {
            _crystal.SetActive(false);
        }
        if(State == StateID.CLEAR) 
        {
            _renderer.sprite = sprite;
        }
    }

    //-----------------------------------------------------------------
    //★★公開関数★★(公)

    //―シーン取得関数―(公)
    public string GetScene
    {
        get
        {
            return _scene;
        }
    }
    //―当たった時の関数―(公)
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // もしプレイヤーに当たったなら
        if (collision.tag == "Player")
        {
            _selected.AreaPos = transform;  //　自身のTransformをセット
            _selected.SelectObject = transform.GetChild(0).gameObject;

            if(_selected != null) 
            {
                _Stagemanager.SetStageData(_Areanumber, _number);
                _selected.SelectScene(GetScene);
            }
            // 状態がエリア移動じゃなければ
            if (_selectMove != null && _selectMove.State != SelectMove.SelectPlayerState.AREA_CHANGE)
            {
                _selectMove.State = SelectMove.SelectPlayerState.STOP;
                _selectMove.SelectScene(GetScene);
                _selectMove.StageNumber = _number;
                //Debug.Log("hit");
            }
        }
    }
    //―出た時の関数―(公)
    public void OnTriggerExit2D(Collider2D collision)
    {
        // もしプレイヤーが出たら
        if (collision.tag == "Player")
        {
            _selected.SelectObject = null;

            if (_selected != null)
            {
                _selected.SelectScene(null);
            }
            // プレイヤーのSelectMoveを取得する。
            if (_selectMove != null) 
            {
                _selectMove.SelectScene(null);
                _selectMove.StageNumber = 0;
            }
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
            _state = value;
           
        }
    }

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    // ナシ
}