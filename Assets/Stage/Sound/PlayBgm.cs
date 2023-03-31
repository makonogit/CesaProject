//--------------------------------
//担当：菅眞心
//内容：BGMの再生管理
//--------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBgm : MonoBehaviour
{
    //---------------------------------------
    //それぞれのサウンドのAudioSourceを取得
    AudioSource Intro;  // イントロ
    AudioSource Loop;   // ループ

    // 二宮追加
    [Header("ループに切り替わる時間")]
    public float LoopStartTime;

    // Start is called before the first frame update
    void Start()
    {

        //---------------------------------------------------------
        // AudioSourceを取得
        Intro = transform.GetChild(0).GetComponent<AudioSource>();
        Loop = transform.GetChild(1).GetComponent<AudioSource>();
      

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(Intro.time);

        // イントロが再生が終了したら
        if (Intro.time > LoopStartTime && Intro.isPlaying)
        {
            Debug.Log("loop");
            Intro.Stop();
            Loop.Play();

        }
        
    }
}
