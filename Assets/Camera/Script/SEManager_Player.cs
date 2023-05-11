//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�֌W��SE���Ǘ�����
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager_Player : MonoBehaviour
{
    //---------------------------------------------------------------------
    // - �ϐ��錾 -

    public AudioClip se_Hammer; // �n���}�[�Œ@��
    public AudioClip se_crack1; // �Ђт���i���j
    public AudioClip se_drop; // ���n
    public AudioClip se_jimp; // �W�����v
    public AudioClip se_crackmove; // �Ђшړ����̉�

    // ����
    public AudioClip se_town_run1;
    public AudioClip se_town_run2;
    public AudioClip se_town_run3;
    public AudioClip se_town_run4;

    // ����
    public AudioClip se_town_walk1;
    public AudioClip se_town_walk2;
    public AudioClip se_town_walk3;
    public AudioClip se_town_walk4;

    // ��Ԃɂ���ăN���b�v���Z�b�g����
    private AudioClip[] se_move = new AudioClip[4]; // 1:�����n�� 2,3:���݂ɌJ��Ԃ� 4:�~�܂�Ƃ��̑���

    // �ړ�SE�p�ϐ�
    private bool MoveStart = false; // �����n��
    private bool Moving = false; // �����Ă���r��
    private bool MoveFinish = false; // �����I���
    private int MoveProcess = 0; // �ړ��pSE�̓Y����
    private float MoveDelayTime = 0.3f; //SE��炷�Ԋu
    private float SoundTime = 0.0f; // �������Ă���̌o�ߎ���

    private AudioSource audioSource; // �I�u�W�F�N�g������AudioSource���擾����ϐ�

    private PlayerMove.MOVESTATUS oldPlayerMoveStatus = PlayerMove.MOVESTATUS.NONE;

    // �O���擾
    private GameObject player;
    private PlayerMove move;
    private GroundCheck GC;
    private AudioSource Se2; //�����Đ��pSE

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Se2 = GetComponentInChildren<AudioSource>();

        player = GameObject.Find("player");
        move = player.GetComponent<PlayerMove>();
        GC = player.GetComponent<GroundCheck>();

        oldPlayerMoveStatus = move.MoveSta;

        // �����ݒ�
        se_move[0] = se_town_walk1;
        se_move[1] = se_town_walk2;
        se_move[2] = se_town_walk3;
        se_move[3] = se_town_walk4;
    }

    // Update is called once per frame
    void Update()
    {
        if (move.MoveSta != oldPlayerMoveStatus)
        {
            // �Ή������Ԃ�audioclip���Z�b�g����֐�
            ClipSet();
        }

        // �ړ������邩�n�ʂɂ��Ă����
        if (MoveStart && GC.IsGroundCircle())
        {
            PlaySE_Move();
        }
        oldPlayerMoveStatus = move.MoveSta;
    }
    private void PlaySE_Move()
    {
        // �{�����[������
        audioSource.volume = 0.1f;

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

    private void ClipSet()
    {
        switch (move.MoveSta) {
            case PlayerMove.MOVESTATUS.WALK:
                se_move[0] = se_town_walk1;
                se_move[1] = se_town_walk2;
                se_move[2] = se_town_walk3;
                se_move[3] = se_town_walk4;

                MoveDelayTime = 0.6f;
                break;

            case PlayerMove.MOVESTATUS.RUN:
                se_move[0] = se_town_run1;
                se_move[1] = se_town_run2;
                se_move[2] = se_town_run3;
                se_move[3] = se_town_run4;

                MoveDelayTime = 0.2f;
                break;
        }
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

    public void PlayHammer()
    {
        // �{�����[������
        Se2.volume = 0.5f;

        Se2.PlayOneShot(se_Hammer);
    }

    public void PlaySE_Crack1()
    {
        // �{�����[������
        audioSource.volume = 0.5f;

        audioSource.PlayOneShot(se_crack1);
    }

    public void PlaySE_Drop()
    {
        // �{�����[������
        //audioSource.volume = 0.5f;

        //audioSource.PlayOneShot(se_drop);
    }

    public void PlaySE_Jump()
    {
        // �{�����[������
        audioSource.volume = 0.1f;

        audioSource.PlayOneShot(se_jimp);
    }

    public void PlaySE_CrackMove()
    {
        // �{�����[������
        audioSource.volume = 0.5f;

        // �Đ����łȂ��Ȃ�
        if(audioSource.isPlaying == false){
            // ���[�v������
            audioSource.loop = true;

            audioSource.PlayOneShot(se_crackmove);
        }
    }
    public void StopSE_crackMove()
    {
        // ���[�v������
        audioSource.loop = false;

        audioSource.Stop();
    }
}
