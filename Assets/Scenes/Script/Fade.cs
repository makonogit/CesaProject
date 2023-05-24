//---------------------------------------------------------
//担当者：二宮怜
//内容　：フェードイン・アウトさせる
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FadeAlpha
{
    public static bool black = false;
};

public static class InMainScene
{
    public static bool inMainScene = false;
}

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
        if (InMainScene.inMainScene == true)
        {
            if (FadeAlpha.black == false)
            {
                _panel.SetAlpha(0.0f);
            }
            else
            {
                _panel.SetAlpha(1.0f);
            }
        }

        //Debug.Log(InMainScene.inMainScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (InMainScene.inMainScene == false)
        {
            InMainScene.inMainScene = true;
        }

        // 画面が明るくなっていく
        if (_fadeIn)
        {

            // ロードに何秒もかかるとunscaledDeltaTimeがつじつまを合わせようと大きな数字になるので
            // 1を超えた値が出たときは無視
            if (Time.unscaledDeltaTime < 0.01f)
            {
                // _nowTime が0に近づくとフェードパネルのalphaの割合も0に近づく
                _nowTime -= Time.unscaledDeltaTime;
            }
            else
            {
                _nowTime -= 0.01f;
            }

            // フェードイン終了
            if(_nowTime <= 0f)
            {
                // 初期化
                _nowTime = 0f;
                _fadeIn = false;
                RunTime = false;

                _fadeState = FadeState.FadeIn_Finish;

                FadeAlpha.black = false;

                //Debug.Log("フェードイン終了");
            }
        }
        // 画面が暗くなっていく
        else if (_fadeOut)
        {
            // ロードに何秒もかかるとunscaledDeltaTimeがつじつまを合わせようと大きな数字になるので
            // 1を超えた値が出たときは無視
            if (Time.unscaledDeltaTime < 0.01f)
            {
                _nowTime += Time.unscaledDeltaTime;
            }else
            {
                _nowTime += 0.01f;
            }

            // フェードアウト終了
            if(_nowTime >= _fadeTime)
            {
                // 初期化
                _nowTime = _fadeTime;
                RunTime = false;
                _fadeOut = false;

                _fadeState = FadeState.FadeOut_Finish;

                FadeAlpha.black = true;

                //Debug.Log("フェードアウト終了");

            }
        }

        // フェード命令があるときだけalphaをセット
        if(_fadeIn || _fadeOut)
        {
            _panel.SetAlpha(_nowTime / _fadeTime);
        }

        //Debug.Log(_fadeState);
        //Debug.Log(_panel.GetAlpha());
        //Debug.Log("_nowTime" + _nowTime);
        //Debug.Log("_fadeTime" + _fadeTime);
    }

    public void FadeIn()
    {
        if (RunTime == false)
        {
            //Debug.Log("フェードイン開始");

            _fadeIn = true;
            RunTime = true;
            _nowTime = 1f;

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
            _nowTime = 0f;

            _fadeState = FadeState.FadeOut;
        }
    }

    public FadeState GetFadeState()
    {
        return _fadeState;
    }
}
