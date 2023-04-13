//---------------------------------------------------------
//
//担当者：中川直登
//
//内容　：溜め技のエフェクトアニメーション―Script version
//
//コメント：これでだめならアニメーションで作ってください。
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class haloEffect : MonoBehaviour
{
    //-----------------------------------------------------------------
    //―公開変数―(公)
    public enum StateID // 列挙型状態ID
    {
        SATND_BY,   // 待機
        RESTART,    // 再スタート
        PLAYING,    // 再生中
        STOP,       // 停止
    }

    //-----------------------------------------------------------------
    //―秘匿変数―(私)

    [Space(10)]
    [Header("時間設定")]
    [Space(5)]
    [SerializeField, Tooltip("アニメーション時間の設定")]
    private float _animTime = 1.0f;
    [SerializeField, Tooltip("開始するまでの時間")]
    private float _startTime = 0.0f;
    [SerializeField, Tooltip("アニメーション速度の設定")]
    private float _speed = 1;
    [SerializeField, Tooltip("何秒おきに1処理をするのかの設定")]
    private float _oneTime = 0.001f;

    private float _nowTime;

    [Space(10)]
    [Header("動き設定")]
    [Space(5)]
    //[SerializeField, Tooltip("開始時のサイズ")]
    //private Vector2 _startSize;
    //[SerializeField, Tooltip("終了時サイズ")]
    //private Vector2 _endSize;
    [SerializeField, Tooltip("アニメーションの動きを設定する")]
    private AnimationCurve _motion;
    [GradientUsage(true),SerializeField, Tooltip("グラデーション")]
    private Gradient _gradient;

    private SpriteRenderer _spriteRenderer;
    private Material _material;
    [SerializeField]
    private StateID _state;

    private float now;
    private bool _start;
    private bool _stop;
    private bool _end;

#if UNITY_EDITOR    // エディター時
    private float _playTime;
#endif

    //-----------------------------------------------------------------
    //★★公開関数★★(公)

    //
    // 関数：State 
    //
    // 目的：クラスの外から参照するため
    // 
    public StateID State 
    {
        get 
        {
            return _state;
        }
    }

    //
    // 関数：Play()
    //
    // 目的：アニメーションを開始する為の関数
    // 
    public void Play()
    {
        // 初期化
        Init();
        // 開始フラグ
        _start = true;

#if UNITY_EDITOR    // エディター時
        // 呼び出し続けてる時間
        _playTime += Time.deltaTime;
        if (_playTime >= 1) Debug.LogWarning("Play()関数を呼びすぎではないでしょうか？");
#endif
    }

    //
    // 関数：Replay() 
    //
    // 目的：アニメーションを再開する
    // 
    public void Replay() 
    {
        // 停止しているなら
        if(_state == StateID.STOP) 
        {
            _stop = false;
            _end = false;
            StartCoroutine(HaloAnimation());
            // 状態変更：再生中
            _state = StateID.PLAYING;
        }
    }

    //
    // 関数：Stop()
    //
    // 目的：アニメーションを停止する
    // 
    public void Stop()
    {
        _stop = true;
        // 状態変更：停止
        _state = StateID.STOP;
    }
    //
    // 関数：End() 
    //
    // 目的：アニメーションを強制的に終わらせる
    // 
    public void End() 
    {
        _end = true;
        _material.SetFloat("_Radius", 0.0f);
        // 状態変更：待機
        _state = StateID.SATND_BY;
    }

    //-----------------------------------------------------------------
    //☆☆秘匿関数☆☆(私)

    //
    // 関数：Start()
    //
    private void Start()
    {
        //--------------------------
        // コンポーネントを取得

        // SpriteRenderer
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null) Debug.LogError("SpriteRendererコンポーネントの取得が出来ませんでした。");
        // Renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) Debug.LogError("Rendererコンポーネントを取得できませんでした。");
        // Material
        _material = renderer.material;
        if (_material == null) Debug.LogError("Materialコンポーネントを取得できませんでした。");
        //--------------------------
        // 初期化
        Init();

        // テスト用
        //Play();
    }

    //
    // 関数：Update()
    //
    private void Update()
    {
        if (StartAnimation)
        {
            // 状態変更：再生中
            _state = StateID.PLAYING;
            // アニメーション開始
            StartCoroutine(HaloAnimation());
            _start = false;
            _nowTime = 0.0f;
        }
        if (_start) 
        {
            _nowTime += Time.deltaTime;
        }

        // テスト用
        //if (_state == StateID.SATND_BY)
        //{
        //    Play();
        //}
    }

    //
    // 関数：StartAnimation
    //
    // 目的：アニメーション開始条件の結果を返す
    // 
    private bool StartAnimation
    {
        get 
        {
            // 再生中でなければ
            bool _isntPlaying = _state != StateID.PLAYING;
            
            // 開始時間を過ぎたか
            bool _isStartTime = _nowTime >= _startTime;
            
            // 開始フラグと時間が経過したか。
            bool _result = _start && _isStartTime && _isntPlaying;

            return _result;
        }
    }

    //
    // 関数：Init()
    //
    // 目的：初期化
    // 
    private void Init()
    {
        _start = false;// 開始フラグ
        _stop = false;// 停止フラグ
        _end = false;// 終了フラグ
        now = 0.0f;// 再生経過時間のリセット
        // 再生中でなければ
        if (_state != StateID.PLAYING && _state != StateID.RESTART)
        {
            _nowTime = 0.0f;// 経過時間
            _state = StateID.SATND_BY;// 状態変更：待機
#if UNITY_EDITOR    // エディター時
            _playTime = 0.0f;// Play()が呼ばれた時間
#endif
        }
        else _state = StateID.RESTART;// 状態変更：再スタート
        
    }

    //
    // 関数：HaloAnimation() 
    //
    // 目的：アニメーションをコルーチンで実行
    // 
    private IEnumerator HaloAnimation() 
    {
        while (true) 
        {
            // 割合
            float ratio = _motion.Evaluate(now/_animTime);
            //transform.localScale = _startSize * (-1 * ratio) + _endSize * ratio;
            _material.SetFloat("_Radius", ratio);
            // 色変更
            _spriteRenderer.color = _gradient.Evaluate(now / _animTime);

            // 時間計算
            now += _oneTime * _speed;
            // 待つ
            yield return new WaitForSeconds(_oneTime);

            // 時間が経過したら
            if (_isOverAnimationTime) _state = StateID.SATND_BY;

            if (_isBreakAnim)
            {
                yield break;
            }
        }
    }

    //
    // 関数：_isBreakAnim 
    //
    // 目的：アニメーションを抜ける条件
    // 
    private bool _isBreakAnim 
    {
        get 
        {
            // 時間が経過したか
            if (_isOverAnimationTime) return true;
            // 停止要請があるか
            if (_stop) return true;
            // 終了要請があるか
            if (_end) return true;

            return false;
        }
    }

    //
    // 関数： _isOverAnimationTime
    //
    // 目的：時間が経過したか
    // 
    private bool _isOverAnimationTime
    {
        get 
        {
            return (now > _animTime); 
        } 
    }

    //
    // 関数：
    //
    // 目的：
    // 
    // コメント：
    //
}