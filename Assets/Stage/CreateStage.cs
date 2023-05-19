//-------------------------------
//　担当：菅眞心
//　内容：ステージの生成
//-------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStage : MonoBehaviour
{
    //-------------------------
    //ステージ生成用のデータ
  
    StageManager _stagemanager;

    // Start is called before the first frame update
    void Start()
    {
        //-----------------------------------
        //　ステージ生成する
        _stagemanager = GetComponent<StageManager>();
        _stagemanager.CreateStage();
        
    }

}
