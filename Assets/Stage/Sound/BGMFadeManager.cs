//--------------------------------
//�S���F��{��
//���e�FBGM�̃t�F�[�h���Ǘ�����(�e�t���O���������Ƃ���FadeBGM�ɃA�N�Z�X���t�F�[�h���J�n������)
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMFadeManager : MonoBehaviour
{
    // �ϐ��錾

    // �t���O����
    private bool _smallBossflg; // �{�X�����j���ɗ��Ă�
    private bool _bigBossBGM; // �{�X��J�n���ɗ��Ă�
    private bool _smallStageBGMflg; // �Z���N�g�ɖ߂�Ƃ��A���S���A�N���A��
    private bool _bigStageBGMflg; // �X�e�[�W�ɓ��������A���X�|�[�����A
    private bool _bigSpecialBGMflg; // ���S���A�N���A��
    private bool _smallSpecialBGMflg; // ���S���o�I����Ăяo��

    [SerializeField] private FadeBGM _fadeBGM; // �t�F�[�h�����X�N���v�g

    // Update is called once per frame
    void Update()
    {
        // �{��BGM������
        if(_smallBossflg == true)
        {
            _fadeBGM.Boss.FadeOut = true;

            _smallBossflg = false;
        }

        // �{�XBGM��������
        if(_bigBossBGM == true)
        {
            _fadeBGM.Boss.FadeIn = true;

            _bigBossBGM = false;
        }

        // �X�e�[�WBGM������
        if(_smallStageBGMflg == true)
        {
            _fadeBGM.Stage.FadeOut = true;

            _smallStageBGMflg = false;
        }

        // �X�e�[�WBGM��������
        if(_bigStageBGMflg == true)
        {
            _fadeBGM.Stage.FadeIn = true;

            _bigStageBGMflg = false;
        }

        // ����BGM��������
        if(_bigSpecialBGMflg == true)
        {
            _fadeBGM.Special.FadeIn = true;

            _bigSpecialBGMflg = false;
        }

        // ����BGM������
        if(_smallSpecialBGMflg == true)
        {
            _fadeBGM.Special.FadeOut = true;

            _smallSpecialBGMflg = false;
        }
    }

    // �{�X���j���ɌĂяo��
    public void SmallBossBGM()
    {
        _smallBossflg = true;
    }

    // �{�X��J�n���ɌĂяo��
    public void BigBossBGM()
    {
        _bigBossBGM = true;
    }

    // �Z���N�g�ɖ߂�Ƃ��A���S���A�N���A���Ăяo��
    public void SmallStageBGM()
    {
        _smallStageBGMflg = true;
    }

    //�X�e�[�W�ɓ��������A���X�|�[�����Ăяo��
    public void BigStageBGM()
    {
        _bigStageBGMflg = true;
    }

    // �X�e�[�W�N���ABGM�Ȃ炵�������Ăяo��
    public void StageClear()
    {
        _fadeBGM.StageClear();
    }

    // ���S���ɌĂяo��
    public void BigSpecialBGM()
    {
        _bigSpecialBGMflg = true;
    }

    public void smallSpecialBGM()
    {
        _smallSpecialBGMflg = true;
    }

    // BGM�̐i�s������Z�b�g����
    public void ResetBGM()
    {
        _fadeBGM.ResetBGM();
    }
}
