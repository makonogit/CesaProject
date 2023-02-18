//---------------------------------------------------------
//担当者：二宮怜
//内容　：プレイヤーの移動
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    // 移動用
    public float Speed = 5f; // 移動速度用変数
    Vector2 movement; // 入力量を取得する変数

    // 外部取得
    public GameObject PlayerInputManager; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform thisTransform; // 自身のTransformを取得する変数

    public LayerMask BlockLayer;

    //----------------------------------------------------------------------------------------------------------
    // - 初期化処理 -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

        //----------------------------------------------------------------------------------------------------------
        // 自身(player)の持つTransformを取得する
        thisTransform = this.GetComponent<Transform>();
    }

    //----------------------------------------------------------------------------------------------------------
    // - 更新処理 -
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // 移動量をPlayerInputManagerからとってくる
        movement = ScriptPIManager.GetMovement();

        //----------------------------------------------------------------------------------------------------------
        // プレイヤーのTransformに移動量を適応する
        // スティックで上入力すると少し浮く問題があるため、Y,Zには直接値を入れて修正
        thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);

        //Vector3 origin = new Vector3(transform.position.x + 0.5f,transform.position.y,transform.position.z);
        //Vector3 Distance = Vector3.right * 10.0f;

        //RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, 10.0f,BlockLayer);

        //Debug.DrawRay(origin, Vector3.right,Color.red);

        //if (hit)
        //{
        //    Debug.Log(hit.collider);
        //}
        

    }
}
