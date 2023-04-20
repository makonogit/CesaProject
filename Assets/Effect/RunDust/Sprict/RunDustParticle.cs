//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：SelectSceneのプレイヤーの移動処理
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RunDustParticle : MonoBehaviour
{
    private bool _isMove;
    private bool _isJump;
    [SerializeField]
    private ParticleSystem _RunDust;

    private bool _playing;
    // Use this for initialization
    void Start()
    {
        if (_RunDust == null) Debug.LogError("_RunDust設定されていません。");
        _playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_playing && isPlay) 
        {
            _RunDust.Play();
            _playing = true;
        }
        if(_playing && !isPlay) 
        {
            _RunDust.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _playing = false;
        }
    }

    public bool IsMove 
    {
        set {
            _isMove = value;
        }
    }
    public bool IsJump 
    {
        set 
        {
            _isJump = value;
        }
    }

    private bool isPlay 
    {
        get 
        {
            if (!_isMove) return false;
            if (_isJump) return false;
            return true;
        }
    }
}