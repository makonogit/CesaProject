//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤーの移動
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    // 移動用
    public float BaseSpeed = 5f; // 移動速度用変数
    private Vector2 movement; // 入力量を取得する変数
    bool Moveflg = true;    //移動フラグ　追加担当：菅

    [Header("歩くときは走るときのどれくらいのスピードか")]
    public float magnification = 1.5f; // 歩くときのスピードの倍率

    PlayerInputManager.DIRECTION oldDire = PlayerInputManager.DIRECTION.RIGHT; // 前フレームの向きを入れておくための変数

    public float ideal_IdleTime = 2.0f; //  立ち止まってからアイドル状態になるまでの時間
    private float IdleTime = 0.0f; // 立ち止まってからの経過時間

    public enum MOVESTATUS
    {
        NONE,
        WALK,
        RUN,
        FRIEZE, // アイドル準備段階 
    }

    public MOVESTATUS MoveSta = MOVESTATUS.NONE;

    public bool debug = false;

    // 外部取得
    private GameObject PlayerInputMana; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform thisTransform; // 自身のTransformを取得する変数

    public LayerMask BlockLayer;

    private Animator anim; // アニメーターを取得するための変数

    private GameObject se;
    private SEManager_Player seMana;

    private PlayerStatas playerStatus;

    // プレイヤープルプル
    private WallCheck _wallCheck;

    //--------------------------------------
    // 追加担当者:中川直登
    private RunDustParticle _runDust;
    //--------------------------------------

    //----------------------------------------------------------------------------------------------------------
    // - 初期化処理 -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManagerを探す
        PlayerInputMana = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        oldDire = ScriptPIManager.Direction;

        //----------------------------------------------------------------------------------------------------------
        // 自身(player)の持つTransformを取得する
        thisTransform = this.GetComponent<Transform>();

        // アニメーター取得
        anim = GetComponent<Animator>();

        se = GameObject.Find("SE");
        // Seコンポーネント取得
        seMana = se.GetComponent<SEManager_Player>();

        playerStatus = GetComponent<PlayerStatas>();
        //--------------------------------------
        // 追加担当者:中川直登
        _runDust = GetComponent<RunDustParticle>();
        if(_runDust == null) Debug.LogError("RunDustParticleのコンポーネントを取得できませんでした。");
        //--------------------------------------

        // 壁との接触判定スクリプト
        _wallCheck = GetComponent<WallCheck>();
    }

    //----------------------------------------------------------------------------------------------------------
    // - 更新処理 -
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // 普通の移動
        //----------------------------------------------------------------------------------------------------------

        if (Moveflg && !playerStatus.GetHitStop()) // 移動フラグがたっているか、ヒットストップ中じゃない
        {
            // 移動量をPlayerInputManagerからとってくる
            movement = ScriptPIManager.GetMovement();
        }
        else
        {
            movement = Vector2.zero;
        }

        // 何の動きもなければ
        if (movement.x == 0.0f)
        {
            MoveSta = MOVESTATUS.FRIEZE;
            IdleTime += Time.deltaTime;

            // 最後のse再生変数セット
            seMana.SetMoveFinish();
        }

        if(movement.x != 0.0f || !(anim.GetBool("frieze")))
        {
            IdleTime = 0.0f;        
        }

        if ((movement.x > 0.0f && movement.x < 0.5f) || (movement.x < 0.0f && movement.x > -0.5f))
        {
            MoveSta = MOVESTATUS.WALK;

            // se再生開始
            seMana.SetMoveStart();
        }
        else if((movement.x >= 0.5f && movement.x <= 1.0f) || (movement.x <= -0.5f && movement.x >= -1.0f))
        {
            MoveSta = MOVESTATUS.RUN;
                
            // se再生開始
            seMana.SetMoveStart();
        }
        else if(movement.x == 0)
        {
            // 立ち止まってからの経過時間が指定の時間以上ならアイドル状態になる
            if (IdleTime >= ideal_IdleTime)
            {
                MoveSta = MOVESTATUS.NONE;
            }
        }

        float Speed = 0.0f;
        switch (MoveSta)
        {
            case MOVESTATUS.WALK:
                Speed = BaseSpeed * magnification;
                
                break;

            case MOVESTATUS.RUN:
                Speed = BaseSpeed;
                break;
        }

        //-----------------------------------------------------------------
        // アニメーション関係
        // movementのxの値によってかわる
        anim.SetBool("walk", MoveSta == MOVESTATUS.WALK); // スティック入力の左右半分までなら歩く
        anim.SetBool("run", MoveSta == MOVESTATUS.RUN); // スティック入力の左右半分以上なら走る
        anim.SetBool("frieze", MoveSta == MOVESTATUS.FRIEZE); // スティック入力が無ければ準備状態

        //-------------------------------------------------------------------
        // ここより下は強制的にmovementの値をいじる時あり
        // スクリプトがついてなくてもエラーにならない
        if (_wallCheck != null)
        {
            // 壁との接触判定
            bool LeftWall = _wallCheck.LeftCheck();
            bool RightWall = _wallCheck.RightCheck();

            // 壁と接触したレイの方向に進もうとしたらmovementを強制的に0にしてプルプル制御

            // 左壁と接触
            if (LeftWall == true && RightWall == false)
            {
                // 左に進行しようとしている
                if (movement.x < 0f)
                {
                    movement.x = 0f;
                }
            }
            // 右壁と接触
            else if (LeftWall == false && RightWall == true)
            {
                // 右に進行しようとしている
                if (movement.x > 0f)
                {
                    movement.x = 0f;
                }
            }
            // 両方trueもしくは両方falseなら何もしない
        }

        //----------------------------------------------------------------------------------------------------------
        // プレイヤーのTransformに移動量を適応する
        // スティックで上入力すると少し浮く問題があるため、Y,Zには直接値を入れて修正
        thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);
        
        if (oldDire != ScriptPIManager.Direction)
        {
            // プレイヤーの向きを今と逆にする
            thisTransform.localScale = new Vector3(-thisTransform.localScale.x, thisTransform.localScale.y, thisTransform.localScale.z);
        }
        //--------------------------------------
        // 追加担当者:中川直登
        RunDust();
        //--------------------------------------
        // 前フレームの向きとして保存
        oldDire = ScriptPIManager.Direction;
    }

    //-------------------------------------
    //　移動設定関数
    //　引数：true 移動　false 移動しない
    //　戻り値：なし
    //　追加担当：菅
    public void SetMovement(bool moveflg)
    {
        Moveflg = moveflg;
    }

    //-------------------------------------
    //　移動設定取得関数
    //　引数：なし
    //　戻り値：移動フラグ
    //　追加担当：菅
    public bool GetMovementFlg()
    {
        return Moveflg;
    }


    //-------------------------------------
    //　入力量取得関数
    //　引数：なし
    //　戻り値：入力量
    //　追加担当：菅
    public Vector2 GetMovement()
    {
        return movement;
    }

    //--------------------------------------
    // 追加担当者:中川直登
    // 関数：RunDust() 
    // 目的：走った時のエフェクトのオンオフ
    private void RunDust() 
    {
        _runDust.IsMove = (MoveSta != MOVESTATUS.FRIEZE);
    }
    //--------------------------------------
}
