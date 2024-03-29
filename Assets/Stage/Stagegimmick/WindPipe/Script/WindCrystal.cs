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

    New_PlayerJump Jump;
    Rigidbody2D rigid;

    // Ray関連
    [SerializeField] private LayerMask rayLayer;// Rayのレンダー

    //----------------------------------
    // 発生させる風関連
    //----------------------------------

    [Header("[風の設定]")]
    [Header("・発生させるパーティクル")]
    public GameObject particle;

    [Header("・上昇スピード")]
    public float rise_speed = 10.0f;

    [Header("・上昇する高さ")]
    public float rise = 10.0f;

    //==================================
    // *** 初期化処理 ***
    //==================================
    public void Init()
    {
        // 状態を初期化
        nowState = StateID.NORMAL;
        oldState = StateID.NULL; // 前の状態
        nextState = StateID.NULL;// 次の状態

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);
        if(transform.childCount > 2)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
        }

        Jump.enabled = true;
        rigid.gravityScale = 1.0f;
    }


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
        Jump = player.GetComponent<New_PlayerJump>();
        rigid = player.GetComponent<Rigidbody2D>();

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
                GameObject obj = Instantiate(particle, pos, Quaternion.identity);
                obj.transform.parent = transform;
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
            new Vector2(0.5f, rise - 1.0f),
            0.0f,
            Vector2.up,
            0.0f,
            rayLayer
            );

        // 無重力状態にするかを判定するRay
        RaycastHit2D raycastHit01 = Physics2D.BoxCast(
           transform.position,
           new Vector2(1.0f, rise),
           0.0f,
           Vector2.up,
           0.0f,
           rayLayer
           );

        //--------------------------------------------------------
        // 風で上昇する処理
        //--------------------------------------------------------

        if (raycastHit)
        {
            // 重力をOFF
            if (raycastHit01)
            {
                Jump.enabled = false;
                //Jump.MoveY = 0;
                rigid.gravityScale = 0.0f;
                //rigid.isKinematic = true;
            }
            //else
            //{
            //    if (player.GetComponent<CrackAutoMove>().movestate == CrackAutoMove.MoveState.Walk)
            //    {
            //        Jump.enabled = true;
            //        //Jump.MoveY = 0;
            //        rigid.gravityScale = 1.0f;
            //    }
            //}

            player = raycastHit.collider.gameObject;
            Vector3 pos = player.transform.position;
            pos.y += rise_speed * Time.deltaTime;
            player.transform.position = pos;
        }

        // 重力をON
        else
        {
            if (player != null && player.GetComponent<CrackAutoMove>().movestate == CrackAutoMove.MoveState.Walk)
            {
                Jump.enabled = true;
                rigid.gravityScale = 1.0f;
            }
            //rigid.isKinematic = false;
        }
    }
}
