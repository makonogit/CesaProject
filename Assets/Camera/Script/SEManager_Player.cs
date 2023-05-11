//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤー関係のSEを管理する
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager_Player : MonoBehaviour
{
    //---------------------------------------------------------------------
    // - 変数宣言 -

    public AudioClip se_Hammer; // ハンマーで叩く
    public AudioClip se_crack1; // ひびつくる（長）
    public AudioClip se_drop; // 着地
    public AudioClip se_jimp; // ジャンプ
    public AudioClip se_crackmove; // ひび移動中の音

    // 走る
    public AudioClip se_town_run1;
    public AudioClip se_town_run2;
    public AudioClip se_town_run3;
    public AudioClip se_town_run4;

    // 歩く
    public AudioClip se_town_walk1;
    public AudioClip se_town_walk2;
    public AudioClip se_town_walk3;
    public AudioClip se_town_walk4;

    // 状態によってクリップをセットする
    private AudioClip[] se_move = new AudioClip[4]; // 1:動き始め 2,3:交互に繰り返す 4:止まるときの足音

    // 移動SE用変数
    private bool MoveStart = false; // 動き始め
    private bool Moving = false; // 動いている途中
    private bool MoveFinish = false; // 動き終わり
    private int MoveProcess = 0; // 移動用SEの添え字
    private float MoveDelayTime = 0.3f; //SEを鳴らす間隔
    private float SoundTime = 0.0f; // 音が鳴ってからの経過時間

    private AudioSource audioSource; // オブジェクトがもつAudioSourceを取得する変数

    private PlayerMove.MOVESTATUS oldPlayerMoveStatus = PlayerMove.MOVESTATUS.NONE;

    // 外部取得
    private GameObject player;
    private PlayerMove move;
    private GroundCheck GC;
    private AudioSource Se2; //同時再生用SE

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Se2 = GetComponentInChildren<AudioSource>();

        player = GameObject.Find("player");
        move = player.GetComponent<PlayerMove>();
        GC = player.GetComponent<GroundCheck>();

        oldPlayerMoveStatus = move.MoveSta;

        // 初期設定
        se_move[0] = se_town_walk1;
        se_move[1] = se_town_walk2;
        se_move[2] = se_town_walk3;
        se_move[3] = se_town_walk4;
    }

    // Update is called once per frame
    void Update()
    {
        if (move.MoveSta != oldPlayerMoveStatus)
        {
            // 対応する状態のaudioclipをセットする関数
            ClipSet();
        }

        // 移動があるかつ地面についていれば
        if (MoveStart && GC.IsGroundCircle())
        {
            PlaySE_Move();
        }
        oldPlayerMoveStatus = move.MoveSta;
    }
    private void PlaySE_Move()
    {
        // ボリューム調整
        audioSource.volume = 0.1f;

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

    private void ClipSet()
    {
        switch (move.MoveSta) {
            case PlayerMove.MOVESTATUS.WALK:
                se_move[0] = se_town_walk1;
                se_move[1] = se_town_walk2;
                se_move[2] = se_town_walk3;
                se_move[3] = se_town_walk4;

                MoveDelayTime = 0.6f;
                break;

            case PlayerMove.MOVESTATUS.RUN:
                se_move[0] = se_town_run1;
                se_move[1] = se_town_run2;
                se_move[2] = se_town_run3;
                se_move[3] = se_town_run4;

                MoveDelayTime = 0.2f;
                break;
        }
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

    public void PlayHammer()
    {
        // ボリューム調整
        Se2.volume = 0.5f;

        Se2.PlayOneShot(se_Hammer);
    }

    public void PlaySE_Crack1()
    {
        // ボリューム調整
        audioSource.volume = 0.5f;

        audioSource.PlayOneShot(se_crack1);
    }

    public void PlaySE_Drop()
    {
        // ボリューム調整
        //audioSource.volume = 0.5f;

        //audioSource.PlayOneShot(se_drop);
    }

    public void PlaySE_Jump()
    {
        // ボリューム調整
        audioSource.volume = 0.1f;

        audioSource.PlayOneShot(se_jimp);
    }

    public void PlaySE_CrackMove()
    {
        // ボリューム調整
        audioSource.volume = 0.5f;

        // 再生中でないなら
        if(audioSource.isPlaying == false){
            // ループさせる
            audioSource.loop = true;

            audioSource.PlayOneShot(se_crackmove);
        }
    }
    public void StopSE_crackMove()
    {
        // ループさせる
        audioSource.loop = false;

        audioSource.Stop();
    }
}
