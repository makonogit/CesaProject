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

    [Header("InputManager用オブジェクト")]
    private GameObject PlayerInputManager;       // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager;  // PlayerInputManager
    private InputTrigger InputTrigger;           // InputTrigger

    private GameObject NowMoveCrack;            //　移動中のヒビのオブジェクト
    
    [SerializeField, Header("光るひびのマテリアル")]
    private Material FrashCrackMat;
    [SerializeField, Header("通常のひびのマテリアル")]
    private Material NomalCrackMat;


    PlayerJump Jump;                    // ジャンプスクリプト
    PlayerMove Move;                    // 移動スクリプト
    GroundCheck GroundCheck;            // 接地判定
    Rigidbody2D thisrigidbody;          // このオブジェクトのrigitbody
    SpriteRenderer thisRenderer;        // このオブジェクトのspriterenderer
    CapsuleCollider2D thiscol;          // このオブジェクトのあたり判定
    Animator anim;                      // このオブジェクトのAnimator

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
        // Animator取得
        anim = GetComponent<Animator>();

        PlayerInputManager = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        InputTrigger = PlayerInputManager.GetComponent<InputTrigger>();

        Jump = this.gameObject.GetComponent<PlayerJump>();
        Move = this.gameObject.GetComponent<PlayerMove>();
        GroundCheck = this.gameObject.GetComponent<GroundCheck>();

        // パーティクルシステムを取得
        ParticleSystemObj = GameObject.Find("Particle");
        if (ParticleSystemObj != null)
        {
            ParticleSystemObj.active = false;
        }
        NowCrackspeed = CrackMoveSpeed;
        movestate = MoveState.Walk;
        Distance = 0.0f;
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

                // 移動中は自分のあたり判定を無効化
                thiscol.enabled = false;
                
                //始点・終点まで移動したら移動終了
                if (MinPointNum == 0)
                {
                    //----------------------------------
                    //目的地(Point座標)まで移動
                    NowCrackspeed += CrackMoveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, Edge.points[NowPointNum], NowCrackspeed * Time.deltaTime);

                    //-------------------------------------------------------------
                    //目的地との距離を求める
                    Distance = Vector3.Distance(this.transform.position,
                                     new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, 0.0f));

                    if (Distance <= 0)
                    {
                        //-----------------------------------------
                        //終点まで移動したら終了
                        if (NowPointNum == PointNum - 1)
                        {
                            // ひびを消す
                            Destroy(NowMoveCrack);
                            HitFlg = false;
                            NowCrackspeed = CrackMoveSpeed;
                            MoveFlg = false;
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

                if (MinPointNum == PointNum - 1)
                {
                   
                    NowCrackspeed += CrackMoveSpeed * Time.deltaTime;
                    //----------------------------------
                    //目的地(Point座標)まで移動
                    transform.position = Vector3.MoveTowards(transform.position, Edge.points[NowPointNum], CrackMoveSpeed * Time.deltaTime);

                    //-------------------------------------------------------------
                    //目的地との距離を求める
                    Distance = Vector3.Distance(this.transform.position,
                                     new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, 0.0f));

                    if (Distance <= 0)
                    {
                        //-----------------------------------------
                        //終点まで移動したら終了
                        if (NowPointNum == 0)
                        {
                            // ひびを消す
                            Destroy(NowMoveCrack);
                            HitFlg = false;
                            NowCrackspeed = CrackMoveSpeed;
                            MoveFlg = false;
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

                break;
            case MoveState.CrackMoveEnd:

                //釘の上に移動させる
                //this.transform.position = new Vector3(Edge.points[Edge.pointCount - 1].x, Edge.points[Edge.pointCount - 1].y + 0.9f, 0.0f);

                thisrigidbody.constraints = RigidbodyConstraints2D.None;
                thisrigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                // 自分のあたり判定を有効にする
                thiscol.enabled = true;
                // アニメーション終了していたらMove,Jumpを再開
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
                    

                    // パーティクルシステムを非表示、自身を表示
                    if (ParticleSystemObj != null)
                    {

                        ParticleSystemObj.active = false;
                    }

                }

                //thisRenderer.enabled = true;

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
            Vector3.Distance(this.transform.position, new Vector3(Edge.points[0].x, Edge.points[0].y, 0.0f));
            MinPointNum = 0;

            for (int i = 1; i < PointNum; i++)
            {
                //-----------------------------------------
                //距離を求める
                float NearPoint =
                Vector3.Distance(this.transform.position, new Vector3(Edge.points[i].x, Edge.points[i].y, 0.0f));

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
            if (MinPointNum == 0 || MinPointNum == Edge.pointCount - 1)
            {
                HitFlg = true;

                //---------------------------------------
                //　子オブジェクトのマテリルをすべて変更
                for (int i = 0; i < crackobj.transform.childCount - 1; i++)
                {
                    crackobj.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().material
                        = FrashCrackMat;
                }

                // Aボタンで入る
                if (InputTrigger.GetJumpTrigger())
                {
                    NowPointNum = MinPointNum;
                    
                    //　ひびのオブジェクトを取得
                    NowMoveCrack = collision.transform.gameObject;

                    movestate = MoveState.CrackHold;
                    //collision.ClosestPoint(this.transform.position);
                    thisrigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

                    //Jump.JumpHeight = 0.0f;
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
                crackobj.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().material
                    = NomalCrackMat;
            }
        }
        //--------------------------------------------
        //移動終了していてひびから出たら歩行状態に遷移
        if (movestate == MoveState.CrackMoveEnd && Line >= 1.0f)
        {
            //Move.SetMovement(true);
            movestate = MoveState.Walk;
        }
    }


}
