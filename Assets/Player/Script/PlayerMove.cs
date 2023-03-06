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
    public float Speed = 5f; // 移動速度用変数
    Vector2 movement; // 入力量を取得する変数

    // モード
    public enum PLAYERMODE
    {
        FOLLOW, // 追従モード
        STOP // 停止モード
    }

    [System.NonSerialized]
    public PLAYERMODE mode = PLAYERMODE.FOLLOW; // プレイヤーの移動時のモード（追従、停止）
    private float LeaveTime; // 一定距離離れてからの経過時間
    //[Header("妖精とプレイヤーが保つ距離")]
    [Header("プレイヤーが追従できる距離")]
    public float moveDistance = 3.0f; // 妖精とプレイヤーが保つ距離
    [Header("プレイヤーが動き始めるまでの時間")]
    public float delayTime = 0.8f; // 動き始めるまでの時間
    [Header("プレイヤーと妖精のソーシャルディスタンス")]
    public float socialDistance = 0.3f; // プレイヤーと妖精が重ならないようにする

    // 外部取得
    private GameObject PlayerInputMana; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform thisTransform; // 自身のTransformを取得する変数

    private GameObject Fairy; // ゲームオブジェクト妖精を保持する
    private Transform fairyTransform; // 妖精の座標

    public LayerMask BlockLayer;

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

        //----------------------------------------------------------------------------------------------------------
        // 自身(player)の持つTransformを取得する
        thisTransform = this.GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // 妖精探す
        Fairy = GameObject.Find("NailTarget");

        // 妖精のTransform情報
        fairyTransform = Fairy.transform;
    }

    //----------------------------------------------------------------------------------------------------------
    // - 更新処理 -
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // 移動量をPlayerInputManagerからとってくる
        //movement = ScriptPIManager.GetMovement();

        //----------------------------------------------------------------------------------------------------------
        // プレイヤーのTransformに移動量を適応する
        // スティックで上入力すると少し浮く問題があるため、Y,Zには直接値を入れて修正
        //thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);

        //Vector3 origin = new Vector3(transform.position.x + 0.5f,transform.position.y,transform.position.z);
        //Vector3 Distance = Vector3.right * 10.0f;

        //RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, 10.0f,BlockLayer);

        //Debug.DrawRay(origin, Vector3.right,Color.red);

        //if (hit)
        //{
        //    Debug.Log(hit.collider);
        //}

        //----------------------------------------------------------------------------------------------------------
        // 普通の移動
        //----------------------------------------------------------------------------------------------------------
        // 移動量をPlayerInputManagerからとってくる
        movement = ScriptPIManager.GetMovement();

        //----------------------------------------------------------------------------------------------------------
        // プレイヤーのTransformに移動量を適応する
        // スティックで上入力すると少し浮く問題があるため、Y,Zには直接値を入れて修正
        thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);

        // 移動モードなら
        if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.MOVE)
        {
            


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //妖精に追従する時の処理
            ////----------------------------------------------------------------------------------------------------------
            //// "プレイヤーと妖精の距離が一定以上"かつ"距離が離れてから一定時間経過した"なら追従

            //// プレイヤーから妖精へのベクトルを求める
            //Vector3 vector_PlayerFairy = fairyTransform.position - thisTransform.position;

            //// x方向の距離だけ必要
            //vector_PlayerFairy.y = 0.0f;

            //// 妖精とプレイヤーの距離を求める
            //float Distance = vector_PlayerFairy.magnitude;

            //// プレイヤーと妖精の距離が一定距離以上なら
            //if(Distance >= socialDistance && Distance <= moveDistance)  // 一定距離以上一定距離内にいるときに追従
            ////if(Distance >= moveDistance) 一定距離以上にいるときに追従
            //{
            //    // 一定時間経過したら動き始める
            //    if (LeaveTime >= delayTime)
            //    {
            //        //----------------------------------------------------------------------------------------------------------
            //        // プレイヤーの移動
            //        thisTransform.Translate(vector_PlayerFairy.normalized.x * Speed * Time.deltaTime, 0.0f, 0.0f);
            //    }

            //    // 時間加算
            //    LeaveTime += Time.deltaTime;
            //}
            //else
            //{
            //    // 初期化
            //    LeaveTime = 0.0f;
            //}

            //// 追従モード中に右クリックされたら停止モードにする
            //if (Mouse.current.rightButton.wasPressedThisFrame)
            //{
            //    mode = PLAYERMODE.STOP;
            //}
        }
        // 停止モードなら
        else if(ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.AIM)
        {
            // 移動はしない

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //妖精に追従する時の処理
            //// 停止モード中に右クリックされたら追従モードにする
            //if (Mouse.current.rightButton.wasPressedThisFrame)
            //{
            //    mode = PLAYERMODE.FOLLOW;

            //    // 初期化
            //    LeaveTime = 0.0f;
            //}
        }
    }
}
