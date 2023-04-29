//----------------------------------------------------------
// 担当者：二宮怜
// 内容  ：ボスの突進中にプレイヤーに当たったら止まる
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RammingStop : MonoBehaviour
{
    private string PlayerTag = "Player";

    // 外部取得
    private TownBossMove townBossMove;

    // Start is called before the first frame update
    void Start()
    {
        townBossMove = transform.parent.gameObject.GetComponent<TownBossMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーと当たったら
        if(collision.gameObject.tag == PlayerTag)
        {
            // 当たった情報を送る
            townBossMove.SetHitPlayer(true);
        }
    }
}
