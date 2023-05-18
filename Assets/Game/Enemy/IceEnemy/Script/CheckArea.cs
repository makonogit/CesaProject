//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：プレイヤーが範囲に入ったかどうか
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CheckArea : MonoBehaviour
{
    [SerializeField, Header("判定したいオブジェクト名")]
    private string _name;
    [SerializeField, Header("判定したいタグ名")]
    private string _tag;
    private bool _isEnter;

    private GameObject player;
    private New_PlayerJump _playerJump;
    private bool ride;
    private void Start()
    {
        if (_name == null && _tag == null) Debug.LogError("名前かタグの設定をしてください。");
        _isEnter = false;

        player = GameObject.Find("player");
        _playerJump = player.GetComponent<New_PlayerJump>();

        ride = (name == "PlayerOnArea");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(name + ":" + collision.name);
        if (_name != null&& collision.name == _name)
        {
            _isEnter = true;
            RideOn(true);
        }
        if (_tag != null&& collision.tag == _tag) 
        {
            _isEnter = true;
            RideOn(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(name + ":" + collision.name);
        if (_name != null && collision.name == _name)
        {
            _isEnter = false;
            RideOn(false);
        }
        if (_tag != null && collision.tag == _tag)
        {
            _isEnter = false;
            RideOn(false);
        }
    }
    private void RideOn(bool value) 
    {
        if (ride) _playerJump.RideOn = value;
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    Debug.Log(name+":"+collision.name);
    //    if (_name != null) _isEnter = (collision.name == _name);
    //    if (_tag != null) _isEnter = (collision.tag == _tag);
    //}
    public bool IsEnter 
    {
        get 
        {
            //Debug.Log(this.name + _isEnter);
            return _isEnter;
        }
    }
}