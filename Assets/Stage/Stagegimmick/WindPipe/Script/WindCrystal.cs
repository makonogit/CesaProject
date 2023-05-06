//---------------------------------
// 担当：藤原昂祐
// 内容：風を発生させるクリスタル
//---------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCrystal : MonoBehaviour
{
    //==================================
    // *** 変数宣言 ***
    //==================================

    //----------------------------------
    // 状態関連
    //----------------------------------
    enum StateID// 状態ID
    {
        NULL,   // 状態無し
        NORMAL, // 通常状態
        BREAK,  // 破壊状態     
        
    }
    StateID oldState = StateID.NULL; // 前の状態
    StateID nowState = StateID.NULL; // 今の状態
    StateID nextState = StateID.NULL;// 次の状態

    //----------------------------------
    // スクリプト関連
    //----------------------------------

    SpriteRenderer sr;// 色

    //----------------------------------
    // レイ関連
    //----------------------------------

    GameObject player;// レイが衝突したプレイヤー

    PlayerJump Jump;
    Rigidbody2D rigid;

    int LayerMask;    // LayerMask

    //----------------------------------
    // 発生させる風関連
    //----------------------------------

    [Header("[発生させるパーティクル]")]
    public GameObject particle;

    [Header("[上昇スピード]")]
    public float rise_speed = 0.1f;

    [Header("[上昇する高さ]")]
    public float rise = 2.0f;

    //==================================
    // *** 初期化処理 ***
    //==================================

    void Start()
    {
        //----------------------------------
        // 変数の初期化
        //----------------------------------

        // 状態を初期化
        nowState = StateID.NORMAL;

        // このオブジェクトの色を取得
        sr = GetComponent<SpriteRenderer>();

        player = GameObject.Find("player");
        Jump = player.GetComponent<PlayerJump>();
        rigid = player.GetComponent<Rigidbody2D>();

        LayerMask = 1 << 12;    //プレイヤーとだけ判定
    }

    //==================================
    // *** 更新処理 ***
    //==================================

    void Update()
    {
        //----------------------------------
        // 状態を更新
        //----------------------------------

        if (nextState != StateID.NULL)
        {
            oldState = nowState;
            nowState = nextState;
            nextState = StateID.NULL;
        }

        //----------------------------------
        // 現在の状態によって処理を分岐
        //----------------------------------

        switch (nowState)
        {
            // 通常状態
            case StateID.NORMAL:
                break;
            // 破壊状態
            case StateID.BREAK:
                Break();
                break;
        }
    }

    //============================================================
    // *** 衝突判定 ***
    //============================================================

    void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------------
        // 通常状態でひびと衝突したら破壊状態に遷移
        //--------------------------------------------------------

        if (nowState == StateID.NORMAL)
        {
            if (collision.gameObject.tag == "Crack")
            {
                // 次の状態を破壊状態に遷移
                nextState = StateID.BREAK;
                // クリスタルを見えなくする
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f);
                // パーティクルを発生させる
                Vector3 pos = transform.position;
                pos.y -= 2.0f;
                Instantiate(particle, pos, Quaternion.identity);
            }

        }
    }

    //============================================================
    // *** 破壊状態の処理 ***
    //============================================================

    void Break()
    {
        //--------------------------------------------------------
        // Rayを作成して当たり判定を取る
        //--------------------------------------------------------

        // 風に当たっているかを判定するRay
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            transform.position,
            new Vector2(1.0f, rise),
            0.0f,
            Vector2.up,
            0.0f
            //LayerMask
            );

        // 無重力状態を判定するRay
        RaycastHit2D raycastHit01 = Physics2D.BoxCast(
           transform.position,
           new Vector2(1.0f, rise - 1.0f),
           0.0f,
           Vector2.up,
           0.0f
           //LayerMask
           );

        //--------------------------------------------------------
        // 風で上昇する処理
        //--------------------------------------------------------

        if (raycastHit01.collider.tag == "Player")
        {
            player = raycastHit.collider.gameObject;

            Vector3 pos = player.transform.position;
            pos.y += rise_speed * Time.deltaTime;
            player.transform.position = pos;
        }

        //--------------------------------------------------------
        // 重力をOFFにする処理
        //--------------------------------------------------------

        if (raycastHit.collider.tag == "Player")
        {
            // OFF
            //PlayerJump playerJump;
            //playerJump = raycastHit.collider.gameObject.GetComponent<PlayerJump>();
            Jump.enabled = false;

            //Rigidbody2D rb;
            //rb = raycastHit.collider.gameObject.GetComponent<Rigidbody2D>();
            rigid.isKinematic = true;
        }
        else
        {
            // ON
            //PlayerJump playerJump;
            //playerJump = player.GetComponent<PlayerJump>();
            Jump.enabled = true;

            //Rigidbody2D rb;
            //rb = player.GetComponent<Rigidbody2D>();
            rigid.isKinematic = false;
        }
       
    }
}
