//---------------------------------------
// プラント場のボスの初期化
// 担当：菅
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBossInit : MonoBehaviour
{
    [SerializeField] private GameObject Boss;
    [SerializeField] private Transform BossTrans;

    [SerializeField] private GameObject PipeEnemyManager;
    private List<WindCrystal> winds = new List<WindCrystal>();

    [SerializeField] private Transform gatepos;

    public void init()
    {
        Destroy(transform.GetChild(0).gameObject);
        GameObject bossobj = Instantiate(Boss,BossTrans);

        // パイプを取得
        GameObject pipe = GameObject.Find("BossWindPipe");
        for (int i = 0; i < pipe.transform.childCount; i++)
        {
            winds.Add(pipe.transform.GetChild(i).GetComponent<WindCrystal>());
        }

        //パイプの初期化
        for (int i = 0; i< winds.Count; i++)
        {
            winds[i].Init();
        }

        //ちび敵の初期化
        for (int i = 0; i < PipeEnemyManager.transform.childCount; i++)
        {
            Destroy(PipeEnemyManager.transform.GetChild(i).gameObject);
        }


        //　ひびを取得
        GameObject CrackManager = GameObject.Find("CrackManager");
        //  ひびの初期化
        for (int i = 0; i < CrackManager.transform.childCount; i++)
        {
            GameObject Crack = CrackManager.transform.GetChild(i).gameObject;
            if (Crack.transform.GetChild(2).transform.position.x > gatepos.position.x)
            {
                Destroy(Crack);
            }
        }

    }
}
