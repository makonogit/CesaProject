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
    private GameObject Stage;
    private StageStatas Stagestatus;
    private GameObject Resultobj;   // リザルト演出用のオブジェクト
    private ResultManager resultmanager;
    private bool Firstcheck = false;
    public bool BossStage = false; // ボスステージ用
    public float WaitFlame = 0.0f;

    [SerializeField,Header("プレイヤー")]
    private GameObject Player;

    // クリア時に停止するプレイヤーのコンポーネント
    //private PlayerJump jump;
    private New_PlayerJump jump;
    private PlayerMove move;
    private Hammer hammer;
    private CrackAutoMove crackmove;

    SetStage _setstage = new SetStage();

    //----追加者：中川直登----
    private Clear clear;// クリアしたかどうかをセレクトに持っていく
    //------------------------
    
    // Start is called before the first frame update
    void Start()
    {
        Stage = GameObject.Find("StageData");
       
        //------------------------------------------------
        // リザルト演出用のシステム取得
        Resultobj = GameObject.Find("Result_StageClear");
        resultmanager = Resultobj.GetComponent<ResultManager>();

        //----追加者：中川直登----
        clear = new Clear();
        //------------------------

        // プレイヤーを動かすコンポーネントの取得
        //jump = Player.GetComponent<PlayerJump>();
        jump = Player.GetComponent<New_PlayerJump>();
        move = Player.GetComponent<PlayerMove>();
        hammer = Player.GetComponent<Hammer>();
        crackmove = Player.GetComponent<CrackAutoMove>();

    }

    // Update is called once per frame
    void Update()
    {
        

        if (Firstcheck)
        {

            // ボスステージ用
            if (BossStage)
            {
                //ボスが見つからなくなったら
                if (GameObject.Find("BossEnemy").transform.childCount == 0)
                {
                    //コアを破壊してリザルト
                    if (Stagestatus.GetStageCrystal() == 0 && WaitFlame > 0.2f)
                    {
                        Result();
                    }
                    //zoomしてしまうので待機
                    WaitFlame += Time.deltaTime;

                }

            }
            else
            {
                // 全てクリスタルを壊したらリザルト画面に移動
                if (Stagestatus.GetStageCrystal() == 0)
                {
                    Result();
                }
            }
        }
        else
        {
            Stagestatus = Stage.transform.GetChild(0).GetComponent<StageStatas>();

            if (GameObject.Find("BossEnemy"))
            {
                BossStage = true;
            }
            else
            {
                BossStage = false;
            }

            Firstcheck = true;
        }
    }

    private void Result()
    {
        var gamepad = Gamepad.current;
        GameObject stage = GameObject.Find("StageData").transform.GetChild(0).gameObject;
        CameraZoom zoom = stage.GetComponent<CameraZoom>();
      
        move.SetMovement(false);
        jump.enabled = false;
        crackmove.enabled = false;
        hammer.enabled = false;

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
