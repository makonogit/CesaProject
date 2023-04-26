//---------------------------------
//担当：二宮怜
//内容：条件を満たしたらリザルトシーンに移行
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ChangeResultScene : MonoBehaviour
{
    // - 変数宣言 -
    public float time = 0.0f; // クリスタルを全部壊してからの経過時間
    private float WaitTime = 2.0f; // シーン遷移するまでの待ち時間

    // 外部取得
    private GameObject player;
    private PlayerStatas playerStatus;
    private GameObject Resultobj;   // リザルト演出用のオブジェクト
    private ResultManager resultmanager;

    //----追加者：中川直登----
    private Clear clear;// クリアしたかどうかをセレクトに持っていく
    //------------------------
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        playerStatus = player.GetComponent<PlayerStatas>();

        //------------------------------------------------
        // リザルト演出用のシステム取得
        Resultobj = GameObject.Find("Result_StageClear");
        resultmanager = Resultobj.GetComponent<ResultManager>();

        //----追加者：中川直登----
        clear = new Clear();
        //------------------------
}

// Update is called once per frame
void Update()
    {
        var gamepad = Gamepad.current;

        // 3つのクリスタルを壊したらリザルト画面に移動
        if (playerStatus.GetBreakCrystalNum() == 3)
        {
            GameObject stage = GameObject.Find("StageData").transform.GetChild(0).gameObject;
            CameraZoom zoom = stage.GetComponent<CameraZoom>();
            player.GetComponent<PlayerMove>().SetMovement(false);
            

            //playerStatus.AddBreakCrystal();
            //演出開始
            //result.SetFadeFlg(true);
            
            if (gamepad != null)
            {
                //　振動停止
                gamepad.SetMotorSpeeds(0.0f, 0.0f);
            }
            if (zoom.ZoomEnd)
            {
                resultmanager.PlayResult();
            }
            //----追加者：中川直登----
            clear.GameClear();// クリアした
            //------------------------
        }
    }
}
