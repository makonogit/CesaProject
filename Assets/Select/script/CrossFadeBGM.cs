//--------------------------------
//�S���F��{��
//���e�F�Z���N�gBGM�N���X�t�F�[�h������
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeBGM : MonoBehaviour
{
    // �ϐ��錾

    [SerializeField] private AudioSource BGM_1;
    [SerializeField] private AudioSource BGM_2;

    // �ړ���̃G���A���擾���邽��
    [SerializeField] private SelectArea _selectArea;

    public enum Sound
    {
        bgm_1,
        bgm_2,
    }

    // ���݂Ȃ��Ă���BGM
    Sound _sound = Sound.bgm_1;

    // true:�N���X�t�F�[�h�J�n
    private bool CrossFade = false;

    [SerializeField] private List<AudioClip> AudioClipList = new List<AudioClip>();

    [SerializeField] private float FadeSpeed = 0.002f;
    private float ElapsedTime = 0f; // �n�܂��Ă���̌o�ߎ���
    [SerializeField] private float MaxVolume = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        BGM_1.volume = MaxVolume;
        BGM_2.volume = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // �N���X�t�F�[�h
        if(CrossFade == true)
        {
            if (_sound == Sound.bgm_1)
            {
                XFadeBGM(BGM_1,BGM_2);
            }
            else
            {
                XFadeBGM(BGM_2, BGM_1);
            }
        }

        //Debug.Log(BGM_1.volume);
    }

    public void PreXFadeBGM()
    {
        // ���̊֐����Ăяo���ꂽ���ɂȂ��Ă���AudioSource��BGM_1�Ȃ�
        if(_sound == Sound.bgm_1)
        {
            // �o�ߎ��Ԏ擾
            ElapsedTime = BGM_1.time;

            // BGM_2�Ɉړ���̃G���A��AudioClip���Z�b�g
            BGM_2.clip = AudioClipList[_selectArea._nextArea];
            BGM_2.time = ElapsedTime;
            BGM_2.Play();
        }
        else
        {
            // �o�ߎ��Ԏ擾
            ElapsedTime = BGM_2.time;

            // BGM_1�Ɉړ���̃G���A��AudioClip���Z�b�g
            BGM_1.clip = AudioClipList[_selectArea._nextArea];
            BGM_1.time = ElapsedTime;
            BGM_1.Play();
        }

        CrossFade = true;
    }

    // ������:�t�F�[�h�A�E�g���Ă���AudioSource
    // ������:�t�F�[�h�C�����Ă���AudioSource
    private void XFadeBGM(AudioSource _audio1, AudioSource _audio2)
    {
        // �t�F�[�h�A�E�g
        if (_audio1.volume > 0)
        {
            _audio1.volume -= Time.unscaledTime * FadeSpeed;
        }
        else
        {
            _audio1.volume = 0;
        }

        // �t�F�[�h�C��
        if (_audio2.volume < MaxVolume)
        {
            _audio2.volume += Time.unscaledTime * FadeSpeed;
        }
        else
        {
            _audio2.volume = MaxVolume;
        }

        if(_sound == Sound.bgm_1)
        {
            BGM_1.volume = _audio1.volume;
            BGM_2.volume = _audio2.volume;
        }
        else
        {
            BGM_2.volume = _audio1.volume;
            BGM_1.volume = _audio2.volume;
        }

        // �ǂ�����ڕW�̒l�ɂȂ�����
        if(_audio1.volume == 0 && _audio2.volume == MaxVolume)
        {
            // �t�F�[�h�I��
            CrossFade = false;

            // �������o���Ă���AudioSource��\���ϐ��؂�ւ�
            if(_sound == Sound.bgm_1)
            {
                _sound = Sound.bgm_2;
            }
            else
            {
                _sound = Sound.bgm_1;
            }
        }
    }
}
