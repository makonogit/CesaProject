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

    private int CoreNum;            // コアの番号

    [SerializeField, Header("ピラミッドが出てくるまでの時間")]
    private float WaitTime; 
    private float TimeMeasure;      // 時間計測用

    public GameObject PyramidList;  // ピラミッド管理オブジェクト
    
    [SerializeField]
    private List<int> Appearance;       // 出現するピラミッド番号

    public bool Breaking = false;   // ピラミッドが壊れたか 

    public enum DesertBossState
    {
        NONE,   // 何もしていない
        ATTACK, // 攻撃
        CLEAN,  // ピラミッドを片付ける
    }

    public DesertBossState BossState;  // ボスの状態管理用変数

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------
        //　生成するピラミッドの数分ゲームオブジェクトを追加
        PyramidList = GameObject.Find("PyramidList");

        for(int i = 0; i < PyramidNum; i++)
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

        // 何もしていない状態にする
        BossState = DesertBossState.NONE;

    }

    // Update is called once per frame
    void Update()
    {
        if (BossState == DesertBossState.NONE)
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
                BossState = DesertBossState.CLEAN;
            }

            
        }

        //---------------------------------------
        //　攻撃状態
        if(BossState == DesertBossState.ATTACK)
        {
            BossAttack();
        }

        //---------------------------------------
        //　片付ける状態
        if(BossState == DesertBossState.CLEAN)
        {
            PyramidClean();

        }
    }

    //------------------------------------
    //　ボスの攻撃処理
    private void BossAttack()
    {
        //-----------------------------------------
        //　出現するピラミッドを設定
        for(int i = 0; i < 3; i++)
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


        //-----------------------------------------------------------
        // ピラミッドがなければピラミッドを生成(子オブジェクトにする)
        GameObject Pyramid1 = GameObject.Find("Pyramid1");
        GameObject obj = PyramidList.transform.GetChild(Appearance[0]).gameObject;    // 左
        obj.transform.position = Pyramid1.transform.position;
        SpriteRenderer objrender = obj.GetComponent<SpriteRenderer>();
        objrender.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
      
        GameObject Pyramid2 = GameObject.Find("Pyramid2");
        GameObject obj2 = PyramidList.transform.GetChild(Appearance[1]).gameObject;   // 真ん中
        obj2.transform.position = Pyramid2.transform.position;
        SpriteRenderer obj2render = obj2.GetComponent<SpriteRenderer>();
        obj2render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
     
        GameObject Pyramid3 = GameObject.Find("Pyramid3");
        GameObject obj3 = PyramidList.transform.GetChild(Appearance[2]).gameObject;   // 右
        obj3.transform.position = Pyramid3.transform.position;
        SpriteRenderer obj3render = obj3.GetComponent<SpriteRenderer>();
        obj3render.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        
        obj.transform.parent = Pyramid1.transform;
        obj2.transform.parent = Pyramid2.transform;
        obj3.transform.parent = Pyramid3.transform;


        BossState = DesertBossState.NONE;
    }


    //---------------------------------
    //　ピラミッドを片付ける処理
    private void PyramidClean()
    {
        GameObject Pyramid1 = GameObject.Find("Pyramid1");
        GameObject Pyramid2 = GameObject.Find("Pyramid2");
        GameObject Pyramid3 = GameObject.Find("Pyramid3");

        Pyramid1.transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
        Pyramid2.transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;
        Pyramid3.transform.GetChild(0).gameObject.GetComponent<PyramidData>().Clean = true;

    }

}
