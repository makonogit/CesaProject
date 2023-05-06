//---------------------------------------------------------
//担当者：二宮怜
//内容　：フェードイン・アウトさせる
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    // 変数宣言
    public enum FadeState
    {
        None,
        FadeIn,
        FadeIn_Finish,
        FadeOut,
        FadeOut_Finish
    }

    // フェードの状態を表す
    private FadeState _fadeState = FadeState.None;

    [SerializeField, Header("フェードアウトの時間")]
    private float _fadeTime;
    private float _nowTime;
    [SerializeField, Header("パネルオブジェ")]
    private GameObject _panelObj;
    private CanvasRenderer _panel;
    public bool _fadeOut = false;  //　フェードアウト
    public bool _fadeIn = false;   //　フェードイン

    private bool RunTime = false; // 実行中か

    // Start is called before the first frame update
    void Start()
    {
        if (_panelObj == null) Debug.LogError("パネルオブジェをセットしてください");
        _panel = _panelObj.GetComponent<CanvasRenderer>();
        if (_panel == null) Debug.LogError("CanvasRendererのコンポーネントを取得できませんでした。");
        _nowTime = 0.0f;

        _fadeOut = false;
        _fadeIn = false;

        _panelObj.SetActive(true);
        _panel.SetAlpha(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // 画面が明るくなっていく
        if (_fadeIn)
        {
            // _nowTime が0に近づくとフェードパネルのalphaの割合も0に近づく
            _nowTime -= Time.deltaTime;

            // フェードイン終了
            if(_nowTime <= 0f)
            {
                // 初期化
                _nowTime = 0f;
                _fadeIn = false;
                RunTime = false;

                _fadeState = FadeState.FadeIn_Finish;
            }
        }
        // 画面が暗くなっていく
        else if (_fadeOut)
        {
            _nowTime += Time.deltaTime;
        
            // フェードアウト終了
            if(_nowTime >= _fadeTime)
            {
                // 初期化
                _nowTime = _fadeTime;
                RunTime = false;
                _fadeOut = false;

                _fadeState = FadeState.FadeOut_Finish;
            }
        }

        // フェード命令があるときだけalphaをセット
        if(_fadeIn || _fadeOut)
        {
            _panel.SetAlpha(_nowTime / _fadeTime);
        }
    }

    public void FadeIn()
    {
        if (RunTime == false)
        {
            Debug.Log("フェードイン開始");

            _fadeIn = true;
            RunTime = true;

            _fadeState = FadeState.FadeIn;
        }
    }

    public void FadeOut()
    {
        if (RunTime == false)
        {
            Debug.Log("フェードアウト開始");

            _fadeOut = true;
            RunTime = true;
            
            _fadeState = FadeState.FadeOut;
        }
    }

    public FadeState GetFadeState()
    {
        return _fadeState;
    }
}
