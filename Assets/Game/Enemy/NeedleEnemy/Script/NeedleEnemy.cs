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
    // *** 変数宣言 ***
    //=====================================

    //-------------------------------------
    // Ray関連
    //-------------------------------------

    [SerializeField] private LayerMask rayLayer;// Rayのレンダー

    //-------------------------------------
    // 状態関連
    //-------------------------------------

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

    //-------------------------------------
    // 移動関連
    //-------------------------------------

    [Header("[移動関連]")]
    [Header("・移動範囲")]
    public float moveRange = 3.0f; // 移動範囲
    [Header("・移動速度")]
    public float moveSpeed = 0.01f;// 移動速度
    [Header("・落下速度")]
    public float gravity = 0.02f;  // 落下速度
    float g = 0.02f;               // 落下による移動量
    Vector2 moveVector;            // 移動方向
    Vector2 startPos;              // 初期位置
    Vector2 center;                // 回転の中心座標
    float angle;                   // 回転角度
    float radius = 0.25f;          // 円の半径

    //-------------------------------------
    // 攻撃関連
    //-------------------------------------

    [Header("[攻撃関連]")]
    [Header("・索敵範囲")]
    public float attackRange = 6.0f;
    [Header("・攻撃するまでの待機時間")]
    public int attackDelay = 250;// 攻撃するまでの待機時間
    [Header("・攻撃している時間")]
    public int attackTime = 750; // 攻撃している時間
    int frameCount = 0;          // フレームカウント
    GameObject exclamationmark;  // ！オブジェクト

    // アニメーション関連
    [Header("[アニメーションコントローラー]")]
    public Animator animator;// アニメーションコントローラー
    float animSpeed = 1.0f;  // アニメーションの速さ
    
    // 撃破関連
    [Header("[撃破時エフェクト]")]
    public GameObject effect;  // エフェクト
    SpriteRenderer sr;         // 色
    GameObject obj_hitcollider;// プレイヤーとの当たり判定

    // 二宮追加
    private PlayEnemySound _playEnemySound;

    //=====================================
    // 初期化処理

    void Start()
    {
        // プレイヤーとの当たり判定を取得
        obj_hitcollider = transform.Find("HitCollider").gameObject;

        // びっくりマークのオブジェクトを取得
        exclamationmark = transform.Find("ExclamationMark").gameObject;

        // スタート時の状態を設定
        nextState = StateID.MOVE;

        // 色を取得
        sr = GetComponent<SpriteRenderer>();

        // 初期位置を保存
        startPos = transform.position;

        // 移動方向を初期化
        moveVector.x = 1.0f;

        // 重力による移動量を初期化
        g = gravity;

        // 敵se取得
        _playEnemySound = GameObject.Find("EnemySE").GetComponent<PlayEnemySound>();
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

        //--------------------------
        // 重力による落下
        //--------------------------

        Gravity();

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
            // 当たったひびのCrackOrderを取得
            var order = collision.gameObject.GetComponent<CrackCreater>();

            //生成中なら
            if (order != null)
            {
                if (order.State != CrackCreater.CrackCreaterState.CRAETED)
                {
                    nextState = StateID.DEATH;
                    Instantiate(effect, transform.position, Quaternion.identity);
                    // 回転の中心座標に初期位置を保存
                    center = transform.position;
                    obj_hitcollider.SetActive(false);

                    // 音鳴らす
                    _playEnemySound.PlayEnemySE(PlayEnemySound.EnemySoundList.Destroy);
                    // 敵消す
                    Destroy(gameObject);
                }
            }
        }
    }

    //=====================================
    // 移動処理

    void Move()
    {
        //---------------------------------
        // 移動範囲内を左右に反復して移動
        //---------------------------------

        if (g == 0)
        {

            Vector2 position = transform.position;

            position.x += moveSpeed * moveVector.x;


            transform.position = position;

            if (position.x > startPos.x + moveRange)
            {
                moveVector.x = -1.0f;
            }

            if (position.x < startPos.x - moveRange)
            {
                moveVector.x = 1.0f;
            }
        }

        //---------------------------------
        // 壁に衝突しないようにRayで判定
        //---------------------------------

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, moveVector, attackRange))
        {
            if (hit_view)
            {
                if (hit_view.collider.gameObject.CompareTag("Ground"))
                {
                    moveVector.x *= -1.0f;
                }
              
            }
        }

        //---------------------------------
        // 進行方向にプレイヤーがいるかをRayで判定
        //---------------------------------

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, moveVector, attackRange))
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

    //=====================================
    // *** 攻撃待機処理 ***
    //=====================================

    void AttackStandby()
    {
        //--------------------------------------------
        // 攻撃待機時間をカウント
        //--------------------------------------------

        frameCount++;

        //--------------------------------------------
        // びっくりマークを出す
        //--------------------------------------------

        if (attackDelay / 2 > frameCount)
        {
            exclamationmark.SetActive(true);

        }

        //--------------------------------------------
        // 待機時間が終わったら攻撃状態に変更
        //--------------------------------------------

        if (attackDelay == frameCount)
        {
            frameCount = 0;
            nextState = StateID.ATTACK;

            exclamationmark.SetActive(false);
        }
    }

    //=====================================
    // *** 攻撃処理 ***
    //=====================================

    void Attack()
    {
        //--------------------------------------------
        // 攻撃時間をカウント
        //--------------------------------------------

        frameCount++;

        //--------------------------------------------
        // 攻撃アニメーションを再生
        //--------------------------------------------

        if (frameCount == 1)
        {
            animator.SetTrigger("AttackStart");
        }

        //--------------------------------------------
        // 攻撃時間が経過したら攻撃終了
        //--------------------------------------------

        if (attackTime == frameCount)
        {
            animator.SetTrigger("AttackEnd");
            nextState = StateID.MOVE;
            frameCount = 0;

        }
    }

    //===========================================
    // *** 戦闘不能状態の処理 ***
    //===========================================

    void Death()
    {
        //---------------------------------------------------
        //  敵を左右に振動
        //---------------------------------------------------

        // 現在のトランスフォームを取得
        Vector3 pos = this.transform.position;
        // 角度をラジアンに変換
        float rd = -angle * Mathf.PI / 180.0f;
        // 回転後の座標を計算
        pos.x = center.x + (Mathf.Cos(rd) * radius) + radius;
        // 変更を反映
        this.transform.position = pos;
        // 角度を加算
        angle += 5.0f;

        //-------------------------------------------------------------------
        // 徐々に透明にする。完全に透明になったらこのオブジェクトを破棄する
        //-------------------------------------------------------------------

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - 0.005f);

        if (sr.color.a < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    //=========================================
    // *** 重力 ***
    //=========================================

    void Gravity()
    {
        Vector2 position = transform.position;
        position.y -= g;
        transform.position = position;

        g = gravity;

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, Vector2.down, 0.5f)){ 
            if (hit_view)
            {
                if (hit_view.collider.gameObject.CompareTag("Ground"))
                {
                    g = 0;
                }

            }
        }
    }

    //=====================================
    // *** 初期化処理 ***
    //=====================================

    public void Init()
    {
        // 座標を初期化
        transform.position = startPos;

        // スタート時の状態を設定
        nextState = StateID.MOVE;

        // 移動方向を初期化
        moveVector.x = 1.0f;

        // 重力による移動量を初期化
        g = gravity;
    }
}

