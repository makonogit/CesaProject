//-----------------------------------------
// 担当：藤原昂祐
// 内容：棘を伸ばす敵
//-----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleEnemy : MonoBehaviour
{
    //=====================================
    // 変数宣言

    // Ray関連
    [SerializeField] private LayerMask rayLayer;// Rayのレンダー

    // 状態関連
    enum StateID// 状態ID
    {
        NULL,          // 状態なし
        MOVE,          // 移動状態
        ATTACK_STANDBY,// 攻撃待機状態
        ATTACK,        // 攻撃状態
        DEATH,         // 戦闘不能状態
    }
    StateID oldState = StateID.NULL; // 前の状態
    StateID nowState = StateID.NULL; // 現在の状態
    StateID nextState = StateID.NULL;// 次の状態

    // 移動関連
    [Header("移動範囲")]
    public float moveRange = 3.0f; // 移動範囲
    [Header("移動速度")]
    public float moveSpeed = 0.01f;// 移動速度
    Vector2 moveVector;            // 移動方向
    Vector2 startPos;              // 初期位置

    // 攻撃関連
    [Header("攻撃するまでの待機時間")]
    public int attackDelay = 500;// 攻撃するまでの待機時間
    [Header("攻撃している時間")]
    public int attackTime = 500; // 攻撃している時間
    int frameCount = 0;          // フレームカウント

    // アニメーション関連
    [Header("アニメーションコントローラー")]
    public Animator animator;// アニメーションコントローラー
    float animSpeed = 1.0f;  // アニメーションの速さ

    // マテリアル関連
    SpriteRenderer sr;// 色

    //=====================================
    // 初期化処理

    void Start()
    {
        //--------------------------------
        // 変数の初期化

        // スタート時の状態を設定
        nextState = StateID.MOVE;

        // 色を取得
        sr = GetComponent<SpriteRenderer>();

        // 初期位置を保存
        startPos = transform.position;

        // 移動方向を初期化
        moveVector.x = 1.0f;

    }

    //=====================================
    // 更新処理

    void Update()
    {
        //---------------------------------
        // 現在の状態によって処理を分岐

        if (nextState != StateID.NULL)
        {
            oldState = nowState;
            nowState = nextState;
            nextState = StateID.NULL;
        }

        switch (nowState)
        {
            // 移動状態
            case StateID.MOVE:
                Move();
                break;
            // 攻撃待機状態
            case StateID.ATTACK_STANDBY:
                AttackStandby();
                break;
            // 攻撃状態
            case StateID.ATTACK:
                Attack();
                break;
            // 戦闘不能状態
            case StateID.DEATH:
                Death();
                break;

        }

        //---------------------------------
        // アニメーションコントローラーの値を制御

        animator.SetFloat("Horizontal", moveVector.x);// 横
        animator.SetFloat("Vertical", moveVector.y);  // 縦
        animator.SetFloat("Speed", animSpeed);        // 再生速度
    }

    //============================================================
    // *** 衝突判定 ***
    //============================================================

    void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------------
        // ひびとぶつかったら戦闘不能状態に遷移
        //--------------------------------------------------------

        if (collision.gameObject.tag == "Crack")
        {
            nextState = StateID.DEATH;
        }

    }

    //=====================================
    // 移動処理

    void Move()
    {
        //---------------------------------
        // 移動範囲内を左右に反復して移動

        Vector2 position = transform.position;

        position.x += moveSpeed * moveVector.x;

        transform.position = position;

        if(position.x > startPos.x + moveRange)
        {
            moveVector.x = -1.0f;
        }

        if(position.x < startPos.x - moveRange)
        {
            moveVector.x = 1.0f;
        }

        //---------------------------------
        // 壁に衝突しないようにRayで判定

        // 右側
        foreach (RaycastHit2D hit_R in Physics2D.RaycastAll(transform.position, Vector2.right, transform.localScale.x * 0.2f))
        {
            if (hit_R)
            {
                if (hit_R.collider.gameObject.CompareTag("Ground"))
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
            }
        }

        //---------------------------------
        // Playerが周りに居るかをRayで判定

        for(float x = -1.0f; x < 1.0f; x += 0.1f)
        {
            for (float y = 0.0f; y < 1.0f; y += 0.1f)
            {
                Vector2 vector = new Vector2(x,y);

                foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, vector, transform.localScale.x * 0.5f))
                {
                    if (hit_view)
                    {
                        if (hit_view.collider.gameObject.CompareTag("Player"))
                        {
                            nextState = StateID.ATTACK_STANDBY;
                            
                        }
                    }
                }
            }
        }
    }

    //=====================================
    // 攻撃待機処理

    void AttackStandby()
    {
        //--------------------------------------------
        // 攻撃待機時間をカウント

        frameCount++;

        //--------------------------------------------
        // 待機時間が終わったら攻撃状態に変更

        if (attackDelay == frameCount)
        {
            frameCount = 0;
            nextState = StateID.ATTACK;
        }
    }

    //=====================================
    // 攻撃処理

    void Attack()
    {
        //--------------------------------------------
        // 攻撃時間をカウント

        frameCount++;

        //--------------------------------------------
        // 体を大きくする

        if (frameCount == 1)
        {
            Transform objTransform = transform;

            // 大きさの指定
            Vector3 scale = objTransform.localScale;
            scale.x *= 2;
            scale.y *= 2;

            // 大きさを敵用
            objTransform.localScale = scale;
        }

        //--------------------------------------------
        // 体を元の大きさに戻して、移動状態に変更

        if (attackTime == frameCount)
        {
            nextState = StateID.MOVE;
            frameCount = 0;

            Transform objTransform = transform;

            // 大きさの指定
            Vector3 scale = objTransform.localScale;
            scale.x /= 2;
            scale.y /= 2;

            // 大きさを敵用
            objTransform.localScale = scale;
        }
    }

    //===========================================
    // *** 戦闘不能状態の処理
    //===========================================

    void Death()
    {
        //-------------------------------------------------------------------
        // 徐々に透明にする。完全に透明になったらこのオブジェクトを破棄する
        //-------------------------------------------------------------------
        
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - 0.005f);

        if (sr.color.a < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}

