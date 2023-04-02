//---------------------------------------------
// 担当：菅眞心
// 内容：各ステージの情報を持つスクリプト
//---------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    [Header("ステージ管理")]
    public List<GameObject> StageObj;

    //----------------------------------------------
    //　ステージを生成する関数
    //  引数：なし
    //　戻り値：なし
    public void CreateStage()
    {
        if (StageData.StageNum > -1 && StageData.AreaNum > -1 &&
            StageData.StageNum < 5 && StageData.AreaNum < 5)
        {
            GameObject obj = Instantiate(StageObj[StageData.AreaNum * 5 + StageData.StageNum]);
            obj.transform.parent = this.transform;                  //子オブジェクトにする

        }
        else
        {
            Debug.Log("範囲外のステージ番号が割り振られています");
        }

    }
}

public static class StageData
{
  
    [Header("エリア番号")]
    public static int AreaNum = 0;        // エリアの番号を持つ変数
    [Header("ステージ番号")]
    public static int StageNum = 0;       // ステージの番号を持つ変数

}

public class SetStage {

    //----------------------------------------------
    //　ステージ番号を指定する関数
    //  引数：1~5　(エリア番号) 1~5 (ステージ番号)
    //　戻り値：なし
    public void SetStageData(int _AreaNum, int _StageNum)
    {
        StageData.AreaNum = _AreaNum - 1;
        StageData.StageNum = _StageNum - 1;
    }
}