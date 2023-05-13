//=========================================
// 担当：藤原昂祐
// 内容：洞窟のボスの行動を制御
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_CaveBoss : MonoBehaviour
{
    //=====================================
    // *** 変数宣言 ***
    //=====================================

    //-------------------------------------
    // 初期化関連 
    //-------------------------------------

    // 座標
    Vector3 init_boss_pos;    // ボスの初期位置
    Vector3 init_lefthand_pos;// 左手の初期位置
    Vector3 init_right_pos;   // 右手の初期位置

    // ステータス
    int init_boss_hp;         // ボスの初期HP

    //-------------------------------------
    // 行動制御関連 
    //-------------------------------------

    // メイン状態ID
    enum MainStateID   
    {
        NULL,          // 状態なし
        MOVE,          // 移動状態
        ATTACK,        // 攻撃状態
        DAMAGE,        // 被弾状態
        DEATH,         // 戦闘不能
    }

    // メイン状態
    MainStateID oldMainState = MainStateID.NULL; // 前の状態
    MainStateID nowMainState = MainStateID.NULL; // 現在の状態
    MainStateID nextMainState = MainStateID.NULL;// 次の状態

    // 攻撃状態ID
    enum AttackStateID
    {
        NULL,         // 状態なし
        ENEMY_DROP,   // 敵を降らせる
        GRIP_PLAYER   // プレイヤーを捕まえる
    }

    // 攻撃状態
    AttackStateID oldAttackState = AttackStateID.NULL;  // 前の状態
    AttackStateID nowAttackState = AttackStateID.NULL;  // 現在の状態
    AttackStateID nextAttackState = AttackStateID.NULL; // 次の状態
    bool isEndState;                                    // 攻撃終了フラグ

    // 待機時間
    [Header("[行動待機時間]")]
    public int mainStateDelay = 500;// 行動待機時間
    int mainStateDelay_Cnt;         // 行動待機時間をカウント

    //-------------------------------------
    // マテリアル関連 
    //-------------------------------------

    // 色
    SpriteRenderer sr_boss;     // ボスの色
    SpriteRenderer sr_lefthand; // 左手の色
    SpriteRenderer sr_righthand;// 右手の色

    //-------------------------------------
    // 被弾状態関連 
    //-------------------------------------

    // 被弾状態
    [Header("[被弾状態]")]
    [Header("・HP")]
    public int hp = 3;               // 体力
    [Header("・撃破時エフェクト")]
    public GameObject effect;        // エフェクト
    [Header("・無敵時間")]
    public int invincible_time = 100;// 無敵時間
    int invincible_time_cnt;         // 無敵時間カウント

    //-------------------------------------
    // 移動関連 
    //-------------------------------------

    // 回転
    Vector2 center;      // 回転の中心座標
    float angle;         // 回転角度
    float radius = 0.25f;// 円の半径
   
    //=====================================
    // *** 初期化 ***
    //=====================================

    void Start()
    {
        // 現在の状態を移動状態に初期化
        nowMainState = MainStateID.MOVE;

        // 色を取得
        sr_boss = GetComponent<SpriteRenderer>();
        sr_lefthand = GameObject.Find("LeftHand").GetComponent<SpriteRenderer>();
        sr_righthand = GameObject.Find("RightHand").GetComponent<SpriteRenderer>();

        // 初期位置を保存
        init_boss_pos = this.transform.position;
        init_lefthand_pos = transform.Find("LeftHand").gameObject.transform.position;
        init_right_pos = transform.Find("RightHand").gameObject.transform.position;

        // 初期HPを保存
        init_boss_hp = hp;
    }

    //=====================================
    // *** 更新処理 ***
    //=====================================

    void Update()
    {
        //---------------------------------------
        // 状態を更新
        //--------------------------------------

        if (nextMainState != MainStateID.NULL)
        {
            oldMainState = nowMainState;
            nowMainState = nextMainState;
            nextMainState = MainStateID.NULL;
        }

        //---------------------------------------
        // 現在の状態によって処理を分岐
        //---------------------------------------

        switch (nowMainState)
        {
            // 移動状態
            case MainStateID.MOVE:
                Move_CaveBoss.instance.Move();
                RandomMainState();
                break;
            // 攻撃状態
            case MainStateID.ATTACK:
                Attack();
                break;
            // 被弾状態
            case MainStateID.DAMAGE:
                Damage();
                break;
            // 戦闘不能
            case MainStateID.DEATH:
                Death();
                break;
        }
    }

    //============================================================
    // *** 衝突判定 ***
    //============================================================

    void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------------
        // 被弾状態以外でひびと衝突したら被弾状態に遷移
        //--------------------------------------------------------

        if (nowMainState != MainStateID.DAMAGE)
        {
            if (collision.gameObject.tag == "Crack")
            {
                // 被弾状態に遷移
                nextMainState = MainStateID.DAMAGE;

                // 衝突したひびを破棄
                Destroy(collision.gameObject);
            }
        }
    }

    //=====================================
    // *** 攻撃処理 ***
    //=====================================

    void Attack()
    {
        //---------------------------------------
        // *** 現在の状態によって処理を分岐 ***

        if (nextAttackState != AttackStateID.NULL)
        {
            oldAttackState = nowAttackState;
            nowAttackState = nextAttackState;
            nextAttackState = AttackStateID.NULL;
        }

        switch (nowAttackState)
        {
            // 敵を降らせる
            case AttackStateID.ENEMY_DROP:
                isEndState = EnemyDrop_CaveBoss.instance.EnemyDrop();
                break;
            // プレイヤーを掴む
            case AttackStateID.GRIP_PLAYER:
                isEndState = GripPlayer_CaveBoss.instance.GripPlayer();
                break;
        }

        //---------------------------------------
        // *** 攻撃が終了したら通常状態に戻す ***

        if (isEndState == true)
        {
            nextMainState = MainStateID.MOVE;
            isEndState = false;
        }
    }

    //=============================================
    // *** ランダムにメイン行動を決定する処理 ***
    //=============================================

    void RandomMainState()
    {
        //---------------------------------
        // *** ランダムに次の行動を決定 ***

        mainStateDelay_Cnt++;

        if (mainStateDelay_Cnt >= mainStateDelay)
        {
            //---------------------------------
            // 敵を降らせる

            int rnd = Random.Range(1, 100 + 1);

            if (rnd > 50)
            {
                nextMainState = MainStateID.ATTACK;
                nextAttackState = AttackStateID.ENEMY_DROP;
            }

            //---------------------------------
            // プレイヤーを捕まえる

            else
            {
                nextMainState = MainStateID.ATTACK;
                nextAttackState = AttackStateID.GRIP_PLAYER;
            }

            mainStateDelay_Cnt = 0;
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
        angle += 10.0f;

        //-------------------------------------------------------------------
        // 徐々に透明にする。完全に透明になったらこのオブジェクトを破棄する
        //-------------------------------------------------------------------

        // αの値を減らす
        sr_boss.color = new Color(sr_boss.color.r, sr_boss.color.g, sr_boss.color.b, sr_boss.color.a - 0.002f);
        sr_lefthand.color = new Color(sr_boss.color.r, sr_boss.color.g, sr_boss.color.b, sr_boss.color.a - 0.002f);
        sr_righthand.color = new Color(sr_boss.color.r, sr_boss.color.g, sr_boss.color.b, sr_boss.color.a - 0.002f);

        // αが0になったらこのオブジェクトを破棄
        if (sr_boss.color.a < 0.0f)
        {
            // このオブジェクトを破棄
            Destroy(gameObject);
        }
    }

    //===========================================
    // *** 被弾状態の処理 ***
    //===========================================

    void Damage()
    {
       
        // 無敵時間をカウント
        invincible_time_cnt++;

        //---------------------------------------
        // HPを1減らす。0なら戦闘不能状態に遷移
        //---------------------------------------

        if (invincible_time_cnt == 1)
        {
            // HPを1減らす
            hp--;

            // 0なら戦闘不能状態に遷移
            if (hp <= 0)
            {
                // 色をデフォルトに戻す
                sr_boss.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                sr_lefthand.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                sr_righthand.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                // 戦闘不能状態に遷移
                nextMainState = MainStateID.DEATH;

                // エフェクトを再生
                Instantiate(effect, transform.position, Quaternion.identity);

                // 回転の中心座標に現在位置を保存
                center = transform.position;
            }
        }

        //---------------------------------------
        // 無敵時間内ならボスを赤色に変更
        //---------------------------------------

        if (invincible_time_cnt < invincible_time)
        {
            // 赤色に変更
            sr_boss.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            sr_lefthand.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            sr_righthand.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        //--------------------------------------------
        // 無敵時間が終了したならボスを元の状態に戻す
        //--------------------------------------------

        else
        {
            // 無敵時間のカウントをリセット
            invincible_time_cnt = 0;

            // 色をデフォルトに戻す
            sr_boss.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            sr_lefthand.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            sr_righthand.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            // ダメージを受ける前の状態に戻す
            nextMainState = oldMainState;
        }

    }

    //===========================================
    // *** 初期化処理 ***
    //===========================================

    public void Init()
    {
        // hpを初期化
        hp = init_boss_hp;

        // 座標を初期化
        this.transform.position = init_boss_pos;
        transform.Find("LeftHand").gameObject.transform.position = init_lefthand_pos;
        transform.Find("RightHand").gameObject.transform.position = init_right_pos;

        // 現在の状態を移動状態に初期化
        nowMainState = MainStateID.MOVE;

        // 角度を初期化
        angle = 0.0f;
    }
}