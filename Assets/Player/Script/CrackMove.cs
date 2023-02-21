//-------------------------------------
//�S���F�����S
//���e�F�Ђт̈ړ�
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrackMove : MonoBehaviour
{

    //---------------------------------
    //�ϐ��錾
    [Header("�Ђшړ����x")]
    public float CrackMoveSpeed;

    public Vector2[] Point;               //��������edgecollider��Point���W
    EdgeCollider2D Edge;           //��������EdgeCollider
    int PointNum;                  //��������edgecollider��Point��

    [Header("point(�f�o�b�O�p�ɕ\��)")]
    [SerializeField] int LeftPointNum;       //����Point�̓Y��
    [SerializeField] int RightPointNum;      //�E��Point�̓Y��
    [SerializeField] int UPPointNum;         //���Point�̓Y��
    [SerializeField] int DownPointNum;     �@//����Point�̓Y��

    Vector2 MinNearPoint;       //1�ԋ߂�Point���W
    float MinNearPointDistance; //1�ԋ߂�Point���W�Ƃ̋���
    public int MinPointNum;     //1�ԋ߂�Point�̓Y��

    float Distance;             //point�Ƃ̋����v�Z�p
    float EndDistance;          //�I�_�E�n�_�Ƃ̋����v�Z�p

    public bool RightMoveFlg = false;   //�E�ړ����t���O
    public bool LeftMoveFlg = false;    //���ړ����t���O
    public bool UpMoveFlg = false;      //��ړ����t���O
    public bool DownMoveFlg = false;    //���ړ����t���O

    [Header("InputManager�p�I�u�W�F�N�g")]
    public GameObject PlayerInputManager;       // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager
    PlayerJump Jump;                    //�W�����v�X�N���v�g
    PlayerMove Move;                    //�ړ��X�N���v�g
    Rigidbody2D thisrigidbody;          //���̃I�u�W�F�N�g��rigitbody

    //�ړ����
    public enum MoveState
    {
        Walk,           //�����Ă���
        CrackHold,      //�Ђт̒��ɓ����Ă���
        CrackMove,      //�Ђт̒����ړ���
        CrackMoveEnd,   //�Ђт̒��̈ړ��I��
    }

    public MoveState movestate;

    //�Ђт̌���
    enum Direction
    {
        UP,
        Down,
        Right,
        Left
    }
    Direction EdgeDirection;

    bool Hit = false;
    Collider2D HitCollider;

    private void Start()
    {

        thisrigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        Jump = this.gameObject.GetComponent<PlayerJump>();
        Move = this.gameObject.GetComponent<PlayerMove>();

        movestate = MoveState.Walk;
        Distance = 0.0f;
    }

    private void Update()
    {
        if (Hit)
        {
            CrackStart(HitCollider);
            if (Gamepad.current.bButton.wasPressedThisFrame)
            {
                Debug.Log("Press");
            }
        }

        //------------------------------------------------
        //�v���C���[���Ђт̒��ɓ����Ă�����ړ�����
        if (movestate >= MoveState.CrackHold)
        {

            //------------------------------------------------
            //�X�e�B�b�N���͂ňʒu�X�V
            if (EdgeDirection == Direction.Right || EdgeDirection == Direction.Left)
            {
                if (ScriptPIManager.GetMovement().x >= 0.9f && LeftMoveFlg == false && RightPointNum > -1 && RightPointNum < PointNum)
                {
                    movestate = MoveState.CrackMove;
                    RightMoveFlg = true;
                }

                if (ScriptPIManager.GetMovement().x <= -0.9f && RightMoveFlg == false && LeftPointNum > -1 && LeftPointNum < PointNum)
                {
                    movestate = MoveState.CrackMove;
                    LeftMoveFlg = true;
                }
            }
            if (EdgeDirection == Direction.UP || EdgeDirection == Direction.Down)
            {
                if (ScriptPIManager.GetMovement().y >= 0.9f && DownMoveFlg == false && UPPointNum > -1 && UPPointNum < PointNum)
                {
                    movestate = MoveState.CrackMove;
                    UpMoveFlg = true;
                }

                if (ScriptPIManager.GetMovement().y <= -0.9f && UpMoveFlg == false && DownPointNum > -1 && DownPointNum < PointNum)
                {
                    movestate = MoveState.CrackMove;
                    DownMoveFlg = true;
                }
            }

            //-------------------------------------
            //�E���͂���ΉE�Ɉړ�
            if (RightMoveFlg)
            {
               // Debug.Log("Move");
                //----------------------------------
                //�ړI�n(Point���W)�܂ňړ�
                transform.position = Vector3.MoveTowards(transform.position, Edge.points[RightPointNum], CrackMoveSpeed * Time.deltaTime);

                //-------------------------------------------------------------
                //�ړI�n�Ƃ̋��������߂�
                Distance = Vector3.Distance(this.transform.position,
                                 new Vector3(Edge.points[RightPointNum].x, Edge.points[RightPointNum].y, 0.0f));

                if (Distance <= 0.15f)
                {
                    //-----------------------------------------
                    //�I�_�܂ňړ�������I��
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
                    //�ړI�n�ɂ����玟�̖ړI�n���w��(0.15f���ꂪ����̂Œ���)
                    if (EdgeDirection == Direction.Right && RightPointNum < PointNum - 1)
                    {
                        LeftPointNum++;
                        RightPointNum++;
                    }
                    if (EdgeDirection == Direction.Left && RightPointNum > 0)
                    {
                        LeftPointNum--;
                        RightPointNum--;
                    }
                }

            }

            //---------------------------------------------
            //�����͂���΍��Ɉړ�
            if (LeftMoveFlg)
            {
                //----------------------------------
                //�ړI�n(Point���W)�܂ňړ�
                transform.position = Vector3.MoveTowards(transform.position, Edge.points[LeftPointNum], CrackMoveSpeed * Time.deltaTime);

                //-------------------------------------------------------------
                //�ړI�n�Ƃ̋��������߂�
                Distance = Vector3.Distance(this.transform.position,
                           new Vector3(Edge.points[LeftPointNum].x, Edge.points[LeftPointNum].y, 0.0f));

                if (Distance <= 0.15f)
                {
                    //-----------------------------------------
                    //�I�_�܂ňړ�������I��
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
                    //�ړI�n�ɂ����玟�̖ړI�n���w��(0.15f���ꂪ����̂Œ���)
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
            //����͂���Ώ�Ɉړ�
            if (UpMoveFlg)
            {
                //----------------------------------
                //�ړI�n(Point���W)�܂ňړ�
                transform.position = Vector3.MoveTowards(transform.position, Edge.points[UPPointNum], CrackMoveSpeed * Time.deltaTime);

                //-------------------------------------------------------------
                //�ړI�n�Ƃ̋��������߂�
                Distance = Vector3.Distance(this.transform.position,
                                 new Vector3(Edge.points[UPPointNum].x, Edge.points[UPPointNum].y, 0.0f));

                if (Distance <= 0.15f)
                {
                    //-----------------------------------------
                    //�I�_�܂ňړ�������I��
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
                    //�ړI�n�ɂ����玟�̖ړI�n���w��(0.15f���ꂪ����̂Œ���)
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
            //�����͂���Ή��Ɉړ�
            if (DownMoveFlg)
            {
                //----------------------------------
                //�ړI�n(Point���W)�܂ňړ�
                transform.position = Vector3.MoveTowards(transform.position, Edge.points[DownPointNum], CrackMoveSpeed * Time.deltaTime);

                //-------------------------------------------------------------
                //�ړI�n�Ƃ̋��������߂�
                Distance = Vector3.Distance(this.transform.position,
                                 new Vector3(Edge.points[DownPointNum].x, Edge.points[DownPointNum].y, 0.0f));

                if (Distance <= 0.15f)
                {
                    //-----------------------------------------
                    //�I�_�܂ňړ�������I��
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
                    //�ړI�n�ɂ����玟�̖ړI�n���w��(0.15f���ꂪ����̂Œ���)
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
            //�n�_or�I�_�ɂ�����{�^�����͂łЂт���o��
            if (!LeftMoveFlg && !RightMoveFlg && !DownMoveFlg && !UpMoveFlg && EndDistance <= 0.15f)
            {
                movestate = MoveState.CrackMoveEnd;
            }

            if (Gamepad.current.bButton.wasPressedThisFrame && movestate == MoveState.CrackMoveEnd)
            {
               // Debug.Log("pressrelease");
                EndDistance = 1.0f;
                movestate = MoveState.Walk;
                thisrigidbody.constraints = RigidbodyConstraints2D.None;
                thisrigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                Jump.enabled = true;
                Move.enabled = true;
            }

        }
    }

    //�Ђт̂����蔻��
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Crack")
        {
            Hit = true;
            HitCollider = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Crack")
        {
            Hit = false;
        }
    }

    void CrackStart(Collider2D HitCollider)
    {
        //----------------------------------------------------------------
        //�ЂтƂԂ����Ă��Ĉړ�������Ȃ����EdgeCollider�̏����擾
        if (HitCollider.gameObject.tag == "Crack")
        {

            if (movestate == MoveState.Walk || movestate == MoveState.CrackMoveEnd)
            {
                // Debug.Log("Hit");
                Edge = HitCollider.gameObject.GetComponent<EdgeCollider2D>();
                Point = Edge.points;
                PointNum = Edge.pointCount;

                //----------------------------------------------------
                //�Ђт̌������擾(�S�������ł�...)
                {

                    if (Edge.pointCount > 2)
                    {
                        //�����
                        if (Edge.points[0].y < Edge.points[1].y &&
                           Edge.points[1].y < Edge.points[2].y)
                        {
                            EdgeDirection = Direction.UP;
                        }

                        //������
                        if (Edge.points[0].y > Edge.points[1].y &&
                           Edge.points[1].y > Edge.points[2].y)
                        {
                            EdgeDirection = Direction.Down;
                        }

                        //�E����
                        if (Edge.points[0].x < Edge.points[1].x &&
                            Edge.points[1].x < Edge.points[2].x)
                        {
                            EdgeDirection = Direction.Right;
                        }

                        //������
                        if (Edge.points[0].x > Edge.points[1].x &&
                            Edge.points[1].x > Edge.points[2].x)
                        {
                            EdgeDirection = Direction.Left;
                        }
                    }
                    else
                    {
                        //�����
                        if (Edge.points[0].y < Edge.points[1].y)
                        {
                            EdgeDirection = Direction.UP;
                        }

                        //������
                        if (Edge.points[0].y > Edge.points[1].y)
                        {
                            EdgeDirection = Direction.Down;
                        }

                        //�E����
                        if (Edge.points[0].x < Edge.points[1].x)
                        {
                            EdgeDirection = Direction.Right;
                        }

                        //������
                        if (Edge.points[0].x > Edge.points[1].x)
                        {
                            EdgeDirection = Direction.Left;
                        }
                    }

                }

            }

            //----------------------------------------------------
            //���͂�����΃v���C���[�̍��W���Œ�(�Ђт̒��ɓ���)
            if (Gamepad.current.bButton.wasPressedThisFrame && movestate == MoveState.Walk)
            {
                Debug.Log("press");
                movestate = MoveState.CrackHold;
                EndDistance = 1.0f;
                HitCollider.ClosestPoint(this.transform.position);
                thisrigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

                Jump.enabled = false;
                Move.enabled = false;

                //---------------------------------------------
                //���ݒn����1�ԋ߂�Point���W�����߂�
                MinNearPoint = Edge.points[0];
                MinNearPointDistance =
                Vector3.Distance(this.transform.position, new Vector3(Edge.points[0].x, Edge.points[0].y, 0.0f));
                MinPointNum = 0;

                for (int i = 1; i < PointNum; i++)
                {
                    //-----------------------------------------
                    //���������߂�
                    float NearPoint =
                    Vector3.Distance(this.transform.position, new Vector3(Edge.points[i].x, Edge.points[i].y, 0.0f));

                    //----------------------------------------
                    //����1�ԋ߂��|�C���g���߂���������W�X�V
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
                    //1�ԋ߂��|�C���g���v���C���[���E�ɂ���������Point�ݒ�
                    if (MinNearPoint.x > this.transform.position.x)
                    {
                        RightPointNum = MinPointNum;
                        //------------------------------------------
                        //�Ђт̌����ɂ���č�����point���w��
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
                    //1�ԋ߂��|�C���g���v���C���[��荶�ɂ���������Point�ݒ�
                    if (MinNearPoint.x < this.transform.position.x)
                    {
                        LeftPointNum = MinPointNum;
                        //------------------------------------------
                        //�Ђт̌����ɂ���č�����point���w��
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
                    //1�ԋ߂��|�C���g���v���C���[����ɂ���������Point�ݒ�
                    if (MinNearPoint.y > this.transform.position.y)
                    {
                        UPPointNum = MinPointNum;
                        //------------------------------------------
                        //�Ђт̌����ɂ���č�����point���w��
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
                    //1�ԋ߂��|�C���g���v���C���[��艺�ɂ���������Point�ݒ�
                    if (MinNearPoint.y < this.transform.position.y)
                    {
                        DownPointNum = MinPointNum;
                        //------------------------------------------
                        //�Ђт̌����ɂ���č�����point���w��
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
