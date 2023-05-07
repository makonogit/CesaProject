//--------------------------------
//  �S��:�����S
//�@���e�F�G��SE�Đ�
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnemySound : MonoBehaviour
{
    public enum EnemySoundList
    {
        Attack,
        Destroy,
        PlantAttack
    }

    [SerializeField,Header("SoundList�m�F�p")]
    private EnemySoundList EnemySound;  //�G��SE���X�g

    [SerializeField, Header("�G��SE���X�g ��EnemySoundList�Ɠ�����")]
    //private List<AudioClip> EnemySE;
    private List<SE> EnemySE;

    private AudioSource Source;

    // ��{�ǉ�
    private int RepeatNum = 0;       // ���݂̌J��Ԃ��J�E���g
    private int maxRepeatNum = 3;    // �ő�J��Ԃ���
    private float RepeatTime = 0.15f; // ���̍Đ��Ɉڂ鎞��
    private float RepeatTimer = 0f;  // �Đ��n�߂Ă���̎���
    private float MiddleStartTime_1 = 0.1f;  // �T�E���h��r������n�߂�Ƃ��̍Đ��J�n����
    private float MiddleStartTime_2 = 0.2f;  // �T�E���h��r������n�߂�Ƃ��̍Đ��J�n����
    private bool PlayKillBoss = false; // �{�X�����j�����Ƃ���true

    // �T�E���h�e�X�g�p
    public bool test = false;
    public float startTime = 0f;
    public float vol = 0.5f;

    private void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (test == true)
        {
            Source.clip = EnemySE[(int)EnemySoundList.Destroy]._SE;
            Source.volume = vol;
            Source.time = startTime;
            Source.Play();

            test = false;
        }

        if (PlayKillBoss == true)
        {
            PlayKillBossSE();
        }
    }

    //-----------------------------
    //�@�G��SE�Đ�
    public void PlayEnemySE(EnemySoundList enemySound)
    {
        // AudioClip��Volume��ݒ�
        Source.clip = EnemySE[(int)enemySound]._SE;
        Source.volume = EnemySE[(int)enemySound].Volume;

        //�Đ�
        Source.Play();
    }

    //-------------------------------
    // �{�X���jSE�Đ�
    private  void PlayKillBossSE()
    {
        if(RepeatTimer == 0f)
        {
            Source.clip = EnemySE[(int)EnemySoundList.Destroy]._SE;
            Source.volume = 1 - 0.3f * RepeatNum;

            Source.Play();
        }

        RepeatTimer += Time.deltaTime;
        if (RepeatTimer > RepeatTime)
        {
            // ������
            RepeatTimer = 0f;

            RepeatNum++;
        }

        // ��肽�����ƏI�������
        if (RepeatNum >= maxRepeatNum)
        {
            PlayKillBoss = false;
            RepeatNum = 0;
        }
    }

    public void KillBossSet()
    {
        PlayKillBoss = true;
    }
}

[System.Serializable]
public class SE
{
    public AudioClip _SE;   // SE�Ǘ�
    public float Volume;   // �{�����[���Ǘ�
 
    public SE(AudioClip _se,float Vol)
    {
        _SE = _se;
        Volume = Vol;
    }

}
