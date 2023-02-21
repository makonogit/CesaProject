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
    private bool isGround = false; // 地面に触れているか
    private bool isOverhead = false; // 天井に触れているか
    private bool isJump = false; // ジャンプ中かどうか
    private float axel = 9.8f; // 重力加速度
    private float JumpTime = 0.0f; // ジャンプが始まってから落ち始めるまでの経過時間
    public float FallTime = 0.0f; // 落ち始めてからの時間

    // 外部取得
    private GameObject PlayerInputManager; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform thisTransform; // 自身のTransformを取得する変数
    private GroundCheck ground; // 接地判定用のスクリプトを取得する変数
    private OverheadCheck overhead; // 接地判定用のスクリプトを取得する変数
    private Rigidbody2D thisRigidbody2d; // rigidbody2dを取得する変数
    private CrackMove crackmove;

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManagerを探す
        PlayerInputManager = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

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

        crackmove = GetComponent<CrackMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // 接地判定を得る
        isGround = ground.IsGround();
        // 天井の衝突判定を得る
        isOverhead = overhead.IsOverHead();

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
            if (Jump == true)
            {
                //----------------------------------------------------------------------------------------------------------
                // ジャンプによって上昇する量を変数にセット
                ySpeed = JumpPower * axel * JumpTime; ;

                //----------------------------------------------------------------------------------------------------------
                // ジャンプした瞬間の位置を記憶
                JumpPos = thisTransform.position.y;

                //----------------------------------------------------------------------------------------------------------
                // 状態をジャンプ中に設定
                isJump = true;
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
            //----------------------------------------------------------------------------------------------------------
            // ジャンプボタンが押されている。かつ,現在の高さがジャンプした位置から自分の決めた位置より下ならジャンプ継続
            if (Jump == true && JumpPos + JumpHeight > transform.position.y)
            {
                //----------------------------------------------------------------------------------------------------------
                // ジャンプによって上昇する量を変数にセット
                ySpeed = JumpPower - (axel * JumpTime);
            }
            else
            {
                //----------------------------------------------------------------------------------------------------------
                // 状態を非ジャンプ中に設定
                isJump = false;

                //---------------------------------------------------------------
                // ジャンプ制御
                ScriptPIManager.SetJumpTrigger(false);
            }
        }

        //----------------------------------------------------------------------------------------------------------
        // 自由落下（加速度加味）

        // 地面についてないかつ、ジャンプ入力もない(PlayerInputManagerスクリプトの変数Resetがtrueか未入力)時
        if(isGround == false && Jump == false && crackmove.movestate == CrackMove.MoveState.Walk)
        {
            //----------------------------------------------------------------------------------------------------------
            // 自由落下、落下状態の時間が経てばたつほど落下速度上昇
            ySpeed = -Gravity - (axel * FallTime);

            //----------------------------------------------------------------------------------------------------------
            // 落下状態での経過時間を加算
            FallTime += Time.deltaTime;
        }
        else
        {
            //----------------------------------------------------------------------------------------------------------
            // 落下状態での経過時間を初期化
            FallTime = 0.0f;
        }

        //----------------------------------------------------------------------------------------------------------
        // ジャンプ入力があるかつ上昇中の時
        if (Jump == true)
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
        thisRigidbody2d.velocity = new Vector2(xSpeed, ySpeed);
    }
}
