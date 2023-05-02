//---------------------------------------------------------
//担当者：二宮怜
//内容　：敵AI(町雑魚)
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    public bool Stop = false; // デバッグ用 敵がその場にとどまる

    enum EnemyDirection
    {
        LEFT,
        RIGHT
    }

    private EnemyDirection Direction = EnemyDirection.RIGHT;

    // レイの衝突判定結果用変数
    RaycastHit2D hit;
    public bool test;
    // レイ調整基準値：正　右にずらす,　負　左にずらす
    private float Scale;
    // レイの位置調整
    public float AdjustX = 0f;
    public float AdjustY = 0f;

    // 敵の巡回開始位置
    private Vector3 start;
    // 敵の移動先
    private Vector3 target;
    //// 敵の移動方向
    //private bool Outbound = true; // true:往路 false:復路
    [Header("敵の移動範囲")]
    public float MoveArea = 5.0f; // 敵の移動範囲
    // 移動距離の割合を表す 0から1
    private float timer;
    private float sizeX; // ローカルサイズ保存

    // 敵のプレイヤーサーチ変数
    private float SubX;    // 求めたX座標の差を保持する変数
    private float SubY;    // 求めたY座標の差を保持する変数
    public float Distance; // 求めた距離を保持する変数
    [Header("プレイヤーを感知する範囲")]
    public float senserDistance = 6.0f; // 判定をとる範囲

    // 敵が攻撃を始める距離
    [Header("攻撃モーションに入る距離")]
    public float attackDistance = 2.0f; // 攻撃する距離

    // 攻撃用変数
    private float AttackStartPositionX; // 攻撃状態になった時の座標
    [Header("一回の攻撃のループに必要な時間")]
    [SerializeField]private float OneAttackNeedTime = 0.5f; // 一回の攻撃のループに必要な時間
    private float AttackTimer = 0f; // 攻撃が始まってからの経過時間(攻撃1ループごとに初期化)
    [Header("攻撃が届く距離")]
    [SerializeField]private float AttackDistance = 0.5f; // 攻撃が届く距離
    private float AttackSign = 1f; // 正か負か
    private bool Return = false;
    private float NowEnemyPos; // 攻撃時の座標変化の結果用変数
    [Header("攻撃後の硬直時間")]
    [SerializeField] private float AttackWaitTime = 0.3f; // 攻撃後の待機時間
    private float AttackWaitTimer = 0f;

    // コルーチン関係
    private bool InCoroutine = false; // コルーチンに入ったか
    private bool AttackStart = false; // 攻撃にディレイを掛ける
    [Header("攻撃を何秒おくらせるか")]public float InCoroutineWaitTime = 0.5f; // 何秒遅らせるか

    // 死亡時の処理が終わったかを持つ変数
    private bool death = false;


    [Header("プレイヤーを追いかける速度")]
    public float TrackingSpeed = 3.0f; // 追跡スピード

    public enum AIState
    {
        INIT_PATROL,   // 巡回準備
        PATROL,        // 巡回
        INIT_TRACKING, // 追跡準備
        TRACKING,      // 追跡
        INIT_ATTACK,   // 攻撃準備
        ATTACK,        // 攻撃
        ATTACK_WAIT,   // 攻撃待機
        DEATH,         // 死亡
    }

    // 敵行動状態
    public AIState EnemyAI = AIState.INIT_PATROL;

    private CircleCollider2D circleCol;
    private float ColRadius;

    // 外部取得
    private Transform thisTransform; // このオブジェクトの座標を持つ変数
    private GameObject player; // プレイヤーのゲームオブジェクト探す用
    private Transform playerTransform;
    private HammerNail hammer; // HammerNailを取得
    private GameObject Child; // 敵自身の子オブジェクト


    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // オブジェクトのTransformを取得
        thisTransform = GetComponent<Transform>();

        // プレイヤーオブジェクト探す
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        sizeX = thisTransform.localScale.x;

        // Hammerスクリプト取得
        hammer = player.GetComponent<HammerNail>();


        // 子オブジェクト取得
        Child = transform.Find("HitCollider").gameObject;

        // コライダー取得
        circleCol = GetComponent<CircleCollider2D>();
        ColRadius = circleCol.radius;
        AdjustX = ColRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (Stop == false)
        {
            // 一定範囲内にプレイヤーが侵入してきたらステータス変化
            // プレイヤーとの距離をもとめる
            var Vec = thisTransform.position - playerTransform.position;

            // 三平方の定理
            Distance = Vec.magnitude; // プレイヤーとの距離が求まった

            if (gameObject.name == "Enemy")
            {
                //Debug.Log(Distance);
                //Debug.Log(senserDistance);
                //Debug.Log(playerTransform.position);
                //Debug.Log(thisTransform.position);
            }

            // 向いている方向をセット
            SetDirection();

            // 進行方向にレイを飛ばす
            CreateRay();

            if (death == false)
            {
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

                    case AIState.ATTACK_WAIT:
                        Attack_Wait();
                        break;

                    case AIState.DEATH:
                        Death();
                        break;
                }
            }
        }
        else
        {
            // 向いている方向をセット
            SetDirection();

            // 進行方向にレイを飛ばす
            CreateRay();
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

            Debug.Log("AAAAAAAAAAAAAAA");
        }
        // レイの衝突による再設定
        else
        {
            // 右壁との衝突後
            if (Direction == EnemyDirection.RIGHT)
            {
                // 敵の開始位置決定
                start = new Vector3(thisTransform.position.x - MoveArea, thisTransform.position.y, 0.0f);
                // 敵の目的地
                target = thisTransform.position;
                // 初期化
                timer = 1.0f;

                // 左向きにする
                Direction = EnemyDirection.LEFT;
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

                // 右向きにする
                Direction = EnemyDirection.RIGHT;
            }
        }

        EnemyAI = AIState.PATROL;
    }
    void Patrol()
    {
        // 2秒で移動方向変更

        // 右に移動しているなら
        if (Direction == EnemyDirection.RIGHT)
        {
            timer += Time.deltaTime / 2;

            if (timer >= 1.0f)
            {
                Direction = EnemyDirection.LEFT;
            }
        }
        // 左に移動しているなら
        else
        {
            timer -= Time.deltaTime / 2;

            if (timer <= 0.0f)
            {
                Direction = EnemyDirection.RIGHT;
            }
        }

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

            Debug.Log("BBBBBBBBBBBBBB");
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

            Debug.Log("CCCCCCCCCCCCCC");
            // パトロールの時から向きが変わるなら（敵の後ろから索敵範囲に入ったら）
            if (thisTransform.position.x < playerTransform.position.x)
            {
                Direction = EnemyDirection.RIGHT;
            }
            else if (thisTransform.position.x > playerTransform.position.x)
            {
                Direction = EnemyDirection.LEFT;
            }

            // 壁にレイが接触したら追わない
            if (!hit)
            {
                // プレイヤーに向かって進む
                // プレイヤーが敵自身より右にいるなら
                if (thisTransform.position.x < playerTransform.position.x)
                {
                    thisTransform.Translate(TrackingSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
                // プレイヤーが敵自身より左にいるなら
                else if (thisTransform.position.x > playerTransform.position.x)
                {
                    thisTransform.Translate(-1 * TrackingSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
            }

            // 近づきすぎたら
            if (Distance < attackDistance)
            {
                // 攻撃状態に変化
                EnemyAI = AIState.INIT_ATTACK;
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
        // 体当たり準備

        // プレイヤーが敵自身より右にいるなら
        if (thisTransform.position.x < playerTransform.position.x)
        {
            AttackSign = 1f;
        }
        // プレイヤーが敵自身より左にいるなら
        else if (thisTransform.position.x > playerTransform.position.x)
        {
            AttackSign = -1f;
        }

        // 攻撃になった時のX座標を取得
        AttackStartPositionX = NowEnemyPos = thisTransform.localPosition.x;

        // 初期化
        AttackTimer = 0f;

        EnemyAI = AIState.ATTACK;

    }
    void Attack()
    {
        // タックル

        // 最初のフレームでディレイかける
        if(InCoroutine == false)
        {
            StartCoroutine(WaitTimer());
        }

        if (AttackStart == true)
        {
            if (Return == false)
            {
                // 座標計算
                NowEnemyPos += AttackSign * AttackDistance * Time.deltaTime * 2f;

                // AttackTimerの値によって座標変化
                thisTransform.localPosition = new Vector3(
                    NowEnemyPos,
                    thisTransform.localPosition.y,
                    thisTransform.localPosition.z);
            }
            else
            {
                // 座標計算
                // 0に近づいていく
                NowEnemyPos -= AttackSign * AttackDistance * Time.deltaTime * 2f;

                // 元の位置を通り過ぎないようにする
                if (AttackSign * NowEnemyPos < 0)
                {
                    NowEnemyPos = AttackStartPositionX;
                }

                thisTransform.localPosition = new Vector3(
                    NowEnemyPos,
                    thisTransform.localPosition.y,
                    thisTransform.localPosition.z);
            }

            AttackTimer += Time.deltaTime;

            //  時間経過でもとの地点に戻るようの変数
            if (AttackTimer > OneAttackNeedTime / 2)
            {
                Return = true;
            }

            // 一回の攻撃ループにかかる時間が経過したら
            if (AttackTimer > OneAttackNeedTime)
            {
                // 繰り返し
                EnemyAI = AIState.ATTACK_WAIT;

                // 初期化
                Return = false;
                InCoroutine = false;
                AttackStart = false;
            }
        }

        if (Distance > attackDistance)
        {
            //EnemyAI = AIState.TRACKING;
        }
    }

    private void Attack_Wait()
    {
        // 待機
        AttackWaitTimer += Time.deltaTime;

        // 一定時間経過したら
        if(AttackWaitTimer > AttackWaitTime)
        {
            // 攻撃に移行
            EnemyAI = AIState.INIT_ATTACK;

            AttackWaitTimer = 0;
        }

        if (Distance > attackDistance)
        {
            EnemyAI = AIState.TRACKING;

            AttackWaitTimer = 0;
        }
    }

    private void Death()
    {
        // 死亡状態

        if (death == false) 
        {
            // プレイヤーとの当たり判定を
            Child.GetComponent<CircleCollider2D>().enabled = false;

            death = true;
        }
    }

    IEnumerator WaitTimer()
    {
        InCoroutine = true;
        // 指定時間待機
        yield return new WaitForSeconds(InCoroutineWaitTime);

        AttackStart = true;
    }

    private void CreateRay()
    {
        // 進行方向にレイを飛ばして壁にぶつかったら進行方向を変える
        Vector2 origin = new Vector2(
            thisTransform.position.x + AdjustX,
            thisTransform.position.y + AdjustY
            );

        // レイを飛ばす方向
        Vector2 RayDirection = Vector2.zero;

        // ボスの向きによってレイをとばす方向が変化
        switch (Direction)
        {
            case EnemyDirection.LEFT:
                RayDirection = new Vector2(-1, 0); // 左向き
                break;

            case EnemyDirection.RIGHT:
                RayDirection = new Vector2(1, 0); // 右向き
                break;
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
        test = hit;

        // 描画
        Debug.DrawRay(origin, distance, Color.red);
        //-----------------------------------------------------------------------------------------------
    }

    private void SetDirection()
    {
        if (Direction == EnemyDirection.LEFT)
        {
            // 左向き
            thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            AdjustX = -ColRadius * 0.5f;
        }
        else
        {
            // 右向き
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            AdjustX = ColRadius * 0.5f;
        }
    }
}
