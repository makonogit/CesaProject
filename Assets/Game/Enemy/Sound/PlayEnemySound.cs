//--------------------------------
//  担当:菅眞心
//　内容：敵のSE再生
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEnemySound : MonoBehaviour
{
    public enum EnemySoundList
    {
        Attack,
        Destroy,
        PlantAttack
    }

    [SerializeField,Header("SoundList確認用")]
    private EnemySoundList EnemySound;  //敵のSEリスト

    [SerializeField, Header("敵のSEリスト ※EnemySoundListと同じ順")]
    //private List<AudioClip> EnemySE;
    private List<SE> EnemySE;

    private AudioSource Source;

    // 二宮追加
    private int RepeatNum = 0;       // 現在の繰り返しカウント
    private int maxRepeatNum = 3;    // 最大繰り返し回数
    private float RepeatTime = 0.15f; // 次の再生に移る時間
    private float RepeatTimer = 0f;  // 再生始めてからの時間
    private float MiddleStartTime_1 = 0.1f;  // サウンドを途中から始めるときの再生開始時間
    private float MiddleStartTime_2 = 0.2f;  // サウンドを途中から始めるときの再生開始時間
    private bool PlayKillBoss = false; // ボスを撃破したときにtrue

    // サウンドテスト用
    public bool test = false;
    public float startTime = 0f;
    public float vol = 0.5f;

    private void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (test == true)
        {
            Source.clip = EnemySE[(int)EnemySoundList.Destroy]._SE;
            Source.volume = vol;
            Source.time = startTime;
            Source.Play();

            test = false;
        }

        if (PlayKillBoss == true)
        {
            PlayKillBossSE();
        }
    }

    //-----------------------------
    //　敵のSE再生
    public void PlayEnemySE(EnemySoundList enemySound)
    {
        // AudioClipとVolumeを設定
        Source.clip = EnemySE[(int)enemySound]._SE;
        Source.volume = EnemySE[(int)enemySound].Volume;

        //再生
        Source.Play();
    }

    //-------------------------------
    // ボス撃破SE再生
    private  void PlayKillBossSE()
    {
        if(RepeatTimer == 0f)
        {
            Source.clip = EnemySE[(int)EnemySoundList.Destroy]._SE;
            Source.volume = 1 - 0.3f * RepeatNum;

            Source.Play();
        }

        RepeatTimer += Time.deltaTime;
        if (RepeatTimer > RepeatTime)
        {
            // 初期化
            RepeatTimer = 0f;

            RepeatNum++;
        }

        // やりたいこと終わったら
        if (RepeatNum >= maxRepeatNum)
        {
            PlayKillBoss = false;
            RepeatNum = 0;
        }
    }

    public void KillBossSet()
    {
        PlayKillBoss = true;
    }
}

[System.Serializable]
public class SE
{
    public AudioClip _SE;   // SE管理
    public float Volume;   // ボリューム管理
 
    public SE(AudioClip _se,float Vol)
    {
        _SE = _se;
        Volume = Vol;
    }

}
