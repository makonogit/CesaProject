//--------------------------------
//�S���F�����S
//���e�FBGM�̍Đ��Ǘ�
//--------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStageBgm : MonoBehaviour
{
    //---------------------------------------
    //���ꂼ��̃T�E���h��AudioSource���擾
    AudioSource Intro;  // �C���g��
    AudioSource Loop;   // ���[�v
   
    // Start is called before the first frame update
    void Start()
    {

        //---------------------------------------------------------
        // AudioSource���擾
        Intro = transform.GetChild(0).GetComponent<AudioSource>();
        Loop = transform.GetChild(1).GetComponent<AudioSource>();
      

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(Intro.time);

        // �C���g�����Đ����I��������
        if (Intro.time > 11.5f && Intro.isPlaying)
        {
            Debug.Log("loop");
            Intro.Stop();
            Loop.Play();

        }
        
    }
}
