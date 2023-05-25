//----------------------------------
// 担当：菅
// ヒビ入りクリスタル管理用
//----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCrackManager : MonoBehaviour
{
    [SerializeField,Header("ひび入りクリスタル")]
    private List<CrackStart> crackcrystal;
    int breakcrystal = 0;   //ひび入れ中のクリスタル

    public bool clear = false;  //ステージクリアしたか

    // Update is called once per frame
    void Update()
    {
        //　ステージクリアしたら開始
        if (clear)
        {
            // 順番にひび入れを始めていく
            if (breakcrystal < crackcrystal.Count)
            {
                if (!crackcrystal[breakcrystal].StartFlg)
                {
                    crackcrystal[breakcrystal].StartCrack();    //ひび入れ開始
                }


                if (crackcrystal[breakcrystal].EndFlg)
                {
                    breakcrystal++;
                }
            }
        }
    }
}
