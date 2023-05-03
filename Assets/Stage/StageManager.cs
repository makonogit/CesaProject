//---------------------------------------------
// 担当：菅眞心
// 内容：各ステージの情報を持つスクリプト
//---------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageChild
{

    public GameObject StageObj;   // ステージ管理
    public Vector2 PlayerPos;     // プレイヤー初期座標

    
    public StageChild(GameObject _stageobj,Vector2 _playerpos)
    {
        StageObj = _stageobj;
        PlayerPos = _playerpos;
    }


}

[System.Serializable]
public class Stage
{
    public List<StageChild> stage;

    public Stage(List<StageChild> _stage)
    {
        stage = _stage;
    }
}

public class StageManager : MonoBehaviour
{

    [Header("ステージ管理")]
    public List<Stage> stage;

    //-----------------------------------------
    //　外部取得
    private GameObject player;
    private Transform PlayerTrans;  //PlayerのTransform

    //----------------------------------------------
    //　ステージを生成する関数
    //  引数：なし
    //　戻り値：なし
    public void CreateStage()
    {
        if (StageData.StageNum > -1 && StageData.AreaNum > -1 &&
            StageData.StageNum < 5 && StageData.AreaNum < 5)
        {
           GameObject obj = Instantiate(stage[StageData.AreaNum].stage[StageData.StageNum].StageObj);
           obj.transform.parent = this.transform;                  //子オブジェクトにする

        }
        else
        {
            Debug.Log("範囲外のステージ番号が割り振られています");
        }

        //---------------------------------
        // プレイヤーの初期座標指定
        player = GameObject.Find("player");
        PlayerTrans = player.transform;
        PlayerTrans.localPosition = new Vector3(stage[StageData.AreaNum].stage[StageData.StageNum].PlayerPos.x,
            stage[StageData.AreaNum].stage[StageData.StageNum].PlayerPos.y, 1.0f);
    }

    // 二宮追加
    // 引数　：エリア番号とステージ番号
    // 戻り値：プレイヤーの初期位置
    public Vector3 GetInitPlayerPos(int _areaNum, int _stageNum)
    {
        Vector3 pos = new Vector3(stage[_areaNum].stage[_stageNum].PlayerPos.x,
            stage[_areaNum].stage[_stageNum].PlayerPos.y,
            1.0f);

        return pos;
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

    //----------------------------------------------
    //　ステージ番号を獲得する関数
    //  引数：なし   
    //　戻り値：ステージ番号
    public int GetStageNum()
    {
        return StageData.StageNum;
    }

    //----------------------------------------------
    //　エリア番号を獲得する関数
    //  引数：なし   
    //　戻り値：エリア番号
    public int GetAreaNum()
    {
        return StageData.AreaNum;
    }

}