//------------------------------------------
// �S��:�����S
// ���e:�M�~�b�N�pSE�Đ�
//------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickPlaySound : MonoBehaviour
{
    public enum GimmickSEList
    {
        SAND_LOOP,
        TOLOLLEY_LOOP,
        TOLOLLEYLIGHT,
        PIPEWIND,
        PIPEFALL,
    }

    [SerializeField,Header("�m�F�p")]
    private GimmickSEList list;

    public List<GimmikSE> se;   // SE���X�g

    private AudioSource source; //���̃I�u�W�F�N�g��AudioSource

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    //--------------------------------------------------------
    //�@SE���Đ�����֐�
    //�@�����F�Đ�List�ԍ�,Loop�p�@true:Loop�Z,false:Loop�~
    //---------------------------------------------------------
    public void PlayerGimmickSE(GimmickSEList _list)
    {
        //�@�����ł����������ݒ�
        source.clip = se[(int)_list]._SE;
        source.volume = se[(int)_list].Volume;
        source.loop = se[(int)_list].Loop;
        //�@�Đ�
        source.Play();
    }

}


[System.Serializable]
public class GimmikSE
{
    public AudioClip _SE;   // SE�Ǘ�
    public float Volume;    // �{�����[���Ǘ�
    public bool Loop;       // Loop���邩

    public GimmikSE(AudioClip _se, float Vol,bool _loop)
    {
        _SE = _se;
        Volume = Vol;
        Loop = _loop;
    }

}