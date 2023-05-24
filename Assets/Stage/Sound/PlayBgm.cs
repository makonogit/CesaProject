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
    // AudioSource���擾
    [SerializeField] private AudioSource Loop;   // ���[�v

    [SerializeField, Header("�e�G���ABGM")]
    private List<AudioClip> Loop_Bgm;

    private SetStage Stage;

    // �{�X�킪�n�܂��Ă�����true
    public bool StartBossBattle = false;
    public bool Death = false; 

    private bool Init = false;

    private GameObject BossPassage;
    private GettingSmallerBGM _smallerBGM;
    [SerializeField] private BGMFadeManager _BGMfadeMana;


    // Start is called before the first frame update
    void Start()
    {
        Stage = new SetStage();

        Loop.clip = Loop_Bgm[Stage.GetAreaNum()];
        Loop.Play();
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

        if (_smallerBGM != null)
        {
            // �{�X�Ƃ̐퓬���n�܂��ĂȂ����
            if (StartBossBattle == false)
            {
                // �{�X�̒ʘH�ɓ�������
                if (_smallerBGM.GetInPassageArea() == true)
                {
                    if(Death == false)
                    {
                        _BGMfadeMana.SmallStageBGM();
                    }
                }
                else
                {
                    if(Death == false)
                    {
                        _BGMfadeMana.BigStageBGM();
                    }
                }
            }
        }
    }
}
