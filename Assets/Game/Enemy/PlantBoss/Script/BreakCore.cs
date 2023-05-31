//---------------------------------
//　担当：菅眞心
//　プラント場ボスのダメージ処理
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakCore : MonoBehaviour
{
    [SerializeField] Directing_BossLight light; //爆散用ライト
    [SerializeField] PlantBossMove move;        //行動スクリプト


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //　ひびが当たったら消去
        if (collision.gameObject.tag == "Crack")
        {
            if (collision.gameObject.GetComponent<CrackCreater>() && (collision.gameObject.GetComponent<CrackCreater>().GetState() == CrackCreater.CrackCreaterState.CREATING ||
            collision.gameObject.GetComponent<CrackCreater>().GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
            {
                if (light != null)
                {
                    if (move.GetState() == PlantBossMove.PlantBossMoveState.DETH)
                    {
                        light.Flash();
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //　ひびが当たったら消去
        if(collision.tag == "Crack")
        {
            if (collision.GetComponent<CrackCreater>() && (collision.GetComponent<CrackCreater>().GetState() == CrackCreater.CrackCreaterState.CREATING ||
            collision.GetComponent<CrackCreater>().GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
            {
                if (light != null)
                {
                    if (move.GetState() == PlantBossMove.PlantBossMoveState.DETH)
                    {
                        light.Flash();
                    }
                    Destroy(collision.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
