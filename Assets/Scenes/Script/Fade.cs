//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�t�F�[�h�C���E�A�E�g������
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
    // �ϐ��錾
    public enum FadeState
    {
        None,
        FadeIn,
        FadeIn_Finish,
        FadeOut,
        FadeOut_Finish
    }

    // �t�F�[�h�̏�Ԃ�\��
    private FadeState _fadeState = FadeState.None;

    [SerializeField, Header("�t�F�[�h�A�E�g�̎���")]
    private float _fadeTime;
    private float _nowTime;
    [SerializeField, Header("�p�l���I�u�W�F")]
    private GameObject _panelObj;
    private CanvasRenderer _panel;
    public bool _fadeOut = false;  //�@�t�F�[�h�A�E�g
    public bool _fadeIn = false;   //�@�t�F�[�h�C��

    private bool RunTime = false; // ���s����

    // Start is called before the first frame update
    void Start()
    {
        if (_panelObj == null) Debug.LogError("�p�l���I�u�W�F���Z�b�g���Ă�������");
        _panel = _panelObj.GetComponent<CanvasRenderer>();
        if (_panel == null) Debug.LogError("CanvasRenderer�̃R���|�[�l���g���擾�ł��܂���ł����B");
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

        // ��ʂ����邭�Ȃ��Ă���
        if (_fadeIn)
        {

            // ���[�h�ɉ��b���������unscaledDeltaTime�����܂����킹�悤�Ƒ傫�Ȑ����ɂȂ�̂�
            // 1�𒴂����l���o���Ƃ��͖���
            if (Time.unscaledDeltaTime < 0.01f)
            {
                // _nowTime ��0�ɋ߂Â��ƃt�F�[�h�p�l����alpha�̊�����0�ɋ߂Â�
                _nowTime -= Time.unscaledDeltaTime;
            }
            else
            {
                _nowTime -= 0.01f;
            }

            // �t�F�[�h�C���I��
            if(_nowTime <= 0f)
            {
                // ������
                _nowTime = 0f;
                _fadeIn = false;
                RunTime = false;

                _fadeState = FadeState.FadeIn_Finish;

                FadeAlpha.black = false;

                //Debug.Log("�t�F�[�h�C���I��");
            }
        }
        // ��ʂ��Â��Ȃ��Ă���
        else if (_fadeOut)
        {
            // ���[�h�ɉ��b���������unscaledDeltaTime�����܂����킹�悤�Ƒ傫�Ȑ����ɂȂ�̂�
            // 1�𒴂����l���o���Ƃ��͖���
            if (Time.unscaledDeltaTime < 0.01f)
            {
                _nowTime += Time.unscaledDeltaTime;
            }else
            {
                _nowTime += 0.01f;
            }

            // �t�F�[�h�A�E�g�I��
            if(_nowTime >= _fadeTime)
            {
                // ������
                _nowTime = _fadeTime;
                RunTime = false;
                _fadeOut = false;

                _fadeState = FadeState.FadeOut_Finish;

                FadeAlpha.black = true;

                //Debug.Log("�t�F�[�h�A�E�g�I��");

            }
        }

        // �t�F�[�h���߂�����Ƃ�����alpha���Z�b�g
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
            //Debug.Log("�t�F�[�h�C���J�n");

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
            Debug.Log("�t�F�[�h�A�E�g�J�n");

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
