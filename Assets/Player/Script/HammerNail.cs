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
    private HaveNails HaveNails;          //釘の所持数を取得

    private int HammerNails;              //釘を壁に打った数

    [Header("釘のプレハブオブジェクト")]
    public GameObject NailPrehubObj;      //釘のプレハブ

    Transform NailsTrans;                 // 釘のTransForm

    // 外部取得
    private GameObject PlayerInputManager;      // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    public GameObject CrackPrefab;              // ひび生成用プレハブオブジェクト
    private CrackOrder Order;                   // ひび生成用スクリプト

    [SerializeField, Header("釘が生成可能な距離")]
    private float NailAddArea;                   //釘を拡大する範囲
    private float HammerNailArea;                //釘を生成可能な範囲を保存
    private float NailsDistance;                 //前回の釘との距離
    [SerializeField, Header("釘が生成可能か")]
    private bool NailsCreateFlg = true;           //釘が生成可能か
    //public bool MousePointaHit = false;         //マウスポインタが壁に当たっているか

    [Header("ひびが生成されたか")]
    public bool CreateCrack = false;            //ひびが生成されたか
    bool CrackVibration = false;                //振動による影響

    [Header("釘の座標")]
    public List<Vector2> NailsPoint;            //釘の座標を取得

    // private GameObject MousePointa;             //マウスポインタ用オブジェクト
    // private SpriteRenderer MousePointaRender;   //マウスポインタのスプライトレンダー

    private GameObject NailTarget;      //釘照準オブジェクト
    private Transform NailTargetTrans;  //釘照準オブジェクトのTransForm

    private NailTargetMove NailTargetMove;      //釘の移動

    private GameObject FallGage;        //壁の崩壊度UI
    private FallWall fallwall;          //壁の崩壊度スクリプト

    //----------------------------
    //ひびの拡大用コライダー関係
    private GameObject ChilCrackArea;           //ひび拡大用子オブジェクト 
    private CircleCollider2D CircleCol;         //子オブジェクトのコライダーの情報
    [SerializeField,Header("コライダーの最大サイズ")]
    private float MaxColSize;                   //コライダーの最大サイズ
    [SerializeField, Header("コライダーの拡大スピード")]
    private float ColExtendSpeed;               //コライダーの拡大スピード


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
        HaveNails = GetComponent<HaveNails>();

        NailsTrans = NailPrehubObj.transform;

        //--------------------------------------------
        //　ひび生成用スクリプトを取得
        Order = CrackPrefab.GetComponent<CrackOrder>();

        //--------------------------------------------
        //マウスポインタ用オブジェクトを取得
        //MousePointa = GameObject.Find("Fairy");
        //MousePointaRender = MousePointa.GetComponent<SpriteRenderer>();

        //--------------------------------------------
        //釘照準オブジェクトの取得
        NailTarget = GameObject.Find("NailTarget");
        NailTargetTrans = NailTarget.transform;
        NailTargetMove = NailTarget.GetComponent<NailTargetMove>();

        HammerNailArea = NailTargetMove.Radius;

        //----------------------
        //壁崩壊UIの情報取得
        FallGage = GameObject.Find("Gage");
        fallwall = FallGage.GetComponent<FallWall>();

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
        //--------------------------------------------
        //CrackCreaterを取得  //―追加担当者：中川直登―//
        _creater = _crackCreaterObj.GetComponent<CrackCreater>();


    }

    // Update is called once per frame
    void Update()
    {

        //子オブジェクトと座標を同期
        ChilCrackArea.transform.position = transform.position;
        ////マウスポインタの座標を常に更新
        //Vector3 MousePos = (Vector2)Camera.main.ScreenToWorldPoint(ScriptPIManager.GetMousePos());
        //MousePointa.transform.position = MousePos;

        //----------------------------------------
        //釘を持っていたら釘を画面に打つ
        if (HaveNails.NailsNum > 0)
        {
            //---------------------------------------------------------------------------
            //前回打った釘との距離を求める(生成されていなかったらプレイヤーとの距離)
            if (HammerNails > 0)
            {
                NailsDistance = Vector3.Distance(NailTargetTrans.position, NailsPoint[HammerNails - 1]);
            }
            else
            {
                NailsDistance = Vector3.Distance(NailTargetTrans.position, this.transform.position);
            }

            //----------------------------------------------------------------
            //距離が生成可能な距離よりも離れているかで生成可能フラグを指定
            if (NailsDistance < NailTargetMove.Radius && ScriptPIManager.GetPlayerMode() == global::PlayerInputManager.PLAYERMODE.AIM)
            {
                NailsCreateFlg = true;
            }
            else
            {
                NailsCreateFlg = false;
            }

            //----------------------------------------------------------------
            //釘生成可能かどうかでマウスポインタの色を変更
            // MousePointaRender.color = NailsCreateFlg == true ? Color.white : Color.red;

            //---------------------------------------------------------------------------
            //左スティック押し込み検知+ひび生成中じゃない+釘を打てる範囲なら釘を生成
            if (Gamepad.current.leftStickButton.wasPressedThisFrame && !CreateCrack && NailsCreateFlg)
            {

                NailsTrans.position = NailTargetTrans.position;
                // ワールド座標に変換Zのカメラ座標がおかしくなるのでVector2型にキャスト変換して対処
                //NailsTrans.position = (Vector2)Camera.main.ScreenToWorldPoint(NailsTrans.position);

                //ポイント座標を追加
                NailsPoint.Add(NailsTrans.position);
                obj = Instantiate(NailPrehubObj, NailsTrans.position, Quaternion.identity) as GameObject;
                obj.AddComponent<PolygonCollider2D>();

                NailTargetMove.Radius += NailAddArea;
                HammerNails++;
                HaveNails.NailsNum--;

                //---------------------------------------------------------------------------
                //―追加担当者：二宮怜―//
                // 撃たれた瞬間の状態を保持して敵の移動を止める
                MomentHitNails = true;
                ColLimitTime = 0.0f;

                Debug.Log("?????????????????????????");

            }
            //―追加担当者：二宮怜―//
            if(MomentHitNails)
            {
                //---------------------------------------------------------------------------
                // 打ち付けられた瞬間以外のフレームに釘の当たり判定は必要ない

                // 最新のオブジェクトにコライダーがついていたら入る
                if (ColLimitTime > DestroyNailColTime)
                {
                    if (obj.GetComponent<PolygonCollider2D>())
                    {
                        // コライダー消す
                        Destroy(obj.GetComponent<PolygonCollider2D>());
                        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        // 釘を打つ瞬間以外基本false
                       
                    }
                    MomentHitNails = false;
                }
                ColLimitTime += Time.deltaTime;

            }
        }

        //-------------------------------------------------
        //壁に打った釘が2個以上で釘投擲モード解除でひび生成
        if(HammerNails > 1 && Gamepad.current.bButton.wasPressedThisFrame)
        {
            HammerNails = 0;
            CreateCrack = true;
        }

        //------------------------------------
        //ひび生成
        if (CreateCrack)
        {
            NailTargetMove.Radius = HammerNailArea;  //釘生成範囲を初期化

            CallCrackCreater();//―追加担当者：中川直登―//

            fallwall.CreateCrackFlg = CreateCrack;  //ひびの生成情報を崩壊スクリプトに渡す
            fallwall.NowCrackObj = NewCrackObj;
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
        _creater.SetPointList(NailsPoint);
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
