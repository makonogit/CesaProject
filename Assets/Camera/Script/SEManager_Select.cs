//---------------------------------------------------------
//担当者：二宮怜
//内容　：セレクト画面のSEを管理する
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager_Select : MonoBehaviour
{
    //---------------------------------------------------------------------
    // - 変数宣言 -

    // 走る
    public AudioClip se_town_run1;
    public AudioClip se_town_run2;
    public AudioClip se_town_run3;
    public AudioClip se_town_run4;

    // 移動SE用変数
    private bool MoveStart = false; // 動き始め
    private bool Moving = false; // 動いている途中
    private bool MoveFinish = false; // 動き終わり
    private int MoveProcess = 0; // 移動用SEの添え字
    private float MoveDelayTime = 0.3f; //SEを鳴らす間隔
    private float SoundTime = 0.0f; // 音が鳴ってからの経過時間

    // 状態によってクリップをセットする
    private AudioClip[] se_move = new AudioClip[4]; // 1:動き始め 2,3:交互に繰り返す 4:止まるときの足音s

    private AudioSource audioSource; // オブジェクトがもつAudioSourceを取得する変数

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SelectClipSet();

        //Debug.Log(MoveStart);

        if (MoveStart)
        {
            PlaySE_Move();
        }
        else
        {
            SoundTime = 0.0f;
        }
    }

    private void PlaySE_Move()
    {
        // ボリューム調整
        audioSource.volume = 0.3f;

        if (MoveFinish == false)
        {
            // 最初の呼び出し時なら
            if (Moving == false)
            {
                // 一歩目の音再生
                audioSource.PlayOneShot(se_move[MoveProcess]); // se_move[0]
                Moving = true;
                MoveProcess = 1;
                SoundTime = 0.0f;
            }

            // 動いている最中実行
            if (Moving)
            {
                // 一定時間間隔でseを交互に鳴らし続ける
                if (SoundTime > MoveDelayTime)
                {
                    audioSource.PlayOneShot(se_move[MoveProcess]); // se_move[1 or 2]
                    if (MoveProcess == 1)
                    {
                        MoveProcess = 2;
                    }
                    else if (MoveProcess == 2)
                    {
                        MoveProcess = 1;
                    }

                    // 音が鳴ったので初期化
                    SoundTime = 0.0f;
                }
            }
        }
        // 止まる時に実行
        else
        {
            MoveProcess = 3;

            if (true/*SoundTime > MoveDelayTime / 2.0f*/)
            {
                audioSource.PlayOneShot(se_move[MoveProcess]); // se_move[3]

                // 初期化
                MoveProcess = 0;
                MoveStart = false;
                Moving = false;
                MoveFinish = false;
            }
        }

        SoundTime += Time.deltaTime;
    }

    private void SelectClipSet()
    {
        se_move[0] = se_town_run1;
        se_move[1] = se_town_run2;
        se_move[2] = se_town_run3;
        se_move[3] = se_town_run4;
    }

    public void SetMoveStart()
    {
        MoveStart = true;
        MoveFinish = false;
    }

    public void SetMoveFinish()
    {
        MoveFinish = true;
    }
}
