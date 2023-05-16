//=========================================
// 担当：藤原昂祐
// 内容：洞窟のボスの移動処理
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_CaveBoss: MonoBehaviour
{
    //=====================================
    // *** 変数宣言 ***
    //=====================================

    //-------------------------------------
    // *** インスタンス ***

    public static Move_CaveBoss instance;// このクラスのインスタンス

    //-------------------------------------
    // *** 移動関連 ***

    [Header("移動範囲")]
    public float moveRange = 15.0f;// 移動範囲
    [Header("移動速度")]
    public float moveSpeed = 0.01f;// 移動速度
    Vector2 moveVector;            // 移動方向
    Vector2 startPos;              // 初期位置

    //-------------------------------------
    // *** アニメーション関連 ***

    [Header("アニメーションコントローラー")]
    public Animator animator;// アニメーションコントローラー
    float animSpeed = 1.0f;  // アニメーションの速さ

    // 回転
    Vector2 center;      // 回転の中心座標
    float angle;         // 回転角度
    [Header("上下移動の幅")]
    public float radius = 0.5f;// 円の半径
    [Header("上下移動の速さ")]
    public float updown_speed = 0.6f;// 回転角度に加算する値

    //=====================================
    // *** 初期化処理 ***
    //=====================================

    void Start()
    {
        //--------------------------------
        // *** 変数の初期化 ***

        // 回転の中心座標に初期位置を保存
        center = transform.position;

        // 初期位置を保存
        startPos = transform.position;

        // 移動方向を初期化
        moveVector.x = 1.0f;
        moveVector.y = 1.0f;

        // このクラスのインスタンスを生成
        if (instance == null)
        {
            instance = this;
        }
    }

    //=====================================
    // *** 更新処理 ***
    //=====================================

    void Update()
    {
        //----------------------------------------------------------
        // *** アニメーションコントローラーの値を制御 ***

        //animator.SetFloat("Horizontal", moveVector.x);// 横
        //animator.SetFloat("Vertical", moveVector.y);  // 縦
        animator.SetFloat("Speed", animSpeed);          // 再生速度
    }

    //=====================================
    // *** 移動処理 ***
    //=====================================

    public void Move()
    {
        //---------------------------------------------------
        //  上下にふわふわさせる
        //---------------------------------------------------

        // 現在のトランスフォームを取得
        Vector3 pos = this.transform.position;
        // 角度をラジアンに変換
        float rd = -angle * Mathf.PI / 180.0f;
        // 回転後の座標を計算
        //pos.x = center.x + (Mathf.Sin(rd) * radius) + radius + 0.1f;
        pos.y = center.y + (Mathf.Cos(rd) * radius) + radius + 0.1f;
        // 変更を反映
        this.transform.position = pos;
        // 角度を加算
        angle += updown_speed;

        //-----------------------------------------
        // *** 移動範囲内を左右に反復して移動 ***

        Vector2 position = transform.position;

        position.x += moveSpeed * moveVector.x;
        position.y += moveSpeed * moveVector.y;

        transform.position = position;

        // 右側の移動制限
        if (position.x > startPos.x + moveRange)
        {
            moveVector.x = -1.0f;
        }
        // 左側の移動制限
        if (position.x < startPos.x - moveRange)
        {
            moveVector.x = 1.0f;
        }
        // 上側の移動制限
        if (position.y > startPos.y + 0.5f)
        {
            //moveVector.y = -0.5f;
        }
        // 下側の移動制限
        if (position.y < startPos.y - 0.5f)
        {
            //moveVector.y = 0.5f;
        }

        //---------------------------------------
        // *** 壁に衝突しないようにRayで判定 ***

        // 右側
        foreach (RaycastHit2D hit_R in Physics2D.RaycastAll(transform.position, Vector2.right, transform.localScale.x * 0.2f))
        {
            if (hit_R)
            {
                if (hit_R.collider.gameObject.CompareTag("Ground"))
                {
                    moveVector.x = -1.0f;
                }
                else if(hit_R.collider.gameObject.CompareTag("Crack"))
                {
                    moveVector.x = -1.0f;
                }
            }
        }

        // 左側
        foreach (RaycastHit2D hit_L in Physics2D.RaycastAll(transform.position, Vector2.left, transform.localScale.x * 0.2f))
        {
            if (hit_L)
            {
                if (hit_L.collider.gameObject.CompareTag("Ground"))
                {
                    moveVector.x = 1.0f;
                }
                else if (hit_L.collider.gameObject.CompareTag("Crack"))
                {
                    moveVector.x = 1.0f;
                }
            }
        }
    }
}
