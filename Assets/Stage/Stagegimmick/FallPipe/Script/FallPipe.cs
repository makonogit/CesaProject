//---------------------------------------------------------
//担当者：二宮怜
//内容　：中央のパイプの処理
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPipe : MonoBehaviour
{
    // 変数宣言

    // 当たり判定時の必要タグ名
    private string GroundTag = "Ground";
    private string PipeTag = "Pipe";
    private string PlayerTag = "Player";

    // パイプの状態
    public enum PIPESTATUS
    {
        NotBroken,       // どこも壊れていない
        LeftBroken,      // 左のクリスタルだけ壊れた
        RightBroken,     // 右のクリスタルだけ壊れた
        AllBroken,       // 両方のクリスタルが壊れた
        Fell             // 落ちきった
    }

    // 現在のパイプの状態：初期NotBroken
    [SerializeField]public PIPESTATUS pipeStatus = PIPESTATUS.NotBroken;

    public float WaitFallTimer = 0f;
    [Header("プレイヤーが上に乗ってから落ちるまでの待ち時間")]public float WaitFallTime = 0f;
    private bool OnThePipe = false; // 特定の条件の時にパイプの上にのったか

    // 自身のコンポーネント変数
    private Transform thisTransform;
    private Rigidbody2D rigid2D;

    // 外部取得
    // プレイヤー
    private GameObject player;
    private Transform playerTransform;


    // 親オブジェクト(PipeSet)
    private GameObject Parent;
    // クリスタルマネージャー
    private GameObject CrystalManager;
    // 左のクリスタル
    private GameObject LeftUnionCrystal;
    // 右のクリスタル
    private GameObject RightUnionCrystal;

    private BreakUnionCrystal UnionLeft;
    private BreakUnionCrystal UnionRight;

    private GameObject SEObj;               //SE再生用オブジェクト
    private GimmickPlay_2 PlaySound;     //SE再生用スクリプト


    // Start is called before the first frame update
    void Start()
    {
        // プレイヤー取得
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        // トランスフォーム取得
        thisTransform = GetComponent<Transform>();

        // リジッドボディ取得
        rigid2D = GetComponent<Rigidbody2D>();

        // 親取得
        Parent = transform.parent.gameObject;

        // クリスタルマネージャー取得
        CrystalManager = Parent.transform.GetChild(3).gameObject;
        // 左のクリスタル取得
        LeftUnionCrystal = CrystalManager.transform.GetChild(0).gameObject;
        // 右のクリスタル取得
        RightUnionCrystal = CrystalManager.transform.GetChild(1).gameObject;

        // スクリプト取得
        UnionLeft = LeftUnionCrystal.GetComponent<BreakUnionCrystal>();
        UnionRight = RightUnionCrystal.GetComponent<BreakUnionCrystal>();

        SEObj = GameObject.Find("BlockSE");
        PlaySound = SEObj.GetComponent<GimmickPlay_2>();
    }

    // Update is called once per frame
    void Update()
    {
        // ステータスによって異なる処理を行う
        switch (pipeStatus)
        {
            case PIPESTATUS.NotBroken:
                NotBroken();
                break;

            case PIPESTATUS.LeftBroken:
                LeftBroken();
                break;

            case PIPESTATUS.RightBroken:
                RightBroken();
                break;

            case PIPESTATUS.AllBroken:
                AllBroken();
                break;

            case PIPESTATUS.Fell:
                Fell();
                break;
        }

        if(OnThePipe == true)
        {
            WaitTimeOnPipe();
        }
    }

    private void NotBroken()
    {
        // 片方のクリスタルが壊されたら対応する状態に遷移

        // 壊されたか取得
        bool left = UnionLeft.GetBreak();
        bool right = UnionRight.GetBreak();

        // 左が壊された
        if (left)
        {
            pipeStatus = PIPESTATUS.LeftBroken;
        }

        // 右が壊された
        if (right)
        {
            pipeStatus = PIPESTATUS.RightBroken;
        }

        // いらなくなると思うけど例外を除くため
        if (left && right)
        {
            pipeStatus = pipeStatus = PIPESTATUS.AllBroken;
        }
    }

    private void LeftBroken()
    {
        if (OnThePipe == false)
        {
            // 壊れてない方が壊れたら
            bool right = UnionRight.GetBreak();

            if (right)
            {
                pipeStatus = PIPESTATUS.AllBroken;
            }
        }
    }

    private void RightBroken()
    {
        if (OnThePipe == false)
        {
            bool left = UnionLeft.GetBreak();

            if (left)
            {
                pipeStatus = PIPESTATUS.AllBroken;
            }
        }
    }

    private void AllBroken()
    {
        // 最初のフレームだけ
        if (rigid2D.gravityScale != 1f)
        {
            rigid2D.gravityScale = 1.0f;
            rigid2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Fell()
    {
        // 最初のフレームだけ
        if(rigid2D.gravityScale != 0f)
        {
            // 動かなくする
            rigid2D.gravityScale = 0f;
            rigid2D.constraints = RigidbodyConstraints2D.FreezeAll;
            
            if (!PlaySound.IsPlay())
            {
                PlaySound.PlayerGimmickSE(GimmickPlay_2.GimmickSE2List.PIPEFALL);
            }

        }
    }

    private void WaitTimeOnPipe()
    {
        // 初めの一回のみ
        if(WaitFallTimer == 0f)
        {
            // 消えてない方のクリスタルを破壊
            if (UnionLeft.GetBreak() == false)
            {
                UnionLeft.Func_BreakBlock();
            }
            if (UnionRight.GetBreak() == false)
            {
                UnionRight.Func_BreakBlock();
            }
        }

        WaitFallTimer += Time.deltaTime;

        if (WaitFallTimer > WaitFallTime)
        {
            OnThePipe = false;
            pipeStatus = PIPESTATUS.AllBroken;
        }
    }

        private void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面かパイプに当たったら
        if(collision.gameObject.tag == GroundTag || 
            collision.gameObject.tag == PipeTag)
        {
            // パイプの状態が落下する状態なら
            if(pipeStatus == PIPESTATUS.AllBroken)
            {
                pipeStatus = PIPESTATUS.Fell;
            }
        }

        // プレイヤーがパイプより上にいるときに当たったら
        if(collision.gameObject.tag == PlayerTag)
        {
            if (pipeStatus == PIPESTATUS.LeftBroken || pipeStatus == PIPESTATUS.RightBroken)
            {
                // プレイヤーの脚の部分がパイプの上表面より上なら
                if (playerTransform.position.y - playerTransform.localScale.y / 2.0f > thisTransform.position.y + thisTransform.localScale.y / 2.0f)
                {
                    // 待ち時間を設けるためフラグ立てるだけ
                    OnThePipe = true;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 地面かパイプに当たったら
        if (collision.gameObject.tag == GroundTag ||
            collision.gameObject.tag == PipeTag)
        {
            // パイプの状態が落下する状態なら
            if (pipeStatus == PIPESTATUS.AllBroken || pipeStatus == PIPESTATUS.LeftBroken || pipeStatus == PIPESTATUS.RightBroken)
            {
                pipeStatus = PIPESTATUS.Fell;
            }
        }

        // プレイヤーがパイプより上にいるときに当たったら
        if (collision.gameObject.tag == PlayerTag)
        {
            if (pipeStatus == PIPESTATUS.LeftBroken || pipeStatus == PIPESTATUS.RightBroken)
            {
                // プレイヤーの脚の部分がパイプの上表面より上なら
                if (playerTransform.position.y - playerTransform.localScale.y / 2.0f > thisTransform.position.y + thisTransform.localScale.y / 2.0f)
                {
                    // 待ち時間を設けるためフラグ立てるだけ
                    OnThePipe = true;
                }
            }
        }
    }
}
