//------------------------------------------------------------------------------
// 担当者：藤原昂祐
// 内容  ：ひびが入った場所にオブジェクトを生成するギミック
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    //--------------------------------------------------------------------------
    // - 変数宣言 -

    [Header("ひびから生成するオブジェクト")]
    public GameObject makeObj;     // 生成するオブジェクト
    [Header("ひびを生成するオブジェクト")]
    public CrackOrder crackOrder;  // ひびを生成するオブジェクト

    // ひびとの当たり判定関連
    Vector2[] Point;               //当たったedgecolliderのPoint座標
    EdgeCollider2D Edge;           //当たったEdgeCollider

    //============================================================
    // - 衝突処理 -

    void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------------
        // ひびとぶつかった場合の処理

        if (collision.gameObject.tag == "Crack")
        {
            //----------------------------------------------------
            // ひびが入っている場所にオブジェクトを生成する

            // EdgeColliderの情報を取得
            Edge = collision.gameObject.GetComponent<EdgeCollider2D>();

            //このオブジェクトのSpriteRendererを取得
            SpriteRenderer thisSpriteRenderer = this.GetComponent<SpriteRenderer>();

            for (int i = 0; i < (int)crackOrder.numSummon; i++)
            {
                // ひびがこのオブジェクト内に存在するかを判定 
                if (Edge.points[i].y < this.transform.position.y + thisSpriteRenderer.bounds.size.y / 2
                     && Edge.points[i].y > this.transform.position.y - thisSpriteRenderer.bounds.size.y / 2
                     && Edge.points[i].x < this.transform.position.x + thisSpriteRenderer.bounds.size.x / 2
                     && Edge.points[i].x > this.transform.position.x - thisSpriteRenderer.bounds.size.x / 2)
                {

                    // ひびが入っている場所にオブジェクトを生成
                    Vector3 EdgePos = Edge.transform.position;
                    SpriteRenderer crackSr = crackOrder.PrefabObject.GetComponent<SpriteRenderer>();
                    EdgePos = Vector3.MoveTowards(Edge.transform.position, Edge.points[i], crackSr.bounds.size.y * i);
                    GameObject obj = Instantiate(makeObj, EdgePos, Quaternion.identity);

                }
            }
            //----------------------------------------------------
        }
        //--------------------------------------------------------

    }
    //============================================================

}
