//---------------------------------------------------------
//
// 担当者：中川直登
//
// 内容　：トロッコに乗った処理
//
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ride_on_Trolley : MonoBehaviour
{

    [Space(10)]
    [SerializeField,Header("設定")]
    private CheckArea _onPlayer;
    //[SerializeField]
    private GameObject _player;

    [Space(10)]
    [SerializeField, Header("設定")]
    private bool _lockPlayerPosY = false;// プレイヤーのY座標をロックする。

    void Start()
    {
        //
        if (_onPlayer == null) Debug.LogError(name + ":_onPlayerが設定されていません。ride_onPlayer.cs");
        _player = GameObject.Find("player");
        if (_player == null) Debug.LogError(name + ":_playerが設定されていません。ride_onPlayer.cs");
    }

    
    void Update()
    {
        if (_onPlayer.IsEnter) 
        {
            Ride();
        }
    }

    private void Ride() 
    {
        Vector3 _pos = new Vector3(this.transform.position.x, _player.transform.position.y, 0);
        if (_lockPlayerPosY) _pos = new Vector3(this.transform.position.x, this.transform.position.y, 0);

        _player.transform.position = _pos;
    }
    private int LockY 
    {
        get 
        {
            if (!_lockPlayerPosY) return 0;
            return 1;
        }
    }
}