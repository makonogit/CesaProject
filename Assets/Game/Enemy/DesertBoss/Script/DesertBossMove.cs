//-----------------------------------------
//　担当：菅眞心
//　内容：砂漠のボスの行動
//-----------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertBossMove : MonoBehaviour
{
    //--------------------------------------
    // 変数宣言
    
    //--------------------------------------
    // 外部取得
    [SerializeField,Header("生成するピラミッドオブジェクト")]
    private GameObject PyramidObj;

    [SerializeField, Header("ピラミッドの数")]
    private int PyramidNum;

    [SerializeField, Header("ピラミッドの生成位置オブジェクト")]
    private List<Transform> PyramidPos;

    private GameObject[] Pyramid_parent;  // ピラミッド生成親オブジェクト

    private int CoreNum;            // コアの番号

    [SerializeField, Header("ピラミッドが出てくるまでの時間")]
    private float WaitTime; 
    private float TimeMeasure;      // 時間計測用

    [SerializeField,Header("コアが露出してからピラミッドが降りるまでの時間")]
    private float EndTime;       

    public GameObject PyramidList;  // ピラミッド管理オブジェクト
    
    [SerializeField]
    private List<int> Appearance;       // 出現するピラミッド番号

    public bool Breaking = false;   // ピラミッドが壊れたか 

    private VibrationCamera vibration;  //　振動用
    public bool Setvibratioin = false;

    private float Appearance_probability;        //　出現確率

    [SerializeField,Header("ゲート")]
    private GateThrough gate;

    public enum DesertBossState
    {
        NONE,   // 何もしていない
        IDLE,   // 待機
        ATTACK, // 攻撃
        CLEAN,  // ピラミッドを片付ける
        END     // 攻撃終了
    }

    public DesertBossState BossState;  // ボスの状態管理用変数

    private Animator anim;             //　ボスのアニメーション

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------
        //　生成するピラミッドの数分ゲームオブジェクトを追加
        PyramidList = GameObject.Find("PyramidList");

        Pyramid_parent = new GameObject[3];
        //----------------------------------------------------
        //　ピラミッドの親取得
        for (int i = 0; i < 3; i++)
        {
            Pyramid_parent[i] = GameObject.Find("Pyramid" + (i + 1));
        }

        //--------------------------------------------
        //　ピラミッド生成
        for (int i = 0; i < PyramidNum; i++)
        {
            GameObject obj = Instantiate(PyramidObj);
            obj.transform.parent = PyramidList.transform;
            SpriteRenderer objrender = obj.GetComponent<SpriteRenderer>();
            objrender.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        // コアが入っているピラミッドを決める
        CoreNum = Random.Range(0, PyramidNum - 1);
        PyramidData Data = PyramidList.transform.GetChild(CoreNum).gameObject.GetComponent<PyramidData>();
        Data.InsideNum = 1;

        // メモリ確保
        Appearance.Add(0);
        Appearance.Add(0);
        Appearance.Add(0);

        // animatorを取得
        anim = transform.GetChild(0).GetComponent<Animator>();

        vibration = GameObject.Find("Main Camera").GetComponent<VibrationCamera>();

        Appearance_probability = 20.0f; //20％の確率に設定

        // 何もしていない状態にする
        BossState = DesertBossState.NONE;

    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーがゲートを超えたら
        if(gate.Throgh && BossState==DesertBossState.NONE)
        {
            // 敵が出てくる演出
            BossState = DesertBossState.IDLE;

        }

        // 生成中ピラミッドが存在しているか
        if(PyramidPos[0].childCount > 0 && PyramidPos[1].childCount > 0 && PyramidPos[2].childCount > 0)
        {
            //　あれば生成中か取得
            bool Pyramid1 = PyramidPos[0].GetChild(0).GetComponent<PyramidData>().MoveFlg;
            bool Pyramid2 = PyramidPos[1].GetChild(0).GetComponent<PyramidData>().MoveFlg;
            bool Pyramid3 = PyramidPos[2].GetChild(0).GetComponent<PyramidData>().MoveFlg;

            //　あれば片付け中か取得
            bool Pyramid1_c = PyramidPos[0].GetChild(0).GetComponent<PyramidData>().Clean;
            bool Pyramid2_c = PyramidPos[1].GetChild(0).GetComponent<PyramidData>().Clean;
            bool Pyramid3_c = PyramidPos[2].GetChild(0).GetComponent<PyramidData>().Clean;


            //　全て生成中だったら攻撃アニメーションにする
            anim.SetBool("Attack", Pyramid1 && Pyramid2 && Pyramid3);

            // どれかが生成中or片付け中で振動していなかったら
            if(((Pyramid1 && Pyramid2 && Pyramid3) || (Pyramid1_c && Pyramid2_c && Pyramid3_c)) && !Setvibratioin)
            {
                vibration.SetVibration(2.0f);
                Setvibratioin = true;
            }

        }


        if (BossState == DesertBossState.IDLE)
        {

            if (PyramidList.transform.childCount == PyramidNum)
            {
                // 時間計測
                TimeMeasure += Time.deltaTime;

                //------------------------------------
                //　時間経過で攻撃開始
                if (TimeMeasure > WaitTime)
                {
                    BossState = DesertBossState.ATTACK;
                    TimeMeasure = 0.0f;
                }
            }
           
            //　どれか1つが壊れたら片づける
            if(Breaking)
            {
                Setvibratioin = false;
                BossState = DesertBossState.CLEAN;
            }

            
        }

        //---------------------------------------
        //　攻撃状態
        if (BossState == DesertBossState.ATTACK)
        {
            BossAttack();
        }

        //---------------------------------------
        //　片付ける状態
        if(BossState == DesertBossState.CLEAN)
        {
            PyramidClean();

        }

        //---------------------------------------
        //　コアが露出、攻撃終了
        if(BossState == DesertBossState.END)
        {
            Debug.Log("終了");
            //------------------------------------------
            //　指定時間経過で全て片付ける
            if (TimeMeasure > EndTime)
            {
                PyramidClean();
                //Destroy(gameObject);
            }
            else
            {
                TimeMeasure += Time.deltaTime;
            }

        }
    }

    //------------------------------------
    //　ボスの攻撃処理
    private void BossAttack()
    {
        //-----------------------------------------
        //　出現するピラミッドを設定
        do {
            for (int i = 0; i < 3; i++)
            {
                //-----------------------------
                //　重複チェック
                int num = Random.Range(0, PyramidNum - 1);
                if (!Appearance.Contains(num))
                {
                    Appearance[i] = num;
                }
                else
                {
                    i--;
                }
            }

            //  3つとも壊れていたらもう一度設定
        } while ( (PyramidList.transform.GetChild(Appearance[0]).gameObject.GetComponent<PyramidData>().Breaked && 
                   PyramidList.transform.GetChild(Appearance[1]).gameObject.GetComponent<PyramidData>().Breaked &&
                   PyramidList.transform.GetChild(Appearance[2]).gameObject.GetComponent<PyramidData>().Breaked));


        //-----------------------------------------------------------
        // ピラミッドがなければピラミッドを生成(子オブジェクトにする)
        GameObject obj = PyramidList.transform.GetChild(Appearance[0]).gameObject;    // 左
        obj.transform.position = Pyramid_parent[0].transform.position;
        SpriteRenderer objrender = obj.GetComponent<SpriteRenderer>();
        objrender.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
      
        GameObject obj2 = PyramidList.transform.GetChild(Appearance[1]).gameObject;   // 真ん中
        obj2.transform.position = Pyramid_parent[1].transform.position;
        SpriteRenderer obj2render = obj2.GetComponent<SpriteRenderer>();
        obj2render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
     
        GameObject obj3 = PyramidList.transform.GetChild(Appearance[2]).gameObject;   // 右
        obj3.transform.position = Pyramid_parent[2].transform.position;
        SpriteRenderer obj3render = obj3.GetComponent<SpriteRenderer>();
        obj3render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        
        obj.transform.parent = Pyramid_parent[0].transform;
        obj2.transform.parent = Pyramid_parent[1].transform;
        obj3.transform.parent = Pyramid_parent[2].transform;


        BossState = DesertBossState.IDLE;
    }


    //---------------------------------
    //　ピラミッドを片付ける処理
    private void PyramidClean()
    {
        Pyramid_parent[0].transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
        Pyramid_parent[1].transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
        Pyramid_parent[2].transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
    }

}
