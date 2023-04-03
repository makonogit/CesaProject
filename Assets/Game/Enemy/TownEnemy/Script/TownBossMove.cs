//---------------------------------------------------------
//担当者：二宮怜
//内容　：1面ボスAI
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBossMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -

    // 敵が向いている方向
    enum EnemyDirection
    {
        LEFT,
        RIGHT
    }

    private EnemyDirection Direction = EnemyDirection.LEFT;

    private float sizeX; // ローカルサイズ保存

    // 敵のプレイヤーサーチ変数
    private float SubX;    // 求めたX座標の差を保持する変数
    private float SubY;    // 求めたY座標の差を保持する変数
    private float Distance; // 求めた距離を保持する変数

    // レイの衝突判定結果用変数
    RaycastHit2D hit;
    // レイ調整基準値：正　右にずらす,　負　左にずらす
    private float halfScale; 
    // レイの位置調整
    float AdjustX;

    // ボスのアクション後の隙時間
    private float waitTimer;

    // 突進用変数
    private float RammingSpeed = 6.0f; // 突進時の移動速度
    private float PreRammingTimer = 0f; // 突進準備時間
    private int SwitchBack; // 切り返し回数

    public enum AIState
    {
        Wait, // 隙
        RammingInit, // 突進準備
        Ramming, // 突進
        RammingWait, // 突進後の隙
        ThrowShards, // かけら飛ばし
        Death, // 撃破
    }

    // 敵行動状態
    public AIState EnemyAI = AIState.Ramming;

    // 外部取得
    private Transform thisTransform; // このオブジェクトの座標を持つ変数
    private GameObject player; // プレイヤーのゲームオブジェクト探す用
    private Transform playerTransform;
    private GameObject child;
    private TownBossHealth BossHealth;

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // オブジェクトのTransformを取得
        thisTransform = GetComponent<Transform>();

        // プレイヤーオブジェクト探す
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        // 自身の子オブジェクト取得
        child = transform.Find("HitCollider").gameObject;
        // ボスの体力スクリプト取得
        BossHealth = child.GetComponent<TownBossHealth>();

        // レイ位置調整
        halfScale = thisTransform.localScale.x / 2.0f;
        AdjustX = halfScale;

        // サイズを保存
        sizeX = thisTransform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // 一定範囲内にプレイヤーが侵入してきたらステータス変化
        // プレイヤーとの距離をもとめる
        SubX = thisTransform.position.x - playerTransform.position.x; // x差
        SubY = thisTransform.position.y - playerTransform.position.y; // y差

        // 三平方の定理
        Distance = SubX * SubX + SubY * SubY; // プレイヤーとの距離が求まった

        // ボスの向いている方向をセット
        SetDirection();

        // 進行方向にレイを飛ばす
        CreateRay();

        switch (EnemyAI)
        {
            case AIState.Wait:
                Wait();
                break;

            case AIState.Ramming:
                Ramming();
                break;

            case AIState.ThrowShards:
                ThrowShards();
                break;
        }
    }

    private void Wait()
    {
        // 攻撃後の後隙
    }

    private void RammingInit()
    {
        // 突進準備
        // 無敵

        // 切り返し回数を求める
        SwitchBack = BossHealth.MaxBossHealth - BossHealth.BossHealth;

    }

    private void Ramming()
    {
        // 突進攻撃
        // 無敵

        float sign = 0.0f; // 符号

        // 向いている方向によって符号が変わる
        if(Direction == EnemyDirection.RIGHT)
        {
            sign = 1f;
        }
        else
        {
            sign = -1f;
        }

        // 移動距離を求める
        float MoveDistance = sign * RammingSpeed * Time.deltaTime; // 一秒間にRammmingSpeed分移動

        // 壁にぶつかるまで突進
        if (hit == false)
        {
            // ボスの現在位置に加算
            thisTransform.Translate(MoveDistance, 0.0f, 0.0f);
        }
        // 壁にぶつかる
        else
        {
            // 切り返し回数が1以上あれば切り返して逆方向に突進
            if (SwitchBack > 0) 
            {
                if(Direction == EnemyDirection.LEFT)
                {
                    Direction = EnemyDirection.RIGHT;
                }
                else
                {
                    Direction = EnemyDirection.LEFT;
                }
            }
            else
            {
                EnemyAI = AIState.RammingWait;
            }
        }
    }

    private void RammingWait()
    {

    }

    private void ThrowShards()
    {
        // かけら飛ばし
    }

    private void CreateRay()
    {
        // 進行方向にレイを飛ばして壁にぶつかったら進行方向を変える
        Vector2 origin = new Vector2(
            thisTransform.position.x + AdjustX,
            thisTransform.position.y
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

        // 描画
        Debug.DrawRay(origin, distance, Color.red);
        //-----------------------------------------------------------------------------------------------
    }

    private void SetDirection()
    {
        if (Direction == EnemyDirection.LEFT) {
            // 左向き
            thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            // レイ調整
            AdjustX = -halfScale;
        }
        else
        {
            // 右向き
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            // レイ調整
            AdjustX = halfScale;
        }
    }
}
