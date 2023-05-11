//---------------------------------------------------------
//担当者：二宮怜
//内容　：敵AI(プラント場雑魚)
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemyMove : MonoBehaviour
{
    // 変数宣言

    // 敵が向いている方向
    enum EnemyDirection
    {
        LEFT,
        RIGHT
    }

    private EnemyDirection Direction = EnemyDirection.RIGHT;

    // 敵がどちらのパイプにいるか
    enum WhichPipe
    {
        Pipe1,
        Pipe2, 
    }

    // 初期状態はパイプ1の中
    [SerializeField]private WhichPipe whichPipe = WhichPipe.Pipe1;

    private float sizeX; // ローカルサイズ保存

    // 敵のプレイヤーサーチ変数
    private float SubX;    // 求めたX座標の差を保持する変数
    private float SubY;    // 求めたY座標の差を保持する変数
    private float Distance; // 求めた距離を保持する変数

    public enum AIState
    {
        Idle,         // アイドル
        Pre_Attack,   // 攻撃準備
        Attack,       // 攻撃
        Attack_Wait,  // 攻撃後の待機
        Firing,       // 飛び出す
        Confusion,    // 混乱
        Rage,         // じたばた
        Death,        // 死亡
    }

    // 敵行動状態
    public AIState EnemyAI = AIState.Idle;

    // プレイヤーが索敵範囲にいるか
    public bool PlayerHit = false;

    // 攻撃用変数
    
    public float Timer = 0f; // タイマー
    private GameObject TargetObject; // 飛び出した先のオブジェクト
    private Vector3 TargetPosition; // 目標地点
    private Vector3 vector; // 正規化したベクトル

    [Header("予備動作から攻撃までの待機時間(s)")] public float AttackWaitTime = 1.0f; // 顔を出してから攻撃に入るまでの待ち時間
    [Header("攻撃してから次の予備動作に移るまでの時間(s)")] public float AttackIntervalTime = 1.0f; // 攻撃してから次の攻撃に移るまでの時間
    [Header("予備動作時のスピード")]public float PreSpeed = 3f; // 予備動作時の移動速度
    [Header("攻撃モーション時スピード")]public float AttackSpeed = 10f; // 攻撃するときの移動速度

    // 被衝撃時用変数
    public bool CrackInPipe = false; // パイプにひびがあたったか
    private Rigidbody2D rigid2D; // 敵自身のrigidbody2D
    [Header("射出時に加える力")] public float ImpulsePower = 5f; // 射出時に加える力
    [System.NonSerialized]public GameObject CrackInObject = null; // ひびがあたったパイプを保存する
    private Vector3 CenterBetweenPipes; // パイプ間の中心座標
    [Header("射出してから混乱状態になるまでの時間(s)")]public float FiringTime = 0.5f;
    private float Adjustment; // 射出時の目標位置から少し遠ざける
    [SerializeField] private float RageArea = 1f;
    private Vector3 RageStartPos; // じたばた開始時の初期座標
    private float RageCount = 0f;
    [SerializeField] private float JumpInterval = 1.5f;
    private float RageImpulsePower = 2f;

    //マテリアル
    private Material mat;

    // 外部取得
    private Transform thisTransform; // このオブジェクトの座標を持つ変数
    private GameObject player; // プレイヤーのゲームオブジェクト探す用
    private Transform playerTransform; // プレイヤーの座標

    [Header("この敵が所属するPipeEnemyGroupを入れてね")]public GameObject Parent; // 親オブジェクト
    private GameObject Pipe_1; // パイプ1
    private GameObject Pipe_2; // パイプ2
    private GameObject Child; // 敵自身の子オブジェクト

    private void Init()
    {
        // 初期化

        Timer = 0f;
        RageCount = 0;
        EnemyAI = AIState.Idle;
        whichPipe = WhichPipe.Pipe1;
    }

    // Start is called before the first frame update
    void Start()
    {
        // 行き来するパイプのゲームオブジェクト取得
        Pipe_1 = Parent.transform.GetChild(0).gameObject;
        Pipe_2 = Parent.transform.GetChild(1).gameObject;

        // プレイヤー取得
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        transform.localPosition = Pipe_1.transform.localPosition;

        // 自身のトランスフォーム保持
        thisTransform = transform;

        sizeX = thisTransform.localScale.x;

        // rigidbody取得
        rigid2D = GetComponent<Rigidbody2D>();

        // パイプ間の中心座標を取得
        if (Pipe_1.transform.localPosition.x > Pipe_2.transform.localPosition.x) 
        {
            CenterBetweenPipes = (Pipe_1.transform.localPosition - Pipe_2.transform.localPosition) / 2f;
        }
        else
        {
            CenterBetweenPipes = (Pipe_2.transform.localPosition - Pipe_1.transform.localPosition) / 2f;
        }

        // 子オブジェクト取得
        Child = transform.Find("HitCollider").gameObject;

        // マテリアル取得
        mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // 一定範囲内にプレイヤーが侵入してきたらステータス変化
        // プレイヤーとの距離をもとめる
        SubX = thisTransform.localPosition.x - playerTransform.position.x; // x差
        SubY = thisTransform.localPosition.y - playerTransform.position.y; // y差

        // 三平方の定理
        Distance = SubX * SubX + SubY * SubY; // プレイヤーとの距離が求まった

        // 敵の向いている方向をセット
        SetDirection();

        switch (EnemyAI)
        {
            // アイドル
            case AIState.Idle:
                Idle();
                break;

            // 攻撃準備
            case AIState.Pre_Attack:
                Pre_Attack();
                break;

            // 攻撃
            case AIState.Attack:
                Attack();
                break;

            // 攻撃後の隙
            case AIState.Attack_Wait:
                Attack_Wait();
                break;

            // 飛び出してくる
            case AIState.Firing:
                Firing();
                break;

            // 混乱
            case AIState.Confusion:
                Confusion();
                break;

            // じたばた
            case AIState.Rage:
                Rage();
                break;

            // 撃破
            case AIState.Death:
                Death();
                break;
        }


        //Debug.Log(Pipe_1.transform.localPosition);
        //Debug.Log(Pipe_2.transform.localPosition);
        //Debug.Log(CenterBetweenPipes);
    }

    private void Idle()
    {
        if(Timer != 0f)
        {
            Timer = 0f;
        }

        // 自分がいるパイプの中心に向かう
        if(whichPipe == WhichPipe.Pipe1)
        {
            // 移動
            thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, Pipe_1.transform.localPosition, Time.deltaTime);
        }
        else
        {
            // 移動
            thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, Pipe_2.transform.localPosition, Time.deltaTime);
        }

        // プレイヤーが索敵範囲に入ったら
        if(PlayerHit == true)
        {
            EnemyAI = AIState.Pre_Attack;
        }

        // 遷移判断関数
        JudgeFiring();
    }

    private void Pre_Attack()
    {
        if (Timer == 0f)
        {
            SetTargetPipe();
        }

        // 頭をひょこっと出すくらいまで座標変更
        thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, TargetPosition, Time.deltaTime * PreSpeed);

        // カウント
        Timer += Time.deltaTime;

        // 指定時間経過したら
        if (Timer >= AttackWaitTime)
        {
            EnemyAI = AIState.Attack;

            // 初期化
            Timer = 0f;
        }

        // 遷移判断関数
        JudgeFiring();
    }

    private void Attack()
    {
        // 最初のみ入る
        if (Timer == 0f)
        {
            // 飛び出していくパイプの座標を取得
            TargetPosition = new Vector3(
                    TargetObject.transform.localPosition.x,
                    TargetObject.transform.localPosition.y,
                    0f);
        }

        // 目標のパイプの位置まで移動
        thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, TargetPosition, Time.deltaTime * AttackSpeed);

        // カウント
        Timer += Time.deltaTime;

        // 目標地点に到達したら
        if (thisTransform.localPosition == TargetPosition)
        {
            // インターバル入る
            EnemyAI = AIState.Attack_Wait;

            // 初期化
            Timer = 0f;
        }
    }

    private void Attack_Wait()
    {
        // 敵の持つデータ更新
        if(Timer == 0f)
        {
            // 自分のいるパイプを切り替え
            if (whichPipe == WhichPipe.Pipe1)
            {
                whichPipe = WhichPipe.Pipe2;
            }
            else
            {
                whichPipe = WhichPipe.Pipe1;
            }

            // 向き切り替え
            if (Direction == EnemyDirection.LEFT)
            {
                Direction = EnemyDirection.RIGHT;
            }
            else
            {
                Direction = EnemyDirection.LEFT;
            }
        }

        // 攻撃インターバル中にプレイヤーが索敵範囲から外れたら
        if(Timer != 0f && PlayerHit == false)
        {
            // アイドル状態に戻る
            EnemyAI = AIState.Idle;
        }

        // カウント
        Timer += Time.deltaTime;

        // インターバルタイムが経過したら
        if(Timer >= AttackIntervalTime)
        {
            EnemyAI = AIState.Pre_Attack;

            // 初期化
            Timer = 0f;
        }

        // 遷移判断関数
        JudgeFiring();
    }

    private void Firing()
    {
        if(Timer == 0f)
        {
            // 目標地点をずらすための値作り
            if(whichPipe == WhichPipe.Pipe1)
            {
                Adjustment = CenterBetweenPipes.x * 0.2f;
            }
            else
            {
                Adjustment = -CenterBetweenPipes.x * 0.2f;
            }
        }

        // 指定時間が経過してから重力の影響を与える
        if(Timer >= 0.1f)
        {
            rigid2D.gravityScale = 1f;
        }

        // 最終目標位置
        var target = new Vector3(
            CenterBetweenPipes.x + Adjustment,
            CenterBetweenPipes.y,
            CenterBetweenPipes.z
            );

        // y座標は自由落下
        var PositionY = thisTransform.localPosition.y;

        // パイプの中心まで移動
        thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, target, Time.deltaTime * ImpulsePower);

        // y座標を正しいものに置き換える
        thisTransform.localPosition = new Vector3(
            thisTransform.localPosition.x,
            thisTransform.localPosition.y,
            thisTransform.localPosition.z
            );

        Timer += Time.deltaTime;

        // 一定時間たったら
        if (Timer >= FiringTime)
        {
            // 混乱状態へ
            EnemyAI = AIState.Confusion;
            Timer = 0f;
        }
    }

    private void Confusion()
    {
        // 一定時間たったら
        Timer += Time.deltaTime;
        if(Timer > 2f)
        {
            // じたばたさせる
            EnemyAI = AIState.Rage;

            // じたばた開始時の座標
            RageStartPos = thisTransform.position;

            RageCount = 0f;
            Timer = 0f;
        }
    }

    private void Rage()
    {
        // 暴れ始めた地点を中心に
        thisTransform.position = new Vector3(RageStartPos.x + Mathf.Sin(Timer) / 2f, thisTransform.position.y,RageStartPos.z);

        if(RageCount == 0f)
        {
            // 上方向のに跳ねる
            rigid2D.AddForce(new Vector3(0f, 1f * RageImpulsePower, 0f), ForceMode2D.Impulse);
        }

        // インパルス用
        RageCount += Time.deltaTime;
        if(RageCount > JumpInterval)
        {
            RageImpulsePower = Random.Range(2, 4); // パワーはランダム
            RageCount = 0f;
        }

        Timer += Time.deltaTime;
    }

    private void Death()
    {
        // プレイヤーとの当たり判定をけす
        Child.GetComponent<CircleCollider2D>().enabled = false;

        // 重力がついてないならつける
        if(rigid2D.gravityScale == 0f)
        {
            rigid2D.gravityScale = 1f;
        }
    }

    private void SetDirection()
    {
        if (Direction == EnemyDirection.LEFT)
        {
            // 左向き
            thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
        }
        else
        {
            // 右向き
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
        }
    }

    // 次に向かうターゲットを設定
    private void SetTargetPipe()
    {
        // 反対側のパイプを目的にする
        if (whichPipe == WhichPipe.Pipe1)
        {
            // パイプ2を目的地にする
            TargetObject = Pipe_2;
        }
        else
        {
            // パイプ1を目的地にする
            TargetObject = Pipe_1;
        }

        // 現在地から目的地までのベクトルを取得
        var Vector_TargetEnemy = TargetObject.transform.localPosition - thisTransform.localPosition;

        // ベクトルを正規化
        vector = Vector_TargetEnemy.normalized;

        TargetPosition = new Vector3(
            thisTransform.localPosition.x + vector.x * 1.0f,
            thisTransform.localPosition.y + vector.y * 1.0f,
            0f);
    }

    // Firing状態に遷移するか判断する関数
    private void JudgeFiring()
    {
        // ひびが敵自身のいるパイプにあたったら
        if (CrackInPipe == true)
        {
            // 自身がいるパイプがパイプ1のときひびが当たったパイプもパイプ1だったら
            if (whichPipe == WhichPipe.Pipe1 && CrackInObject == Pipe_1)
            {
                EnemyAI = AIState.Firing;
                Timer = 0f;
                //Debug.Log("ジャッジメントですの");

            }
            // 自身がいるパイプがパイプ2のときひびが当たったパイプもパイプ2だったら
            else if (whichPipe == WhichPipe.Pipe2 && CrackInObject == Pipe_2)
            {
                EnemyAI = AIState.Firing;
                Timer = 0f;
                //Debug.Log("ジャッジメントですの");
            }

            CrackInPipe = false;
        }
    }
}

