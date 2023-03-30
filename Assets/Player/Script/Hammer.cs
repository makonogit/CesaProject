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
    private PlayerMove Move;    　              // 移動スクリプト
    private GameObject CrackManager;            // 全てのひびの親オブジェクト
    private CrackCreater NowCrack;              // 現在のひびのCreater

    public bool AddCrackFlg = false;           // ひびが伸びるフラグ
    private bool LongCrack = false;             // 伸びているひびなのか
    private float angle;                        // ひびを入れる角度
    private Vector2 OldFirstPoint;              // 前回の始点座標

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

        AngleTest = GameObject.Find("Target");

    }

    // Update is called once per frame
    void Update()
    {

        //------------------------------------------------------
        // 状態によって処理
        switch (hammerstate)
        {
            case HammerState.NONE:

                //　ひびの始点を常に自分の座標に指定
                CrackPointList[0] = transform.position;

                //トリガーを押したら方向決定状態
                if (InputManager.GetNail_Right())
                {
                    //---------------------------------------------
                    // 前回の位置と移動していなかったらポイント追加
                    if (CrackPointList[0] == OldFirstPoint)
                    {
                        AddCrackFlg = true;
                    }

                    hammerstate = HammerState.DIRECTION;
                }
                break;
            case HammerState.DIRECTION:

                //　移動できないようにする
                if (Move.enabled)
                {
                    Move.enabled = false;
                }

                //デバッグ用
                AngleTest.transform.position = new Vector3(CrackPointList[1].x, CrackPointList[1].y, 0.0f);

                if(!AddCrackFlg) 
                //-----------------------------------------------------------------------------
                // 角度と距離から座標を計算
                {
                    // 左スティックの入力から角度を取得する
                    Vector2 LeftStick = InputManager.GetMovement();
                    angle = Mathf.Atan2(LeftStick.y, LeftStick.x) * Mathf.Rad2Deg;

                    // 角度を正規化
                    if (angle < 0)
                    {
                        angle += 360;
                    }

                    //　角度を45度ずつで管理
                    angle = ((int)(angle / 45.0f)) * 45.0f;

                    // 角度と距離からPoint座標を求める
                    CrackPointList[1] = new Vector2(CrackPointList[0].x + (CrackLength * Mathf.Cos(angle)), CrackPointList[0].y + (CrackLength * Mathf.Sin(angle)));
                }

                //トリガーを離したらひび生成状態
                if (!InputManager.GetNail_Right())
                {
                    hammerstate = HammerState.HAMMER;
                }

                break;
            case HammerState.HAMMER:

                //----------------------------------------------------
                //　前回の位置から移動していなかったらポイントを追加
                if (AddCrackFlg)
                {
                    //Debug.Log(NowCrack);
                    NowCrack = CrackManager.transform.GetChild(CrackManager.transform.childCount - 1).GetComponent<CrackCreater>();
                    NowCrack.SetState(CrackCreater.CrackCreaterState.ADD_CREATE);
                    AddCrackFlg = false;

                }
                else
                {
                    CallCrackCreater();  //ひび生成
                }

                OldFirstPoint = CrackPointList[0];  // 生成時の座標を保存

                hammerstate = HammerState.NONE;

                break;
            default:
                Debug.Log("HammerStateに設定できない数値が代入されています");
                break;
        }

        //----------------------------------------
        //　生成終了したら移動解除
        if (NowCrack != null &&
            NowCrack.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        {
            Move.enabled = true;
        }
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
        for (int i = 0; CrackPointList.Count < 2; i++)
        {
            CrackPointList[i] = Vector2.zero;
        }

    }
    //-----------------------------------------------------------------


}

