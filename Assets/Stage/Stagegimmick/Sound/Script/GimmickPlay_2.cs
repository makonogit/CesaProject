//-------------------------------------
//　担当:菅眞心
//　内容：ブロックなどのSE
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickPlay_2 : MonoBehaviour
{
    public enum GimmickSE2List
    {
        ICEBLOCK,   //氷が割れる
        ROCKBLOCK,  //岩が割れる
        PIPEFALL,   //パイプが落ちる
    }

    [SerializeField, Header("確認用")]
    private GimmickSE2List list;

    public List<GimmikSE2> se;   // SEリスト

    private AudioSource source; //このオブジェクトのAudioSource

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    //--------------------------------------------------------
    //　SEを再生する関数
    //　引数：再生List番号,Loop用　true:Loop〇,false:Loop×
    //---------------------------------------------------------
    public void PlayerGimmickSE(GimmickSE2List _list)
    {
        list = _list;

        //　引数でもらった情報を設定
        source.clip = se[(int)_list]._SE;
        source.volume = se[(int)_list].Volume;
        source.loop = se[(int)_list].Loop;
        //　再生
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
    public AudioClip _SE;   // SE管理
    public float Volume;    // ボリューム管理
    public bool Loop;       // Loopするか

    public GimmikSE2(AudioClip _se, float Vol, bool _loop)
    {
        _SE = _se;
        Volume = Vol;
        Loop = _loop;
    }

}