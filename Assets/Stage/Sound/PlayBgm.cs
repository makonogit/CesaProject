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
    // AudioSourceを取得
    [SerializeField] private AudioSource Loop;   // ループ

    [SerializeField, Header("各エリアBGM")]
    private List<AudioClip> Loop_Bgm;

    private SetStage Stage;

    // ボス戦が始まっていたらtrue
    public bool StartBossBattle = false;
    public bool Death = false; 

    private bool Init = false;

    private GameObject BossPassage;
    private GettingSmallerBGM _smallerBGM;
    [SerializeField] private BGMFadeManager _BGMfadeMana;


    // Start is called before the first frame update
    void Start()
    {
        Stage = new SetStage();

        Loop.clip = Loop_Bgm[Stage.GetAreaNum()];
        Loop.Play();
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

        if (_smallerBGM != null)
        {
            // ボスとの戦闘が始まってなければ
            if (StartBossBattle == false)
            {
                // ボスの通路に入ったら
                if (_smallerBGM.GetInPassageArea() == true)
                {
                    if(Death == false)
                    {
                        _BGMfadeMana.SmallStageBGM();
                    }
                }
                else
                {
                    if(Death == false)
                    {
                        _BGMfadeMana.BigStageBGM();
                    }
                }
            }
        }
    }
}
