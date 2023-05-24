//--------------------------------
//担当：二宮怜
//内容：BGMのフェードを管理する(各フラグがたったときにFadeBGMにアクセスしフェードを開始させる)
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMFadeManager : MonoBehaviour
{
    // 変数宣言

    // フラグたち
    private bool _smallBossflg; // ボスを撃破時に立てる
    private bool _bigBossBGM; // ボス戦開始時に立てる
    private bool _smallStageBGMflg; // セレクトに戻るとき、死亡時、クリア時
    private bool _bigStageBGMflg; // ステージに入った時、リスポーン時、
    private bool _bigSpecialBGMflg; // 死亡時、クリア時
    private bool _smallSpecialBGMflg; // 死亡演出終了後呼び出し

    [SerializeField] private FadeBGM _fadeBGM; // フェード処理スクリプト

    // Update is called once per frame
    void Update()
    {
        // ボすBGM小さく
        if(_smallBossflg == true)
        {
            _fadeBGM.Boss.FadeOut = true;

            _smallBossflg = false;
        }

        // ボスBGMおおきく
        if(_bigBossBGM == true)
        {
            _fadeBGM.Boss.FadeIn = true;

            _bigBossBGM = false;
        }

        // ステージBGM小さく
        if(_smallStageBGMflg == true)
        {
            _fadeBGM.Stage.FadeOut = true;

            _smallStageBGMflg = false;
        }

        // ステージBGMおおきく
        if(_bigStageBGMflg == true)
        {
            _fadeBGM.Stage.FadeIn = true;

            _bigStageBGMflg = false;
        }

        // 特殊BGMおおきく
        if(_bigSpecialBGMflg == true)
        {
            _fadeBGM.Special.FadeIn = true;

            _bigSpecialBGMflg = false;
        }

        // 特殊BGM小さく
        if(_smallSpecialBGMflg == true)
        {
            _fadeBGM.Special.FadeOut = true;

            _smallSpecialBGMflg = false;
        }
    }

    // ボス撃破時に呼び出し
    public void SmallBossBGM()
    {
        _smallBossflg = true;
    }

    // ボス戦開始時に呼び出し
    public void BigBossBGM()
    {
        _bigBossBGM = true;
    }

    // セレクトに戻るとき、死亡時、クリア時呼び出し
    public void SmallStageBGM()
    {
        _smallStageBGMflg = true;
    }

    //ステージに入った時、リスポーン時呼び出し
    public void BigStageBGM()
    {
        _bigStageBGMflg = true;
    }

    // ステージクリアBGMならしたい時呼び出し
    public void StageClear()
    {
        _fadeBGM.StageClear();
    }

    // 死亡時に呼び出し
    public void BigSpecialBGM()
    {
        _bigSpecialBGMflg = true;
    }

    public void smallSpecialBGM()
    {
        _smallSpecialBGMflg = true;
    }

    // BGMの進行具合をリセットする
    public void ResetBGM()
    {
        _fadeBGM.ResetBGM();
    }
}
