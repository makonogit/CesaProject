//-----------------------------------------
//担当者：菅眞心
//役割  ：釘を打つ
//-----------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HammerNail : MonoBehaviour
{
    private int HammerNails;              //釘を壁に打った数

    [Header("釘のプレハブオブジェクト")]
    public GameObject NailPrehubObj;      //釘のプレハブ

    public Transform NailsTrans;                 // 釘のTransForm

    // 外部取得
    // 入力関係
    private GameObject PlayerInputManager;      // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private InputTrigger ScriptPITrigger;       // InputTriggerを取得すりゅ変数

    public GameObject CrackPrefab;              // ひび生成用プレハブオブジェクト
    private CrackOrder Order;                   // ひび生成用スクリプト

    [SerializeField, Header("釘が生成可能な距離")]
    private float NailAddArea;                   //釘を拡大する範囲
    private float HammerNailArea;                //釘を生成可能な範囲を保存
    private float NailsDistance;                 //前回の釘との距離
    [SerializeField, Header("釘が生成可能か")]
    private bool NailsCreateFlg = true;           //釘が生成可能か

    private GameObject UsedNailManager;           //壁に打った釘の管理用親オブジェ

    //public bool MousePointaHit = false;         //マウスポインタが壁に当たっているか

    [Header("ひびが生成されたか")]
    public bool CreateCrack = false;            //ひびが生成されたか
    bool CrackVibration = false;                //振動による影響

    [Header("釘の座標")]
    public List<Vector2> NailsPoint;            //釘の座標を取得

    private GameObject NailTarget;      //釘照準オブジェクト
    private Transform NailTargetTrans;  //釘照準オブジェクトのTransForm
    private SpriteRenderer TargetRender;

    private NailTargetMove NailTargetMove;      //釘の移動

    private GameObject FallGage;        //壁の崩壊度UI
    private FallWall fallwall;          //壁の崩壊度スクリプト

    // 二宮追加
    private GameObject Target;
    private NailTargetMove TargetMove;
    private PlayerStatas status;

    // SEの効果音-------担当：尾花--------
    [Header("効果音")]
    private AudioSource audioSource;  // 取得したAudioSourceコンポーネント
    [SerializeField] AudioClip sound1; // 音声ファイル


    //----------------------------
    //ひびの拡大用コライダー関係
    private GameObject ChilCrackArea;           //ひび拡大用子オブジェクト 
    private CircleCollider2D CircleCol;         //子オブジェクトのコライダーの情報
    [SerializeField,Header("コライダーの最大サイズ")]
    private float MaxColSize;                   //コライダーの最大サイズ
    [SerializeField, Header("コライダーの拡大スピード")]
    private float ColExtendSpeed;               //コライダーの拡大スピード

    //---------------------------------------------------------------
    private CircleCollider2D NailAreaCol;      //ひびがつながる範囲

    //---------------------------------------
    //打ち込み状態

    public enum HammerState
    {
        NONE,   //何もしていない
        NAILSET,//構える
        HAMMER, //打つ
    }

    public HammerState _HammerState;

    GetCrackPoint getCrackPoint; //ひびのリストを取得
    SetNailList setNailList;     //生成済み状態にするため

    //―追加担当者：中川直登―//
    [Header("ひびを作るobj")]
    public GameObject _crackCreaterObj;
    [System.NonSerialized]
    public GameObject NewCrackObj;  //新しいヒビのオブジェクト
    CrackCreater _creater;
    //――――――――――――//

    //―追加担当者：二宮怜―//
    [Header("ひびが撃たれた瞬間のみtrueになる")]
    public bool MomentHitNails = false;

    public float ColLimitTime; // 使用済み釘が生成された瞬間からの時間
    private float DestroyNailColTime = 0.2f; // 使用済みの釘のコライダーが消える時間 

    GameObject obj; // 作ったゲームオブジェクトをキャストするため



    // Start is called before the first frame update
    void Start()
    {
        HammerNails = 0;

        //--------------------------------------------
        // 釘の管理スクリプトを取得
        status = GetComponent<PlayerStatas>();

        NailsTrans = NailPrehubObj.transform;

        //--------------------------------------------
        //　ひび生成用スクリプトを取得
        Order = CrackPrefab.GetComponent<CrackOrder>();

        getCrackPoint = GetComponentInChildren<GetCrackPoint>();

        //--------------------------------------------
        //釘照準オブジェクトの取得
        NailTarget = GameObject.Find("NailTarget");
        NailTargetTrans = NailTarget.transform;
        NailTargetMove = NailTarget.GetComponent<NailTargetMove>();
        TargetRender = NailTarget.GetComponent<SpriteRenderer>();
        HammerNailArea = NailTargetMove.Radius;

        //----------------------
        //壁崩壊UIの情報取得
        //FallGage = GameObject.Find("Gage");
        //fallwall = FallGage.GetComponent<FallWall>();

        //--------------------------------------------
        // 子オブジェクトのCicreColliderを取得
        ChilCrackArea = GameObject.Find("CrackGrowArea");
        CircleCol = ChilCrackArea.GetComponent<CircleCollider2D>();
        MaxColSize = 4.0f;
        ColExtendSpeed = 15.0f;

        //--------------------------------------------
        //InputManagrを取得
        PlayerInputManager = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        ScriptPITrigger = PlayerInputManager.GetComponent<InputTrigger>();

        //--------------------------------------------
        //CrackCreaterを取得  //―追加担当者：中川直登―//
        _creater = _crackCreaterObj.GetComponent<CrackCreater>();

        //---------------------------------------------
        //打ち込み状態を何もしていないにする
        _HammerState = HammerState.NONE;

        Target = GameObject.Find("NailTarget");
        TargetMove = Target.GetComponent<NailTargetMove>();

        //-----------------------------------------------
        // 使用済み釘管理オブジェクトを取得
        UsedNailManager = GameObject.Find("UsedNailManager");

        //------------------------------------------------
        // AudioSourceを取得----追加担当：尾花-------
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        //子オブジェクトと座標を同期
        ChilCrackArea.transform.position = transform.position;

        //-----------------------------------------------------------
        //状態遷移
        //片方だけ押し込みされたら構える状態にする
        if (((!ScriptPITrigger.GetNailTrigger_Left() && ScriptPITrigger.GetNailTrigger_Right()) ||
            (ScriptPITrigger.GetNailTrigger_Left() && !ScriptPITrigger.GetNailTrigger_Right())) &&
            _HammerState == HammerState.NONE)
        {
            _HammerState = HammerState.NAILSET;

        }
        //----------------------------------------------------------
        //構え中に両方押し込まれたら釘を打てない状態にする
        if (ScriptPIManager.GetNail_Left() && ScriptPIManager.GetNail_Right() &&
           _HammerState == HammerState.NAILSET)
        {
            _HammerState = HammerState.NONE;
        }

        //----------------------------------------------------------
        //両方離されたら打ち込み状態にする
        if (!ScriptPIManager.GetNail_Left() && !ScriptPIManager.GetNail_Right())
        {
            //離された瞬間打ち込み状態にする
            if (_HammerState == HammerState.NAILSET)
            {
                _HammerState = HammerState.HAMMER;
            }
            else
            {
                _HammerState = HammerState.NONE;
            }

        }

        //----------------------------------------
        //釘を持っていたら釘を画面に打つ
        if (status.GetNail() > 0)
        {
            //---------------------------------------------------------------------------
            //前回打った釘との距離を求める(生成されていなかったらプレイヤーとの距離)
            //if (HammerNails > 0)
            //{
            //    NailsDistance = Vector3.Distance(NailTargetTrans.position, NailsPoint[HammerNails - 1]);
            //}
            //else
            //{
            //    NailsDistance = Vector3.Distance(NailTargetTrans.position, this.transform.position);
            //}

            //----------------------------------------------------------------
            //距離が生成可能な距離よりも離れているかで生成可能フラグを指定
            //if (NailsDistance < NailTargetMove.Radius)
            //{
            //    NailsCreateFlg = true;
            //}
            //else
            //{
            //    NailsCreateFlg = false;
            //}

            

            //---------------------------------------------------------------------------
            //構えていたら釘の範囲を可視化
            if (_HammerState == HammerState.NAILSET)
            {

            }

            //---------------------------------------------------------------------------
            //打ち込み状態なら生成
            if (_HammerState == HammerState.HAMMER && TargetRender.color == Color.cyan)
            {
                //---------------------------------------------------------------------------------
                // 音声ファイルを再生する-----担当：尾花-------
                //if (!audioSource.isPlaying)
                //{
                //    audioSource.PlayOneShot(sound1);
                //}

                NailsTrans.position = NailTargetTrans.position;
                
                //ポイント座標を追加
                //NailsPoint.Add(NailsTrans.position);
                obj = Instantiate(NailPrehubObj, transform.position, Quaternion.identity) as GameObject;
                
                // UsedNailManagerの子オブジェクトとして生成
                obj.transform.parent = UsedNailManager.transform;
                //obj.AddComponent<PolygonCollider2D>();
                
                NailTargetMove.Radius += NailAddArea;
                HammerNails++;
                status.SetNail(status.GetNail() - 1);

                //---------------------------------------------------------------------------
                //―追加担当者：二宮怜―//
                // 撃たれた瞬間の状態を保持して敵の移動を止める
                MomentHitNails = true;
                ColLimitTime = 0.0f;
                //---------------------------------------------------------------------------

            }
           
        }

        //―追加担当者：二宮怜―//
        //if (MomentHitNails)
        //{
        //    //---------------------------------------------------------------------------
        //    // 打ち付けられた瞬間以外のフレームに釘の当たり判定は必要ない

        //    // 最新のオブジェクトにコライダーがついていたら入る
        //    if (ColLimitTime > DestroyNailColTime)
        //    {
        //        if (obj.GetComponent<PolygonCollider2D>())
        //        {
        //            // コライダー消す
        //            Destroy(obj.GetComponent<PolygonCollider2D>());

        //        }

        //        // 釘を打つ瞬間以外基本false
        //        MomentHitNails = false;
        //    }
        //    ColLimitTime += Time.deltaTime;

        //}

        //-------------------------------------------------
        //壁に打った釘が2個以上でBボタン押すとひび生成
        if (HammerNails > 1 && getCrackPoint.GetPointLest().Count >= 2 &&
            (ScriptPIManager.GetNail_Left() && ScriptPIManager.GetNail_Right()))
        {
            Debug.Log("ひび生成");
            HammerNails = 0;
            CreateCrack = true;
        }

        //------------------------------------
        //ひび生成
        if (CreateCrack)
        {

            CallCrackCreater();//―追加担当者：中川直登―//

            //fallwall.CreateCrackFlg = CreateCrack;  //ひびの生成情報を崩壊スクリプトに渡す
            //fallwall.NowCrackObj = NewCrackObj;

            TargetMove.CreateCrack = CreateCrack;

            for (int i = 1; i < getCrackPoint.objectList.Count; i++)
            {
                setNailList = getCrackPoint.objectList[i].GetComponentInChildren<SetNailList>();
                setNailList.Crackend = true;
            }

            //-------------------------------------------------
            //ひびの生成が終了したらポイントリストを初期化する
            getCrackPoint.objectList.Clear();
            getCrackPoint.objectList.Add(gameObject);
            getCrackPoint.GetPointLest().Clear();
            getCrackPoint.SetPoint(transform.position);

            CreateCrack = false;
            CrackVibration = true;
            CircleCol.enabled = true;
        }

        //------------------------------------
        //ひびを成長させる範囲を拡大していく
        if (CrackVibration)
        {
            //------------------------
            //コライダーを拡大する
            if (CircleCol.radius < MaxColSize)
            {
                CircleCol.radius += ColExtendSpeed * Time.deltaTime;
            }
            else
            {
                //--------------------------------
                //最大まで大きくなったら初期化
                CircleCol.radius = 0.0f;
                CrackVibration = false;
                CircleCol.enabled = false;
            }
        }

    }


    
    //-----------------------------------------------------------------
    //―CrackCreaterを呼ぶ関数―           //―追加担当者：中川直登―//
    private void CallCrackCreater()
    {
        // ネイル座標リストを渡す
        _creater.SetPointList(getCrackPoint.GetPointLest());
        // CrackCreaterを作る
        NewCrackObj = Instantiate(_crackCreaterObj);
        // ネイル座標リストを初期化
        for (; 0 < NailsPoint.Count;)
        {
            NailsPoint.RemoveAt(0);
        }
    }
    //-----------------------------------------------------------------

}
