//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�Z���N�g��ʂ�SE���Ǘ�����
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager_Select : MonoBehaviour
{
    //---------------------------------------------------------------------
    // - �ϐ��錾 -

    // ����
    public AudioClip se_town_run1;
    public AudioClip se_town_run2;
    public AudioClip se_town_run3;
    public AudioClip se_town_run4;

    // �ړ�SE�p�ϐ�
    private bool MoveStart = false; // �����n��
    private bool Moving = false; // �����Ă���r��
    private bool MoveFinish = false; // �����I���
    private int MoveProcess = 0; // �ړ��pSE�̓Y����
    private float MoveDelayTime = 0.3f; //SE��炷�Ԋu
    private float SoundTime = 0.0f; // �������Ă���̌o�ߎ���

    // ��Ԃɂ���ăN���b�v���Z�b�g����
    private AudioClip[] se_move = new AudioClip[4]; // 1:�����n�� 2,3:���݂ɌJ��Ԃ� 4:�~�܂�Ƃ��̑���s

    private AudioSource audioSource; // �I�u�W�F�N�g������AudioSource���擾����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectClipSet();

        //Debug.Log(MoveStart);

        if (MoveStart)
        {
            PlaySE_Move();
        }
        else
        {
            SoundTime = 0.0f;
        }
    }

    private void PlaySE_Move()
    {
        // �{�����[������
        audioSource.volume = 0.3f;

        if (MoveFinish == false)
        {
            // �ŏ��̌Ăяo�����Ȃ�
            if (Moving == false)
            {
                // ����ڂ̉��Đ�
                audioSource.PlayOneShot(se_move[MoveProcess]); // se_move[0]
                Moving = true;
                MoveProcess = 1;
                SoundTime = 0.0f;
            }

            // �����Ă���Œ����s
            if (Moving)
            {
                // ��莞�ԊԊu��se�����݂ɖ炵������
                if (SoundTime > MoveDelayTime)
                {
                    audioSource.PlayOneShot(se_move[MoveProcess]); // se_move[1 or 2]
                    if (MoveProcess == 1)
                    {
                        MoveProcess = 2;
                    }
                    else if (MoveProcess == 2)
                    {
                        MoveProcess = 1;
                    }

                    // ���������̂ŏ�����
                    SoundTime = 0.0f;
                }
            }
        }
        // �~�܂鎞�Ɏ��s
        else
        {
            MoveProcess = 3;

            if (true/*SoundTime > MoveDelayTime / 2.0f*/)
            {
                audioSource.PlayOneShot(se_move[MoveProcess]); // se_move[3]

                // ������
                MoveProcess = 0;
                MoveStart = false;
                Moving = false;
                MoveFinish = false;
            }
        }

        SoundTime += Time.deltaTime;
    }

    private void SelectClipSet()
    {
        se_move[0] = se_town_run1;
        se_move[1] = se_town_run2;
        se_move[2] = se_town_run3;
        se_move[3] = se_town_run4;
    }

    public void SetMoveStart()
    {
        MoveStart = true;
        MoveFinish = false;
    }

    public void SetMoveFinish()
    {
        MoveFinish = true;
    }
}
