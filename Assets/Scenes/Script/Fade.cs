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
        if (FadeAlpha.black == false)
        {
            _panel.SetAlpha(0.0f);
        }
        else
        {
            _panel.SetAlpha(1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ��ʂ����邭�Ȃ��Ă���
        if (_fadeIn)
        {
            // _nowTime ��0�ɋ߂Â��ƃt�F�[�h�p�l����alpha�̊�����0�ɋ߂Â�
            _nowTime -= Time.unscaledDeltaTime;

            // �t�F�[�h�C���I��
            if(_nowTime <= 0f)
            {
                // ������
                _nowTime = 0f;
                _fadeIn = false;
                RunTime = false;

                _fadeState = FadeState.FadeIn_Finish;

                FadeAlpha.black = false;
            }
        }
        // ��ʂ��Â��Ȃ��Ă���
        else if (_fadeOut)
        {
            _nowTime += Time.unscaledDeltaTime;
        
            // �t�F�[�h�A�E�g�I��
            if(_nowTime >= _fadeTime)
            {
                // ������
                _nowTime = _fadeTime;
                RunTime = false;
                _fadeOut = false;

                _fadeState = FadeState.FadeOut_Finish;

                FadeAlpha.black = true;
            }
        }

        // �t�F�[�h���߂�����Ƃ�����alpha���Z�b�g
        if(_fadeIn || _fadeOut)
        {
            _panel.SetAlpha(_nowTime / _fadeTime);
        }
    }

    public void FadeIn()
    {
        if (RunTime == false)
        {
            Debug.Log("�t�F�[�h�C���J�n");

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