//---------------------------------------
//担当：二宮怜
//内容：現在再生しているBGMのフェード処理（あまり触らないでほしい）
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGMstatus
{
    // フェード処理実行用のフラグ
    public bool FadeIn = false;
    public bool FadeOut = false;

    // 最大音量
    public float MaxVolume = 0f;

    public BGMstatus(float volume)
    {
        MaxVolume = volume;
    }
}          

public class FadeBGM : MonoBehaviour
{
    // 変数宣言

    // MainSceneのBGM用AudioSource
    [SerializeField] private AudioSource StageBGM;
    [SerializeField] private AudioSource BossBGM_Intro;
    [SerializeField] private AudioSource BossBGM_Loop;
    [SerializeField] private AudioSource SpecialBGM; // 特殊BGM

    // クリアBGM用AudioClip
    [SerializeField] private AudioClip AC_Clear;
    [SerializeField] private bool _clearBGMflg = false; // クリアBGM再生開始時にtrue

    // BGMフェード用クラス
    public BGMstatus Stage; // ステージ 
    public BGMstatus Boss;  // ボス
    public BGMstatus Special; // ゲームオーバー、クリア

    [SerializeField,Header("フェードの速度")] private float _speed = 1.0f;

    private void Start()
    {
        // 各BGMの初期音量は0
        //StageBGM_Intro.volume = 0f;
        StageBGM.volume = 0f;
        BossBGM_Intro.volume = 0f;
        BossBGM_Loop.volume = 0f;
        SpecialBGM.volume = 0f;

        Stage.FadeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        //------------------------------------
        // ステージ用
        if (Stage.FadeOut == true)
        {
            StageBGMFadeOut();
        }
        if(Stage.FadeIn == true)
        {
            StageBGMFadeIn();
        }

        // フェードインとフェードアウトがかぶったらどちらも終了
        if(Stage.FadeIn == true && Stage.FadeOut == true)
        {
            Stage.FadeIn = false;
            Stage.FadeOut = false;
        }

        //-------------------------------------
        // ボス用
        if (Boss.FadeOut == true)
        {
            BossBGMFadeOut();
        }
        if (Boss.FadeIn == true)
        {
            BossBGMFadeIn();
        }

        if (Boss.FadeIn == true && Boss.FadeOut == true)
        {
            Boss.FadeIn = false;
            Boss.FadeOut = false;
        }

        //------------------------------------------
        // 特殊BGM
        if(Special.FadeOut == true)
        {
            SpecialBGMFadeOut();
        }
        if(Special.FadeIn == true)
        {
            SpecialBGMFadeIn();
        }

        if(Special.FadeIn == true && Special.FadeOut == true)
        {
            Special.FadeIn = false;
            Special.FadeOut = false;
        }

        //------------------------------------------
        // クリア時呼び出し
        if(_clearBGMflg == true)
        {
            ChangeClearBGM();
        }
    }

    // ステージBGMのフェードアウト
    private void StageBGMFadeOut()
    {
        // BGMの音量を0に近づけていく
        if(StageBGM.volume > 0f)
        {
            StageBGM.volume -= Time.unscaledTime * _speed;
        }
        else
        {
            StageBGM.volume  = 0f;

            Stage.FadeOut = false;
        }
    }

    // ステージBGMのフェードイン
    private void StageBGMFadeIn()
    {
        // BGMの音量を最大音量に近づけていく
        if (StageBGM.volume < Stage.MaxVolume)
        {
            StageBGM.volume += Time.unscaledTime * _speed;
        }
        else
        {
            StageBGM.volume = Stage.MaxVolume;

            Stage.FadeIn = false;
        }
    }

    // ボスBGMのフェードイン
    private void BossBGMFadeIn()
    {
        // BGMの音量を最大音量に近づけていく
        if (BossBGM_Loop.volume < Boss.MaxVolume)
        {
            BossBGM_Intro.volume += Time.unscaledTime * _speed;
            BossBGM_Loop.volume += Time.unscaledTime * _speed;
        }
        else
        {
            BossBGM_Intro.volume = Boss.MaxVolume;
            BossBGM_Loop.volume = Boss.MaxVolume;

            Boss.FadeIn = false;
        }
    }

    // ボスBGMのフェードアウト
    private void BossBGMFadeOut()
    {
        // BGMの音量を0に近づけていく
        if (BossBGM_Loop.volume > 0)
        {
            BossBGM_Intro.volume -= Time.unscaledTime * _speed;
            BossBGM_Loop.volume -= Time.unscaledTime * _speed;
        }
        else
        {
            BossBGM_Intro.volume = 0;
            BossBGM_Loop.volume = 0;

            Boss.FadeOut = false;
        }
    }

    // 特殊BGMのフェードイン
    private void SpecialBGMFadeIn()
    {
        // BGMの音量を最大音量に近づけていく
        if(SpecialBGM.volume < Special.MaxVolume)
        {
            SpecialBGM.volume += Time.unscaledTime * _speed;
        }
        else
        {
            SpecialBGM.volume = Special.MaxVolume;

            Special.FadeIn = false;
        }
    }

    // 特殊BGMのフェードアウト
    private void SpecialBGMFadeOut()
    {
        // BGMの音量を0に近づけていく
        if (SpecialBGM.volume > 0)
        {
            SpecialBGM.volume -= Time.unscaledTime * _speed;
        }
        else
        {
            SpecialBGM.volume = 0;

            Special.FadeOut = false;
        }
    }

    //  セッター
    public void StageClear()
    {
        _clearBGMflg = true;
    }

    // StageBGMをステージクリアBGMに切り替え
    private void ChangeClearBGM()
    {
        // クリアBGMクリップをセット
        SpecialBGM.clip = AC_Clear;
        // BGM開始位置を初めにする
        SpecialBGM.Play();
        // BGMフェードインさせる
        Special.FadeIn = true;
        // クリアフラグおろす
        _clearBGMflg = false;
    }

    // BGMの進行具合をリセットする
    public void ResetBGM()
    {
        // ステージBGMを最初から再生
        StageBGM.Play();

        // ボスBGMの再生時間を初期化
        BossBGM_Intro.time = 0f;
    }
}