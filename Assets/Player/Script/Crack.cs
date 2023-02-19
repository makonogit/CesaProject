//-----------------------------------
//担当：菅眞心
//内容：ひびの角度・座標・数を指定して生成
//-----------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    // ひび生成用
    [Header("ひびを入れる力(デバッグ用に表示)")]
    public float CrackPower;   //ひびを入れる力

    [Header("ひびを入れる力の加算数")]
    public float AddCrackPower;

    [Header("最大ひび生成数")]
    public float MaxCrackNum;

    [System.NonSerialized]
    public Vector3 CrackPos;    //ひびの座標

    [System.NonSerialized]
    public bool CreateCrackFlg = false;

    // 外部取得
    public GameObject PlayerInputManager;       // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数
    private Transform Trans;                    // Transformを取得する変数
    public GameObject PrehubObject;             // ひびを生成するオブジェクト

    CrackOrder Order;

    [System.NonSerialized]
    public bool RadialCrackFlg = false;   //放射線状にひびを生成

    // 二宮追加
    private PlayerDamaged playerDamaged; // HP減らすためのセッター呼び出す用


    //----------------------------------------------------------------------------------------------------------
    // - 初期化処理 -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        
        Order = PrehubObject.GetComponent<CrackOrder>();
        
        //----------------------------------------------------------------------------------------------------------
        // PlayerのTransformを取得する
        Trans = this.GetComponent<Transform>();

        CrackPower = 0.0f;

        // 二宮追加
        playerDamaged = GetComponent<PlayerDamaged>();

    }


    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------
        //はじき入力
        if((ScriptPIManager.GetCarackPower().x == 1 || ScriptPIManager.GetCarackPower().x == -1) ||
            ScriptPIManager.GetCarackPower().y == 1 || ScriptPIManager.GetCarackPower().y == -1)
        {
            //----------------------------------------------------------------------------------------------------------
            // ひびを入れる強さを加算
            CrackPower += AddCrackPower * Time.deltaTime;

            //-----------------------------------------------------------
            //入力量によって角度を求める
            Order.CrackAngle = Mathf.Atan2(ScriptPIManager.GetRmove().y, ScriptPIManager.GetRmove().x) * Mathf.Rad2Deg;

            //-----------------------------------------------------------
            //ひびの座標を更新
            CrackPos = transform.position;

        }
        else if (CrackPower > 0.0f)
        {
            //パワーによって放射線状にひびが入る
            if (CrackPower < MaxCrackNum)
            {
                CreateCrackFlg = true;
            }
            else if(CrackPower > MaxCrackNum)
            {
                CrackPower = 0.0f;
               // RadialCrackFlg = true;
            }
        }

        //--------------------------
        //ひびの個数を指定して生成
        if (CreateCrackFlg)
        {
            //-------------------------------
            // HP減少情報セット
            playerDamaged.SetCrackInfo(CrackPower);

            Order.CrackFlg = CreateCrackFlg;
            Order.numSummon = CrackPower;
            Order.CrackPos = CrackPos;
            //-----------------------------
            //生成指定したら初期化して終了
            CrackPower = 0.0f;
            CreateCrackFlg = false;
            GameObject obj = Instantiate(PrehubObject, CrackPos, Quaternion.identity);
        }

        //-----------------------------------------
        //放射線状にひびを入れる
        if (RadialCrackFlg)
        {
            //for(int i = 0; i< 3; i++)
            //{
            //    Order.CrackFlg = RadialCrackFlg;
            //    Order.numSummon = Random.Range(CrackPower - 5.0f,CrackPower);
            //    Order.CrackPos = new Vector3(CrackPos.x - i * 0.5f,CrackPos.y,CrackPos.z);
            //    //-----------------------------
            //    //生成指定したら初期化して終了
            //    CrackPower = 0.0f;
            //    GameObject obj = Instantiate(PrehubObject, CrackPos, Quaternion.identity);
            //}
            RadialCrackFlg = false;
        }

        //if(CrackPower.x > 0.0f)
        //{
        //    Order.numSummon = 10;
        //    Order.CrackFlg = true;
        //}

    }
}
