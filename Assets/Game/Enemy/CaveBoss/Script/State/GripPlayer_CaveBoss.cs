//=========================================
// 担当：藤原昂祐
// 内容：洞窟のボスのプレイヤーを掴む攻撃
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripPlayer_CaveBoss : MonoBehaviour
{
    //=====================================
    // *** 変数宣言 ***
    //=====================================

    //-------------------------------------
    // *** インスタンス ***

    public static GripPlayer_CaveBoss instance;// このクラスのインスタンス

    //-------------------------------------
    // *** 状態関連 ***

    enum GripPlayerStateID // 状態ID
    {
        NULL,          // 状態なし
        ACCESS,        // 探索状態
        MOVE,          // 移動状態
        APPEAR,        // 出現状態
        ATTACK,        // 攻撃状態
        GRIP,          // 捕まえた状態
        RETURN,        // 戻り状態
        END            // 行動終了
    }
    GripPlayerStateID oldGripPlayerState = GripPlayerStateID.NULL;  // 前の状態
    GripPlayerStateID nowGripPlayerState = GripPlayerStateID.ACCESS;// 現在の状態
    GripPlayerStateID nextGripPlayerState = GripPlayerStateID.NULL; // 次の状態

    //-------------------------------------
    // *** 攻撃関連 ***

    [Header("右手")]
    public GameObject rightHand;// 右手
    [Header("左手")]
    public GameObject leftHand; // 左手
    [Header("捕まえる時間")]
    public int gripTim = 1000;     // 拘束時間
    [Header("拘束ダメージを受けるまでの時間")]
    public int gripDamageTim = 500;// 拘束ダメージを受けるまでの時間

    int gripTimCnt;             // 拘束時間をカウント
    Vector2 playerGripPos;      // プレイヤーを拘束する座標

    //-------------------------------------
    // *** 移動関連 ***

    [Header("手の移動速度")]
    public float moveSpeed = 0.01f;// 手の移動速度

    Vector2 startPos_R;            // 右手の初期位置
    Vector2 startPos_L;            // 左手の初期位置

    // 大きさ
    Vector3 startScale;

    // 回転
    Vector2 center_R;      // 回転の中心座標
    Vector2 center_L;      // 回転の中心座標
    float angle;         // 回転角度
    float radius = 0.25f;// 円の半径

    //-------------------------------------
    // 出現状態

    [Header("出現速度")]
    public float appearSpeed = 0.01f;

    //-------------------------------------
    // *** 外部オブジェクト ***

    GameObject objPlayer;      // プレイヤー
    private HitEnemy _hitEnemy;// 無敵時間関係スクリプト

    Animator animLeft; // 左手のアニメーター
    Animator animRight;// 右手のアニメーター

    //-------------------------------------
    // マテリアル関連 

    // 色
    SpriteRenderer sr_boss;     // ボスの色
    SpriteRenderer sr_lefthand; // 左手の色
    SpriteRenderer sr_righthand;// 右手の色

    float alpha = 0.0f;// 透明度

    //=====================================
    // *** 初期化処理 ***
    //=====================================

    void Start()
    {

        // 色を取得
        sr_boss = GetComponent<SpriteRenderer>();
        sr_lefthand = GameObject.Find("LeftHand").GetComponent<SpriteRenderer>();
        sr_righthand = GameObject.Find("RightHand").GetComponent<SpriteRenderer>();


        animLeft = GameObject.Find("LeftHand").GetComponent<Animator>();
        animRight = GameObject.Find("RightHand").GetComponent<Animator>();

        //--------------------------------
        // *** 変数の初期化 ***

        // このクラスのインスタンスを生成
        if (instance == null)
        {
            instance = this;
        }

        // プレイヤーのオブジェクトを取得
        objPlayer = GameObject.Find("player");
        _hitEnemy = objPlayer.GetComponent<HitEnemy>();

        startScale = rightHand.transform.localScale;
    }

    //=====================================
    // *** プレイヤーを掴む処理 ***
    //
    // 引数　：無し
    // 戻り値：攻撃が終了したか（true：終了、false：攻撃中）
    //=====================================

    public bool GripPlayer()
    {
        //---------------------------------------
        // *** 現在の状態によって処理を分岐 ***

        if (nextGripPlayerState != GripPlayerStateID.NULL)
        {
            oldGripPlayerState = nowGripPlayerState;
            nowGripPlayerState = nextGripPlayerState;
            nextGripPlayerState = GripPlayerStateID.NULL;
        }

        switch (nowGripPlayerState)
        {

            // 索敵状態
            case GripPlayerStateID.ACCESS:
                Access();
                break;
            // 出現状態
            case GripPlayerStateID.APPEAR:
                Appear();
                break;
            // 移動状態
            case GripPlayerStateID.MOVE:
                Move();
                break;
            // 攻撃状態
            case GripPlayerStateID.ATTACK:
                Attack();
                break;
            // 捕まえた状態
            case GripPlayerStateID.GRIP:
                Grip();
                break;
            // 戻り状態
            case GripPlayerStateID.RETURN:
                Return();
                break;
            // 終了状態
            case GripPlayerStateID.END:
                return End();
                break;
        }

        return false;
    }

    //=====================================
    // *** 索敵処理 ***
    //=====================================

    void Access()
    {
        //-----------------------------------------
        // *** 移動範囲内を左右に反復して移動 ***

        Move_CaveBoss.instance.Move();

        //-----------------------------------------
        // *** Playerが真下に居るかをRayで判定 ***

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, Vector2.down))
        {
            if (hit_view)
            {
                if (hit_view.collider.gameObject.CompareTag("Player"))
                {
                    // 手の初期位置を保存
                    startPos_R = rightHand.transform.position;
                    startPos_L = leftHand.transform.position;

                    // 手を移動状態に変更
                    nextGripPlayerState = GripPlayerStateID.APPEAR;
                }
            }
        }
    }

    //=====================================
    // *** 出現状態 ***
    //=====================================

    void Appear()
    {
        // 透明度を加算
        alpha += appearSpeed;

        // 透明度を敵用
        sr_boss.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        sr_lefthand.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        sr_righthand.color = new Color(1.0f, 1.0f, 1.0f, alpha);

        // 完全に出現したら攻撃状態に遷移
        if (alpha >= 1.0f)
        {
            nextGripPlayerState = GripPlayerStateID.MOVE;
        }
    }

    //=====================================
    // *** 移動処理 ***
    //=====================================

    void Move()
    {
        // 大きさの指定
        Vector3 scale = rightHand.transform.localScale;
        scale.x += 0.001f;
        scale.y += 0.001f;
        rightHand.transform.localScale = scale;
        leftHand.transform.localScale = scale;

        //--------------------------------------------------
        // *** プレイヤーの座標まで移動 ***

        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        position_R.x += moveSpeed * 0.5f;
        position_L.x -= moveSpeed * 0.5f;
        position_R.y -= moveSpeed * 1.5f;
        position_L.y -= moveSpeed * 1.5f;

        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;

        //--------------------------------------------------
        // *** プレイヤーの座標に着いたら攻撃状態にする ***

        if (objPlayer.transform.position.y > position_R.y)
        {
            nextGripPlayerState = GripPlayerStateID.ATTACK;
        }
    }

    //=====================================
    // *** 攻撃処理 ***
    //=====================================

    void Attack()
    {
        // 大きさの指定
        Vector3 scale = rightHand.transform.localScale;
        scale.x += 0.001f;
        scale.y += 0.001f;

        rightHand.transform.localScale = scale;
        leftHand.transform.localScale = scale;

        //--------------------------------------------------
        // *** 手をプレイヤーを挟むように移動する ***

        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        position_R.x -= moveSpeed * 2.0f;
        position_L.x += moveSpeed * 2.0f;

        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;

        //--------------------------------------------------
        // *** プレイヤーを捕まえたかをRayで判定 ***

        if ((objPlayer.transform.position.x < position_L.x + 0.0f) || (objPlayer.transform.position.x > position_R.x + 0.0f))
        {

            nextGripPlayerState = GripPlayerStateID.RETURN;

            // 左手
            foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(leftHand.transform.position, Vector2.right, 2.0f))
            {
             
                if (hit_view)
                {
                    //---------------------------------------------
                    // 捕まえたらGRIP状態にする

                    if (hit_view.collider.gameObject.CompareTag("Player"))
                    {
                        nextGripPlayerState = GripPlayerStateID.GRIP;
                    }
                }
               
            }

            // 右手
            foreach (RaycastHit2D hit_R in Physics2D.RaycastAll(rightHand.transform.position, Vector2.left, 2.0f))
            {

                if (hit_R)
                {
                    //---------------------------------------------
                    // 捕まえたらGRIP状態にする

                    if (hit_R.collider.gameObject.CompareTag("Player"))
                    {
                        nextGripPlayerState = GripPlayerStateID.GRIP;
                    }
                }

            }
        }
            
    }

    //=====================================
    // *** 捕まえる処理 ***
    //=====================================

    void Grip()
    {
        //---------------------------------
        // *** プレイヤーを捕まえる ***

        // 捕まえている時間をカウント
        gripTimCnt++;

        // 捕まえた座標を保存
        if (gripTimCnt == 1)
        {
            animLeft.SetTrigger("Trigger");
            animRight.SetTrigger("Trigger");

            center_L = leftHand.transform.position;
            center_R = rightHand.transform.position;

            playerGripPos = objPlayer.transform.position;
        }

        //---------------------------------------------------
        //  上下にふわふわさせる
        //---------------------------------------------------

        // 現在のトランスフォームを取得
        Vector3 pos = rightHand.transform.position;
        // 角度をラジアンに変換
        float rd = -angle * Mathf.PI / 180.0f;
        // 回転後の座標を計算
        //pos.x = center.x + (Mathf.Sin(rd) * radius) + radius + 0.1f;
        pos.y = center_R.y + (Mathf.Cos(rd) * radius) + radius + 0.1f;
        // 変更を反映
        rightHand.transform.position = pos;
        pos = leftHand.transform.position;
        // 角度をラジアンに変換
        rd = -angle * Mathf.PI / 180.0f;
        // 回転後の座標を計算
        //pos.x = center.x + (Mathf.Sin(rd) * radius) + radius + 0.1f;
        pos.y = center_L.y + (Mathf.Cos(rd) * radius) + radius + 0.1f;
        // 変更を反映
        leftHand.transform.position = pos;
        // 角度を加算
        angle += 0.2f;

        // 移動ステックが動いたらダメージを受けるまでの時間を延長
        if (playerGripPos.x != objPlayer.transform.position.x)
        {
            //gripTimCnt++;
            gripDamageTim++;
        }

        // ダメージを受けるまでの時間が過ぎたらダメージを与える
        if (gripDamageTim < gripTimCnt)
        {
            // 敵とプレイヤーが接触したときの処理関数呼び出し
            _hitEnemy.HitPlayer(objPlayer.transform);
        }

        // 時間が経ったらRETURN状態にする
        if (gripTimCnt > gripTim)
        {
            nextGripPlayerState = GripPlayerStateID.RETURN;
            gripTimCnt = 0;
            
            animRight.SetTrigger("ResetTrigger");
            animLeft.SetTrigger("ResetTrigger");
        }

        // プレイヤーを保存した座標に移動
        objPlayer.transform.position = playerGripPos;
    }

    //=====================================
    // *** 元の位置に戻る処理 ***
    //=====================================

    void Return()
    {
       
        if(startScale.x < rightHand.transform.localScale.x)
        {
            // 大きさの指定
            Vector3 scale = rightHand.transform.localScale;
            scale.x -= 0.0015f;
            scale.y -= 0.0015f;

            rightHand.transform.localScale = scale;
            leftHand.transform.localScale = scale;
        }

        //---------------------------------------
        // *** 手を元の座標に移動 ***

        // 手の座標を取得
        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        // 右手
        if (position_R.x > startPos_R.x)
        {
            position_R.x -= moveSpeed;
        }
        if (position_R.x < startPos_R.x)
        {
            position_R.x += moveSpeed;
        }
        if (position_R.y < startPos_R.y)
        {
            position_R.y += moveSpeed;
        }

        // 左手
        if (position_L.x > startPos_L.x)
        {
            position_L.x -= moveSpeed;
        }
        if (position_L.x < startPos_L.x)
        {
            position_L.x += moveSpeed;
        }
        if (position_L.y < startPos_L.y)
        {
            position_L.y += moveSpeed;
        }

        //---------------------------------------
        // *** 移動が終わったらEND状態にする ***

        else
        {
            nextGripPlayerState = GripPlayerStateID.END;

            
        }

        // 座標の変更を適用
        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;
    }

    //=====================================
    // *** 終了処理 ***
    //=====================================

    bool End()
    {
        // 透明度を減算
        alpha -= appearSpeed;

        // 透明度を敵用
        sr_boss.color = new Color(1.0f, 1.0f, 1.0f,alpha);
        sr_lefthand.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        sr_righthand.color = new Color(1.0f, 1.0f, 1.0f, alpha);

        // 完全に消えたら攻撃状態に遷移
        if (alpha <= 0.0f)
        {
            nextGripPlayerState = GripPlayerStateID.ACCESS;
            return true;
        }

        return false;
    }
}
