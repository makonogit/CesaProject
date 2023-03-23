//---------------------------------------------------------
//担当者：二宮怜
//内容　：設定したタグと接地しているかRayCastで判定
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCheck : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    private string groundTag = "Ground"; // Groundタグ名を文字列型で持っている変数
    private string iceTag = "Ice";

    public bool isOverhead = false; // 最終的に天井と衝突しているかしていないかの情報を持つ
    public float AdjustY = -0.15f; // 画像の空白部分を無視するための調整用変数
    public float AdjustX = 0.41f; // 画像の空白部分を無視するための調整用変数
    public float AdjustCenter = 0.15f; // 中央ぞろえ用変数

    PlayerInputManager.DIRECTION oldDire; // 前フレームの向きを入れておくための変数

    // 外部取得
    private GameObject PlayerInputMana; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変
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
            AdjustCenter = -AdjustCenter;
        }

        // 前フレームの向きとして保存
        oldDire = ScriptPIManager.Direction;
    }

    //----------------------------------------------------------------------------------------------------------
    //接地判定を返すメソッド
    //物理判定の更新毎に呼ぶ必要がある
    public bool IsOverHead()
    {
        //----------------------------------------------------------------------------------------------------------
        // レイキャストに必要な引数の準備

        // x:left y:top
        Vector2 origin_left = new Vector2(thistransform.position.x - thistransform.localScale.x / 2.0f + AdjustX + AdjustCenter,
            thistransform.position.y + thistransform.localScale.y / 2.0f + AdjustY);
        // x:right y:top
        Vector2 origin_right = new Vector2(thistransform.position.x + thistransform.localScale.x / 2.0f - AdjustX + AdjustCenter,
            thistransform.position.y + thistransform.localScale.y / 2.0f + AdjustY);
        // 向き
        Vector2 direction = new Vector2(0, 1);
        // 長さ
        float length = 0.1f;
        // 距離
        Vector2 distance = direction * length;

        //----------------------------------------------------------------------------------------------------------
        // レイ飛ばして何かとぶつかったら生成やめる
        RaycastHit2D hit_l = Physics2D.Raycast(origin_left, direction, length, 5); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー
        RaycastHit2D hit_r = Physics2D.Raycast(origin_right, direction, length, 5); // 第三引数 レイ長さ 、第四引数 レイヤー -1は全てのレイヤー

        //----------------------------------------------------------------------------------------------------------
        // レイを描画
        Debug.DrawRay(origin_left, distance, Color.blue);
        Debug.DrawRay(origin_right, distance, Color.blue);

        // if文見やすくするため変数定義
        bool leftJudge = (hit_l && ((hit_l.collider.gameObject.tag == groundTag) || (hit_l.collider.gameObject.tag == iceTag)));
        bool rightJudge = (hit_r && ((hit_r.collider.gameObject.tag == groundTag) || (hit_r.collider.gameObject.tag == iceTag)));

        //----------------------------------------------------------------------------------------------------------
        // 当たっているかつ、タグがGroundならisGroundをtrueにする
        if (leftJudge || rightJudge)
        {
            isOverhead = true;
        }
        else
        {
            isOverhead = false;
        }

        //----------------------------------------------------------------------------------------------------------
        // 判定を返す
        return isOverhead;
    }
}
