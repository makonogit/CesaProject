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

    [SerializeField,Header("釘が生成可能な距離")]
    private float NailsCreateDistance = 2.5f;   //釘が生成可能な距離
    public float NailsDistance;                 //前回の釘との距離
    [SerializeField,Header("釘が生成可能か")]
    private bool NailsCreateFlg = true;         //釘が生成可能か
    public bool MousePointaHit = false;         //マウスポインタが壁に当たっているか

    bool CreateCrack = false;                   //ひびが生成されたか
    public List<Vector2> NailsPoint;            //釘の座標を取得

    private GameObject MousePointa;             //マウスポインタ用オブジェクト
    private SpriteRenderer MousePointaRender;   //マウスポインタのスプライトレンダー

    // Start is called before the first frame update
    void Start()
    {
        HammerNails = 0;
        NailsDistance = 0.0f;

        //--------------------------------------------
        // 釘の管理スクリプトを取得
        HaveNails = GetComponent<HaveNails>();
        
        NailsTrans = NailPrehubObj.transform;

        //--------------------------------------------
        //　ひび生成用スクリプトを取得
        Order = CrackPrefab.GetComponent<CrackOrder>();

        //--------------------------------------------
        //マウスポインタ用オブジェクトを取得
        MousePointa = GameObject.Find("MousePointa");
        MousePointaRender = MousePointa.GetComponent<SpriteRenderer>();

        //--------------------------------------------
        //InputManagrを取得
        PlayerInputManager = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //マウスポインタの座標を常に更新
        Vector3 MousePos = (Vector2)Camera.main.ScreenToWorldPoint(ScriptPIManager.GetMousePos());
        MousePointa.transform.position = MousePos;

        //----------------------------------------
        //釘を持っていたら釘を画面に打つ
        if (HaveNails.NailsNum > 0)
        {
            //---------------------------------------------------------------------------
            //前回打った釘との距離を求める(生成されていなかったらプレイヤーとの距離)
            if (HammerNails > 0)
            {
                NailsDistance = Vector3.Distance(MousePos, NailsPoint[HammerNails - 1]);
            }
            else
            {
                NailsDistance = Vector3.Distance(MousePos, this.transform.position);
            }

            //----------------------------------------------------------------
            //距離が生成可能な距離よりも離れているかで生成可能フラグを指定
            if(NailsDistance < NailsCreateDistance && !MousePointaHit)
            {
                NailsCreateFlg = true;
            }
            else
            {
                NailsCreateFlg = false;
            }
           
            //----------------------------------------------------------------
            //釘生成可能かどうかでマウスポインタの色を変更
            MousePointaRender.color = NailsCreateFlg == true ? Color.white : Color.red;

            //---------------------------------------------------------------------------
            //左クリック検知+ひび生成中じゃない+釘を打てる範囲ならマウス座標に釘を生成
            if (Mouse.current.leftButton.wasPressedThisFrame && !CreateCrack && NailsCreateFlg)
            {
                NailsTrans.position = new Vector3(ScriptPIManager.GetMousePos().x, ScriptPIManager.GetMousePos().y, 0);
                // ワールド座標に変換Zのカメラ座標がおかしくなるのでVector2型にキャスト変換して対処
                NailsTrans.position = (Vector2)Camera.main.ScreenToWorldPoint(NailsTrans.position);

                //ポイント座標を追加
                NailsPoint.Add(item:NailsTrans.position);

                Instantiate(NailPrehubObj,NailsTrans.position,Quaternion.identity);

                HammerNails++;
                HaveNails.NailsNum--;
            }
           
        }

        //----------------------------------------
        //壁に打った釘が1個以上あればひびを生成
        if (HammerNails > 0)
        {
            //-------------------------------------
            //右クリック検知でひびを生成
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                CreateCrack = true;
            }

            //------------------------------------
            //ひび生成
            if (CreateCrack)
            {
                Order.numSummon = HammerNails;
                Order.CrackPos = NailsPoint[NailsPoint.Count - 1];
                Order.CrackFlg = CreateCrack;
                //Quaternion.LookRotation()
                HammerNails = 0;
                Instantiate(CrackPrefab, NailsPoint[NailsPoint.Count - 1], Quaternion.identity);
                CreateCrack = false;
            }
        }



    }
}
