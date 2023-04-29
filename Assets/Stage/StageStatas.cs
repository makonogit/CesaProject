//---------------------------------
// 担当：菅眞心
// 内容：ステージのステータス管理
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStatas : MonoBehaviour
{
    [SerializeField,Header("壊さないといけないクリスタル")]
    private int StageCrystal;

    private CameraZoom Zoom;

    private void Start()
    {
        //-------------------------------------
        //　ステージ上のクリスタルを取得
        GameObject Core = GameObject.Find("Core");
        StageCrystal = Core.transform.childCount;

        if(StageCrystal == 0)
        {
            Zoom.stagestatas = null;
        }

    }

    //--------------------------------
    // クリスタルの数をセットする関数
    // 引数：クリスタルの数
    // 戻り値：なし
    //--------------------------------
    public void SetStageCrystal(int _crystalnum)
    {
        StageCrystal = _crystalnum;
    }

    //------------------------------
    // クリスタルの数を獲得する関数
    // 引数：なし
    // 戻り値：クリスタルの数
    //------------------------------
    public int GetStageCrystal()
    {
        return StageCrystal;
    }
}
