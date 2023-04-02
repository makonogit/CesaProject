//---------------------------------------------------------
//担当者：二宮怜
//内容　：敵の移動
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    public bool Stop = false; // デバッグ用 敵がその場にとどまる

    // 敵の巡回開始位置
    private Vector3 start;
    // 敵の移動先
    private Vector3 target;
    // 敵の移動方向
    private bool Outbound = true; // true:往路 false:復路
    [Header("敵の移動範囲")]
    public float MoveArea = 5.0f; // 敵の移動範囲
    // 移動距離の割合を表す 0から1
    private float timer;
    // レイの衝突判定結果用変数
    RaycastHit2D hit;
    // レイの位置調整
    float AdjustX;
    private float sizeX; // ローカルサイズ保存

    // 敵のプレイヤーサーチ変数
    private float SubX;    // 求めたX座標の差を保持する変数
    private float SubY;    // 求めたY座標の差を保持する変数
    private float Distance; // 求めた距離を保持する変数
    [Header("プレイヤーを感知する範囲")]
    public float senserDistance = 6.0f; // 判定をとる範囲

    // 敵が攻撃を始める距離
    [Header("攻撃モーションに入る距離")]
    public float attackDistance = 2.0f; // 攻撃する距離

    [Header("プレイヤーを追いかける速度")]
    public float TrackingSpeed = 3.0f; // 追跡スピード

    private enum AIState
    {
        INIT_PATROL,
        PATROL,
        INIT_TRACKING,
        TRACKING,
        INIT_ATTACK,
        ATTACK
    }

    // 敵行動状態
    AIState EnemyAI = AIState.INIT_PATROL;

    // 外部取得
    private Transform thisTransform; // このオブジェクトの座標を持つ変数
    private GameObject player; // プレイヤーのゲームオブジェクト探す用
    private Transform playerTransform;
    private HammerNail hammer; // HammerNailを取得

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // オブジェクトのTransformを取得
        thisTransform = GetComponent<Transform>();

        // プレイヤーオブジェクト探す
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        AdjustX = thisTransform.localScale.x / 2.0f;
        sizeX = thisTransform.localScale.x;

        // Hammerスクリプト取得
        hammer = player.GetComponent<HammerNail>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Stop == false)
        {
            // 一定範囲内にプレイヤーが侵入してきたらステータス変化
            // プレイヤーとの距離をもとめる
            SubX = thisTransform.position.x - playerTransform.position.x; // x差
            SubY = thisTransform.position.y - playerTransform.position.y; // y差

            // 三平方の定理
            Distance = SubX * SubX + SubY * SubY; // プレイヤーとの距離が求まった

            switch (EnemyAI)
            {
                case AIState.INIT_PATROL:
                    Init_Patrol();
                    break;

                case AIState.PATROL:
                    Patrol();
                    break;

                case AIState.TRACKING:
                    Tracking();
                    break;

                case AIState.INIT_TRACKING:
                    Init_Tracking();
                    break;

                case AIState.INIT_ATTACK:
                    Init_Attack();
                    break;

                case AIState.ATTACK:
                    Attack();
                    break;
            }
        }
    }

    void Init_Patrol()
    {
        // レイの衝突が無かった時の再設定
        if (hit == false)
        {
            // 敵の開始位置決定
            start = thisTransform.position;
            // 敵の目的地
            target = new Vector3(thisTransform.position.x + MoveArea, thisTransform.position.y, 0.0f);
            // 初期化
            timer = 0;
        }
        // レイの衝突による再設定
        else
        {
            // 右壁との衝突後
            if (Outbound)
            {
                // 敵の開始位置決定
                start = new Vector3(thisTransform.position.x - MoveArea, thisTransform.position.y, 0.0f);
                // 敵の目的地
                target = thisTransform.position;
                // 初期化
                timer = 1.0f;

                AdjustX = -1 * thisTransform.localScale.x / 2.0f;
                thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z); // 左向き


                Outbound = false;
            }
            // 左壁との衝突後
            else
            {
                // 敵の開始位置決定
                start = thisTransform.position;
                // 敵の目的地
                target = new Vector3(thisTransform.position.x + MoveArea, thisTransform.position.y, 0.0f);
                // 初期化
                timer = 0;

                AdjustX = thisTransform.localScale.x / 2.0f;
                thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z); // 右向き


                Outbound = true;
            }
        }

        EnemyAI = AIState.PATROL;
    }
    void Patrol()
    {
        // 2秒で移動方向変更

        // 右に移動しているなら
        if (Outbound)
        {
            timer += Time.deltaTime / 2;

            if (timer >= 1.0f)
            {
                Outbound = false;
                AdjustX = -AdjustX;

                thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            }
        }
        // 左に移動しているなら
        else
        {
            timer -= Time.deltaTime / 2;

            if (timer <= 0.0f)
            {
                Outbound = true;
                AdjustX = -AdjustX;

                thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);

            }
        }

        //---------------------------------------------------------------------
        // 壁に向かってのレイ

        // 進行方向にレイを飛ばして壁にぶつかったら進行方向を変える
        Vector2 origin = new Vector2(
            thisTransform.position.x + AdjustX,
            thisTransform.position.y
            );

        // レイを飛ばす方向
        Vector2 RayDirection;
        if (Outbound)
        {
            RayDirection = new Vector2(1, 0); // 右向き
        }
        else
        {
            RayDirection = new Vector2(-1, 0); // 左向き
        }

        // 長さ
        float length = 0.1f;
        // 距離
        Vector2 distance = RayDirection * length;
        // 特定のレイヤーのモノとだけ衝突判定をとる
        // レイヤーマスクは二進数を利用
        // 例:layerMask = 1 << 2 は二進数表示で100。上から三つ目のレイヤーとだけという意味
        LayerMask layerMask = 1 << 10; // 左シフト演算、1を<<の右の数だけ左にシフト

        // レイ飛ばしてステージとぶつかったら生成やめる
        hit = Physics2D.Raycast(origin, RayDirection, length, layerMask); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

        Debug.DrawRay(origin, distance, Color.red);
        //-----------------------------------------------------------------------------------------------

        // レイがステージに衝突してなければ移動処理
        if (!hit)
        {
            float positonY = thisTransform.position.y;

            // startとtargetの位置間を移動
            thisTransform.position = Vector3.Lerp(start, target, timer);

            thisTransform.position = new Vector3(thisTransform.position.x, positonY, thisTransform.position.z);
        }
        else
        {
            // 巡回再設定
            EnemyAI = AIState.INIT_PATROL;
        }

        // 一定距離内にプレイヤーがいる
        if (Distance < senserDistance)
        {
            // 追跡準備
            EnemyAI = AIState.INIT_TRACKING;
        }
    }

    void Init_Tracking()
    {
        // 追跡
        EnemyAI = AIState.TRACKING;
    }

    void Tracking()
    {
        // 一定距離内にプレイヤーがいる
        if (Distance < senserDistance)
        {
            // パトロールの時から向きが変わるなら（敵の後ろから索敵範囲に入ったら）
            if (SubX < 0.0f)
            {
                // 向きが変わるなら
                if (Outbound == false)
                {
                    AdjustX = -AdjustX;
                }
                Outbound = true;
            }
            else if (SubX > 0.0f)
            {
                if (Outbound == true)
                {
                    AdjustX = -AdjustX;
                }
                Outbound = false;
            }

            //---------------------------------------------------------------------
            // 壁に向かってのレイ

            // 進行方向にレイを飛ばして壁にぶつかったら進行方向を変える
            Vector2 origin = new Vector2(
                thisTransform.position.x + AdjustX,
                thisTransform.position.y
                );

            // レイを飛ばす方向
            Vector2 RayDirection;
            if (Outbound)
            {
                RayDirection = new Vector2(1, 0); // 右向き
                // 右向く
                thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            }
            else
            {
                RayDirection = new Vector2(-1, 0); // 左向き
                // 左向く
                thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            }

            // 長さ
            float length = 0.1f;
            // 距離
            Vector2 distance = RayDirection * length;
            // 特定のレイヤーのモノとだけ衝突判定をとる
            // レイヤーマスクは二進数を利用
            // 例:layerMask = 1 << 2 は二進数表示で100。上から三つ目のレイヤーとだけという意味
            LayerMask layerMask = 1 << 10; // 左シフト演算、1を<<の右の数だけ左にシフト

            // レイ飛ばしてステージとぶつかったら生成やめる
            hit = Physics2D.Raycast(origin, RayDirection, length, layerMask); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

            Debug.DrawRay(origin, distance, Color.red);

            // 壁にレイが接触したら追わない
            if (!hit)
            {
                // プレイヤーに向かって進む
                // プレイヤーが敵自身より右にいるなら
                if (SubX < 0.0f)
                {
                    thisTransform.Translate(TrackingSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
                // プレイヤーが敵自身より左にいるなら
                else if (SubX > 0.0f)
                {
                    thisTransform.Translate(-1 * TrackingSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
            }

            // 近づきすぎたら
            if (Distance < attackDistance)
            {
                // 攻撃状態に変化
                EnemyAI = AIState.ATTACK;
            }
        }
        // 追跡範囲外にプレイヤーがでたら巡回に戻る
        else
        {
            EnemyAI = AIState.INIT_PATROL;
        }
    }
    void Init_Attack()
    {
        EnemyAI = AIState.ATTACK;
    }
    void Attack()
    {
        if (Distance > attackDistance)
        {
            EnemyAI = AIState.TRACKING;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == NailTag)
    //    {
    //        Debug.Log("tag");

    //        if(hammer.MomentHitNails == true)
    //        {
    //            Destroy(this.gameObject);
    //        }
    //    }
    //}
}
