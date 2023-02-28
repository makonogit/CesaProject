using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackAutoMove : MonoBehaviour
{
    //--------------------------------
    //変数宣言
    [Header("ひび移動速度")]
    public float CrackMoveSpeed;

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

    [Header("InputManager用オブジェクト")]
    private GameObject PlayerInputManager;       // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager;  // PlayerInputManager
  
    PlayerJump Jump;                    //ジャンプスクリプト
    PlayerMove Move;                    //移動スクリプト
    GroundCheck GroundCheck;            //接地判定
    Rigidbody2D thisrigidbody;          //このオブジェクトのrigitbody

    //移動状態
    public enum MoveState
    {
        Walk,           //歩いている
        CrackHold,      //ひびの中に入っている
        CrackMove,      //ひびの中を移動中
        CrackMoveEnd,   //ひびの中の移動終了
        CrackDown       //ひびの中から移動
    }

    public MoveState movestate;

    Collider2D HitCollider;

    // Start is called before the first frame update
    void Start()
    {
        thisrigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        PlayerInputManager = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        Jump = this.gameObject.GetComponent<PlayerJump>();
        Move = this.gameObject.GetComponent<PlayerMove>();
        GroundCheck = this.gameObject.GetComponent<GroundCheck>();

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
                //ひびに入っていたら移動開始(ここは秒数待っても良さそう)
                movestate = MoveState.CrackMove;
                break;
            case MoveState.CrackMove:
                //始点・終点まで移動したら移動終了
                if (MinPointNum == 0)
                {
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
                        if (NowPointNum == PointNum - 1)
                        {
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
                //移動終了していたらMove,Jumpを再開
                thisrigidbody.constraints = RigidbodyConstraints2D.None;
                thisrigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                Jump.enabled = true;
                Move.enabled = true;
                break;

        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
     
        //--------------------------------
        //ひびとぶつかったら
        if(collision.tag == "Crack" && movestate == MoveState.Walk)
        {
            //----------------------------------
            //ひびの情報を取得
            Edge = collision.gameObject.GetComponent<EdgeCollider2D>();
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
                NowPointNum = MinPointNum;

                movestate = MoveState.CrackHold;
                //collision.ClosestPoint(this.transform.position);
                thisrigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

                Jump.enabled = false;
                Move.enabled = false;
            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //--------------------------------------------
        //移動終了していてひびから出たら歩行状態に遷移
        if (movestate == MoveState.CrackMoveEnd)
        {
            movestate = MoveState.Walk;
        }
    }


}
