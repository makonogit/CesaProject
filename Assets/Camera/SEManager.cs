//---------------------------------------------------------
//担当者：二宮怜
//内容　：SEを管理する
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    //---------------------------------------------------------------------
    // - 変数宣言 -

    public AudioClip se_crack1; // ひびつくる（長）

    public AudioClip[] se_run; // 1:走り始め 2,3:交互に繰り返す 4:止まるときの足音

    // 移動SE用変数
    private bool MoveStart = false; // 動き始め
    private bool Moving = false; // 動いている途中
    private bool MoveFinish = false; // 動き終わり
    private int MoveProcess = 0; // 移動用SEの添え字
    private float MoveDelayTime = 0.1f; //SEを鳴らす間隔
    private float MoveTime = 0.0f; // 動き始めてからの経過時間

    private AudioSource audioSource; // オブジェクトがもつAudioSourceを取得する変数

    // 外部取得
    private GameObject player;
    private PlayerMove move;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        player = GameObject.Find("player");
        move = player.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        // 移動があれば
        if (MoveStart)
        {
            PlaySE_Move();
        }
    }
    private void PlaySE_Move()
    {
        // 最初の呼び出し時なら
        if (Moving == false)
        {
            audioSource.PlayOneShot(se_run[MoveProcess]);
            Moving = true;
            MoveProcess = 1;
            MoveTime = 0.0f;
        }

        // 動いている最中実行
        if (Moving)
        {
            if(MoveTime > MoveDelayTime)
            {
                if(MoveProcess == 1)
                {
                    audioSource.PlayOneShot(se_run[MoveProcess]);
                }
                else if(MoveProcess == 2)
                {
                    audioSource.PlayOneShot(se_run[MoveProcess]);

                }
            }
        }

        MoveTime += Time.deltaTime;
    }

    public void PlaySE_Crack1()
    {
        audioSource.PlayOneShot(se_crack1);
    }
}
