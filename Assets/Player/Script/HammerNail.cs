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
    private float NailsDistance;                 //前回の釘との距離
    [SerializeField, Header("釘が生成可能か")]
    private bool NailsCreateFlg = true;         //釘が生成可能か
    //public bool MousePointaHit = false;         //マウスポインタが壁に当たっているか

    bool CreateCrack = false;                   //ひびが生成されたか
    [Header("釘の座標")]
    public List<Vector2> NailsPoint;            //釘の座標を取得

    // private GameObject MousePointa;             //マウスポインタ用オブジェクト
    // private SpriteRenderer MousePointaRender;   //マウスポインタのスプライトレンダー

    private GameObject NailTarget;      //釘照準オブジェクト
    private Transform NailTargetTrans;  //釘照準オブジェクトのTransForm

    private NailTargetMove NailTargetMove;         //釘の移動

    //―追加担当者：中川直登―//
    [Header("ひびを作るobj")]
    public GameObject _crackCreaterObj;
    CrackCreater _creater;
    //――――――――――――//


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

        NailsDistance = NailTargetMove.Radius;

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

                Instantiate(NailPrehubObj, NailsTrans.position, Quaternion.identity);

                NailTargetMove.Radius += NailAddArea;
                HammerNails++;
                HaveNails.NailsNum--;
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
            Debug.Log("!");
            NailTargetMove.Radius = NailsDistance;
            CallCrackCreater();//―追加担当者：中川直登―//
            CreateCrack = false;
        }
    }


    
    //-----------------------------------------------------------------
    //―CrackCreaterを呼ぶ関数―           //―追加担当者：中川直登―//
    private void CallCrackCreater()
    {
        // ネイル座標リストを渡す
        _creater.SetPointList(NailsPoint);
        // CrackCreaterを作る
        GameObject obj = Instantiate(_crackCreaterObj);
        // ネイル座標リストを初期化
        for (; 0 < NailsPoint.Count;)
        {
            NailsPoint.RemoveAt(0);
        }
    }
    //-----------------------------------------------------------------
}
