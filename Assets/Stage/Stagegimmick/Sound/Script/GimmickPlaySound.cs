//------------------------------------------
// 担当:菅眞心
// 内容:ギミック用SE再生
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

    [SerializeField,Header("確認用")]
    private GimmickSEList list;

    public List<GimmikSE> se;   // SEリスト

    private AudioSource source; //このオブジェクトのAudioSource

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    //--------------------------------------------------------
    //　SEを再生する関数
    //　引数：再生List番号,Loop用　true:Loop〇,false:Loop×
    //---------------------------------------------------------
    public void PlayerGimmickSE(GimmickSEList _list)
    {
        //　引数でもらった情報を設定
        source.clip = se[(int)_list]._SE;
        source.volume = se[(int)_list].Volume;
        source.loop = se[(int)_list].Loop;
        //　再生
        source.Play();
    }

}


[System.Serializable]
public class GimmikSE
{
    public AudioClip _SE;   // SE管理
    public float Volume;    // ボリューム管理
    public bool Loop;       // Loopするか

    public GimmikSE(AudioClip _se, float Vol,bool _loop)
    {
        _SE = _se;
        Volume = Vol;
        Loop = _loop;
    }

}