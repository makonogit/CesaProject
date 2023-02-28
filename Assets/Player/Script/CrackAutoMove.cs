using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackAutoMove : MonoBehaviour
{
    //--------------------------------
    //�ϐ��錾
    [Header("�Ђшړ����x")]
    public float CrackMoveSpeed;

    EdgeCollider2D Edge;           //��������EdgeCollider
    public Vector2[] Point;        //��������edgecollider��Point���W
    int PointNum;                  //��������edgecollider��Point��

    [Header("point(�f�o�b�O�p�ɕ\��)")]
    [SerializeField] int NowPointNum;       //���݂�Point�̓Y��

    Vector2 MinNearPoint;       //1�ԋ߂�Point���W
    float MinNearPointDistance; //1�ԋ߂�Point���W�Ƃ̋���
    public int MinPointNum;     //1�ԋ߂�Point�̓Y��

    float Distance;             //point�Ƃ̋����v�Z�p

    public bool MoveFlg = false;   //�ړ����t���O

    [Header("InputManager�p�I�u�W�F�N�g")]
    private GameObject PlayerInputManager;       // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager;  // PlayerInputManager
  
    PlayerJump Jump;                    //�W�����v�X�N���v�g
    PlayerMove Move;                    //�ړ��X�N���v�g
    GroundCheck GroundCheck;            //�ڒn����
    Rigidbody2D thisrigidbody;          //���̃I�u�W�F�N�g��rigitbody

    //�ړ����
    public enum MoveState
    {
        Walk,           //�����Ă���
        CrackHold,      //�Ђт̒��ɓ����Ă���
        CrackMove,      //�Ђт̒����ړ���
        CrackMoveEnd,   //�Ђт̒��̈ړ��I��
        CrackDown       //�Ђт̒�����ړ�
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
        //��Ԃɂ���čs��
        switch (movestate)
        {
            case MoveState.CrackHold:
                //�Ђтɓ����Ă�����ړ��J�n(�����͕b���҂��Ă��ǂ�����)
                movestate = MoveState.CrackMove;
                break;
            case MoveState.CrackMove:
                //�n�_�E�I�_�܂ňړ�������ړ��I��
                if (MinPointNum == 0)
                {
                    //----------------------------------
                    //�ړI�n(Point���W)�܂ňړ�
                    transform.position = Vector3.MoveTowards(transform.position, Edge.points[NowPointNum], CrackMoveSpeed * Time.deltaTime);

                    //-------------------------------------------------------------
                    //�ړI�n�Ƃ̋��������߂�
                    Distance = Vector3.Distance(this.transform.position,
                                     new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, 0.0f));

                    if (Distance <= 0)
                    {
                        //-----------------------------------------
                        //�I�_�܂ňړ�������I��
                        if (NowPointNum == PointNum - 1)
                        {
                            MoveFlg = false;
                            movestate = MoveState.CrackMoveEnd;
                        }

                        //-------------------------------------------------------------
                        //�ړI�n�ɂ����玟�̖ړI�n���w��
                        if (NowPointNum < PointNum - 1)
                        {
                            NowPointNum++;
                        }
                    }
                }

                if (MinPointNum == PointNum - 1)
                {
                    //----------------------------------
                    //�ړI�n(Point���W)�܂ňړ�
                    transform.position = Vector3.MoveTowards(transform.position, Edge.points[NowPointNum], CrackMoveSpeed * Time.deltaTime);

                    //-------------------------------------------------------------
                    //�ړI�n�Ƃ̋��������߂�
                    Distance = Vector3.Distance(this.transform.position,
                                     new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, 0.0f));

                    if (Distance <= 0)
                    {
                        //-----------------------------------------
                        //�I�_�܂ňړ�������I��
                        if (NowPointNum == 0)
                        {
                            MoveFlg = false;
                            movestate = MoveState.CrackMoveEnd;
                        }

                        //-------------------------------------------------------------
                        //�ړI�n�ɂ����玟�̖ړI�n���w��
                        if (NowPointNum > 0)
                        {
                            NowPointNum--;
                        }
                    }


                }

                break;
            case MoveState.CrackMoveEnd:
                //�ړ��I�����Ă�����Move,Jump���ĊJ
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
        //�ЂтƂԂ�������
        if(collision.tag == "Crack" && movestate == MoveState.Walk)
        {
            //----------------------------------
            //�Ђт̏����擾
            Edge = collision.gameObject.GetComponent<EdgeCollider2D>();
            Point = Edge.points;
            PointNum = Edge.pointCount;

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

            //---------------------------------------------
            //1�ԋ߂����W���n�_or�I�_�Ȃ�Ђтɓ���
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
        //�ړ��I�����Ă��ĂЂт���o������s��ԂɑJ��
        if (movestate == MoveState.CrackMoveEnd)
        {
            movestate = MoveState.Walk;
        }
    }


}
