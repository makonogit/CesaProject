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
    private SpriteRenderer renderer;            // Spriterenderer
    private GameObject AngleTest;               // 角度を可視化するため
    private SpriteRenderer TargtRenderer;       // 角度可視化用のレンダー
    private TestTargetState Targetstate;        // ひびを作れるか判断
    private PlayerMove Move;    　              // 移動スクリプト
    private CrackAutoMove crackmove;            // ひびの移動スクリプト
    private GameObject CrackManager;            // 全てのひびの親オブジェクト
    private CrackCreater NowCrack;              // 現在のひびのCreater
    private GameObject seobj;                   // SEオブジェクト
    private SEManager_Player se;                // SE再生用
    private GameObject Camera;                  // カメラ
    private VibrationCamera vibration;          // カメラ振動スクリプト


    private bool AngleLook = false;
    public bool AddCrackFlg = false;            // ひびが伸びるフラグ
    private bool LongCrack = false;             // 伸びているひびなのか
    private float angle = 0;                    // ひびを入れる角度
    public Vector2 OldFirstPoint;              // 前回の始点座標
   
    [Header("ひびの長さ")]
    public float CrackLength;

    [SerializeField,Header("溜め技のかける力")]
    private float CrackPower;
    private float MoveLength;                  // 長さを保持する変数
    [SerializeField]
    private float AddPower = 1;                  // ひび追加時のかける力
    private float Power = 0;
    private bool AddPowerFlg = false;

    public List<Vector2> CrackPointList;       //ひびのリスト
    private List<Vector2> BranchCrackList = new List<Vector2>();     //分岐用

    //状態管理
    public enum HammerState
    {
        NONE,       // 何もしていない   
        POWER,      // 溜め技
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
   
    private bool _isStartHaloAnimation;// エフェクト開始フラグ
    private haloEffect _haloEffect;// エフェクト
    //――――――――――――//

    // 二宮追加
    private float stopTime; // ヒットストップしている時間
    public float HitStopTime = 0.3f; // ヒットストップ終了時間

    private Animator anim;
    private PlayerStatas playerStatus;
    [SerializeField,Header("ハンマー打ち込むまでの待機時間")]
    private float WaitHammer;                  // ハンマーを打つまでの待ち時間
    private float WaitHammerMeasure = 0.0f;    // ハンマー打ち込み時間を計測する変数


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

        MoveLength = CrackLength;   //　ひびの長さを保持

        // ひびのポイントに自分の座標を指定
        CrackPointList.Add(transform.position);
        CrackPointList.Add(Vector2.zero);       //Listの1番目を確保
        BranchCrackList.Add(transform.position);
        BranchCrackList.Add(Vector2.zero);

        //--------------------------------------------
        //CrackCreaterを取得  //―追加担当者：中川直登―//
        _creater = _crackCreaterObj.GetComponent<CrackCreater>();
      
        // 移動スクリプトを取得する
        Move = GetComponent<PlayerMove>();
        crackmove = GetComponent<CrackAutoMove>();

        //--------------------------------------------
        //haloEffectを取得  //―追加担当者：中川直登―//
        _haloEffect = GameObject.Find("HaloObj").GetComponent<haloEffect>();
        if (_haloEffect == null) Debug.LogError("haloEffectのコンポーネントを取得できませんでした。");

        _isStartHaloAnimation = false;

        //----------------------------------------------
        // SE再生用スクリプト取得
        seobj = GameObject.Find("SE");
        se = seobj.GetComponent<SEManager_Player>();

        //----------------------------------------------
        // カメラ振動スクリプトの取得
        Camera = GameObject.Find("Main Camera");
        vibration = Camera.GetComponent<VibrationCamera>();

        AngleTest = GameObject.Find("Target");
        TargtRenderer = AngleTest.GetComponent<SpriteRenderer>();
        Targetstate = AngleTest.GetComponent<TestTargetState>();
        AngleTest.transform.position = transform.position;
      
        // アニメーター取得
        anim = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatas>();


        // スプライトレンダーを取得
        if (AngleTest.transform.childCount > 0)
        {
            renderer = AngleTest.transform.GetChild(1).GetComponent<SpriteRenderer>();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //　ひびの始点を常に自分の座標に指定
        CrackPointList[0] = transform.position;

        if (crackmove.movestate == CrackAutoMove.MoveState.Walk)
        {
            //------------------------------------------------------
            // 状態によって処理
            switch (hammerstate)
            {
                case HammerState.NONE:

                    Move.SetMovement(true);
                    // 角度の可視化
                    //TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    //AngleTest.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                    // 前回の座標との距離を求める
                    float Distance = Vector3.Magnitude(CrackPointList[0] - OldFirstPoint);
                    if (Distance < 0.5f)
                    {
                        if (CrackManager.transform.childCount > 0)
                        {
                            NowCrack = CrackManager.transform.GetChild(CrackManager.transform.childCount - 1).GetComponent<CrackCreater>();
                            AddCrackFlg = true;
                        }
                    }
                    else
                    {
                        OldFirstPoint = Vector2.zero;
                        AddCrackFlg = false;
                    }

                    //　両方押しで溜め技
                    if (InputManager.GetNail_Left() && InputManager.GetNail_Right())
                    {
                        hammerstate = HammerState.POWER;
                    }

                    //トリガーを押したら方向決定状態
                    if (InputManager.GetNail_Right() && !InputManager.GetNail_Left())
                    {
                        // 前回の座標との距離を求める
                        //float Distance = Vector3.Magnitude(CrackPointList[0] - OldFirstPoint);
                        //-----------------------------------------------------------------------
                        // 前回の位置とあまり移動していなかくて、前のひびが残ってたらポイント追加
                        if (Distance < 0.5f && CrackManager.transform.childCount > 0)
                        {
                            ////　ひびを取得
                            //NowCrack = CrackManager.transform.GetChild(CrackManager.transform.childCount - 1).GetComponent<CrackCreater>();

                            //AddCrackFlg = true;

                        }
                        else
                        {
                            // 角度の可視化
                            //TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        }

                        //　照準(仮)が壁にめりこんでなかったら
                        if (!Targetstate.CheeckGround || (AddCrackFlg && Targetstate.CheeckGround))
                        {
                            hammerstate = HammerState.DIRECTION;
                        }
                    }

                    break;

                case HammerState.POWER:

                    //　移動できないようにする
                    Move.SetMovement(false);

                    if (!AddCrackFlg)
                    //-----------------------------------------------------------------------------
                    // 角度と距離から座標を計算
                    {
                        // 左スティックの入力から角度を取得する
                        Vector2 LeftStick = InputManager.GetMovement();

                        //----------------------------------------
                        //　スティックの入力があれば角度計算
                        if (LeftStick != Vector2.zero)
                        {
                            if (!AngleLook)
                            {
                                angle = Mathf.Atan2(LeftStick.y, LeftStick.x) * Mathf.Rad2Deg;
                                //angle += LeftStick.y + 6.0f * Time.deltaTime;

                                // 角度を正規化
                                if (angle < 0)
                                {
                                    angle += 360;
                                }
                            }

                            //　角度を45度ずつで管理
                            //angle = (((int)angle / 20)) * 20.0f;

                            //　角度を45度ずつで管理
                            //angle = ((int)(angle / 22.5f)) * 22.5f;


                        }
                        else
                        {
                            // なければ
                            angle = angle;

                        }

                    }

                        // 角度と距離からPoint座標を求める
                        CrackPointList[1] = new Vector2(CrackPointList[0].x + (MoveLength * Mathf.Cos(angle * (Mathf.PI / 180))), CrackPointList[0].y + (MoveLength * Mathf.Sin(angle * (Mathf.PI / 180))));

                        //デバッグ用
                        //AngleTest.transform.position = new Vector3(CrackPointList[1].x, CrackPointList[1].y, 0.0f);


                    //----------------------------------------------
                    //　両方押されていたら長さを更新
                    if (InputManager.GetNail_Left() && InputManager.GetNail_Right())
                    {
                        // 照準の非表示
                        //TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                        //AngleTest.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                        vibration.SetControlerVibration();
                        
                        if (!AddCrackFlg)
                        {
                            MoveLength += CrackPower * Time.deltaTime;
                        }
                        else
                        {
                            AddPower += (CrackPower / 2) * Time.deltaTime;
                        }

                        StartHaloAnimation();//←追加者:中川直登 アニメーション開始
                    }
                    else
                    {
                        if (!AddCrackFlg) {
                            if (MoveLength > CrackLength)
                            {
                                //　分割数を求める
                                int segment = (int)(MoveLength / CrackLength);

                                //　前方の分割
                                for (int i = 0; i < segment / 2; i++)
                                {

                                    CrackPointList.Insert(1, Vector2.Lerp(CrackPointList[0], CrackPointList[1], 0.5f));
                                    //Debug.Log(CrackPointList[1]);
                                }

                                //　後方の追加
                                for (int i = 0; i < segment - (segment / 2); i++)
                                {
                                    CrackPointList.Insert(CrackPointList.Count - 1,
                                        Vector2.Lerp(CrackPointList[CrackPointList.Count - 2], CrackPointList[CrackPointList.Count - 1], 0.5f));
                                }
                            }

                        }

                        // SE再生
                        vibration.SetVibration(0.5f);
                        se.PlaySE_Crack1();
                        se.PlayHammer();

                        // ヒットストップ初期化
                        playerStatus.SetHitStop(true);
                        anim.speed = 0.02f;
                        stopTime = 0.0f;

                        MoveLength = CrackLength;   //　長さの初期化
                                                    //　離されたら打ち込み状態にする
                        hammerstate = HammerState.HAMMER;

                        EndHaloAnimation();//←追加者:中川直登 アニメーション停止
                    }

                    
                    break;

                case HammerState.DIRECTION:

                    //　移動できないようにする
                    Move.SetMovement(false);

                    //　左を押されたら状態を戻す
                    if (InputManager.GetNail_Left())
                    {
                        hammerstate = HammerState.POWER;
                    }

                    if (!AddCrackFlg)
                    //-----------------------------------------------------------------------------
                    // 角度と距離から座標を計算
                    {
                        // 左スティックの入力から角度を取得する
                        Vector2 LeftStick = InputManager.GetMovement();

                        //----------------------------------------
                        //　スティックの入力があれば角度計算
                        if (LeftStick != Vector2.zero)
                        {
                            if (!AngleLook)
                            {
                                angle = Mathf.Atan2(LeftStick.y, LeftStick.x) * Mathf.Rad2Deg;
                                //angle += LeftStick.y + 6.0f * Time.deltaTime;

                                // 角度を正規化
                                if (angle < 0)
                                {
                                    angle += 360;
                                }
                                
                            }

                            //　角度を45度ずつで管理
                           // angle = ((int)(angle / 22.5f)) * 22.5f;
                          
                        }
                        else
                        {
                            // なければ
                            angle = angle;

                        }
                    }


                    // 角度と距離からPoint座標を求める
                    CrackPointList[1] = new Vector2(CrackPointList[0].x + (CrackLength * Mathf.Cos(angle * (Mathf.PI / 180))), CrackPointList[0].y + (CrackLength * Mathf.Sin(angle * (Mathf.PI / 180))));
                    Vector2 ArmPos = new Vector2(CrackPointList[0].x + (0.6f * Mathf.Cos(angle * (Mathf.PI / 180))), (CrackPointList[0].y - 0.25f) + (0.6f * Mathf.Sin(angle * (Mathf.PI / 180))));

                    //デバッグ用
                    AngleTest.transform.position = new Vector3(ArmPos.x, ArmPos.y, 0.0f);
                    AngleTest.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    //トリガーを離したらひび生成状態(アニメーション終了)
                    if (!InputManager.GetNail_Right())
                    {
                        //　照準(仮)が壁にめりこんでなかったら
                        if (!Targetstate.CheeckGround || (AddCrackFlg && Targetstate.CheeckGround))
                        {

                            // SE再生
                            vibration.SetVibration(0.5f);
                            se.PlaySE_Crack1();
                            se.PlayHammer();


                            // ヒットストップ初期化
                            playerStatus.SetHitStop(true);
                            anim.speed = 0.02f;
                            stopTime = 0.0f;

                            angle = 0.0f; //角度初期化
                            hammerstate = HammerState.HAMMER;


                        }
                        else
                        {
                            // Point座標を初期化
                            AngleTest.transform.position = CrackPointList[0];
                            // 照準非表示
                            //TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                            //AngleTest.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                            //// Point座標を初期化
                            //AngleTest.transform.position = CrackPointList[0];
                            //　移動制限解除
                            Move.SetMovement(true);
                            hammerstate = HammerState.NONE;
                        }

                    }


                    break;
                case HammerState.HAMMER:

                    //　待機時間計測
                    WaitHammerMeasure += Time.deltaTime;

                    //---------------------------------
                    // 待機時間経過していたら
                    if (WaitHammerMeasure > WaitHammer)
                    {
                        //-----------------------------------------------------
                        //　前回の位置から移動していなかったらポイントを追加
                        if (AddCrackFlg)
                        {
                            if (NowCrack != null)
                            {
                                //　溜めた力分伸ばす(通常は1回)
                                if (Power < AddPower)
                                {

                                    if (NowCrack.GetState() == CrackCreater.CrackCreaterState.CRAETED)
                                    {
                                        Move.SetMovement(true);
                                        NowCrack.SetState(CrackCreater.CrackCreaterState.ADD_CREATEBACK);
                                        Power++;
                                    }

                                }
                                else
                                {

                                    Power = 0;
                                    AddPower = 1;   //　力の初期化
                                    OldFirstPoint = CrackPointList[0];  // 生成時の座標を保存
                                    WaitHammerMeasure = 0.0f;       // 経過用変数初期化

                                    AngleTest.transform.position = CrackPointList[0];   // Point座標を初期化
                                    //Move.SetMovement(true);
                                    AddCrackFlg = false;
                                    hammerstate = HammerState.NONE;
                                }
                            }
                            else
                            {
                                OldFirstPoint = CrackPointList[0];  // 生成時の座標を保存
                                WaitHammerMeasure = 0.0f;       // 経過用変数初期化

                                AngleTest.transform.position = CrackPointList[0];   // Point座標を初期化
                                Move.SetMovement(true);
                                AddCrackFlg = false;
                                hammerstate = HammerState.NONE;

                                Debug.Log("ひびが見つかりません");
                            }

                        }
                        else
                        {
                            CallCrackCreater();  //ひび生成

                            OldFirstPoint = CrackPointList[0];  // 生成時の座標を保存
                            WaitHammerMeasure = 0.0f;       // 経過用変数初期化

                            AngleTest.transform.position = CrackPointList[0];   // Point座標を初期化
                            Move.SetMovement(true);
                            hammerstate = HammerState.NONE;
                        }
                    }

                    break;
                default:
                    Debug.Log("HammerStateに設定できない数値が代入されています");
                    break;
            }

            // 二宮追加
            // ヒットストップ
            if (stopTime < HitStopTime)
            {
                stopTime += Time.deltaTime;
            }
            else
            {
                // ヒットストップ終了
                anim.speed = 1f;
                playerStatus.SetHitStop(false);
            }

            //Debug.Log(NewCrackObj.GetComponent<CrackCreater>().GetState());


            // アニメーション関係
            if (TargtRenderer != null || renderer != null)
            {
                if (angle >= 45 && angle <= 135)
                {
                    renderer.sortingOrder = 8;
                    TargtRenderer.sortingOrder = 9;
                }
                else
                {
                    renderer.sortingOrder = 21;
                    TargtRenderer.sortingOrder = 22;
                }
            }

            if (anim.GetBool("accumulate") || anim.GetBool("angle"))
            {
                // 角度の可視化
                TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                AngleTest.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            else
            {
                // 角度の可視化
                TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                AngleTest.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }

            // ためアニメーション
            anim.SetBool("accumulate",(hammerstate == HammerState.POWER || hammerstate == HammerState.DIRECTION) && (angle < 45 || angle > 135));
            // ためアニメーション
            anim.SetBool("angle", (hammerstate == HammerState.POWER || hammerstate == HammerState.DIRECTION) && (angle >= 45 && angle <= 135));
            // ひびアニメーション
            anim.SetBool("crack", hammerstate == HammerState.HAMMER);
            // キャンセル
            anim.SetBool("cansel", hammerstate == HammerState.NONE);

        }
        //Debug.Log(hammerstate);
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
        //Destroy(obj.GetComponent<EdgeCollider2D>());

        // ネイル座標リストを初期化
        for (int i = 1; i < 2; i++)
        {
            CrackPointList[i] = Vector2.zero;
        }

        //　ポイントが2つより多かったら削除
        if(CrackPointList.Count > 2)
        {
            CrackPointList.RemoveRange(2, CrackPointList.Count - 2);
        }

    }

    //
    // 関数：StartHaloAnimation()
    //
    // 目的：Haloアニメーションを一回だけ再生する
    // 
    private void StartHaloAnimation()
    {

        if (_isStartHaloAnimation != true)
        {
            _haloEffect.Play();
            _isStartHaloAnimation = true;
        }
    }
    //
    // 関数：EndHaloAnimation()
    //
    // 目的：Haloアニメーションを強制的に終了する
    // 
    private void EndHaloAnimation() 
    {
        if(_isStartHaloAnimation == true) 
        {
            _haloEffect.End();
            _isStartHaloAnimation = false;
        }
    }

    //----------------------
    //　分岐の生成関数
    //　担当:菅眞心
    public int CreateBranch(GameObject _Branchobj, GameObject creater, CrackCreater _crackCreater, int StartBranch)
    {
        int OldBranchNum = 0;
        int RandomBranch = 0;
        //　生成する数をランダムで設定
        int[] RandomCreate = new int[Random.Range(1, 3)];

        for (int i = 0; i < RandomCreate.Length;i++)
        {
            RandomCreate[i] = Random.Range(1, 3);   //生成する向きを決める　1:右　2:左

            //--------------------------------------
            //　分岐用Listを設定、生成
            if (RandomCreate[i] == 1)
            {
                //右側
                {
                    while (RandomBranch == OldBranchNum)
                    {
                        RandomBranch = Random.Range(StartBranch, creater.transform.childCount - 1);
                    }
                    OldBranchNum = RandomBranch;
                    BranchCrackList[0] = new Vector2(creater.transform.GetChild(RandomBranch).gameObject.transform.position.x,
                        creater.transform.GetChild(RandomBranch).gameObject.transform.position.y);

                    //Debug.Log();

                    BranchCrackList[1] =
                    new Vector2(BranchCrackList[0].x + ((CrackLength / 2) * Mathf.Cos(Random.Range(angle + 40.0f, angle + 90.0f) * (Mathf.PI / 180))),
                    BranchCrackList[0].y + ((CrackLength / 2) * Mathf.Sin(Random.Range(angle + 40.0f, angle + 90.0f) * (Mathf.PI / 180))));

                    _crackCreater.SetPointList(BranchCrackList);
                    GameObject obj = Instantiate(_Branchobj);
                    obj.transform.parent = creater.transform;
                   
                }
            }
            if (RandomCreate[i] == 2)
            {
                //左側
                {
                    while (RandomBranch == OldBranchNum)
                    {
                        RandomBranch = Random.Range(StartBranch, creater.transform.childCount - 1);
                    }
                    OldBranchNum = RandomBranch;
                    BranchCrackList[0] = new Vector2(creater.transform.GetChild(RandomBranch).gameObject.transform.position.x,
                        creater.transform.GetChild(RandomBranch).gameObject.transform.position.y);
                    BranchCrackList[1] =
                    new Vector2(BranchCrackList[0].x + ((CrackLength / 2) * Mathf.Cos(Random.Range(angle - 90.0f, angle - 40.0f) * (Mathf.PI / 180))),
                    BranchCrackList[0].y + ((CrackLength / 2) * Mathf.Sin(Random.Range(angle - 90.0f, angle - 40.0f) * (Mathf.PI / 180))));

                    _crackCreater.SetPointList(BranchCrackList);
                    GameObject obj = Instantiate(_Branchobj);
                    obj.transform.parent = creater.transform;

                }
            }
        }

        return StartBranch = creater.transform.childCount - 1; //　末尾の要素を返却
    }

    //------------------------------
    // angleを取得する
    public Vector2 GetInput()
    {
        return InputManager.GetMovement();
    }

    public void SetAngleLook(bool _look)
    {
        AngleLook = _look;
    }

}

