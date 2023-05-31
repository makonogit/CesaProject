using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackAutoMove : MonoBehaviour
{
    //--------------------------------
    //変数宣言
    [Header("ひび移動開始速度")]
    public float CrackMoveSpeed;
    [SerializeField,Header("現在のひび移動速度")]
    private float NowCrackspeed;   //現在のひび移動速度

    EdgeCollider2D Edge;           //当たったEdgeCollider
    public Vector2[] Point;        //当たったedgecolliderのPoint座標
    int PointNum;                  //当たったedgecolliderのPoint数

    [Header("point(デバッグ用に表示)")]
    [SerializeField] int NowPointNum;       //現在のPointの添字

    Vector2 MinNearPoint;       //1番近いPoint座標
    float MinNearPointDistance; //1番近いPoint座標との距離
    public int MinPointNum;     //1番近いPointの添字

    float Distance;             //pointとの距離計算用

    public bool MoveFlg = false;   //移動中フラグ
    public bool HitFlg = false;    //ヒビに当たっているフラグ     

    private bool InCrack = false;

    [Header("InputManager用オブジェクト")]
    private GameObject PlayerInputManager;       // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager;  // PlayerInputManager
    private InputTrigger InputTrigger;           // InputTrigger

    private GameObject NowMoveCrack;            //　移動中のヒビのオブジェクト
    
    [SerializeField, Header("光るひびのマテリアル")]
    private Material FrashCrackMat;
    [SerializeField, Header("通常のひびのマテリアル")]
    private Material NomalCrackMat;


    New_PlayerJump Jump;                    // ジャンプスクリプト
    PlayerMove Move;                    // 移動スクリプト
    GroundCheck GroundCheck;            // 接地判定
    Rigidbody2D thisrigidbody;          // このオブジェクトのrigitbody
    SpriteRenderer thisRenderer;        // このオブジェクトのspriterenderer
    CapsuleCollider2D thiscol;          // このオブジェクトのあたり判定
    Vector2 colsize;                    // コライダーのサイズ
    Animator anim;                      // このオブジェクトのAnimator
    PlayerStatas statas;                //　クリスタル所持数を取得

    float Line = 1.0f;                  // ひびに入るアニメーション用変数
    [SerializeField,Header("アニメーション速度")]
    private float AnimSpeed;            // アニメーション速度

    //移動状態
    public enum MoveState
    {
        Walk,           // 歩いている
        CrackHold,      // ひびの中に入っている
        CrackMove,      // ひびの中を移動中
        CrackMoveEnd,   // ひびの中の移動終了
        CrackDown       // ひびの中から移動
    }

    public MoveState movestate;

    Collider2D HitCollider;

    // サウンド関係
    private GameObject se;
    private SEManager_Player seMana;

    //--------------------------------
    // パーティクルシステム
    GameObject ParticleSystemObj;         
   
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------
        //このオブジェクトの情報を取得
        thisrigidbody = GetComponent<Rigidbody2D>();
        thisRenderer = GetComponent<SpriteRenderer>();
        thiscol = GetComponent<CapsuleCollider2D>();
        colsize = thiscol.size;

        // Animator取得
        anim = GetComponent<Animator>();

        PlayerInputManager = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        InputTrigger = PlayerInputManager.GetComponent<InputTrigger>();

        Jump = this.gameObject.GetComponent<New_PlayerJump>();
        Move = this.gameObject.GetComponent<PlayerMove>();
        GroundCheck = this.gameObject.GetComponent<GroundCheck>();
        statas = GetComponent<PlayerStatas>();


        // パーティクルシステムを取得
        ParticleSystemObj = GameObject.Find("Particle");
        if (ParticleSystemObj != null)
        {
            ParticleSystemObj.active = false;
        }
        NowCrackspeed = CrackMoveSpeed;
        movestate = MoveState.Walk;
        Distance = 0.0f;

        // サウンド関係
        se = GameObject.Find("SE");
        // Seコンポーネント取得
        seMana = se.GetComponent<SEManager_Player>();
    }

    // Update is called once per frame
    void Update()
    {
       
        //状態によって行動
        switch (movestate)
        {
            case MoveState.CrackHold:

                // パーティクルシステムを起動、自身は非表示
                if (ParticleSystemObj != null)
                {
                    ParticleSystemObj.active = true;
                }

                //thisRenderer.enabled = false;
                if(Line >= 0.0f)
                {
                    Line -= AnimSpeed * Time.deltaTime;
                    thisRenderer.material.SetFloat("_Border", Line);
                }
                else
                {
                    // アニメーション終了したら移動開始
                    movestate = MoveState.CrackMove;
                }
               
                break;
            case MoveState.CrackMove:

                if(Edge == null)
                {
                    movestate = MoveState.CrackMoveEnd; 
                }

                // 移動中は自分のあたり判定を無効化
                thiscol.size = new Vector2(1.0f,1.0f);
                
                //始点・終点まで移動したら移動終了
                if (MinPointNum == 0 || MinPointNum == 1)
                {
                    //----------------------------------
                    //目的地(Point座標)まで移動
                    NowCrackspeed += CrackMoveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, transform.position.z), NowCrackspeed * Time.deltaTime);

                    //-------------------------------------------------------------
                    //目的地との距離を求める
                    Distance = Vector3.Distance(this.transform.position,
                                     new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, transform.position.z));

                    if (Distance <= 0)
                    {
                        //-----------------------------------------
                        //終点まで移動したら終了
                        if (NowPointNum == PointNum - 1)
                        {
                            if (statas.GetCrystal() > 0)
                            {
                                // ひびを消す
                                Destroy(NowMoveCrack);
                                statas.SetCrystal(statas.GetCrystal() - 1); //クリスタル所持数を減らす
                            }
                            HitFlg = false;
                            NowCrackspeed = CrackMoveSpeed;
                            MoveFlg = false;
                            ScriptPIManager.SetCrackMove(false);
                            movestate = MoveState.CrackMoveEnd;
                        }

                        //-------------------------------------------------------------
                        //目的地についたら次の目的地を指定
                        if (NowPointNum < PointNum - 1)
                        {
                            NowPointNum++;
                        }
                    }
                }

                if (MinPointNum == PointNum - 1 || MinPointNum == PointNum - 2)
                {
                   
                    NowCrackspeed += CrackMoveSpeed * Time.deltaTime;
                    //----------------------------------
                    //目的地(Point座標)まで移動
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, transform.position.z), CrackMoveSpeed * Time.deltaTime);

                    //-------------------------------------------------------------
                    //目的地との距離を求める
                    Distance = Vector3.Distance(this.transform.position,
                                     new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, transform.position.z));

                    if (Distance <= 0)
                    {
                        //-----------------------------------------
                        //終点まで移動したら終了
                        if (NowPointNum == 0)
                        {
                            if (statas.GetCrystal() > 0)
                            {
                                // ひびを消す
                                Destroy(NowMoveCrack);
                                statas.SetCrystal(statas.GetCrystal() - 1); //クリスタル所持数を減らす
                            }
                            HitFlg = false;
                            NowCrackspeed = CrackMoveSpeed;
                            MoveFlg = false;
                            ScriptPIManager.SetCrackMove(false);
                            movestate = MoveState.CrackMoveEnd;
                        }

                        //-------------------------------------------------------------
                        //目的地についたら次の目的地を指定
                        if (NowPointNum > 0)
                        {
                            NowPointNum--;
                        }
                    }


                }

                // サウンド関係
                seMana.PlaySE_CrackMove();

                break;
            case MoveState.CrackMoveEnd:

                thisrigidbody.constraints = RigidbodyConstraints2D.None;
                thisrigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                // 自分のあたり判定を有効にする
                //thiscol.enabled = true;
                thiscol.size = colsize;
                // アニメーション終了していたらMove,Jumpを再開
                Jump.enabled = true;
                InCrack = false;
                Move.SetMovement(true);
                HitFlg = false;

                //--------------------------------
                // アニメーション
                if (Line <= 1.0f)
                {
                    Line += AnimSpeed * Time.deltaTime;
                    thisRenderer.material.SetFloat("_Border", Line);
                   
                }
                else
                {
                    //--------------------------------------------
                    //移動終了していてひびから出たら歩行状態に遷移
                    movestate = MoveState.Walk;
                    // パーティクルシステムを非表示、自身を表示
                    if (ParticleSystemObj != null)
                    {

                        ParticleSystemObj.active = false;
                    }

                }

                //thisRenderer.enabled = true;
                // サウンド関係
                seMana.StopSE_crackMove();
                break;
            case MoveState.Walk:
                ScriptPIManager.SetCrackMove(false);
                break;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
     
        //--------------------------------
        //ひびとぶつかったら
        if(collision.tag == "Crack" && movestate == MoveState.Walk)
        {
            //----------------------------------
            //ひびの情報を取得
            GameObject crackobj = collision.gameObject;
            Edge = crackobj.GetComponent<EdgeCollider2D>(); 

            Point = Edge.points;
            PointNum = Edge.pointCount;

            //---------------------------------------------
            //現在地から1番近いPoint座標を求める
            MinNearPoint = Edge.points[0];
            MinNearPointDistance =
            Vector3.Distance(this.transform.position, new Vector3(Edge.points[0].x, Edge.points[0].y, transform.position.z));
            MinPointNum = 0;

            for (int i = 1; i < PointNum; i++)
            {
                //-----------------------------------------
                //距離を求める
                float NearPoint =
                Vector3.Distance(this.transform.position, new Vector3(Edge.points[i].x, Edge.points[i].y, transform.position.z));

                //----------------------------------------
                //現在1番近いポイントより近かったら座標更新
                if (NearPoint < MinNearPointDistance)
                {
                    MinNearPoint = Edge.points[i];
                    MinNearPointDistance = NearPoint;
                    MinPointNum = i;
                }
            }

            //---------------------------------------------
            //1番近い座標が始点or終点ならひびに入る
            if (MinPointNum == 0 || MinPointNum == 1 || MinPointNum == Edge.pointCount - 1 || MinPointNum == Edge.pointCount - 2)
            {
                HitFlg = true;

                //---------------------------------------
                //　子オブジェクトのマテリルをすべて変更
                for (int i = 0; i < crackobj.transform.childCount; i++)
                {
                    PointMatControl control = crackobj.transform.GetChild(i).gameObject.GetComponent<PointMatControl>();
                    if (control != null)
                    {
                        crackobj.transform.GetChild(i).gameObject.GetComponent<PointMatControl>().FlashCrack();
                            //= FrashCrackMat;
                    }
                }

                for (int i = crackobj.transform.childCount - 1; i > 0; i--)
                {
                    if (crackobj.transform.GetChild(i).gameObject.GetComponent<PointMatControl>())
                    {
                        crackobj.transform.GetChild(i).gameObject.GetComponent<PointMatControl>().FlashCrackEnd();
                        break;
                    }
                }
                // Aボタンで入る
                if (ScriptPIManager.GetCrackMove() && InCrack == false)
                {
                    ScriptPIManager.SetCrackMove(false);
                    InCrack = true;
                    
                    NowPointNum = MinPointNum;
                    
                    //　ひびのオブジェクトを取得
                    NowMoveCrack = collision.transform.gameObject;

                    movestate = MoveState.CrackHold;
                    //collision.ClosestPoint(this.transform.position);
                    thisrigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

                    //Jump.JumpHeight = 0.0f;
                    Jump.enabled = false;
                    Move.SetMovement(false);
                }
            }
            else
            {
                HitFlg = false;
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Crack")
        {
            GameObject crackobj = collision.gameObject;
            //---------------------------------------
            //　子オブジェクトのマテリルをすべて変更
            for (int i = 0; i < crackobj.transform.childCount - 1; i++)
            {
                if (crackobj.transform.GetChild(i).gameObject.GetComponent<PointMatControl>())
                    crackobj.transform.GetChild(i).gameObject.GetComponent<PointMatControl>().NomalCrack();
                    //= NomalCrackMat;
            }

            for (int i = crackobj.transform.childCount - 1; i > 0; i--)
            {
                if (crackobj.transform.GetChild(i).gameObject.GetComponent<PointMatControl>())
                {
                    crackobj.transform.GetChild(i).gameObject.GetComponent<PointMatControl>().NormalCrackEnd();
                    break;
                }
            }
        }
      
    }


}
