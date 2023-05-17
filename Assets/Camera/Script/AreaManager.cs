//-----------------------------------
//担当：菅眞心
//内容：ステージ内エリアの管理
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [Header("エリアの数")]
    public int AreaNum;

    [Header("エリアのサイズ")]
    public float AreaSize = 10;

    private void Start()
    {
        SetStage stage = new SetStage();
        if(stage.GetStageNum() == 4)
        {
            AreaSize = 107;
        }
    }

}
