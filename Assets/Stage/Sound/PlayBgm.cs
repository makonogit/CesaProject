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

   [SerializeField, Header("各エリアイントロBGM")]
    private List<AudioClip> Loop_Bgm;

    private SetStage Stage;

    // 二宮追加
    [Header("ループに切り替わる時間")]
    public float LoopStartTime;
    [SerializeField,Header("音が変化するスピード")] private float changeSpeed = 1f;
    // ボス戦が始まっていたらtrue
    public bool StartBossBattle = false;

    private bool Init = false;

    private GameObject BossPassage;
    private GettingSmallerBGM _smallerBGM;
    [SerializeField] private BGMFadeManager _BGMfadeMana;


    // Start is called before the first frame update
    void Start()
    {

        //---------------------------------------------------------
        // AudioSourceを取得
        Intro = transform.GetChild(0).GetComponent<AudioSource>();
        Loop = transform.GetChild(1).GetComponent<AudioSource>();

        Stage = new SetStage();
        if (Stage.GetAreaNum() != 0) LoopStartTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Intro.time);

        if(Init == false)
        {
            // ボス通路
            BossPassage = GameObject.Find("BossPassage");
            if (BossPassage != null)
            {
                _smallerBGM = BossPassage.GetComponent<GettingSmallerBGM>();
            }

            //Debug.Log(_smallerBGM);

            Init = true;
        }

        // イントロが再生が終了したら
        if (Intro.time > LoopStartTime && Intro.isPlaying)
        {
            Loop.clip = Loop_Bgm[Stage.GetAreaNum()];
            //Debug.Log("loop");
            Intro.Stop();
            Loop.Play();

            Debug.Log("AAAAAAAAA");
        }

        if (_smallerBGM != null)
        {
            // ボスとの戦闘が始まってなければ
            if (StartBossBattle == false)
            {
                // ボスの通路に入ったら
                if (_smallerBGM.GetInPassageArea() == true)
                {
                    // ループBGMのvolumeを徐々に0に近づける
                    if (Loop.volume > 0f)
                    {
                        Loop.volume -= Time.deltaTime * changeSpeed;
                    }
                    else
                    {
                        Loop.volume = 0f;
                    }
                }
                else
                {
                    // ループBGMのvolumeを徐々に0.2に近づける
                    if (Loop.volume < 0.2f)
                    {
                        Loop.volume += Time.deltaTime * changeSpeed;
                    }
                    else
                    {
                        Loop.volume = 0.2f;
                    }
                }
            }
        }
    }
}
