//---------------------------------
//担当：菅眞心
//内容：プレイヤーのステータス管理
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnStatus
{
    public int NowRespawnNumber = 0;  // 最新のリスポーン地点の座標を持つオブジェクトの固有番号
    public Vector3 PlayerRespawnPos;  // プレイヤーの最新リスポーン座標を保持
    public int RespawnCrystalNum = 0; // 最新リスポーンでのクリスタル所持数
}

public class PlayerStatas : MonoBehaviour
{
    [SerializeField,Header("釘所持数")]
    private int HaveNail;

    [SerializeField, Header("クリスタル所持数")]
    private int HaveCrystal;

    public bool UpdateCrystalNum = false;

    // 二宮追加
    private int BreakCrystalNum = 0; // 壊したクリスタルの数

    [SerializeField] private bool HitStop = false; // ヒットストップ中か]

    public RespawnStatus respawnStatus = new RespawnStatus();

    public RespawnStatus GetRespawnStatus()
    {
        return respawnStatus;
    }

    public void SetRespawnCrystalNum()
    {
        respawnStatus.RespawnCrystalNum = HaveCrystal;
    }

    public void SetRespawnCrystalNum(int _num)
    {
        respawnStatus.RespawnCrystalNum = _num;
    }

    public void SetRespawnNum(int _num)
    {
        respawnStatus.NowRespawnNumber = _num;
    }

    //public int GetNowRespawnNum()
    //{
    //    return respawnStatus.NowRespawnNumber;
    //}

    //// リスポーン座標をかえす
    //public Vector3 GetRespawn()
    //{
    //    return respawnStatus.PlayerRespawnPos;
    //}

    // リスポーン座標を設定
    public void SetRespawn(Vector3 _respawn)
    {
        respawnStatus.PlayerRespawnPos = _respawn;
    }

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
    // クリスタルの所持数を取得する関数
    // 引数：なし
    // 戻り値：クリスタルの所持数
    //-------------------------------------
    public int GetCrystal()
    {
        return HaveCrystal;
    }

    public bool IsHitStop()
    {
        return HitStop;
    }

    public bool GetHitStop()
    {
        return HitStop;
    }

    public void SetHitStop(bool value)
    {
        HitStop = value;
    }
}
