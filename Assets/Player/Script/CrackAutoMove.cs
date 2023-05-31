using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackAutoMove : MonoBehaviour
{
    //--------------------------------
    //�ϐ��錾
    [Header("�Ђшړ��J�n���x")]
    public float CrackMoveSpeed;
    [SerializeField,Header("���݂̂Ђшړ����x")]
    private float NowCrackspeed;   //���݂̂Ђшړ����x

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
    public bool HitFlg = false;    //�q�r�ɓ������Ă���t���O     

    private bool InCrack = false;

    [Header("InputManager�p�I�u�W�F�N�g")]
    private GameObject PlayerInputManager;       // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager;  // PlayerInputManager
    private InputTrigger InputTrigger;           // InputTrigger

    private GameObject NowMoveCrack;            //�@�ړ����̃q�r�̃I�u�W�F�N�g
    
    [SerializeField, Header("����Ђт̃}�e���A��")]
    private Material FrashCrackMat;
    [SerializeField, Header("�ʏ�̂Ђт̃}�e���A��")]
    private Material NomalCrackMat;


    New_PlayerJump Jump;                    // �W�����v�X�N���v�g
    PlayerMove Move;                    // �ړ��X�N���v�g
    GroundCheck GroundCheck;            // �ڒn����
    Rigidbody2D thisrigidbody;          // ���̃I�u�W�F�N�g��rigitbody
    SpriteRenderer thisRenderer;        // ���̃I�u�W�F�N�g��spriterenderer
    CapsuleCollider2D thiscol;          // ���̃I�u�W�F�N�g�̂����蔻��
    Vector2 colsize;                    // �R���C�_�[�̃T�C�Y
    Animator anim;                      // ���̃I�u�W�F�N�g��Animator
    PlayerStatas statas;                //�@�N���X�^�����������擾

    float Line = 1.0f;                  // �Ђтɓ���A�j���[�V�����p�ϐ�
    [SerializeField,Header("�A�j���[�V�������x")]
    private float AnimSpeed;            // �A�j���[�V�������x

    //�ړ����
    public enum MoveState
    {
        Walk,           // �����Ă���
        CrackHold,      // �Ђт̒��ɓ����Ă���
        CrackMove,      // �Ђт̒����ړ���
        CrackMoveEnd,   // �Ђт̒��̈ړ��I��
        CrackDown       // �Ђт̒�����ړ�
    }

    public MoveState movestate;

    Collider2D HitCollider;

    // �T�E���h�֌W
    private GameObject se;
    private SEManager_Player seMana;

    //--------------------------------
    // �p�[�e�B�N���V�X�e��
    GameObject ParticleSystemObj;         
   
    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------
        //���̃I�u�W�F�N�g�̏����擾
        thisrigidbody = GetComponent<Rigidbody2D>();
        thisRenderer = GetComponent<SpriteRenderer>();
        thiscol = GetComponent<CapsuleCollider2D>();
        colsize = thiscol.size;

        // Animator�擾
        anim = GetComponent<Animator>();

        PlayerInputManager = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        InputTrigger = PlayerInputManager.GetComponent<InputTrigger>();

        Jump = this.gameObject.GetComponent<New_PlayerJump>();
        Move = this.gameObject.GetComponent<PlayerMove>();
        GroundCheck = this.gameObject.GetComponent<GroundCheck>();
        statas = GetComponent<PlayerStatas>();


        // �p�[�e�B�N���V�X�e�����擾
        ParticleSystemObj = GameObject.Find("Particle");
        if (ParticleSystemObj != null)
        {
            ParticleSystemObj.active = false;
        }
        NowCrackspeed = CrackMoveSpeed;
        movestate = MoveState.Walk;
        Distance = 0.0f;

        // �T�E���h�֌W
        se = GameObject.Find("SE");
        // Se�R���|�[�l���g�擾
        seMana = se.GetComponent<SEManager_Player>();
    }

    // Update is called once per frame
    void Update()
    {
       
        //��Ԃɂ���čs��
        switch (movestate)
        {
            case MoveState.CrackHold:

                // �p�[�e�B�N���V�X�e�����N���A���g�͔�\��
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
                    // �A�j���[�V�����I��������ړ��J�n
                    movestate = MoveState.CrackMove;
                }
               
                break;
            case MoveState.CrackMove:

                if(Edge == null)
                {
                    movestate = MoveState.CrackMoveEnd; 
                }

                // �ړ����͎����̂����蔻��𖳌���
                thiscol.size = new Vector2(1.0f,1.0f);
                
                //�n�_�E�I�_�܂ňړ�������ړ��I��
                if (MinPointNum == 0 || MinPointNum == 1)
                {
                    //----------------------------------
                    //�ړI�n(Point���W)�܂ňړ�
                    NowCrackspeed += CrackMoveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, transform.position.z), NowCrackspeed * Time.deltaTime);

                    //-------------------------------------------------------------
                    //�ړI�n�Ƃ̋��������߂�
                    Distance = Vector3.Distance(this.transform.position,
                                     new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, transform.position.z));

                    if (Distance <= 0)
                    {
                        //-----------------------------------------
                        //�I�_�܂ňړ�������I��
                        if (NowPointNum == PointNum - 1)
                        {
                            if (statas.GetCrystal() > 0)
                            {
                                // �Ђт�����
                                Destroy(NowMoveCrack);
                                statas.SetCrystal(statas.GetCrystal() - 1); //�N���X�^�������������炷
                            }
                            HitFlg = false;
                            NowCrackspeed = CrackMoveSpeed;
                            MoveFlg = false;
                            ScriptPIManager.SetCrackMove(false);
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

                if (MinPointNum == PointNum - 1 || MinPointNum == PointNum - 2)
                {
                   
                    NowCrackspeed += CrackMoveSpeed * Time.deltaTime;
                    //----------------------------------
                    //�ړI�n(Point���W)�܂ňړ�
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, transform.position.z), CrackMoveSpeed * Time.deltaTime);

                    //-------------------------------------------------------------
                    //�ړI�n�Ƃ̋��������߂�
                    Distance = Vector3.Distance(this.transform.position,
                                     new Vector3(Edge.points[NowPointNum].x, Edge.points[NowPointNum].y, transform.position.z));

                    if (Distance <= 0)
                    {
                        //-----------------------------------------
                        //�I�_�܂ňړ�������I��
                        if (NowPointNum == 0)
                        {
                            if (statas.GetCrystal() > 0)
                            {
                                // �Ђт�����
                                Destroy(NowMoveCrack);
                                statas.SetCrystal(statas.GetCrystal() - 1); //�N���X�^�������������炷
                            }
                            HitFlg = false;
                            NowCrackspeed = CrackMoveSpeed;
                            MoveFlg = false;
                            ScriptPIManager.SetCrackMove(false);
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

                // �T�E���h�֌W
                seMana.PlaySE_CrackMove();

                break;
            case MoveState.CrackMoveEnd:

                thisrigidbody.constraints = RigidbodyConstraints2D.None;
                thisrigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

                // �����̂����蔻���L���ɂ���
                //thiscol.enabled = true;
                thiscol.size = colsize;
                // �A�j���[�V�����I�����Ă�����Move,Jump���ĊJ
                Jump.enabled = true;
                InCrack = false;
                Move.SetMovement(true);
                HitFlg = false;

                //--------------------------------
                // �A�j���[�V����
                if (Line <= 1.0f)
                {
                    Line += AnimSpeed * Time.deltaTime;
                    thisRenderer.material.SetFloat("_Border", Line);
                   
                }
                else
                {
                    //--------------------------------------------
                    //�ړ��I�����Ă��ĂЂт���o������s��ԂɑJ��
                    movestate = MoveState.Walk;
                    // �p�[�e�B�N���V�X�e�����\���A���g��\��
                    if (ParticleSystemObj != null)
                    {

                        ParticleSystemObj.active = false;
                    }

                }

                //thisRenderer.enabled = true;
                // �T�E���h�֌W
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
        //�ЂтƂԂ�������
        if(collision.tag == "Crack" && movestate == MoveState.Walk)
        {
            //----------------------------------
            //�Ђт̏����擾
            GameObject crackobj = collision.gameObject;
            Edge = crackobj.GetComponent<EdgeCollider2D>(); 

            Point = Edge.points;
            PointNum = Edge.pointCount;

            //---------------------------------------------
            //���ݒn����1�ԋ߂�Point���W�����߂�
            MinNearPoint = Edge.points[0];
            MinNearPointDistance =
            Vector3.Distance(this.transform.position, new Vector3(Edge.points[0].x, Edge.points[0].y, transform.position.z));
            MinPointNum = 0;

            for (int i = 1; i < PointNum; i++)
            {
                //-----------------------------------------
                //���������߂�
                float NearPoint =
                Vector3.Distance(this.transform.position, new Vector3(Edge.points[i].x, Edge.points[i].y, transform.position.z));

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
            if (MinPointNum == 0 || MinPointNum == 1 || MinPointNum == Edge.pointCount - 1 || MinPointNum == Edge.pointCount - 2)
            {
                HitFlg = true;

                //---------------------------------------
                //�@�q�I�u�W�F�N�g�̃}�e���������ׂĕύX
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
                // A�{�^���œ���
                if (ScriptPIManager.GetCrackMove() && InCrack == false)
                {
                    ScriptPIManager.SetCrackMove(false);
                    InCrack = true;
                    
                    NowPointNum = MinPointNum;
                    
                    //�@�Ђт̃I�u�W�F�N�g���擾
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
            //�@�q�I�u�W�F�N�g�̃}�e���������ׂĕύX
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
