//---------------------------------------------------------
//担当者：二宮怜
//内容　：照準の移動（コントローラーRスティック）、プレイヤーを中心とした半径rの円周上を移動
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailTargetMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    private string GroundTag = "Ground";

    [Header("照準の移動速度")]
    public float Speed = 5.0f; // プレイヤーとの距離
    private Vector2 movement; // 入力量を取得する変数
    public float Radius = 3.0f; // プレイヤーと離れられる距離
    [Header("プレイヤーとの距離")]
    public float Distance; // プレイヤーと妖精の距離を持つ変数
    [Header("照準が現れるときのプレイヤーとの差の基準")]
    public float InitPosDistance = 2.0f; // 照準が現れるときのプレイヤーとの差の基準

    private Vector3 offset_TargetStop; // 照準を動かしてない時のプレイヤーと照準のベクトル用変数
    bool Move = true; // 照準が動いているか動いていないか判別
    bool touchGround = false; // 照準がGroundタグのオブジェクトと触れているか

    [System.NonSerialized]
    public bool CreateCrack = false;

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

        //-----------------------------------------------------------------------------------------------------------
        // 照準の位置を初期化
        thisTransform.position = new Vector3(playerTransform.position.x + 0.3f,playerTransform.position.y + 0.3f,0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------------------------------------------------------
        // 照準の表示、移動

        //----------------------------------------------------------------------------------------------------------
        Vector3 vector_PlayerFairy = thisTransform.position - playerTransform.position;

        // 照準からプレイヤーの距離
        Distance = vector_PlayerFairy.magnitude;

        //----------------------------------------------------------------------------------------------------------
        // 移動量をPlayerInputManagerからとってくる
        movement = ScriptPIManager.GetRmove();

        // 右スティックの入力が無ければ
        if(movement.x == 0.0f && movement.y == 0.0f)
        {
            // 前のフレームまで照準が動いていたなら
            if (Move == true)
            {
                // プレイヤーと照準のベクトルを保存
                offset_TargetStop = vector_PlayerFairy;

                // このif文に入らないためにfalse
                Move = false;
            }

            // 親子関係のときのような動きを再現
            thisTransform.position = new Vector3(
                playerTransform.position.x + offset_TargetStop.x,
                playerTransform.position.y + offset_TargetStop.y,
                0.0f);
        }
        else
        {
            if(Move == false)
            {
                Move = true;
            }
        }

        // 表示していない時に照準の位置を方向を維持したまま近づける
        // 表示しない状態なら
        if (CreateCrack == true)
        {
            // 照準をベクトルを維持したまま指定した距離の位置に移動させる
            thisTransform.position = new Vector3(
                playerTransform.position.x + offset_TargetStop.normalized.x * InitPosDistance,
                playerTransform.position.y + offset_TargetStop.normalized.y * InitPosDistance,
                0.0f);

            // 座標が更新されたのでオフセットも更新
            offset_TargetStop = thisTransform.position - playerTransform.position;

            // 次ひびが生成されるまでfalse
            CreateCrack = false;
        }
        
        if (hammerNail._HammerState == global::HammerNail.HammerState.NAILSET)
        {
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
                    -vector_PlayerFairy.normalized.x * Speed * Time.deltaTime,
                    -vector_PlayerFairy.normalized.y * Speed * Time.deltaTime,
                    0.0f);
            }

            //Debug.Log(thisTransform.position);
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
        }
        // NAILSET 以外なら
        else
        {
            if (col.a == 1.0f)
            {
                GetComponent<SpriteRenderer>().color = new Color(col.r, col.g, col.b, 0.0f);

            }
        }
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
