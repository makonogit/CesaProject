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

    PlayerInputManager.DIRECTION oldDire; // 前フレームの向きを入れておくための変数

    // 外部取得
    private GameObject PlayerInputMana; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform thisTransform; // 自身のTransformを取得する変数

    public LayerMask BlockLayer;

    private Animator anim; // アニメーターを取得するための変数

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
    }

    //----------------------------------------------------------------------------------------------------------
    // - 更新処理 -
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // 普通の移動
        //----------------------------------------------------------------------------------------------------------
        // 移動量をPlayerInputManagerからとってくる
        movement = ScriptPIManager.GetMovement();

        //----------------------------------------------------------------------------------------------------------
        // プレイヤーのTransformに移動量を適応する
        // スティックで上入力すると少し浮く問題があるため、Y,Zには直接値を入れて修正
        thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);

        //-----------------------------------------------------------------
        // アニメーション関係
        // movementのxの値によってwalkかrunになる
        anim.SetBool("walk", (movement.x > 0.0f && movement.x < 0.5f) || (movement.x < 0.0f && movement.x > -0.5f)); // スティック入力の左右半分までなら歩く
        anim.SetBool("run", (movement.x >= 0.5f && movement.x <= 1.0f) || (movement.x <= -0.5f && movement.x >= -1.0f)); // スティック入力の左右半分以上なら走る

        if (oldDire != ScriptPIManager.Direction)
        {
            // プレイヤーの向きを今と逆にする
            thisTransform.localScale = new Vector3(-thisTransform.localScale.x, thisTransform.localScale.y, thisTransform.localScale.z);
        }

        // 前フレームの向きとして保存
        oldDire = ScriptPIManager.Direction;
    }
}
