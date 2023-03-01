//---------------------------------------------------------
//担当者：二宮怜
//内容　：妖精の移動（コントローラー）、プレイヤーを中心とした半径rの円周上を移動
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailTargetMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    private string GroundTag = "Ground";

    [Header("プレイヤーとの距離")]
    public float Speed = 5.0f; // プレイヤーとの距離
    private Vector2 movement; // 入力量を取得する変数
    public float Radius = 3.0f; // プレイヤーと離れられる距離
    private float Distance; // プレイヤーと妖精の距離を持つ変数
    private bool OldActive = false; // 前フレームのアクティブ状態
    [Header("プレイヤーとの差X")]
    public float AdjustX = 2.0f; // アクティブ時のプレイヤーとの座標差X
    [Header("プレイヤーとの差Y")]
    public float AdjustY = 1.0f; // アクティブ時のプレイヤーとの座標差Y

    // 外部取得
    private GameObject PlayerInputMana; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform thisTransform; // 自身のTransformを取得する変数
    private GameObject player; // 自身のTransformを取得する変数
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManagerを探す
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        //----------------------------------------------------------------------------------------------------------
        // 自身(妖精)の持つTransformを取得する
        thisTransform = this.GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // プレイヤー探す
        player = GameObject.Find("player");

        playerTransform = player.GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // 照準モードの時表示、移動

        // 最初のフレームのみ入る
        if (OldActive == false)
        {
            //// 出現位置固定
            //thisTransform.position = new Vector3(
            //    playerTransform.position.x + AdjustX,
            //    playerTransform.position.y + AdjustY,
            //    playerTransform.position.z);
            // 照準表示
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        //----------------------------------------------------------------------------------------------------------
        Vector3 vector_FairyPlayer = playerTransform.position - thisTransform.position;

        // 妖精からプレイヤーの距離
        Distance = vector_FairyPlayer.magnitude;

        //----------------------------------------------------------------------------------------------------------
        // 移動量をPlayerInputManagerからとってくる
        movement = ScriptPIManager.GetRmove();

        if (Distance <= Radius)
        {
            //----------------------------------------------------------------------------------------------------------
            // プレイヤーの座標を基準に妖精の位置を計算
            thisTransform.Translate(movement.x * Speed * Time.deltaTime, movement.y * Speed * Time.deltaTime, 0.0f);
        }
        else
        {
            thisTransform.Translate(
                vector_FairyPlayer.normalized.x * Speed * Time.deltaTime,
                vector_FairyPlayer.normalized.y * Speed * Time.deltaTime,
                0.0f);
        }

        if (OldActive == false)
        {
            //OldActive = true;
        }
        if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.AIM)
        {
           
        }
        else
        {
            // モードが変わって最初のフレームの時に入る
            if (OldActive == true)
            {
                // 非表示
               // this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }

            if (OldActive == true)
            {
                OldActive = false;
            }
        }

        //Debug.Log(OldActive);
    }
}
