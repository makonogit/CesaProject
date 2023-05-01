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

        // �C���g�����Đ����I��������
        if (Intro.time > LoopStartTime && Intro.isPlaying)
        {
            Loop.clip = Loop_Bgm[Stage.GetAreaNum()];
            Debug.Log("loop");
            Intro.Stop();
            Loop.Play();

        }
        
    }
}
