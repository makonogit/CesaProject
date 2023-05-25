//----------------------------------
//  担当：菅
//　内容：ポーズ中のsnapshot切替
//----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseSnap : MonoBehaviour
{
  
    [SerializeField]
    private AudioMixerSnapshot _PauseSnap;   //ポーズ用

    [SerializeField]
    private AudioMixerSnapshot _StartSnap;   //開始時


    // ポーズ用に変更
    public void PauseSnapChange()
    {
        _PauseSnap.TransitionTo(0.1f);
    }

    // 通常に変更
    public void NormalSnapChange()
    {
        _StartSnap.TransitionTo(0.1f);
    }

}
