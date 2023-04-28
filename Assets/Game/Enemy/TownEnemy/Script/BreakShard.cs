//---------------------------------------------------------
//担当者：二宮怜
//内容　：欠片が壁に当たったら壊れる
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakShard : MonoBehaviour
{
    // 変数宣言
    private string GroundTag = "Ground";

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったのがGroundタグだったら
        if(collision.gameObject.tag == GroundTag)
        {
            // かけらオブジェクト（自身）を消す
            Destroy(this.gameObject);
        }
    }
}
