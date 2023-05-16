//---------------------------------
// 担当：菅
// 内容：ボスのコアが壊れた処理
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBossCore : MonoBehaviour
{
    private Directing_BossLight LightEffect;  // 爆発エフェクト用

    private void Start()
    {
        LightEffect = GameObject.Find("Directing_BossLight").GetComponent<Directing_BossLight>();  //爆発用
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CrackCreater creater = collision.GetComponent<CrackCreater>();
        if (collision.tag == "Crack" &&
            (creater.GetState() == CrackCreater.CrackCreaterState.CREATING ||
            creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
        {
            //Destroy(GameObject.Find("BossEnemy").transform.GetChild(0).gameObject);
            LightEffect.Flash();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

}
