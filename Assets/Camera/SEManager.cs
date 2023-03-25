//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�FSE���Ǘ�����
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    //---------------------------------------------------------------------
    // - �ϐ��錾 -

    public AudioClip se_crack1; // �Ђт���i���j

    public AudioClip[] se_run; // 1:����n�� 2,3:���݂ɌJ��Ԃ� 4:�~�܂�Ƃ��̑���

    // �ړ�SE�p�ϐ�
    private bool MoveStart = false; // �����n��
    private bool Moving = false; // �����Ă���r��
    private bool MoveFinish = false; // �����I���
    private int MoveProcess = 0; // �ړ��pSE�̓Y����
    private float MoveDelayTime = 0.1f; //SE��炷�Ԋu
    private float MoveTime = 0.0f; // �����n�߂Ă���̌o�ߎ���

    private AudioSource audioSource; // �I�u�W�F�N�g������AudioSource���擾����ϐ�

    // �O���擾
    private GameObject player;
    private PlayerMove move;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        player = GameObject.Find("player");
        move = player.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        // �ړ��������
        if (MoveStart)
        {
            PlaySE_Move();
        }
    }
    private void PlaySE_Move()
    {
        // �ŏ��̌Ăяo�����Ȃ�
        if (Moving == false)
        {
            audioSource.PlayOneShot(se_run[MoveProcess]);
            Moving = true;
            MoveProcess = 1;
            MoveTime = 0.0f;
        }

        // �����Ă���Œ����s
        if (Moving)
        {
            if(MoveTime > MoveDelayTime)
            {
                if(MoveProcess == 1)
                {
                    audioSource.PlayOneShot(se_run[MoveProcess]);
                }
                else if(MoveProcess == 2)
                {
                    audioSource.PlayOneShot(se_run[MoveProcess]);

                }
            }
        }

        MoveTime += Time.deltaTime;
    }

    public void PlaySE_Crack1()
    {
        audioSource.PlayOneShot(se_crack1);
    }
}
