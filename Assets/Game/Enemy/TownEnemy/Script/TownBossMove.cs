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

    // レイを伸ばす用のタイマー変数
    private float RayLengthTimer = 0.0f;
    [Header("歩く時のスピード")] public float WalkSpeed = 2.0f;
    [Header("歩く時間")] public float WalkTime = 2.0f;
    private float WalkTimer = 0f;
    bool raycast = false; // レイを飛ばすかどうか

    // プレイヤーに気づく距離
    [SerializeField] private float MoveStartDistance = 120f;
    
    // 突進用変数
    [Header("突進時のスピード")]public float RammingSpeed = 5.0f;     // 突進時の移動速度
    [Header("突進準備時間")] public float PreRammingTime = 3.0f;      // 突進準備時間
    private float PreRammingTimer = 0f;                               // 突進準備タイマー
    private int SwitchBack;                                           // 切り返し回数
    [Header("突進後の後隙")] public float RammingWaitTime = 3.0f;     // 突進後の後隙時間
    private float RammingWaitTimer;                                   // 突進後の隙時間タイマー
    private bool HitPlayer = false;
    [SerializeField] private GameObject SandSmoke; // 突進準備時の砂煙パーティクル
    [SerializeField,Header("座標調整用(左足)")] private Vector2 sandSmokeOffset_left; // 座標調整用(左足)
    private float sandSmokeAdjustX_left;   // ボスの向きで値を変える必要があるため
    [SerializeField,Header("座標調整用(右足)")] private Vector2 sandSmokeOffset_right; // 座標調整用(右足)
    private float sandSmokeAdjustX_right;  // ボスの向きで値を変える必要があるため

    //// かけら飛ばし用変数
    //private float CreateShardsNeedTime = 0.6f;   // かけらを作るのにかかる時間
    //private float ShardCreateTimer;            // かけらを生成し始めてからの経過時間
    //private int CreatedNum = 0; // かけらを作った数
    //public GameObject Shards_Prefab; // かけらのプレハブを持っておく変数
    //private GameObject[] shardObj = new GameObject[18]; // 作成したかけらオブジェクトをいれる配列
    //private GameObject shardParent; // かけらの親オブジェクト
    //private Vector3[] ShardVelocity = new Vector3[18]; // 作ったかけらを移動させるための値を入れる配列
    //private bool AllAddVelocity = false; // 作成した全てのかけらにVelocityを加算したか
    //[Header("かけらの移動スピード")]public float ShardSpeed = 5f;
    //private float ShardWaitTime = 3f; // かけらを飛ばした後の次のモーションまでの待ち時間
    //private float ShardThrowTimer = 0f; // かけらをとばしてからの経過時間
    //private int ShardWaveNum = 0; // なんかいのウェーブがあるか
    //private int NowShardWave = 0; // 今何ウェーブ目か
    //[Header("1ウェーブに何個飛ばすか(最大6個)"),SerializeField] private int CreateShardNum = 6;
    //// 欠片生成時のエフェクト
    //public GameObject ChargeEffect;

    //// かけら配置用変数
    //[Header("何度間隔でかけらを配置するか(初期値10)")]public float SpacingDeg = 10f; // 何度間隔で配置するか
    //private float shardDeg = 0f; // 角度（何度から始まるのか)               
    //private float radius = 2.5f;   // ボスを原点とした円の半径

    //無敵
    //[System.NonSerialized]public bool invincibility = false;
    //[System.NonSerialized] public bool Damaged = false;
    //private GameObject Barrier; // バリアオブジェクト
    //private Material BariMat;   // マテリアル

    // 死亡
    private bool death = false;
    private Directing_BossLight bossDirecting;

    // 初期化用変数
    private Vector3 InitPosition; // 初期位置

    public enum AIState
    {
        None,            // 待機
        Walk,            // 散歩
        WalkInit,        // 散歩準備
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
    private BGMFadeManager _BGMfadeMana;

    public void Init()
    {
        // 座標初期化
        thisTransform.position = InitPosition;

        // 状態セット
        EnemyAI = AIState.None;

        // 向き
        Direction = EnemyDirection.LEFT;

        // 体力最大にする
        BossHealth.BossHealth = BossHealth.MaxBossHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // オブジェクトのTransformを取得
        thisTransform = GetComponent<Transform>();

        // 初期化用の座標保存
        InitPosition = thisTransform.position;

        // プレイヤーオブジェクト探す
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        // 自身の子オブジェクト取得
        Colchild = transform.GetChild(0).gameObject;
        // ボスの体力スクリプト取得
        BossHealth = Colchild.GetComponent<TownBossHealth>();

        // バリアオブジェクト取得
        //Barrier = transform.GetChild(2).gameObject;
        //BariMat = Barrier.GetComponent<SpriteRenderer>().material;
        //BariMat.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        // ShardParent取得
        //shardParent = transform.GetChild(1).gameObject;

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

        // ボス撃破用演出
        bossDirecting = transform.GetChild(3).gameObject.GetComponent<Directing_BossLight>();
        _BGMfadeMana = mainCam.GetComponent<BGMFadeManager>();
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

        //Debug.Log(Distance);

        // ボスの向いている方向をセット
        SetDirection();

        // 進行方向にレイを飛ばす
        CreateRay();

        switch (EnemyAI)
        {
            // 何もしない
            case AIState.None:
                None();
                break;

            // 散歩準備
            case AIState.WalkInit:
                WalkInit();
                break;

            // 散歩
            case AIState.Walk:
                Walk();
                break;

            // 行動抽選
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
                //ThrowShards();
                break;

            // 撃破
            case AIState.Death:
                Death();
                break;
        }

        //Debug.Log(invincibility);
        // バリア描画 そんなものはない
        //DrawBarrier();
    }

    private void Lottery()
    {
        // 次の行動を抽選
        var Action = Random.Range(0, 1);

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

        //Damaged = false;
        //invincibility = false;
    }

    private void RammingInit()
    {
        // 突進準備
        // 無敵
        //invincibility = true;

        // 切り返し回数を求める
        SwitchBack = BossHealth.MaxBossHealth - BossHealth.BossHealth;

        // 突進準備モーション時間分待つ
        PreRammingTimer += Time.deltaTime;

        // 突進準備アニメーションが終わったら
        if (PreRammingTimer > PreRammingTime)
        {
            // 突進開始
            EnemyAI = AIState.Ramming;

            // 初期化
            PreRammingTimer = 0f;
        }
    }

    private void Ramming()
    {
        // 突進攻撃
        // 無敵

        // 壁にぶつかる前にプレイヤーにぶつかったら
        if (HitPlayer == true)
        {
            EnemyAI = AIState.WalkInit;

            // 初期化
            RayLengthTimer = 0f;

            HitPlayer = false;

            // 無敵解除
            //invincibility = false;
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
    }

    private void RammingWait()
    {
        //// 隙（ぴよってる）
        //if (Damaged == false)
        //{
        //    // 無敵解除
        //    invincibility = false;
        //}
        //else
        //{
        //    // ダメージをくらったら無敵
        //    invincibility = true;
        //}

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
        if (RammingWaitTimer > RammingWaitTime)
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
    }

    //Vector3 CreatePos;
    //Quaternion CreateRotate;
    //GameObject effect;
    private void ThrowShardsInit()
    {
        // かけら準備
        // 無敵
        //invincibility = true;

    //    // ボスのHPによってかけら飛ばしの回数が変わる
    //    ShardWaveNum = BossHealth.MaxBossHealth - BossHealth.BossHealth;

    //    if(ShardCreateTimer == 0)
    //    {
    //        // 角度degからラジアンを作成
    //        var rad = (shardDeg + (CreatedNum % CreateShardNum) * SpacingDeg) * Mathf.Deg2Rad;

    //        // ラジアンを用いてsinとcosを求める
    //        var sin = Mathf.Sin(rad);
    //        var cos = Mathf.Cos(rad);

    //        var sign = 0f;
    //        // 向いている方向によって符号が変わる
    //        if (Direction == EnemyDirection.RIGHT)
    //        {
    //            sign = 1f;
    //        }
    //        else
    //        {
    //            sign = -1f;
    //        }

    //        // ボスを中心とした円周上の点を求める
    //        CreatePos = new Vector3(thisTransform.position.x + sign * cos * radius, thisTransform.position.y + AdjustY + sin * radius, 0f);

    //        // そのかけらの回転角度を求める
    //        // 第一引数 回転させたい角度
    //        // 第二引数 回転させたい軸 right,up,forward
    //        CreateRotate = Quaternion.AngleAxis(
    //            (sign *               // 符号（どちら向きに飛ばすのか）
    //            (90f +                // 呼び出す欠片を飛ばす方向にむかせるための角度
    //            SpacingDeg *          // 何度間隔で配置するのか
    //            (CreatedNum % CreateShardNum)))    // そのウェーブの中で何番目に生成されるモノか(1ウェーブ6個)
    //            , Vector3.forward);   // z軸回転させたい

    //        // 中心に集まってくるエフェクト生成
    //        effect = Instantiate(ChargeEffect);
    //        effect.transform.position = CreatePos;

    //        // ボスの中心座標をAdjustYでずらしたのでずらした座標を持っておく
    //        Vector3 AdjustYThisPos = new Vector3(thisTransform.position.x, thisTransform.position.y + AdjustY, thisTransform.position.z);

    //        // 作成したかけらからボスの中心座標までのベクトルを求める
    //        var Vector_Shrad_Boss = CreatePos - AdjustYThisPos;
    //        // もとめたベクトルのオブジェクトの作成番号と配列の添え字が一致するようにベクトルを保存
    //        ShardVelocity[CreatedNum] = Vector_Shrad_Boss;
    //    }

    //    // かけらを生成してからの経過時間
    //    ShardCreateTimer += Time.deltaTime;

    //    if(ShardCreateTimer > CreateShardsNeedTime)
    //    {
    //        // 第一引数 作成するオブジェクトの素となるプレハブ
    //        // 第二引数 作成する位置
    //        // 第三引数 作成するときの角度
    //        // 第四引数 作成するオブジェクトの親オブジェクト
    //        shardObj[CreatedNum] = Instantiate(Shards_Prefab, CreatePos, CreateRotate, shardParent.transform);

    //        // 大きさ調整
    //        shardObj[CreatedNum].transform.localScale = new Vector3(1f, 1f, 1f);

    //        // パーティクルのゲームオブジェクト消す
    //        Destroy(effect);

    //        // 作成数カウント
    //        CreatedNum++;

    //        // 次のかけら作成のため初期化
    //        ShardCreateTimer = 0f;
    //    }

    //    // かけらを作り終えたら
    //    if (CreatedNum >= CreateShardNum + NowShardWave * CreateShardNum)
    //    {
    //        // かけら飛ばしへ
    //        EnemyAI = AIState.ThrowShards;

    //        // 初期化
    //        ShardCreateTimer = 0.0f;
    //    }
    //}

    //private void ThrowShards()
    //{
    //    // かけら飛ばし

    //    if (Damaged == false)
    //    {
    //        // 無敵解除
    //        //invincibility = false;
    //    }

    //    // このAIStateになった最初のフレームのみ入る
    //    if (AllAddVelocity == false)
    //    {
    //        // リジッドボディのvelocityに対応した値を加算
    //        for (int i = 0 + CreateShardNum * NowShardWave; i < CreatedNum; i++)
    //        {
    //            if (shardObj[i] != null)
    //            {
    //                Rigidbody2D rigid2D = shardObj[i].GetComponent<Rigidbody2D>();
    //                rigid2D.velocity = ShardVelocity[i] * ShardSpeed;
    //            }
    //        }
    //    }

    //    // HPが少なくなればなるほどかけらを飛ばす回数が増える
    //    if(NowShardWave < ShardWaveNum)
    //    {
    //        // 次のウェーブに進める
    //        NowShardWave++;

    //        // 次のかけら準備
    //        EnemyAI = AIState.ThrowShardsInit;
    //    }
    //    else
    //    {
    //        AllAddVelocity = true;

    //        ShardThrowTimer += Time.deltaTime;
    //        if (ShardThrowTimer > ShardWaitTime)
    //        {
    //            // 行動抽選
    //            EnemyAI = AIState.Lottery;

    //            // 初期化
    //            for (int i = 0; i < CreatedNum; i++)
    //            {
    //                if (shardObj[i] != null)
    //                {
    //                    // かけらを消去
    //                    Destroy(shardObj[i].gameObject);
    //                    shardObj[i] = null;
    //                }
    //                ShardVelocity[i] = Vector3.zero;
    //            }

    //            CreatedNum = 0;
    //            AllAddVelocity = false;
    //            ShardThrowTimer = 0f;
    //            NowShardWave = 0;
    //        }
    //    }
    }

    private void Death()
    {
        if (death == false)
        {
            // プレイヤーとの当たり判定を
            Colchild.GetComponent<CircleCollider2D>().enabled = false;

            death = true;

            bossDirecting.Flash();

            // ボスBGM小さく
            _BGMfadeMana.SmallBossBGM();

            //// バリア透明にする
            //BariMat.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void None()
    {
        // 一定距離までプレイヤーが近づいたら
        if(Distance < MoveStartDistance)
        {
            EnemyAI = AIState.Lottery;
        }
    }

    private void WalkInit()
    {
        // どちらに移動するかレイを飛ばして決める

        // カウント
        RayLengthTimer += Time.deltaTime;

        // プレイヤーにヒットしてから指定時間待ったら
        if (RayLengthTimer > 1f && raycast == false)
        {
            // レイ作成開始
            raycast = true;
            RayLengthTimer = 0f + Time.deltaTime;
        }

        if (raycast)
        {
            // レイを左右に飛ばす
            // 生成位置
            Vector2 origin = new Vector2(
                thisTransform.position.x,
                thisTransform.position.y + AdjustY
                );

            // レイを飛ばす方向
            Vector2 LeftRay = Vector2.left;
            Vector2 RightRay = Vector2.right;

            // 長さ
            float length = 10f * RayLengthTimer;
            // 距離
            Vector2 DisLeft = LeftRay * length;
            Vector2 DisRight = RightRay * length;

            LayerMask layerMask = 1 << 10; // Groundのみ

            // レイ飛ばしてステージとぶつかったら生成やめる
            bool hit_left = Physics2D.Raycast(origin, LeftRay, length, layerMask);    // 左側方向
            bool hit_right = Physics2D.Raycast(origin, RightRay, length, layerMask);  // 右側方向

            // 描画
            Debug.DrawRay(origin, DisLeft, Color.red);
            Debug.DrawRay(origin, DisRight, Color.blue);

            // レイが先にぶつかった方と反対方向に歩く
            // もし同時にぶつかればランダム
            if (hit_left == true && hit_right == true)
            {
                // ０～１の乱数取得
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    Direction = EnemyDirection.LEFT;
                }
                else
                {
                    Direction = EnemyDirection.RIGHT;
                }
            }
            else if (hit_left == true)
            {
                // 右に歩く
                Direction = EnemyDirection.RIGHT;
            }
            else if (hit_right == true)
            {
                // 左に歩く
                Direction = EnemyDirection.LEFT;
            }

            // もしどちらかがぶつかっていれば状態を遷移
            if (hit_left || hit_right)
            {
                EnemyAI = AIState.Walk;

                // 初期化
                RayLengthTimer = 0f;
                raycast = false;
            }
        }

        //Debug.Log("walkInit");
        //Debug.Log(HitPlayer);     
    }

    private void Walk()
    {
        //if(Damaged == true)
        //{
        //    //invincibility = true;
        //}

        // 指定時間横に歩く
        if(Direction == EnemyDirection.LEFT)
        {
            // 左に移動
            thisTransform.Translate(-WalkSpeed * Time.deltaTime, 0f, 0f);
        }
        else
        {
            // 右に移動
            thisTransform.Translate(WalkSpeed * Time.deltaTime, 0f, 0f);
        }

        WalkTimer += Time.deltaTime;
        if(WalkTimer > WalkTime)
        {
            EnemyAI = AIState.Lottery;

            // 初期化
            WalkTimer = 0f;

            // プレイヤーの位置に向かって攻撃するための処理
            if(playerTransform.position.x > thisTransform.position.x)
            {
                Direction = EnemyDirection.RIGHT;
            }
            else
            {
                Direction = EnemyDirection.LEFT;
            }
        }

        if (hit == true)
        {
            EnemyAI = AIState.Lottery;
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

            // パーティクル位置調整
            sandSmokeAdjustX_left = -sandSmokeOffset_left.x;
            sandSmokeAdjustX_right = -sandSmokeOffset_right.x;
        }
        else
        {
            // 右向き
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            // レイ調整
            AdjustX = -Scale;

            // パーティクル位置調整
            sandSmokeAdjustX_left = sandSmokeOffset_left.x;
            sandSmokeAdjustX_right = sandSmokeOffset_right.x;
        }
    }

    //private void DrawBarrier()
    //{
    //    // 無敵ならバリア描画
    //    if(invincibility == true)
    //    {
    //        BariMat.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //    }
    //    else
    //    {
    //        BariMat.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    //    }
    //}

    public void SetHitPlayer(bool _hit)
    {
        HitPlayer = _hit;
    }

    public void CreateSandSmokeLeft()
    {
         // パーティクル生成
         var obj = Instantiate(SandSmoke);
         // ボスの座標取得
         var pos = GetComponent<Transform>().position;

         // 足元に生成
         obj.transform.position = new Vector3(pos.x + sandSmokeAdjustX_left, pos.y + sandSmokeOffset_left.y, pos.z);
    }

    public void CreateSandSmokeRight()
    {
         // パーティクル生成
         var obj = Instantiate(SandSmoke);
         // ボスの座標取得
         var pos = GetComponent<Transform>().position;

         // 足元に生成
         obj.transform.position = new Vector3(pos.x + sandSmokeAdjustX_right, pos.y + sandSmokeOffset_right.y, pos.z);
    }
}
