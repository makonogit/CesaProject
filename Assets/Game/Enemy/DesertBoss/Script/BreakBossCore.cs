//---------------------------------
// 担当：菅
// 内容：ボスのコアが壊れた処理
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBossCore : MonoBehaviour
{
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ひびに当たったらボスごと消す
        if (collision.tag == "Player")
        {
            Destroy(GameObject.Find("BossEnemy").transform.GetChild(0).gameObject);
            Destroy(gameObject);
        }
    }

}
