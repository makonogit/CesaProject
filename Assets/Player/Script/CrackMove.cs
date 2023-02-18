//-------------------------------------
//担当：菅眞心
//内容：ひびの移動
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackMove : MonoBehaviour
{

    //---------------------------------
    //変数宣言
    [Header("ひび移動速度")]
    public float CrackMoveSpeed;

    Vector2[] Point;               //当たったedgecolliderのPoint座標
    EdgeCollider2D Edge;           //当たったEdgeCollider
    int PointNum;                  //当たったedgecolliderのPoint数
    bool CrackMoveFlg;             //ひびの移動開始フラグ

    [Header("point(デバッグ用に表示)")]
    [SerializeField] int LeftPointNum;       //左のPointの添字
    [SerializeField] int RightPointNum;      //右のPointの添字
    [SerializeField] int UPPointNum;         //上のPointの添字
    [SerializeField] int DownPointNum;     　//下のPointの添字

    Vector2 MinNearPoint;       //1番近いPoint座標
    float MinNearPointDistance; //1番近いPoint座標との距離
    public int MinPointNum;     //1番近いPointの添字

    float Distance;             //pointとの距離計算用
    float EndDistance;          //終点・始点との距離計算用

    bool RightMoveFlg = false;   //右移動中フラグ
    bool LeftMoveFlg = false;    //左移動中フラグ
    bool UpMoveFlg = false;      //上移動中フラグ
    bool DownMoveFlg = false;    //下移動中フラグ

    [Header("InputManager用オブジェクト")]
    public GameObject PlayerInputManager;       // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManager
    PlayerJump Jump;                    //ジャンプスクリプト
    PlayerMove Move;                    //移動スクリプト
    Rigidbody2D thisrigidbody;          //このオブジェクトのrigitbody

    List<Direction> Pointdirections;     //前回Pointに対しての向き

    //ひびの向き
    enum Direction
    {
        UP,
        Down,
        Right,
        Left
    }
    Direction EdgeDirection;

    private void Start()
    {
        CrackMoveFlg = false;
        thisrigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        Jump = this.gameObject.GetComponent<PlayerJump>();
        Move = this.gameObject.GetComponent<PlayerMove>();

        Distance = 0.0f;
    }

    private void Update()
    {
        //------------------------------------------------
        //プレイヤーがひびの中に入っていたら移動処理
        if (CrackMoveFlg)
        {
            //------------------------------------------------
            //スティック入力で位置更新
            if (EdgeDirection == Direction.Right || EdgeDirection == Direction.Left)
            {
                if (ScriptPIManager.GetMovement().x >= 0.9f && LeftMoveFlg == false)
                {
                    RightMoveFlg = true;
                }

                if (ScriptPIManager.GetMovement().x <= -0.9f && RightMoveFlg == false)
                {
                    LeftMoveFlg = true;
                }
            }
            if (EdgeDirection == Direction.UP || EdgeDirection == Direction.Down)
            {
                if (ScriptPIManager.GetMovement().y >= 0.9f && DownMoveFlg == false)
                {
                    UpMoveFlg = true;
                }

                if (ScriptPIManager.GetMovement().y <= -0.9f && UpMoveFlg == false)
                {
                    DownMoveFlg = true;
                }
            }

            //-------------------------------------
            //右入力あれば右に移動
            if (RightMoveFlg)
            {
                //----------------------------------
                //目的地(Point座標)まで移動
                transform.position = Vector3.MoveTowards(transform.position, Edge.points[RightPointNum], CrackMoveSpeed * Time.deltaTime);

                //-------------------------------------------------------------
                //目的地との距離を求める
                Distance = Vector3.Distance(this.transform.position,
                                 new Vector3(Edge.points[RightPointNum].x, Edge.points[RightPointNum].y, 0.0f));

                if (Distance <= 0.15f)
                {
                    //-----------------------------------------
                    //終点まで移動したら終了
                    if (EdgeDirection == Direction.Right && RightPointNum + 1 > PointNum - 1)
                    {
                        RightMoveFlg = false;
                        EndDistance = Vector3.Distance(transform.position,
                        new Vector3(Edge.points[RightPointNum].x, Edge.points[RightPointNum].y, 0.0f));

                    }
                    if (EdgeDirection == Direction.Left && RightPointNum - 1 < 0)
                    {
                        RightMoveFlg = false;
                        EndDistance = Vector3.Distance(transform.position,
                        new Vector3(Edge.points[RightPointNum].x, Edge.points[RightPointNum].y, 0.0f));
                    }


                    //-------------------------------------------------------------
                    //目的地についたら次の目的地を指定(0.15fずれがあるので調整)
                    if (EdgeDirection == Direction.Right && RightPointNum < PointNum - 1)
                    {
                        LeftPointNum++;
                        RightPointNum++;
                    }
                    if(EdgeDirection == Direction.Left && RightPointNum > 0)
                    {
                        LeftPointNum--;
                        RightPointNum--;
                    }
                }

            }

            //---------------------------------------------
            //左入力あれば左に移動
            if (LeftMoveFlg)
            {
                //----------------------------------
                //目的地(Point座標)まで移動
                transform.position = Vector3.MoveTowards(transform.position, Edge.points[LeftPointNum], CrackMoveSpeed * Time.deltaTime);

                //-------------------------------------------------------------
                //目的地との距離を求める
                Distance = Vector3.Distance(this.transform.position,
                           new Vector3(Edge.points[LeftPointNum].x, Edge.points[LeftPointNum].y, 0.0f));

                if (Distance <= 0.15f)
                {
                    //-----------------------------------------
                    //終点まで移動したら終了
                    if (EdgeDirection == Direction.Right && LeftPointNum - 1 < 0)
                    {
                        LeftMoveFlg = false;
                        EndDistance = Vector3.Distance(transform.position,
                        new Vector3(Edge.points[LeftPointNum].x, Edge.points[LeftPointNum].y, 0.0f));
                    }
                    if (EdgeDirection == Direction.Left && LeftPointNum + 1 > PointNum - 1)
                    {
                        LeftMoveFlg = false;
                        EndDistance = Vector3.Distance(transform.position,
                       new Vector3(Edge.points[LeftPointNum].x, Edge.points[LeftPointNum].y, 0.0f));
                    }

                    //-------------------------------------------------------------
                    //目的地についたら次の目的地を指定(0.15fずれがあるので調整)
                    if (EdgeDirection == Direction.Right && LeftPointNum > 0)
                    {
                        LeftPointNum--;
                        RightPointNum--;
                    }
                    if (EdgeDirection == Direction.Left && LeftPointNum < PointNum - 1)
                    {
                        LeftPointNum++;
                        RightPointNum++;
                    }
                }

            }

            //---------------------------------------------
            //上入力あれば上に移動
            if (UpMoveFlg)
            {
                //----------------------------------
                //目的地(Point座標)まで移動
                transform.position = Vector3.MoveTowards(transform.position, Edge.points[UPPointNum], CrackMoveSpeed * Time.deltaTime);

                //-------------------------------------------------------------
                //目的地との距離を求める
                Distance = Vector3.Distance(this.transform.position,
                                 new Vector3(Edge.points[UPPointNum].x, Edge.points[UPPointNum].y, 0.0f));

                if (Distance <= 0.15f)
                {
                    //-----------------------------------------
                    //終点まで移動したら終了
                    if (EdgeDirection == Direction.UP && UPPointNum + 1 > PointNum - 1)
                    {
                        UpMoveFlg = false;
                        EndDistance = Vector3.Distance(transform.position,
                        new Vector3(Edge.points[UPPointNum].x, Edge.points[UPPointNum].y, 0.0f));

                    }
                    if (EdgeDirection == Direction.Down && UPPointNum - 1 < 0)
                    {
                        UpMoveFlg = false;
                        EndDistance = Vector3.Distance(transform.position,
                        new Vector3(Edge.points[UPPointNum].x, Edge.points[UPPointNum].y, 0.0f));
                    }


                    //-------------------------------------------------------------
                    //目的地についたら次の目的地を指定(0.15fずれがあるので調整)
                    if (EdgeDirection == Direction.UP && UPPointNum < PointNum - 1)
                    {
                        UPPointNum++;
                        DownPointNum++;
                    }
                    if (EdgeDirection == Direction.Down && UPPointNum > 0)
                    {
                        UPPointNum--;
                        DownPointNum--;
                    }
                }

            }

            //---------------------------------------------
            //下入力あれば下に移動
            if (DownMoveFlg)
            {
                //----------------------------------
                //目的地(Point座標)まで移動
                transform.position = Vector3.MoveTowards(transform.position, Edge.points[DownPointNum], CrackMoveSpeed * Time.deltaTime);

                //-------------------------------------------------------------
                //目的地との距離を求める
                Distance = Vector3.Distance(this.transform.position,
                                 new Vector3(Edge.points[DownPointNum].x, Edge.points[DownPointNum].y, 0.0f));

                if (Distance <= 0.15f)
                {
                    //-----------------------------------------
                    //終点まで移動したら終了
                    if (EdgeDirection == Direction.Down && DownPointNum + 1 > PointNum - 1)
                    {
                        DownMoveFlg = false;
                        EndDistance = Vector3.Distance(transform.position,
                        new Vector3(Edge.points[DownPointNum].x, Edge.points[DownPointNum].y, 0.0f));

                    }
                    if (EdgeDirection == Direction.UP && DownPointNum - 1 < 0)
                    {
                        DownMoveFlg = false;
                        EndDistance = Vector3.Distance(transform.position,
                        new Vector3(Edge.points[DownPointNum].x, Edge.points[DownPointNum].y, 0.0f));
                    }


                    //-------------------------------------------------------------
                    //目的地についたら次の目的地を指定(0.15fずれがあるので調整)
                    if (EdgeDirection == Direction.Down && DownPointNum < PointNum - 1)
                    {
                        UPPointNum++;
                        DownPointNum++;
                    }
                    if (EdgeDirection == Direction.UP && DownPointNum > 0)
                    {
                        UPPointNum--;
                        DownPointNum--;
                    }
                }

            }

            //--------------------------------------------------
            //始点or終点にいたらボタン入力でひびから出る
            if (!LeftMoveFlg && !RightMoveFlg && !DownMoveFlg && !UpMoveFlg &&EndDistance <= 0.15f)
            {
                if (ScriptPIManager.GetCrackMove())
                {
                    EndDistance = 1.0f;
                    CrackMoveFlg = false;
                    thisrigidbody.constraints = RigidbodyConstraints2D.None;
                    thisrigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    Jump.enabled = true;
                    Move.enabled = true;
                }

            }

        }

    }


    //ひびのあたり判定
    void OnTriggerEnter2D(Collider2D collision)
    {

        //-----------------------------------
        //ひびとぶつかったらEdgeColliderの情報を取得
        if (collision.gameObject.tag == "Crack" && CrackMoveFlg == false)
        {
           // Debug.Log("Hit");
            Edge = collision.gameObject.GetComponent<EdgeCollider2D>();
            Point = Edge.points;
            PointNum = Edge.pointCount;


            //---------------------------------------
            //ひびの向きを取得(ゴリ押しです...)
            {

                //for(int i = 0; i < Edge.pointCount; i++)
                //{
                //    //-------------------------------------------
                //    //X,Yそれぞれの距離を求めて、近い方を優先
                //    float PointDistanceX = 0;
                //    float PointDistanceY = 0;

                //    if(Edge.points[i].y < Edge.points[i + 1].y && PointDistanceX < PointDistanceY)
                //    {
                //        Pointdirections.Add(Direction.UP);
                //    }
                //    if (Edge.points[i].y > Edge.points[i + 1].y && PointDistanceX < PointDistanceY)
                //    {
                //        Pointdirections.Add(Direction.Down);
                //    }

                //    if (Edge.points[i].x < Edge.points[i + 1].x && PointDistanceX > PointDistanceY)
                //    {
                //        Pointdirections.Add(Direction.Right);
                //    }
                //    if (Edge.points[i].x > Edge.points[i + 1].x && PointDistanceX > PointDistanceY)
                //    {
                //        Pointdirections.Add(Direction.Left);
                //    }

                //}

                if (Edge.pointCount > 2)
                {
                    //上向き
                    if (Edge.points[0].y < Edge.points[1].y &&
                       Edge.points[1].y < Edge.points[2].y)
                    {
                        EdgeDirection = Direction.UP;
                    }

                    //下向き
                    if (Edge.points[0].y > Edge.points[1].y &&
                       Edge.points[1].y > Edge.points[2].y)
                    {
                        EdgeDirection = Direction.Down;
                    }

                    //右向き
                    if (Edge.points[0].x < Edge.points[1].x &&
                        Edge.points[1].x < Edge.points[2].x)
                    {
                        EdgeDirection = Direction.Right;
                    }

                    //左向き
                    if (Edge.points[0].x > Edge.points[1].x &&
                        Edge.points[1].x > Edge.points[2].x)
                    {
                        EdgeDirection = Direction.Left;
                    }
                }
                else
                {
                    //上向き
                    if (Edge.points[0].y < Edge.points[1].y)
                    {
                        EdgeDirection = Direction.UP;
                    }

                    //下向き
                    if (Edge.points[0].y > Edge.points[1].y)
                    {
                        EdgeDirection = Direction.Down;
                    }

                    //右向き
                    if (Edge.points[0].x < Edge.points[1].x)
                    {
                        EdgeDirection = Direction.Right;
                    }

                    //左向き
                    if (Edge.points[0].x > Edge.points[1].x)
                    {
                        EdgeDirection = Direction.Left;
                    }
                }

            }

            //----------------------------------------------------
            //入力があればプレイヤーの座標を固定(ひびの中に入る)
            if (ScriptPIManager.GetCrackMove() && CrackMoveFlg == false)
            {
                CrackMoveFlg = true;
                thisrigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
                //thisrigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;    //バグる
                Jump.enabled = false;
                Move.enabled = false;

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

                if (EdgeDirection == Direction.Right || EdgeDirection == Direction.Left)
                {
                    //-------------------------------------------------------
                    //1番近いポイントがプレイヤーより右にあった時のPoint設定
                    if (MinNearPoint.x > this.transform.position.x)
                    {
                        RightPointNum = MinPointNum;
                        //------------------------------------------
                        //ひびの向きによって左側のpointを指定
                        if (EdgeDirection == Direction.Right)
                        {
                            LeftPointNum = MinPointNum - 1;
                        }
                        if (EdgeDirection == Direction.Left)
                        {
                            LeftPointNum = MinPointNum + 1;
                        }
                    }
                    //---------------------------------------------------------
                    //1番近いポイントがプレイヤーより左にあった時のPoint設定
                    if (MinNearPoint.x < this.transform.position.x)
                    {
                        LeftPointNum = MinPointNum;
                        //------------------------------------------
                        //ひびの向きによって左側のpointを指定
                        if (EdgeDirection == Direction.Right)
                        {
                            RightPointNum = MinPointNum + 1;
                        }
                        if (EdgeDirection == Direction.Left)
                        {
                            RightPointNum = MinPointNum - 1;
                        }
                    }
                }

                if (EdgeDirection == Direction.UP || EdgeDirection == Direction.Down)
                {
                    //-------------------------------------------------------
                    //1番近いポイントがプレイヤーより上にあった時のPoint設定
                    if (MinNearPoint.y > this.transform.position.y)
                    {
                        UPPointNum = MinPointNum;
                        //------------------------------------------
                        //ひびの向きによって左側のpointを指定
                        if (EdgeDirection == Direction.UP)
                        {
                            DownPointNum = MinPointNum - 1;
                        }
                        if (EdgeDirection == Direction.Down)
                        {
                            DownPointNum = MinPointNum + 1;
                        }
                    }

                    //-------------------------------------------------------
                    //1番近いポイントがプレイヤーより下にあった時のPoint設定
                    if (MinNearPoint.y < this.transform.position.y)
                    {
                        DownPointNum = MinPointNum;
                        //------------------------------------------
                        //ひびの向きによって左側のpointを指定
                        if (EdgeDirection == Direction.UP)
                        {
                            UPPointNum = MinPointNum + 1;
                        }
                        if (EdgeDirection == Direction.Down)
                        {
                            UPPointNum = MinPointNum - 1;
                        }
                    }

                }

            }

        }
    }


}
