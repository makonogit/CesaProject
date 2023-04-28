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
    private float Scale; 
    // レイの位置調整
    float AdjustX;
    float AdjustY;

    // 突進用変数
    private float RammingSpeed = 5.0f;     // 突進時の移動速度
    private float PreRammingTimer = 0f;    // 突進準備時間
    private int SwitchBack;                // 切り返し回数
    private float RammingWaitTimer;        // 突進後の隙時間

    // かけら飛ばし用変数
    private float CreateShardsNeedTime = 0.6f;   // かけらを作るのにかかる時間
    private float ShardCreateTimer;            // かけらを生成し始めてからの経過時間
    private int CreatedNum = 0; // かけらを作った数
    public GameObject Shards_Prefab; // かけらのプレハブを持っておく変数
    private GameObject[] shardObj = new GameObject[18]; // 作成したかけらオブジェクトをいれる配列
    private GameObject shardParent; // かけらの親オブジェクト
    private Vector3[] ShardVelocity = new Vector3[18]; // 作ったかけらを移動させるための値を入れる配列
    private bool AllAddVelocity = false; // 作成した全てのかけらにVelocityを加算したか
    [Header("かけらの移動スピード")]public float ShardSpeed = 5f;
    private float ShardWaitTime = 3f; // かけらを飛ばした後の次のモーションまでの待ち時間
    private float ShardThrowTimer = 0f; // かけらをとばしてからの経過時間
    private int ShardWaveNum = 0; // なんかいのウェーブがあるか
    private int NowShardWave = 0; // 今何ウェーブ目か

    // かけら配置用変数
    [Header("何度間隔でかけらを配置するか(初期値10)")]public float SpacingDeg = 10f; // 何度間隔で配置するか
    private float shardDeg = -15f; // 角度（何度から始まるのか)               
    private float radius = 2.5f;   // ボスを原点とした円の半径

    private bool HitPlayer = false;

    //無敵
    [System.NonSerialized]public bool invincibility = false;

    public enum AIState
    {
        None,            // 待機
        Walk,            // 散歩
        Lottery,         // 行動抽選
        RammingInit,     // 突進準備
        Ramming,         // 突進
        RammingWait,     // 突進後の隙
        ThrowShardsInit, // かけら準備
        ThrowShards,     // かけら飛ばし
        Death,           // 撃破
    }

    // 敵行動状態
    public AIState EnemyAI = AIState.None;

    // 外部取得
    private Transform thisTransform; // このオブジェクトの座標を持つ変数
    private GameObject player; // プレイヤーのゲームオブジェクト探す用
    private Transform playerTransform;
    private GameObject Colchild;
    private TownBossHealth BossHealth;
    private GameObject mainCam;
    private CameraControl2 cameraControl;   //カメラ追従
    private VibrationCamera vibration;

    private Animator anim;

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
        //child = transform.Find("HitCollider").gameObject;
        Colchild = transform.GetChild(0).gameObject;
        // ボスの体力スクリプト取得
        BossHealth = Colchild.GetComponent<TownBossHealth>();

        // ShardParent取得
        shardParent = transform.GetChild(1).gameObject;

        // カメラ探す
        mainCam = GameObject.Find("Main Camera");
        vibration = mainCam.GetComponent<VibrationCamera>();
        cameraControl = mainCam.GetComponent<CameraControl2>();

        // レイ位置調整
        Scale = thisTransform.localScale.x * 1.3f;
        AdjustX = Scale;
        AdjustY = -0.9f;

        // サイズを保存
        sizeX = thisTransform.localScale.x;

        // アニメーション取得
        anim = GetComponent<Animator>();
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
            case AIState.None:
                None();
                break;

            case AIState.Walk:
                Walk();
                break;

            case AIState.Lottery:
                Lottery();
                break;

            // 突進準備
            case AIState.RammingInit:
                RammingInit();
                break;

            // 突進
            case AIState.Ramming:
                Ramming();
                break;

            // 突進後の隙
            case AIState.RammingWait:
                RammingWait();
                break;

            // かけら準備
            case AIState.ThrowShardsInit:
                ThrowShardsInit();
                break;

            // かけら飛ばし
            case AIState.ThrowShards:
                ThrowShards();
                break;

            // 撃破
            case AIState.Death:
                Death();
                break;
        }
    }

    private void Lottery()
    {
        // 次の行動を抽選
        var Action = Random.Range(0, 2);

        switch (Action)
        {
            // 突進
            case 0:
                EnemyAI = AIState.RammingInit;
                break;

            // かけら飛ばし
            case 1:
                EnemyAI = AIState.ThrowShardsInit;
                break;
        }
    }

    private void RammingInit()
    {
        // 突進準備
        // 無敵
        invincibility = true;

        // 切り返し回数を求める
        SwitchBack = BossHealth.MaxBossHealth - BossHealth.BossHealth;

        // 突進準備モーション時間分待つ
        PreRammingTimer += Time.deltaTime;

        // 突進準備アニメーションが終わったら
        if (PreRammingTimer > 3f)
        {
            // 突進開始
            EnemyAI = AIState.Ramming;

            // 初期化
            PreRammingTimer = 0f;
        }

        anim.SetBool("ramminginit", true);
    }

    private void Ramming()
    {
        // 突進攻撃
        // 無敵

        // 壁にぶつかる前にプレイヤーにぶつかったら
        if (HitPlayer == true)
        {
            EnemyAI = AIState.Walk;
        }

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

                // 切り返し回数減らす
                SwitchBack--;
            }
            else
            {
                EnemyAI = AIState.RammingWait;
            }
        }
        anim.SetBool("ramminginit", false);

        anim.SetBool("ramming", true);
    }

    private void RammingWait()
    {
        // 隙（ぴよってる）
        // 無敵解除
        invincibility = false;

        float vibTime = 1f;
        if(RammingWaitTimer == 0)
        {
            cameraControl.enabled = false;
            // 一秒間振動
            vibration.SetVibration(vibTime);
        }

        RammingWaitTimer += Time.deltaTime;

        if(RammingWaitTimer > vibTime)
        {
            cameraControl.enabled = true;
        }

        // 指定時間ぴよったら向きを変えてAI変化
        if (RammingWaitTimer > 3f)
        {
            if (Direction == EnemyDirection.LEFT)
            {
                Direction = EnemyDirection.RIGHT;
            }
            else
            {
                Direction = EnemyDirection.LEFT;
            }

            // 行動抽選
            EnemyAI = AIState.Lottery;

            // 初期化
            RammingWaitTimer = 0;
        }
        anim.SetBool("ramming", false);
    }

    private void ThrowShardsInit()
    {
        // かけら準備
        // 無敵
        invincibility = true;

        // ボスのHPによってかけら飛ばしの回数が変わる
        ShardWaveNum = BossHealth.MaxBossHealth - BossHealth.BossHealth;

        if(ShardCreateTimer == 0)
        {
            // 角度degからラジアンを作成
            var rad = (shardDeg + (CreatedNum % 6) * SpacingDeg) * Mathf.Deg2Rad;

            // ラジアンを用いてsinとcosを求める
            var sin = Mathf.Sin(rad);
            var cos = Mathf.Cos(rad);

            var sign = 0f;
            // 向いている方向によって符号が変わる
            if (Direction == EnemyDirection.RIGHT)
            {
                sign = 1f;
            }
            else
            {
                sign = -1f;
            }

            // ボスを中心とした円周上の点を求める
            Vector3 CreatePos = new Vector3(thisTransform.position.x + sign * cos * radius, thisTransform.position.y + sin * radius, 0f);

            // そのかけらの回転角度を求める
            // 第一引数 回転させたい角度
            // 第二引数 回転させたい軸 right,up,forward
            Quaternion CreateRotate = Quaternion.AngleAxis((sign * (60f + SpacingDeg * (CreatedNum % 6))), Vector3.forward); 

            // 第一引数 作成するオブジェクトの素となるプレハブ
            // 第二引数 作成する位置
            // 第三引数 作成するときの角度
            // 第四引数 作成するオブジェクトの親オブジェクト
            shardObj[CreatedNum] = Instantiate(Shards_Prefab, CreatePos, CreateRotate, shardParent.transform);

            // 作成したかけらからボスの中心座標までのベクトルを求める
            var Vector_Shrad_Boss = CreatePos - thisTransform.position;
            // もとめたベクトルのオブジェクトの作成番号と配列の添え字が一致するようにベクトルを保存
            ShardVelocity[CreatedNum] = Vector_Shrad_Boss;
            // 大きさ調整
            shardObj[CreatedNum].transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // かけらを生成してからの経過時間
        ShardCreateTimer += Time.deltaTime;

        if(ShardCreateTimer > CreateShardsNeedTime)
        {
            // 作成数カウント
            CreatedNum++;

            // 次のかけら作成のため初期化
            ShardCreateTimer = 0f;
        }

        // かけらを作り終えたら
        if (CreatedNum >= 6 + NowShardWave * 6)
        {
            // かけら飛ばしへ
            EnemyAI = AIState.ThrowShards;

            // 初期化
            ShardCreateTimer = 0.0f;
        }

        // アニメーションセット
        anim.SetBool("charge", true);
    }

    private void ThrowShards()
    {
        // かけら飛ばし
        // 無敵解除
        invincibility = false;

        // このAIStateになった最初のフレームのみ入る
        if (AllAddVelocity == false)
        {
            // リジッドボディのvelocityに対応した値を加算
            for (int i = 0 + 6 * NowShardWave; i < CreatedNum; i++)
            {
                Rigidbody2D rigid2D = shardObj[i].GetComponent<Rigidbody2D>();
                rigid2D.velocity = ShardVelocity[i] * ShardSpeed;
            }

            // アニメーションセット
            anim.SetBool("charge", false);
        }

        // HPが少なくなればなるほどかけらを飛ばす回数が増える
        if(NowShardWave < ShardWaveNum)
        {
            // 次のウェーブに進める
            NowShardWave++;

            // 次のかけら準備
            EnemyAI = AIState.ThrowShardsInit;
        }
        else
        {
            AllAddVelocity = true;

            ShardThrowTimer += Time.deltaTime;
            if (ShardThrowTimer > ShardWaitTime)
            {
                // 行動抽選
                EnemyAI = AIState.Lottery;

                // 初期化
                for (int i = 0; i < CreatedNum; i++)
                {
                    // かけらを消去
                    Destroy(shardObj[i].gameObject);
                    shardObj[i] = null;

                    ShardVelocity[i] = Vector3.zero;
                }

                CreatedNum = 0;
                AllAddVelocity = false;
                ShardThrowTimer = 0f;
                NowShardWave = 0;
            }
        }
    }

    private void Death()
    {
        //mat.color = new Color(2f, 2f, 2f);
    }

    private void None()
    {
        // 一定距離までプレイヤーが近づいたら
        if(Distance < 100f)
        {
            EnemyAI = AIState.Lottery;
        }
    }

    private void Walk()
    {
        anim.SetBool("walk", true);

        if (hit == true)
        {
            EnemyAI = AIState.Lottery;
            anim.SetBool("walk", false);
        }
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
            AdjustX = Scale;
        }
        else
        {
            // 右向き
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            // レイ調整
            AdjustX = -Scale;
        }
    }

    public void SetHitPlayer(bool _hit)
    {
        HitPlayer = _hit;
    }
}
