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

    private BGMFadeManager _BGMfadeMana;

    [SerializeField]
    private Sprite DethBoss;    //死亡時のスプライト

    private void Start()
    {
        LightEffect = GameObject.Find("Directing_BossLight").GetComponent<Directing_BossLight>();  //爆発用

        _BGMfadeMana = GameObject.Find("Main Camera").GetComponent<BGMFadeManager>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Crack")
        {
            CrackCreater creater = collision.GetComponent<CrackCreater>();
            if (creater != null)
            {
                if ((creater.GetState() == CrackCreater.CrackCreaterState.CREATING ||
                    creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
                {
                    GameObject boss = GameObject.Find("BossEnemy").transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
                    boss.GetComponent<Animator>().enabled = false;
                    boss.GetComponent<SpriteRenderer>().sprite = DethBoss;

                    LightEffect.Flash();
                    _BGMfadeMana.SmallBossBGM();
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }

}
