//---------------------------------------------------------
//担当者：二宮怜
//内容　：ボス再挑戦時の初期化
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBossInit : MonoBehaviour
{
    // 変数宣言

    // ボスの初期化関数があるスクリプト割り当て
    [Header("ステージに存在するボスの初期化関数を持つスクリプトのみ割り当て")]
    [SerializeField,Header("町ボス")] private TownBossMove _townMove;               // 町ボス
    [SerializeField,Header("氷ボス")] private IceBoss _iceMove;                     // 氷ボス
    [SerializeField,Header("砂漠ボス")] private DesertBossMove _desertMove;         // 砂漠ボス
    [SerializeField,Header("洞窟ボス")] private StateManager_CaveBoss _caveMove;    // 洞窟ボス
    [SerializeField,Header("工場ボス")] private PlantBossInit _plantInit;           // 工場ボス

    // 現在いるステージを取得するため
    private SetStage setStage = new SetStage();

    public void RespawnInit()
    {
        var AreaNum = setStage.GetAreaNum();
        var StageNum = setStage.GetStageNum();

        switch (AreaNum)
        {
            // 町
            case 0:
                // ボスステージなら
                if(StageNum == 4)
                {
                    if (_townMove != null)
                    {
                        // 町ボス初期化
                        _townMove.Init();
                    }
                    else
                    {
                        Debug.Log("初期化をするスクリプトがセットされていません(町)");
                    }
                }
                break;

            // 氷
            case 1:
                // ボスステージなら
                if (StageNum == 4)
                {
                    if (_iceMove != null)
                    {
                        // 氷ボス初期化
                        _iceMove.Init();
                    }
                    else
                    {
                        Debug.Log("初期化をするスクリプトがセットされていません(氷)");
                    }
                }
                break;

            // 砂漠
            case 2:
                // ボスステージなら
                if (StageNum == 4)
                {

                    if (_desertMove != null)
                    {
                        // 砂漠ボス初期化
                        _desertMove.DesertBossInit();
                    }
                    else
                    {
                        Debug.Log("初期化をするスクリプトがセットされていません(砂漠)");
                    }

                }
                break;

            // 洞窟
            case 3:
                // ボスステージなら
                if (StageNum == 4)
                {

                    if (_caveMove != null)
                    {
                        // 洞窟ボス初期化
                        _caveMove.Init();
                    }
                    else
                    {
                        Debug.Log("初期化をするスクリプトがセットされていません(洞窟)");
                    }

                }
                break;

            // 工場
            case 4:
                // ボスステージなら
                if (StageNum == 4)
                {

                    if (_plantInit != null)
                    {
                        // 工場ボス初期化
                        _plantInit.init();
                    }
                    else
                    {
                        Debug.Log("初期化をするスクリプトがセットされていません(工場)");
                    }

                }
                break;
        }
    }
}
