//---------------------------------------------------------
//担当者：二宮怜
//内容　：設定したタグと接地しているかRayCastで判定
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    public bool isGround = false; // 最終的に接地しているかしていないかの情報を持つ
    public float AdjustY = 0.03f; // 画像の空白部分を無視するための調整用変数
    public float AdjustX = 0.41f; // 画像の空白部分を無視するための調整用変数
    public float AdjustCenter = 0.15f; // 中央ぞろえ用変数
    public int touch; // 地面と触れているレイの本数

    [Header("円形のレイ用")]
    [SerializeField] float groundCheckRadius = 0.4f; // 半径
    [SerializeField] float groundCheckOffsetX = 0.45f; // オフセット
    [SerializeField] float groundCheckOffsetY = 0.45f; // オフセット
    [SerializeField] float groundCheckDistance = 0.2f; // キャストする最大距離 （円の半径＋この変数）がキャスト距離？

    //-----------追加担当：中川-----------
    [SerializeField, Tooltip("レイの長さを調整します。")]
    private float _length = 0.01f;
    //------------------------------------

    PlayerInputManager.DIRECTION oldDire; // 前フレームの向きを入れておくための変数

    // 10 : Ground
    // 14 : Block
    // 17 : EnemyPipe
    // 18 : Pipe
    // 21 : Trolley
    // 22 : IgnoreOverHead
    private LayerMask layerMask = 1 << 10 | 1 << 14 | 1 << 17 | 1 << 18 |1 << 21 | 1 << 22;

    // 外部取得
    private GameObject PlayerInputMana; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform thistransform; // レイによる当たり判定をとるオブジェクトの原点座標

    private void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // 座標情報取得
        thistransform = GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManagerを探す
        PlayerInputMana = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        oldDire = ScriptPIManager.Direction;
    }

    private void Update()
    {
        
        if (oldDire != ScriptPIManager.Direction)
        {
            AdjustX = -AdjustX;
            groundCheckOffsetX = -groundCheckOffsetX;
            AdjustCenter = -AdjustCenter;
        }

        // 前フレームの向きとして保存
        oldDire = ScriptPIManager.Direction;
    }

    //----------------------------------------------------------------------------------------------------------
    //接地判定を返すメソッド
    //物理判定の更新毎に呼ぶ必要がある
    public bool IsGround()
    {
        //----------------------------------------------------------------------------------------------------------
        // レイキャストに必要な引数の準備

        // x:left y:bottom
        Vector2 origin_left = new Vector2(thistransform.position.x - thistransform.localScale.x / 2.0f + AdjustX + AdjustCenter, 
            thistransform.position.y - thistransform.localScale.y / 2.0f + AdjustY);

        // x:原点 y;bottom
        Vector2 origin_middle = new Vector2(thistransform.position.x + AdjustCenter, 
            thistransform.position.y - thistransform.localScale.y / 2.0f + AdjustY);

        // x:right y:bottom
        Vector2 origin_right = new Vector2(thistransform.position.x + thistransform.localScale.x / 2.0f - AdjustX + AdjustCenter, 
            thistransform.position.y - thistransform.localScale.y / 2.0f + AdjustY);
        // 向き
        Vector2 direction = new Vector2(0, -1);
        // 長さ
        float length = _length;// 変更者：中川
        // 距離
        Vector2 distance = direction * length;

        //----------------------------------------------------------------------------------------------------------
        // レイ飛ばして何かとぶつかったら生成やめる
        // 左下
        RaycastHit2D hit_l = Physics2D.Raycast(origin_left, direction, length,layerMask); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

        // 中央
        RaycastHit2D hit_m = Physics2D.Raycast(origin_middle, direction, length, layerMask); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

        // 右下
        RaycastHit2D hit_r = Physics2D.Raycast(origin_right, direction, length, layerMask); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

        //------追加処理 担当：菅眞心 RayにLayerMaskを設定

        //----------------------------------------------------------------------------------------------------------
        // レイを描画
        Debug.DrawRay(origin_left, distance, Color.red);
        Debug.DrawRay(origin_middle, distance, Color.red);
        Debug.DrawRay(origin_right, distance, Color.red);

        touch = 0;

        // 当たっているかつ、タグがGroundならカウントを増やす
        if (hit_l)
        {
            touch++;
        }
        if (hit_m)
        {
            touch++;
        }
        if (hit_r)
        {
            touch++;
        }

       
        //----------------------------------------------------------------------------------------------------------
        // 二本以上地面に触れていたら
        if (touch >= 2)
        {
            //Debug.Log("当たった");

            isGround = true;
            // 当たっているタグ名を表示
            //Debug.Log(hit.collider.gameObject.tag);
        }
        else
        {
            //Debug.Log("当たってない");
            isGround = false;
        }

        //----------------------------------------------------------------------------------------------------------
        // 判定を返す
        return isGround;
    }

    // 円形のレイキャスト
    public bool IsGroundCircle()
    {
        // 戻り値用変数
        bool Return = false;

        RaycastHit2D hit = Physics2D.CircleCast((Vector2)thistransform.position + groundCheckOffsetX * Vector2.right + groundCheckOffsetY * Vector2.up, groundCheckRadius, Vector2.down, groundCheckDistance, layerMask);

        if (hit)
        {
            Return = true;
        }

        //Debug.Log(Return);

        return Return;
    }

    //円形のレイ描画
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere((Vector2)thistransform.position + groundCheckOffsetX * Vector2.right + groundCheckOffsetY * Vector2.up, groundCheckRadius);
    //}

    public int GetRayNum()
    {
        return touch;
    }
}
