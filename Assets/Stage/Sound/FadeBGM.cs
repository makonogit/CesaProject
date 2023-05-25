//---------------------------------------
//�S���F��{��
//���e�F���ݍĐ����Ă���BGM�̃t�F�[�h�����i���܂�G��Ȃ��łق����j
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGMstatus
{
    // �t�F�[�h�������s�p�̃t���O
    public bool FadeIn = false;
    public bool FadeOut = false;

    // �ő剹��
    public float MaxVolume = 0f;

    public BGMstatus(float volume)
    {
        MaxVolume = volume;
    }
}          

public class FadeBGM : MonoBehaviour
{
    // �ϐ��錾

    // MainScene��BGM�pAudioSource
    [SerializeField] private AudioSource StageBGM;
    [SerializeField] private AudioSource BossBGM_Intro;
    [SerializeField] private AudioSource BossBGM_Loop;
    [SerializeField] private AudioSource SpecialBGM; // ����BGM

    // �N���ABGM�pAudioClip
    [SerializeField] private AudioClip AC_Clear;
    [SerializeField] private bool _clearBGMflg = false; // �N���ABGM�Đ��J�n����true

    // BGM�t�F�[�h�p�N���X
    public BGMstatus Stage; // �X�e�[�W 
    public BGMstatus Boss;  // �{�X
    public BGMstatus Special; // �Q�[���I�[�o�[�A�N���A

    [SerializeField,Header("�t�F�[�h�̑��x")] private float _speed = 1.0f;

    private void Start()
    {
        // �eBGM�̏������ʂ�0
        //StageBGM_Intro.volume = 0f;
        StageBGM.volume = 0f;
        BossBGM_Intro.volume = 0f;
        BossBGM_Loop.volume = 0f;
        SpecialBGM.volume = 0f;

        Stage.FadeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        //------------------------------------
        // �X�e�[�W�p
        if (Stage.FadeOut == true)
        {
            StageBGMFadeOut();
        }
        if(Stage.FadeIn == true)
        {
            StageBGMFadeIn();
        }

        // �t�F�[�h�C���ƃt�F�[�h�A�E�g�����Ԃ�����ǂ�����I��
        if(Stage.FadeIn == true && Stage.FadeOut == true)
        {
            Stage.FadeIn = false;
            Stage.FadeOut = false;
        }

        //-------------------------------------
        // �{�X�p
        if (Boss.FadeOut == true)
        {
            BossBGMFadeOut();
        }
        if (Boss.FadeIn == true)
        {
            BossBGMFadeIn();
        }

        if (Boss.FadeIn == true && Boss.FadeOut == true)
        {
            Boss.FadeIn = false;
            Boss.FadeOut = false;
        }

        //------------------------------------------
        // ����BGM
        if(Special.FadeOut == true)
        {
            SpecialBGMFadeOut();
        }
        if(Special.FadeIn == true)
        {
            SpecialBGMFadeIn();
        }

        if(Special.FadeIn == true && Special.FadeOut == true)
        {
            Special.FadeIn = false;
            Special.FadeOut = false;
        }

        //------------------------------------------
        // �N���A���Ăяo��
        if(_clearBGMflg == true)
        {
            ChangeClearBGM();
        }
    }

    // �X�e�[�WBGM�̃t�F�[�h�A�E�g
    private void StageBGMFadeOut()
    {
        // BGM�̉��ʂ�0�ɋ߂Â��Ă���
        if(StageBGM.volume > 0f)
        {
            StageBGM.volume -= Time.unscaledTime * _speed;
        }
        else
        {
            StageBGM.volume  = 0f;

            Stage.FadeOut = false;
        }
    }

    // �X�e�[�WBGM�̃t�F�[�h�C��
    private void StageBGMFadeIn()
    {
        // BGM�̉��ʂ��ő剹�ʂɋ߂Â��Ă���
        if (StageBGM.volume < Stage.MaxVolume)
        {
            StageBGM.volume += Time.unscaledTime * _speed;
        }
        else
        {
            StageBGM.volume = Stage.MaxVolume;

            Stage.FadeIn = false;
        }
    }

    // �{�XBGM�̃t�F�[�h�C��
    private void BossBGMFadeIn()
    {
        // BGM�̉��ʂ��ő剹�ʂɋ߂Â��Ă���
        if (BossBGM_Loop.volume < Boss.MaxVolume)
        {
            BossBGM_Intro.volume += Time.unscaledTime * _speed;
            BossBGM_Loop.volume += Time.unscaledTime * _speed;
        }
        else
        {
            BossBGM_Intro.volume = Boss.MaxVolume;
            BossBGM_Loop.volume = Boss.MaxVolume;

            Boss.FadeIn = false;
        }
    }

    // �{�XBGM�̃t�F�[�h�A�E�g
    private void BossBGMFadeOut()
    {
        // BGM�̉��ʂ�0�ɋ߂Â��Ă���
        if (BossBGM_Loop.volume > 0)
        {
            BossBGM_Intro.volume -= Time.unscaledTime * _speed;
            BossBGM_Loop.volume -= Time.unscaledTime * _speed;
        }
        else
        {
            BossBGM_Intro.volume = 0;
            BossBGM_Loop.volume = 0;

            Boss.FadeOut = false;
        }
    }

    // ����BGM�̃t�F�[�h�C��
    private void SpecialBGMFadeIn()
    {
        // BGM�̉��ʂ��ő剹�ʂɋ߂Â��Ă���
        if(SpecialBGM.volume < Special.MaxVolume)
        {
            SpecialBGM.volume += Time.unscaledTime * _speed;
        }
        else
        {
            SpecialBGM.volume = Special.MaxVolume;

            Special.FadeIn = false;
        }
    }

    // ����BGM�̃t�F�[�h�A�E�g
    private void SpecialBGMFadeOut()
    {
        // BGM�̉��ʂ�0�ɋ߂Â��Ă���
        if (SpecialBGM.volume > 0)
        {
            SpecialBGM.volume -= Time.unscaledTime * _speed;
        }
        else
        {
            SpecialBGM.volume = 0;

            Special.FadeOut = false;
        }
    }

    //  �Z�b�^�[
    public void StageClear()
    {
        _clearBGMflg = true;
    }

    // StageBGM���X�e�[�W�N���ABGM�ɐ؂�ւ�
    private void ChangeClearBGM()
    {
        // �N���ABGM�N���b�v���Z�b�g
        SpecialBGM.clip = AC_Clear;
        // BGM�J�n�ʒu�����߂ɂ���
        SpecialBGM.Play();
        // BGM�t�F�[�h�C��������
        Special.FadeIn = true;
        // �N���A�t���O���낷
        _clearBGMflg = false;
    }

    // BGM�̐i�s������Z�b�g����
    public void ResetBGM()
    {
        // �X�e�[�WBGM���ŏ�����Đ�
        StageBGM.Play();

        // �{�XBGM�̍Đ����Ԃ�������
        BossBGM_Intro.time = 0f;
    }
}