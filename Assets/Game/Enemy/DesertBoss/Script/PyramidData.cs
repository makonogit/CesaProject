//-------------------------------------
// 担当：菅眞心
// 内容：ピラミッドのデータ
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidData : MonoBehaviour
{
    //----------------------------------------
    // 変数宣言

    //----------------------------------------
    // 外部取得
    private DesertBossMove BossMove;    // ボスの行動スクリプト
    private SpriteRenderer renderer;    // このオブジェクトのSpriterenderer
    private GameObject PyramidList;     // ピラミッド管理オブジェクト
    [SerializeField,Header("コアオブジェクト")]
    private GameObject CoreObj;         // コアオブジェクト
    [SerializeField, Header("敵オブジェクト")]
    private GameObject EnemyObj;        // 敵オブジェクト
   

    [SerializeField, Header("壊れたスプライト")]
    private Sprite BreakPyramid;

    [SerializeField, Header("壊れるアニメーション")]
    Animator anim;

    [Header("中身 0:敵 1:コア　触らないで")]
    public int InsideNum = 0;

    [SerializeField, Header("出現スピード")]
    private float MoveSpeed;

    public bool Breaked = false;   // 壊れているかどうか
    public bool Clean = false;     // 片付けるフラグ
    
    private bool HitTrigger = false;    // 1回あたり判定
    public bool MoveFlg = false;   // 移動中かどうか
    [SerializeField]
    private float Ypos;     // Y座標

    private void Start()
    {
        //--------------------------------
        // ボスの行動スクリプトを取得
        GameObject obj = GameObject.Find("DesertBoss");
        BossMove = obj.GetComponent<DesertBossMove>();
        
        renderer = GetComponent<SpriteRenderer>();  // このオブジェクトのレンダラーを取得

        PyramidList = GameObject.Find("PyramidList"); // ピラミッド管理オブジェクトの取得

        Ypos = transform.localPosition.y;

    }

    // Update is called once per frame
    void Update()
    {
        //---------------------
        // 移動中
        if (MoveFlg)
        {
            Ypos += MoveSpeed * Time.deltaTime;  
        }

        //---------------------
        //　片付け状態
        if (Clean)
        {
            
            if(Ypos <= -0.5f)
            {
                Clean = false;
                renderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                transform.parent = PyramidList.transform;
                BossMove.Breaking = false;
                BossMove.Setvibratioin = false;

                //　終了していなかったら更新
                if (BossMove.BossState != DesertBossMove.DesertBossState.END)
                {
                    BossMove.BossState = DesertBossMove.DesertBossState.IDLE;
                }

            }
            else
            {
                Ypos -= MoveSpeed * Time.deltaTime;
            }
        }

        
        //　座標更新
        transform.localPosition = new Vector3(transform.localPosition.x, Ypos, 0.0f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //-------------------------------------
        // 生成中のひびが入ったら壊れる
        if (collision.gameObject.tag == "Crack" && !Breaked && !MoveFlg)
        {
            CrackCreater creater = collision.GetComponent<CrackCreater>();
          
            if (creater != null &&
                (creater.GetState() == CrackCreater.CrackCreaterState.CREATING ||
                creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING))
            {
                Destroy(collision.gameObject);
                anim.SetBool("Break", true);
                //renderer.sprite = BreakPyramid;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //---------------------------------------
        //　地面の中に埋まっていたら移動開始
        if (collision.gameObject.tag == "Ground")
        {
            if (renderer.color.a == 1.0f && !Clean)
            {
                MoveFlg = true;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //---------------------------------------
        //　地面の中から出たら移動終了
        if (collision.gameObject.tag == "Ground")
        {
            MoveFlg = false;

        }
    }

    void CreatePyramidCore()
    {
        Destroy(GetComponent<HitCollider>());
        BossMove.Breaking = true;
        Breaked = true;

        if (InsideNum == 0)
        {
            GameObject obj = Instantiate(EnemyObj, transform.position, Quaternion.identity);
            GameObject Enemy = GameObject.Find("BossEnemyManager");
            obj.transform.parent = Enemy.transform;
        }
        else
        {
            GameObject obj = Instantiate(CoreObj, transform.position, Quaternion.identity);
            //GameObject Core = GameObject.Find("Core");
            //obj.transform.parent = Core.transform;
            BossMove.BossState = DesertBossMove.DesertBossState.END;    // 攻撃終了
        }
    }

}
