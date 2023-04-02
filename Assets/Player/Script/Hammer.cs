//-----------------------------
//担当：菅眞心
//内容：ひびを入れるver2
//-----------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{

    //---------------------------------
    //　変数宣言

    //---------------------------------
    //外部取得
    private GameObject InputanagerObj;          // InputManagerを持つオブジェクト
    private PlayerInputManager InputManager;    // InputManager
    private GameObject AngleTest;               // 角度を可視化するため
    private SpriteRenderer TargtRenderer;       // 角度可視化用のレンダー
    private TestTargetState Targetstate;        // ひびを作れるか判断
    private PlayerMove Move;    　              // 移動スクリプト
    private GameObject CrackManager;            // 全てのひびの親オブジェクト
    private CrackCreater NowCrack;              // 現在のひびのCreater
    private GameObject seobj;                   // SEオブジェクト
    private SEManager_Player se;                // SE再生用
    private GameObject camera;                  // カメラ
    private VibrationCamera vibration;          // カメラ振動スクリプト


    public bool AddCrackFlg = false;            // ひびが伸びるフラグ
    private bool LongCrack = false;             // 伸びているひびなのか
    private float angle;                        // ひびを入れる角度
    public Vector2 OldFirstPoint;              // 前回の始点座標
   
    [Header("ひびの長さ")]
    public float CrackLength;            

    public List<Vector2> CrackPointList;       //ひびのリスト


    //状態管理
    public enum HammerState
    {
        NONE,       // 何もしていない   
        DIRECTION,  // 方向決定
        HAMMER,     // 叩く
    }

    public HammerState hammerstate;     // 状態管理用変数

    //―追加担当者：中川直登―//
    [Header("ひびを作るobj")]
    public GameObject _crackCreaterObj;
    [System.NonSerialized]
    public GameObject NewCrackObj;  //新しいヒビのオブジェクト
    CrackCreater _creater;
    //――――――――――――//

    // 二宮追加
    private float stopTime; // ヒットストップしている時間
    public float HitStopTime = 0.3f; // ヒットストップ終了時間

    private Animator anim;
    private PlayerStatas playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        //-----------------------------------
        // InputManagerを取得する
        InputanagerObj = GameObject.Find("PlayerInputManager");
        InputManager = InputanagerObj.GetComponent<PlayerInputManager>();

        //-----------------------------------
        // ひびの親オブジェクトを取得
        CrackManager = GameObject.Find("CrackManager");

        // 最初は何もしていない状態にする
        hammerstate = HammerState.NONE;

        // ひびのポイントに自分の座標を指定
        CrackPointList.Add(transform.position);
        CrackPointList.Add(Vector2.zero);       //Listの1番目を確保

        //--------------------------------------------
        //CrackCreaterを取得  //―追加担当者：中川直登―//
        _creater = _crackCreaterObj.GetComponent<CrackCreater>();

        // 移動スクリプトを取得する
        Move = GetComponent<PlayerMove>();

        //----------------------------------------------
        // SE再生用スクリプト取得
        seobj = GameObject.Find("SE");
        se = seobj.GetComponent<SEManager_Player>();

        //----------------------------------------------
        // カメラ振動スクリプトの取得
        camera = GameObject.Find("Main Camera");
        vibration = camera.GetComponent<VibrationCamera>();

        AngleTest = GameObject.Find("Target");
        TargtRenderer = AngleTest.GetComponent<SpriteRenderer>();
        Targetstate = AngleTest.GetComponent<TestTargetState>();

        // アニメーター取得
        anim = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatas>();
    }

    // Update is called once per frame
    void Update()
    {

        //　ひびの始点を常に自分の座標に指定
        CrackPointList[0] = transform.position;

        //------------------------------------------------------
        // 状態によって処理
        switch (hammerstate)
        {
            case HammerState.NONE:

                // 角度の可視化
                TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                //トリガーを押したら方向決定状態
                if (InputManager.GetNail_Right() && !InputManager.GetNail_Left())
                {
                    // 前回の座標との距離を求める
                    float Distance = Vector3.Magnitude(CrackPointList[0] - OldFirstPoint);
                    //---------------------------------------------
                    // 前回の位置とあまり移動していなかったらポイント追加
                    if (Distance < 0.5f) 
                    {
                        AddCrackFlg = true;

                    }
                    else
                    {
                        // 角度の可視化
                        TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }

                    //　移動できないようにする
                    Move.SetMovement(false);
                    hammerstate = HammerState.DIRECTION;
                }

                break;
            case HammerState.DIRECTION:

                //　左を押されたら状態を戻す
                if (InputManager.GetNail_Left())
                {
                    hammerstate = HammerState.NONE;
                }

                if(!AddCrackFlg) 
                //-----------------------------------------------------------------------------
                // 角度と距離から座標を計算
                {
                    // 左スティックの入力から角度を取得する
                    Vector2 LeftStick = InputManager.GetMovement();
                    
                    //----------------------------------------
                    //　スティックの入力があれば角度計算
                    if (LeftStick != Vector2.zero)
                    {
                        angle = Mathf.Atan2(LeftStick.y, LeftStick.x) * Mathf.Rad2Deg;

                        // 角度を正規化
                        if (angle < 0)
                        {
                            angle += 360;
                        }

                        //　角度を45度ずつで管理
                        angle = (((int)angle / 45)) * 45.0f;

                    }
                    else
                    {
                        // なければ
                        angle = angle;

                    }
                }


                // 角度と距離からPoint座標を求める
                CrackPointList[1] = new Vector2(CrackPointList[0].x + (CrackLength * Mathf.Cos(angle * (Mathf.PI / 180))), CrackPointList[0].y + (CrackLength * Mathf.Sin(angle * (Mathf.PI / 180))));

                //デバッグ用
                AngleTest.transform.position = new Vector3(CrackPointList[1].x, CrackPointList[1].y, 0.0f);

                //トリガーを離したらひび生成状態
                if (!InputManager.GetNail_Right())
                {
                    //　照準(仮)が壁にめりこんでなかったら
                    if (!Targetstate.CheeckGround || (AddCrackFlg && Targetstate.CheeckGround))
                    {
                        // SE再生
                        se.PlaySE_Crack1();
                        se.PlayHammer();
                        vibration.SetVibration(0.5f);

                        // ヒットストップ初期化
                        playerStatus.SetHitStop(true);
                        anim.speed = 0.02f;
                        stopTime = 0.0f;
                    }
                    hammerstate = HammerState.HAMMER;
                }

                
                break;
            case HammerState.HAMMER:

                //----------------------------------------------------
                //　前回の位置から移動していなかったらポイントを追加
                if (AddCrackFlg)
                {
                    NowCrack = CrackManager.transform.GetChild(CrackManager.transform.childCount - 1).GetComponent<CrackCreater>();

                    if (NowCrack != null)
                    {
                        if (NowCrack.GetState() == CrackCreater.CrackCreaterState.CRAETED)
                        {
                            NowCrack.SetState(CrackCreater.CrackCreaterState.ADD_CREATEBACK);
                        }
                    }
                    else
                    {
                        Debug.Log("ひびが見つかりません");
                    }
                    AddCrackFlg = false;

                }
                else
                {
                    CallCrackCreater();  //ひび生成
                }

                OldFirstPoint = CrackPointList[0];  // 生成時の座標を保存
              
                Move.SetMovement(true);
                hammerstate = HammerState.NONE;

                break;
            default:
                Debug.Log("HammerStateに設定できない数値が代入されています");
                break;
        }
        // 二宮追加
        // ヒットストップ
        if(stopTime < HitStopTime)
        {
            stopTime += Time.deltaTime;
        }
        else
        {
            // ヒットストップ終了
            anim.speed = 1f;
            playerStatus.SetHitStop(false);
        }


        //----------------------------------------
        //　生成終了したら移動解除

    }

    //-----------------------------------------------------------------
    //―CrackCreaterを呼ぶ関数―           //―追加担当者：中川直登―//
    private void CallCrackCreater()
    {
        // ネイル座標リストを渡す
        _creater.SetPointList(CrackPointList);
        // CrackCreaterを作る
        NewCrackObj = Instantiate(_crackCreaterObj);

        // CrackManagerの子オブジェクトにする
        NewCrackObj.transform.parent = CrackManager.transform;

        // ネイル座標リストを初期化
        for (int i = 1; i < 2; i++)
        {
            CrackPointList[i] = Vector2.zero;
        }

    }
    //-----------------------------------------------------------------


}

