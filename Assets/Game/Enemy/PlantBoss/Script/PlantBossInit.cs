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

    [SerializeField] private List<WindCrystal> winds;

    public void init()
    {
        Destroy(transform.GetChild(0).gameObject);
        GameObject bossobj = Instantiate(Boss);

        //パイプの初期化
        for(int i = 0; i< winds.Count; i++)
        {
            winds[i].Init();
        }


    }
}
