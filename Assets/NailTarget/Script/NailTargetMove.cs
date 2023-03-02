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
    [Header("プレイヤーとの距離")]
    public float Distance; // プレイヤーと妖精の距離を持つ変数
    private bool OldActive = false; // 前フレームのアクティブ状態
    [Header("プレイヤーとの差X")]
    public float AdjustX = 2.0f; // アクティブ時のプレイヤーとの座標差X
    [Header("プレイヤーとの差Y")]
    public float AdjustY = 1.0f; // アクティブ時のプレイヤーとの座標差Y

    private Vector3 offset; // 照準を動かしてない時のプレイヤーと照準のベクトル用変数
    bool Move = true; // 照準が動いているか動いていないか判別
    bool touchGround = false; // 照準がGroundタグのオブジェクトと触れているか

    // 外部取得
    private GameObject PlayerInputMana; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform thisTransform; // 自身のTransformを取得する変数
    private GameObject player; // 自身のTransformを取得する変数
    private Transform playerTransform;
    private HammerNail hammerNail; // 釘打つスクリプト取得する変数

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManagerを探す
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        //----------------------------------------------------------------------------------------------------------
        // 自身(照準)の持つTransformを取得する
        thisTransform = this.GetComponent<Transform>();

        // 初期は透明
        Color col = GetComponent<SpriteRenderer>().color;

        col.a = 0.0f;

        //----------------------------------------------------------------------------------------------------------
        // プレイヤー探す
        player = GameObject.Find("player");

        playerTransform = player.GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // HammerNail取得
        hammerNail = player.GetComponent<HammerNail>();
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
            //this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        //----------------------------------------------------------------------------------------------------------
        Vector3 vector_FairyPlayer = playerTransform.position - thisTransform.position;

        // 妖精からプレイヤーの距離
        Distance = vector_FairyPlayer.magnitude;

        //----------------------------------------------------------------------------------------------------------
        // 移動量をPlayerInputManagerからとってくる
        movement = ScriptPIManager.GetRmove();

        // 右スティックの入力が無ければ
        if(movement.x == 0.0f && movement.y == 0.0f)
        {
            // 前のフレームまで照準が動いていたなら
            if(Move == true)
            {
                // プレイヤーと照準のベクトルを保存
                offset = vector_FairyPlayer;
                //Debug.Log(offset);

                // このif文に入らないためにfalse
                Move = false;
            }

            // 親子関係のときのような動きを再現
            thisTransform.position = new Vector3(
                playerTransform.position.x - offset.x,
                playerTransform.position.y - offset.y,
                0.0f);
        }
        else
        {
            if(Move == false)
            {
                Move = true;
            }
        }

        // プレイヤーと照準の距離が一定範囲以下なら
        if (Distance <= Radius)
        {
            //----------------------------------------------------------------------------------------------------------
            // 照準の現在の位置に移動量を加算
            thisTransform.Translate(
            movement.x * Speed * Time.deltaTime,
            movement.y * Speed * Time.deltaTime,
            0.0f);

            if (touchGround == true)
            {
                // 釘を打てない色:赤
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            else
            {
                // 釘を打てる色:シアン
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            // 照準が離れすぎないための処理
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

        // 照準のカラー情報取得
        Color col = GetComponent<SpriteRenderer>().color;

        //---------------------------------------------------------------------
        // HammerNailのHammerStateの状態によってalpha値変える
        // NAILSETなら
        if(hammerNail._HammerState == global::HammerNail.HammerState.NAILSET)
        {

            
            if (col.a == 0.0f)
            {
                GetComponent<SpriteRenderer>().color = new Color(col.r, col.g, col.b,1.0f);


            }

            Debug.Log(col.a);
        }
        // NAILSET 以外なら
        else
        {
            //Debug.Log("???????????????????????");
            if (col.a == 1.0f)
            {
                GetComponent<SpriteRenderer>().color = new Color(col.r, col.g, col.b, 0.0f);

            }
            Debug.Log(col.a);

        }


        Debug.Log(hammerNail._HammerState);

        //Debug.Log(OldActive);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == GroundTag)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            touchGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == GroundTag)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 1.0f, 1.0f);
            touchGround = false;
        }
    }
}
