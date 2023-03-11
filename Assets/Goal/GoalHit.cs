//--------------------------------
//担当：尾花真理子
//内容：ゴールイベントの実装
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalHit : MonoBehaviour
{
    //---------------------------------------------------------------
    // * 外部取得 *
    //---------------------------------------------------------------
    private GameObject PlayerInputManager; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数

    CameraZoom CamZoom; // カメラズームスクリプトを取得
    private GameObject GoalArea; // ゲームオブジェクトGoalAreaを取得する変数

    //---------------------------------------------------------------
    // * 初期化処理 *
    //---------------------------------------------------------------
    void Start()
    {
        // PlayerInputManagerを探す
        PlayerInputManager = GameObject.Find("PlayerInputManager");
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

        //　GoalAreaを探す
        GoalArea = GameObject.Find("GoalArea");
        // カメラスクリプトを取得
        CamZoom = GoalArea.GetComponent<CameraZoom>();


    }


    //---------------------------------------------------------------
    // * 更新処理 *
    //---------------------------------------------------------------
    void Update()
    {
        // ゴールエリアに入っていたらイベント処理
        if (CamZoom.InArea)
        {
            // 力が最大値に達したらボタンでシーン移動
            if (ScriptPIManager.GetHammer() == true) 
            {
                //クリア画面の描画
                SceneManager.LoadScene("ClearScene");
            }
        }

    }
}
