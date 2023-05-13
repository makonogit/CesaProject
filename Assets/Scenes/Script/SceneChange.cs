//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：シーンをロードする
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    [SerializeField,Header("フェードアウトの時間")]
    private float _fadeOutTime;
    private float _nowTime;
    [SerializeField, Header("パネルオブジェ")]
    private GameObject _panelObj;
    private CanvasRenderer _panel;
    private string _loadScene;
    private bool _start;

    // 二宮追加
    [SerializeField] private Fade _fade;
    public bool FirstFadeIn = false; // シーンが始まって一番最初にするフェードイン
    // このスクリプトを持つオブジェクトが存在するシーン
    private enum NowScene
    {
        Title,
        Select,
        Main,
    }

    [SerializeField]private NowScene _nowScene;

    private void Start()
    {
        if(_panelObj == null) Debug.LogError("パネルオブジェをセットしてください");
        _panel = _panelObj.GetComponent<CanvasRenderer>();
        if (_panel == null) Debug.LogError("CanvasRendererのコンポーネントを取得できませんでした。");
        //Debug.Log(_panel.name);
        _start = false;
        _nowTime = 0.0f;

        _panelObj.SetActive(true);
        if (_nowScene != NowScene.Main)
        {
            _panel.SetAlpha(0.0f);
            FirstFadeIn = true;
        }
    }
    private void Update()
    {
        // 二宮追加
        if (FirstFadeIn == false)
        {
            if (_fade.GetFadeState() == Fade.FadeState.FadeIn_Finish)
            {
                FirstFadeIn = true;
            }
            else 
            {
                _fade.FadeIn();
            }


        }

        if (_start) 
        {
            _nowTime += Time.deltaTime;
            _panel.SetAlpha(_nowTime/_fadeOutTime);
            
        }
        if (_nowTime > _fadeOutTime)
        {
            SceneManager.LoadScene(_loadScene);
        }
    }
    public void LoadScene(string _str) 
    {
        _loadScene = _str;
        _start = true;

        InMainScene.inMainScene = false;
        //Debug.Log(_str+"シーンをロードします。");
        //SceneManager.LoadScene(_str);
    }
}