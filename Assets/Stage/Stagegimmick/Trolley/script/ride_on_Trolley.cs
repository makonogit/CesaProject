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
using UnityEngine.Rendering.Universal;

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

    //　菅追加：Light
    [SerializeField, Header("Light")]
    private Light2D _light;

    private GameObject SEobj;           //SE再生用オブジェクト
    private GimmickPlaySound PlaySE;    //SE再生用スクリプト
    private bool _play = false;

    void Start()
    {
        //
        if (_onPlayer == null) Debug.LogError(name + ":_onPlayerが設定されていません。ride_onPlayer.cs");
        _player = GameObject.Find("player");
        if (_player == null) Debug.LogError(name + ":_playerが設定されていません。ride_onPlayer.cs");

        //　SE再生用
        SEobj = GameObject.Find("GimmickSE");
        PlaySE = SEobj.GetComponent<GimmickPlaySound>();
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

        _light.intensity = 1.0f;    //　ライトをつける　菅追加
        if (!_play)
        {
            PlaySE.PlayerGimmickSE(GimmickPlaySound.GimmickSEList.TOLOLLEYLIGHT);
            _play = true;
        }

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