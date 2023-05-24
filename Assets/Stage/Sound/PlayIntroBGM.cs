//----------------------------
//　担当：菅眞心
//　内容：ボスBGMのイントロ部分再生
//----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIntroBGM : MonoBehaviour
{
    // 変数宣言

    [SerializeField] private AudioSource BossBGM_Intro;
    [SerializeField] private AudioSource BossBGM_Loop;
    [SerializeField] private BGMFadeManager _BGMfadeMana;

    private void Update()
    {
        // イントロ再生中に一定時間経ったら
        if(BossBGM_Intro.time > 2.0f && BossBGM_Intro.isPlaying == true)
        {
            // イントロ停止
            BossBGM_Intro.Stop();

            // ループ部分再生開始
            BossBGM_Loop.Play();
        }
    }
}
