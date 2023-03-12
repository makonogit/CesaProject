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

    // ナシ

    //-----------------------------------------------------------------
    //―秘匿変数―(私)

    private SelectMove _player;// プレイヤーのセレクトムーブ
    [SerializeField]
    private Object _scene;// ステージのシーンを入れる
    [SerializeField]
    private int _number;// ステージの番号

    //-----------------------------------------------------------------
    //★★公開関数★★(公)

    //―シーン取得関数―(公)
    public string GetScene
    {
        get
        {
            return _scene.name;
        }
    }
    //―当たった時の関数―(公)
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // もしプレイヤーに当たったなら
        if (collision.tag == "Player")
        {
            // プレイヤーのSelectMoveを取得する。
            _player = collision.gameObject.GetComponent<SelectMove>();
            if (_player == null) Debug.LogError("SelectMoveのコンポーネントを取得できませんでした。");
            // 状態がエリア移動じゃなければ
            if (_player.State != SelectMove.SelectPlayerState.AREA_CHANGE)
            {
                _player.State = SelectMove.SelectPlayerState.STOP;
                _player.SelectScene(GetScene);
                _player.StageNumber = _number;
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
            // プレイヤーのSelectMoveを取得する。
            _player = collision.gameObject.GetComponent<SelectMove>();
            if (_player == null) Debug.LogError("SelectMoveのコンポーネントを取得できませんでした。");
            _player.SelectScene(null);
            _player.StageNumber = 0;
        }
    }

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    // ナシ
}