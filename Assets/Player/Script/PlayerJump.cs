//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤーのジャンプ
//　　　：押してる間高さが上昇するタイプ
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    // ジャンプ用
    public float JumpPower = 20.0f; // ジャンプ力
    public float JumpHeight = 5.0f; // ジャンプできる高さ
    public float Gravity = 6.0f; // 重力
    private float JumpPos; // ジャンプする瞬間のプレイヤーの高さ
    [SerializeField]
    private bool isGround = false; // 地面に触れているか
    private bool isOverhead = false; // 天井に触れているか
    [SerializeField]
    private bool isJump = false; // ジャンプ中かどうか
    public float axel = 6.3f; // 重力加速度  帳尻合わせた5/10
    public float JumpTime = 0.0f; // ジャンプが始まってから落ち始めるまでの経過時間
    public float FallTime = 0.0f; // 落ち始めてからの時間
    public int RayNum; // 当たっているレイの本数
    public bool fall = false;

    // ブロックの上にいる状態でのFallTime増加を防ぐ
    private float oldPosY; // 前フレームのY座標

    // 外部取得
    private GameObject PlayerInputManager; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform thisTransform; // 自身のTransformを取得する変数
    private GroundCheck ground; // 接地判定用のスクリプトを取得する変数
    private OverheadCheck overhead; // 接地判定用のスクリプトを取得する変数
    private Rigidbody2D thisRigidbody2d; // rigidbody2dを取得する変数
    private InputTrigger trigger;

    // サウンド関係
    private GameObject se;
    private SEManager_Player seMana;

    // ジャンプse制御
    private bool playJumpSe = false;

    // アニメーション関係
    private Animator anim;

    private PlayerStatas playerStatus;

    //-------------菅----------------
    private CrackAutoMove crackmove;        
    private Crack createcrack;      //ジャンプ中ひび生成できないようにする
    private GameObject LineObj;
    private PredictionLine Line;    //ジャンプ中の予測線も無効化
    private bool Selected = false;  //セレクト画面用

    // SEの効果音-------担当：尾花--------
    [Header("効果音")]
    private AudioSource audioSource;  // 取得したAudioSourceコンポーネント
    [SerializeField] AudioClip sound1; // 音声ファイル

    //------------中川---------------
    private RunDustParticle _runDust;
    //-------------------------------

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------------------------------------------------------
        // AudioSourceコンポーネントを取得----追加担当：尾花-------
        audioSource = GetComponent<AudioSource>();

        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManagerを探す
        PlayerInputManager = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

        trigger = PlayerInputManager.GetComponent<InputTrigger>();

        //----------------------------------------------------------------------------------------------------------
        // 自身(player)の持つTransformを取得する
        thisTransform = this.GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // 自身のrigidbody2d取得
        thisRigidbody2d = GetComponent<Rigidbody2D>();

        //----------------------------------------------------------------------------------------------------------
        //当たり判定処理取得
        ground = GetComponent<GroundCheck>();
        overhead = GetComponent<OverheadCheck>();

        //------------追加担当：菅--------------------
       // createcrack = GetComponent<Crack>();
        crackmove = GetComponent<CrackAutoMove>();

        //  LineObj = GameObject.Find("Line");
        //  Line = LineObj.GetComponent<PredictionLine>();

        // Animator取得
        anim = GetComponent<Animator>();

        // サウンド関係
        se = GameObject.Find("SE");
        // Seコンポーネント取得
        seMana = se.GetComponent<SEManager_Player>();

        playerStatus = GetComponent<PlayerStatas>();
        //--------------------------------------
        // 追加担当者:中川直登
        _runDust = GetComponent<RunDustParticle>();
        if (_runDust == null) Debug.LogError("RunDustParticleのコンポーネントを取得できませんでした。");
        //--------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------------------
        // 追加担当者:中川直登
        RunDust();
        //--------------------------------------
        // ヒットストップ中でないなら実行
        if (!playerStatus.GetHitStop())
        {
            if(thisRigidbody2d.gravityScale == 0.0f && !Selected)
            {
                thisRigidbody2d.gravityScale = 1.0f;
            }

            //----------------------------------------------------------------------------------------------------------
            // 接地判定を得る
            isGround = ground.IsGround();
            // 地面と触れているレイの数を取得
            RayNum = ground.GetRayNum();
            // 天井の衝突判定を得る
            isOverhead = overhead.IsOverHead();

            if (isJump == true)
            {
                isGround = false;
            }

            //Debug.Log(RayNum);

            //----------------------------------------------------------------------------------------------------------
            // 何も入力されていなければこのままの値をプレイヤーの座標に加算することになる
            float xSpeed = 0.0f; //プレイヤー座標に加算するときに必要な変数、横移動自体はPlayerMoveで実装
            float ySpeed = -Gravity; // 重力による自由落下の値、ジャンプがあれば上書きされる

            //----------------------------------------------------------------------------------------------------------
            // ジャンプ制御用変数
            bool Jump = false;

            //----------------------------------------------------------------------------------------------------------
            // 一度目のジャンプ以降入力をやめるまでジャンプできないようにする
            if (ScriptPIManager.GetJumpTrigger() == true && isOverhead == false)
            {
                //----------------------------------------------------------------------------------------------------------
                // ジャンプボタンが押されているか取得
                Jump = ScriptPIManager.GetJump();
            }

            //----------------------------------------------------------------------------------------------------------
            // 地面についている時にジャンプが入力された
            if (isGround)
            {
                //----------------------------------------------------------------------------------------------------------
                // ジャンプ入力されていれば
                if (Jump)
                {
                    //----------------------------------------------------------------------------------------------------------
                    // ジャンプによって上昇する量を変数にセット
                    ySpeed = JumpPower * axel * JumpTime;

                    //----------------------------------------------------------------------------------------------------------
                    // ジャンプした瞬間の位置を記憶
                    JumpPos = thisTransform.position.y;

                    //----------------------------------------------------------------------------------------------------------
                    // 状態をジャンプ中に設定
                    isJump = true;

                    if (playJumpSe == false)
                    {
                        // ジャンプse再生
                        seMana.PlaySE_Jump();

                        playJumpSe = true;
                    }
                }
                //----------------------------------------------------------------------------------------------------------
                // ジャンプ入力されていなければ
                else
                {
                    //----------------------------------------------------------------------------------------------------------
                    // 状態を非ジャンプ中に設定
                    isJump = false;
                }
            }
            //----------------------------------------------------------------------------------------------------------
            // ジャンプ中にジャンプ入力がある
            else if (isJump)
            {
                //---------------------------------------------------------------------------------
                // 音声ファイルを再生する-----担当：尾花-------
                //if (!audioSource.isPlaying)
                //{
                //    audioSource.PlayOneShot(sound1);
                //}
                playJumpSe = false;

                //----------------------------------------------------------------------------------------------------------
                // ジャンプボタンが押されている。かつ,現在の高さがジャンプした位置から自分の決めた位置より下ならジャンプ継続
                if (Jump == true/* && JumpPos + JumpHeight > transform.position.y*/)
                {
                    //----------------------------------------------------------------------------------------------------------
                    // ジャンプによって上昇する量を変数にセット
                    ySpeed = JumpPower - (axel * JumpTime);

                    if(ySpeed < 0f)
                    {
                        fall = true;
                        JumpTime = 0f;
                    }
                }
                else
                {
                    //----------------------------------------------------------------------------------------------------------
                    // 状態を非ジャンプ中に設定
                    isJump = false;

                    fall = true;
                    JumpTime = 0f;

                    //---------------------------------------------------------------
                    // ジャンプ制御
                    ScriptPIManager.SetJumpTrigger(false);
                }
            }

            //----------------------------------------------------------------------------------------------------------
            // 自由落下（加速度加味）

            // 地面についてないかつ、ジャンプ入力もない(PlayerInputManagerスクリプトの変数Resetがtrueか未入力)時
            //if ((isGround == false && isJump == false && (crackmove.movestate == CrackAutoMove.MoveState.Walk || crackmove.movestate == CrackAutoMove.MoveState.CrackMoveEnd)))
            if(fall == true)
            {
                if (RayNum == 0)
                {
                    //----------------------------------------------------------------------------------------------------------
                    // 自由落下、落下状態の時間が経てばたつほど落下速度上昇
                    ySpeed = -Gravity - (axel * FallTime);

                    if (thisTransform.position.y != oldPosY)
                    {
                        //----------------------------------------------------------------------------------------------------------
                        // 落下状態での経過時間を加算
                        FallTime += Time.deltaTime;
                    }
                }
                else
                {
                    // 落下状態での経過時間を初期化
                    FallTime = 0.0f;

                    fall = false;
                    // 地面に着地する瞬間に再生
                    if (FallTime != 0.0f)
                    {
                        seMana.PlaySE_Drop();
                    }
                }
            }
            else
            {
                
                //----------------------------------------------------------------------------------------------------------
 
            }

            //----------------------------------------------------------------------------------------------------------
            // ジャンプ入力があるかつ上昇中の時
            if (isJump == true)
            {
                //----------------------------------------------------------------------------------------------------------
                // 経過時間を加算
                JumpTime += Time.deltaTime;
            }
            else
            {
                //----------------------------------------------------------------------------------------------------------
                // 経過時間を初期化
                JumpTime = 0.0f;
            }

            //----------------------------------------------------------------------------------------------------------
            // プレイヤーの座標に加算?
            if (!Selected)
            {
                thisRigidbody2d.velocity = new Vector2(xSpeed, ySpeed);
            }

            //---------------------------------------------------------------
            // アニメーション関係
            // ジャンプ中ならジャンプアニメーション遷移用変数をtrue
            if (!crackmove.HitFlg)
            {
                anim.SetBool("jump", isJump);
            }

            // 落下中なら落下アニメーション遷移変数をセット
            anim.SetBool("drop", FallTime > 0.0f);


        }
        else
        {
            // その場でストップさせる
            thisRigidbody2d.velocity = new Vector2(0, 0);
        }

        // 前フレームのy座標として保持
        oldPosY = thisTransform.position.y;
    }

    //　セレクト画面で選択しているか判断する関数
    public void SetSelected(bool select)
    {
        Selected = select;
    }
    private void RunDust ()
    {
        _runDust.IsJump = !isGround;
    }
}
