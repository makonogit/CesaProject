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

    private bool InPipe = false;

    // 自身のコンポーネント変数
    private Transform thisTransform;
    private Rigidbody2D rigid2D;

    // 外部取得
    // プレイヤー
    private GameObject player;
    private Transform playerTransform;

    // 子オブジェクト
    private GameObject Child;
    private EdgeCollider2D ChildEdge;

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

        // 子オブジェクト取得
        Child = transform.GetChild(1).gameObject;

        //子オブジェクトのコライダー取得
        ChildEdge = Child.GetComponent<EdgeCollider2D>();

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
    }

    // Update is called once per frame
    void Update()
    {
        if (InPipe == false)
        {
            // プレイヤーがパイプより上にいたら
            if (playerTransform.position.y > thisTransform.position.y)
            {
                // 判定あり
                ChildEdge.isTrigger = false;
            }
            else
            {
                // 判定無し
                ChildEdge.isTrigger = true;
            }
        }

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
        if(left && right)
        {
            pipeStatus = pipeStatus = PIPESTATUS.AllBroken;
        }
    }

    private void LeftBroken()
    {
        bool right = UnionRight.GetBreak();

        if (right)
        {
            pipeStatus = PIPESTATUS.AllBroken;
        }
    }

    private void RightBroken()
    {
        bool left = UnionRight.GetBreak();

        if (left)
        {
            pipeStatus = PIPESTATUS.AllBroken;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PlayerTag)
        {
            ChildEdge.isTrigger = true;

            InPipe = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PlayerTag)
        {
            ChildEdge.isTrigger = false;

            InPipe = false;
        }
    }
}
