//-------------------------------------
//�@�S��:�����S
//�@���e�F�u���b�N�Ȃǂ�SE
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickPlay_2 : MonoBehaviour
{
    public enum GimmickSE2List
    {
        ICEBLOCK,   //�X�������
        ROCKBLOCK,  //�₪�����
        PIPEFALL,   //�p�C�v��������
    }

    [SerializeField, Header("�m�F�p")]
    private GimmickSE2List list;

    public List<GimmikSE2> se;   // SE���X�g

    private AudioSource source; //���̃I�u�W�F�N�g��AudioSource

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    //--------------------------------------------------------
    //�@SE���Đ�����֐�
    //�@�����F�Đ�List�ԍ�,Loop�p�@true:Loop�Z,false:Loop�~
    //---------------------------------------------------------
    public void PlayerGimmickSE(GimmickSE2List _list)
    {
        list = _list;

        //�@�����ł����������ݒ�
        source.clip = se[(int)_list]._SE;
        source.volume = se[(int)_list].Volume;
        source.loop = se[(int)_list].Loop;
        //�@�Đ�
        source.Play();
    }

    public bool IsPlay()
    {
        return source.isPlaying;
    }

    public void Stop()
    {
        source.Stop();
    }

    public GimmickSE2List NowSE()
    {
        return list;
    }

}

[System.Serializable]
public class GimmikSE2
{
    public AudioClip _SE;   // SE�Ǘ�
    public float Volume;    // �{�����[���Ǘ�
    public bool Loop;       // Loop���邩

    public GimmikSE2(AudioClip _se, float Vol, bool _loop)
    {
        _SE = _se;
        Volume = Vol;
        Loop = _loop;
    }

}