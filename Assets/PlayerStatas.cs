//---------------------------------
//担当：菅眞心
//内容：プレイヤーのステータス管理
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatas : MonoBehaviour
{
    [SerializeField,Header("釘所持数")]
    private int HaveNail;

    [SerializeField, Header("クリスタル所持数")]
    private int HaveCrystal;

    // 二宮追加
    private int BreakCrystalNum = 0; // 壊したクリスタルの数

    public void AddBreakCrystal()
    {
        BreakCrystalNum++;
    }

    public int GetBreakCrystalNum()
    {
        return BreakCrystalNum;
    }

    //------------------------------
    // 釘の所持数をセットする関数
    // 引数：釘の所持数
    // 戻り値：なし
    //------------------------------
    public void SetNail(int _nail)
    {
        HaveNail = _nail;
    }

    //------------------------------
    // 釘の所持数を獲得する関数
    // 引数：なし
    // 戻り値：釘の所持数
    //------------------------------
    public int GetNail()
    {
        return HaveNail;
    }


    //-------------------------------------
    // クリスタルの所持数をセットする関数
    // 引数：クリスタルの所持数
    // 戻り値：なし
    //-------------------------------------
    public void SetCrystal(int _crystal)
    {
        HaveCrystal = _crystal;
    }

    //-------------------------------------
    // クリスタルの所持数を関数する関数
    // 引数：なし
    // 戻り値：クリスタルの所持数
    //-------------------------------------
    public int GetCrystal()
    {
        return HaveCrystal;
    }

}
