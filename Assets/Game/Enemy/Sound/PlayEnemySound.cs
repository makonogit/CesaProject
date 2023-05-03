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
    private void Start()
    {
        Source = GetComponent<AudioSource>();
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
