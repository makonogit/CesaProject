//--------------------------------
//�S���F�����S
//���e�FBGM�̍Đ��Ǘ�
//--------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBgm : MonoBehaviour
{
    //---------------------------------------
    //���ꂼ��̃T�E���h��AudioSource���擾
    AudioSource Intro;  // �C���g��
    AudioSource Loop;   // ���[�v

   [SerializeField, Header("�e�G���A�C���g��BGM")]
    private List<AudioClip> Loop_Bgm;

    private SetStage Stage;

    // ��{�ǉ�
    [Header("���[�v�ɐ؂�ւ�鎞��")]
    public float LoopStartTime;
    [SerializeField,Header("�����ω�����X�s�[�h")] private float changeSpeed = 1f;
    // �{�X�킪�n�܂��Ă�����true
    public bool StartBossBattle = false;

    private bool Init = false;

    private GameObject BossPassage;
    private GettingSmallerBGM _smallerBGM;
    [SerializeField] private BGMFadeManager _BGMfadeMana;


    // Start is called before the first frame update
    void Start()
    {

        //---------------------------------------------------------
        // AudioSource���擾
        Intro = transform.GetChild(0).GetComponent<AudioSource>();
        Loop = transform.GetChild(1).GetComponent<AudioSource>();

        Stage = new SetStage();
        if (Stage.GetAreaNum() != 0) LoopStartTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Intro.time);

        if(Init == false)
        {
            // �{�X�ʘH
            BossPassage = GameObject.Find("BossPassage");
            if (BossPassage != null)
            {
                _smallerBGM = BossPassage.GetComponent<GettingSmallerBGM>();
            }

            //Debug.Log(_smallerBGM);

            Init = true;
        }

        // �C���g�����Đ����I��������
        if (Intro.time > LoopStartTime && Intro.isPlaying)
        {
            Loop.clip = Loop_Bgm[Stage.GetAreaNum()];
            //Debug.Log("loop");
            Intro.Stop();
            Loop.Play();

            Debug.Log("AAAAAAAAA");
        }

        if (_smallerBGM != null)
        {
            // �{�X�Ƃ̐퓬���n�܂��ĂȂ����
            if (StartBossBattle == false)
            {
                // �{�X�̒ʘH�ɓ�������
                if (_smallerBGM.GetInPassageArea() == true)
                {
                    // ���[�vBGM��volume�����X��0�ɋ߂Â���
                    if (Loop.volume > 0f)
                    {
                        Loop.volume -= Time.deltaTime * changeSpeed;
                    }
                    else
                    {
                        Loop.volume = 0f;
                    }
                }
                else
                {
                    // ���[�vBGM��volume�����X��0.2�ɋ߂Â���
                    if (Loop.volume < 0.2f)
                    {
                        Loop.volume += Time.deltaTime * changeSpeed;
                    }
                    else
                    {
                        Loop.volume = 0.2f;
                    }
                }
            }
        }
    }
}
