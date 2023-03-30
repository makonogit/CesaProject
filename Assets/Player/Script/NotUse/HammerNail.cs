//-----------------------------------------
//�S���ҁF�����S
//����  �F�B��ł�
//-----------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HammerNail : MonoBehaviour
{
    private int HammerNails;              //�B��ǂɑł�����

    [Header("�B�̃v���n�u�I�u�W�F�N�g")]
    public GameObject NailPrehubObj;      //�B�̃v���n�u

    public Transform NailsTrans;                 // �B��TransForm

    // �O���擾
    // ���͊֌W
    private GameObject PlayerInputManager;      // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private InputTrigger ScriptPITrigger;       // InputTrigger���擾�����ϐ�

    public GameObject CrackPrefab;              // �Ђѐ����p�v���n�u�I�u�W�F�N�g
    private CrackOrder Order;                   // �Ђѐ����p�X�N���v�g

    [SerializeField, Header("�B�������\�ȋ���")]
    private float NailAddArea;                   //�B���g�傷��͈�
    private float HammerNailArea;                //�B�𐶐��\�Ȕ͈͂�ۑ�
    private float NailsDistance;                 //�O��̓B�Ƃ̋���
    [SerializeField, Header("�B�������\��")]
    private bool NailsCreateFlg = true;           //�B�������\��

    private GameObject UsedNailManager;           //�ǂɑł����B�̊Ǘ��p�e�I�u�W�F

    //public bool MousePointaHit = false;         //�}�E�X�|�C���^���ǂɓ������Ă��邩

    [Header("�Ђт��������ꂽ��")]
    public bool CreateCrack = false;            //�Ђт��������ꂽ��
    bool CrackVibration = false;                //�U���ɂ��e��

    [Header("�B�̍��W")]
    public List<Vector2> NailsPoint;            //�B�̍��W���擾

    private GameObject NailTarget;      //�B�Ə��I�u�W�F�N�g
    private Transform NailTargetTrans;  //�B�Ə��I�u�W�F�N�g��TransForm
    private SpriteRenderer TargetRender;

    private NailTargetMove NailTargetMove;      //�B�̈ړ�

    private GameObject FallGage;        //�ǂ̕���xUI
    private FallWall fallwall;          //�ǂ̕���x�X�N���v�g

    // ��{�ǉ�
    private GameObject Target;
    private NailTargetMove TargetMove;
    private PlayerStatas status;

    // SE�̌��ʉ�-------�S���F����--------
    [Header("���ʉ�")]
    private AudioSource audioSource;  // �擾����AudioSource�R���|�[�l���g
    [SerializeField] AudioClip sound1; // �����t�@�C��


    //----------------------------
    //�Ђт̊g��p�R���C�_�[�֌W
    private GameObject ChilCrackArea;           //�Ђъg��p�q�I�u�W�F�N�g 
    private CircleCollider2D CircleCol;         //�q�I�u�W�F�N�g�̃R���C�_�[�̏��
    [SerializeField,Header("�R���C�_�[�̍ő�T�C�Y")]
    private float MaxColSize;                   //�R���C�_�[�̍ő�T�C�Y
    [SerializeField, Header("�R���C�_�[�̊g��X�s�[�h")]
    private float ColExtendSpeed;               //�R���C�_�[�̊g��X�s�[�h

    //---------------------------------------------------------------
    private CircleCollider2D NailAreaCol;      //�Ђт��Ȃ���͈�

    //---------------------------------------
    //�ł����ݏ��

    public enum HammerState
    {
        NONE,   //�������Ă��Ȃ�
        NAILSET,//�\����
        HAMMER, //�ł�
    }

    public HammerState _HammerState;

    GetCrackPoint getCrackPoint; //�Ђт̃��X�g���擾
    SetNailList setNailList;     //�����ςݏ�Ԃɂ��邽��

    //�\�ǉ��S���ҁF���쒼�o�\//
    [Header("�Ђт����obj")]
    public GameObject _crackCreaterObj;
    [System.NonSerialized]
    public GameObject NewCrackObj;  //�V�����q�r�̃I�u�W�F�N�g
    CrackCreater _creater;
    //�\�\�\�\�\�\�\�\�\�\�\�\//

    //�\�ǉ��S���ҁF��{��\//
    [Header("�Ђт������ꂽ�u�Ԃ̂�true�ɂȂ�")]
    public bool MomentHitNails = false;

    public float ColLimitTime; // �g�p�ςݓB���������ꂽ�u�Ԃ���̎���
    private float DestroyNailColTime = 0.2f; // �g�p�ς݂̓B�̃R���C�_�[�������鎞�� 

    GameObject obj; // ������Q�[���I�u�W�F�N�g���L���X�g���邽��



    // Start is called before the first frame update
    void Start()
    {
        HammerNails = 0;

        //--------------------------------------------
        // �B�̊Ǘ��X�N���v�g���擾
        status = GetComponent<PlayerStatas>();

        NailsTrans = NailPrehubObj.transform;

        //--------------------------------------------
        //�@�Ђѐ����p�X�N���v�g���擾
        Order = CrackPrefab.GetComponent<CrackOrder>();

        getCrackPoint = GetComponentInChildren<GetCrackPoint>();

        //--------------------------------------------
        //�B�Ə��I�u�W�F�N�g�̎擾
        NailTarget = GameObject.Find("NailTarget");
        NailTargetTrans = NailTarget.transform;
        NailTargetMove = NailTarget.GetComponent<NailTargetMove>();
        TargetRender = NailTarget.GetComponent<SpriteRenderer>();
        HammerNailArea = NailTargetMove.Radius;

        //----------------------
        //�Ǖ���UI�̏��擾
        //FallGage = GameObject.Find("Gage");
        //fallwall = FallGage.GetComponent<FallWall>();

        //--------------------------------------------
        // �q�I�u�W�F�N�g��CicreCollider���擾
        ChilCrackArea = GameObject.Find("CrackGrowArea");
        CircleCol = ChilCrackArea.GetComponent<CircleCollider2D>();
        MaxColSize = 4.0f;
        ColExtendSpeed = 15.0f;

        //--------------------------------------------
        //InputManagr���擾
        PlayerInputManager = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();
        ScriptPITrigger = PlayerInputManager.GetComponent<InputTrigger>();

        //--------------------------------------------
        //CrackCreater���擾  //�\�ǉ��S���ҁF���쒼�o�\//
        _creater = _crackCreaterObj.GetComponent<CrackCreater>();

        //---------------------------------------------
        //�ł����ݏ�Ԃ��������Ă��Ȃ��ɂ���
        _HammerState = HammerState.NONE;

        Target = GameObject.Find("NailTarget");
        TargetMove = Target.GetComponent<NailTargetMove>();

        //-----------------------------------------------
        // �g�p�ςݓB�Ǘ��I�u�W�F�N�g���擾
        UsedNailManager = GameObject.Find("UsedNailManager");

        //------------------------------------------------
        // AudioSource���擾----�ǉ��S���F����-------
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

        //�q�I�u�W�F�N�g�ƍ��W�𓯊�
        ChilCrackArea.transform.position = transform.position;

        //-----------------------------------------------------------
        //��ԑJ��
        //�Е������������݂��ꂽ��\�����Ԃɂ���
        if (((!ScriptPITrigger.GetNailTrigger_Left() && ScriptPITrigger.GetNailTrigger_Right()) ||
            (ScriptPITrigger.GetNailTrigger_Left() && !ScriptPITrigger.GetNailTrigger_Right())) &&
            _HammerState == HammerState.NONE)
        {
            _HammerState = HammerState.NAILSET;

        }
        //----------------------------------------------------------
        //�\�����ɗ����������܂ꂽ��B��łĂȂ���Ԃɂ���
        if (ScriptPIManager.GetNail_Left() && ScriptPIManager.GetNail_Right() &&
           _HammerState == HammerState.NAILSET)
        {
            _HammerState = HammerState.NONE;
        }

        //----------------------------------------------------------
        //���������ꂽ��ł����ݏ�Ԃɂ���
        if (!ScriptPIManager.GetNail_Left() && !ScriptPIManager.GetNail_Right())
        {
            //�����ꂽ�u�ԑł����ݏ�Ԃɂ���
            if (_HammerState == HammerState.NAILSET)
            {
                _HammerState = HammerState.HAMMER;
            }
            else
            {
                _HammerState = HammerState.NONE;
            }

        }

        //----------------------------------------
        //�B�������Ă�����B����ʂɑł�
        if (status.GetNail() > 0)
        {
            //---------------------------------------------------------------------------
            //�O��ł����B�Ƃ̋��������߂�(��������Ă��Ȃ�������v���C���[�Ƃ̋���)
            //if (HammerNails > 0)
            //{
            //    NailsDistance = Vector3.Distance(NailTargetTrans.position, NailsPoint[HammerNails - 1]);
            //}
            //else
            //{
            //    NailsDistance = Vector3.Distance(NailTargetTrans.position, this.transform.position);
            //}

            //----------------------------------------------------------------
            //�����������\�ȋ�����������Ă��邩�Ő����\�t���O���w��
            //if (NailsDistance < NailTargetMove.Radius)
            //{
            //    NailsCreateFlg = true;
            //}
            //else
            //{
            //    NailsCreateFlg = false;
            //}

            

            //---------------------------------------------------------------------------
            //�\���Ă�����B�͈̔͂�����
            if (_HammerState == HammerState.NAILSET)
            {

            }

            //---------------------------------------------------------------------------
            //�ł����ݏ�ԂȂ琶��
            if (_HammerState == HammerState.HAMMER && TargetRender.color == Color.cyan)
            {
                //---------------------------------------------------------------------------------
                // �����t�@�C�����Đ�����-----�S���F����-------
                //if (!audioSource.isPlaying)
                //{
                //    audioSource.PlayOneShot(sound1);
                //}

                NailsTrans.position = NailTargetTrans.position;
                
                //�|�C���g���W��ǉ�
                //NailsPoint.Add(NailsTrans.position);
                obj = Instantiate(NailPrehubObj, transform.position, Quaternion.identity) as GameObject;
                
                // UsedNailManager�̎q�I�u�W�F�N�g�Ƃ��Đ���
                obj.transform.parent = UsedNailManager.transform;
                //obj.AddComponent<PolygonCollider2D>();
                
                NailTargetMove.Radius += NailAddArea;
                HammerNails++;
                status.SetNail(status.GetNail() - 1);

                //---------------------------------------------------------------------------
                //�\�ǉ��S���ҁF��{��\//
                // �����ꂽ�u�Ԃ̏�Ԃ�ێ����ēG�̈ړ����~�߂�
                MomentHitNails = true;
                ColLimitTime = 0.0f;
                //---------------------------------------------------------------------------

            }
           
        }

        //�\�ǉ��S���ҁF��{��\//
        //if (MomentHitNails)
        //{
        //    //---------------------------------------------------------------------------
        //    // �ł��t����ꂽ�u�ԈȊO�̃t���[���ɓB�̓����蔻��͕K�v�Ȃ�

        //    // �ŐV�̃I�u�W�F�N�g�ɃR���C�_�[�����Ă��������
        //    if (ColLimitTime > DestroyNailColTime)
        //    {
        //        if (obj.GetComponent<PolygonCollider2D>())
        //        {
        //            // �R���C�_�[����
        //            Destroy(obj.GetComponent<PolygonCollider2D>());

        //        }

        //        // �B��łu�ԈȊO��{false
        //        MomentHitNails = false;
        //    }
        //    ColLimitTime += Time.deltaTime;

        //}

        //-------------------------------------------------
        //�ǂɑł����B��2�ȏ��B�{�^�������ƂЂѐ���
        if (HammerNails > 1 && getCrackPoint.GetPointLest().Count >= 2 &&
            (ScriptPIManager.GetNail_Left() && ScriptPIManager.GetNail_Right()))
        {
            Debug.Log("�Ђѐ���");
            HammerNails = 0;
            CreateCrack = true;
        }

        //------------------------------------
        //�Ђѐ���
        if (CreateCrack)
        {

            CallCrackCreater();//�\�ǉ��S���ҁF���쒼�o�\//

            //fallwall.CreateCrackFlg = CreateCrack;  //�Ђт̐����������X�N���v�g�ɓn��
            //fallwall.NowCrackObj = NewCrackObj;

            TargetMove.CreateCrack = CreateCrack;

            for (int i = 1; i < getCrackPoint.objectList.Count; i++)
            {
                setNailList = getCrackPoint.objectList[i].GetComponentInChildren<SetNailList>();
                setNailList.Crackend = true;
            }

            //-------------------------------------------------
            //�Ђт̐������I��������|�C���g���X�g������������
            getCrackPoint.objectList.Clear();
            getCrackPoint.objectList.Add(gameObject);
            getCrackPoint.GetPointLest().Clear();
            getCrackPoint.SetPoint(transform.position);

            CreateCrack = false;
            CrackVibration = true;
            CircleCol.enabled = true;
        }

        //------------------------------------
        //�Ђт𐬒�������͈͂��g�債�Ă���
        if (CrackVibration)
        {
            //------------------------
            //�R���C�_�[���g�傷��
            if (CircleCol.radius < MaxColSize)
            {
                CircleCol.radius += ColExtendSpeed * Time.deltaTime;
            }
            else
            {
                //--------------------------------
                //�ő�܂ő傫���Ȃ����珉����
                CircleCol.radius = 0.0f;
                CrackVibration = false;
                CircleCol.enabled = false;
            }
        }

    }


    
    //-----------------------------------------------------------------
    //�\CrackCreater���ĂԊ֐��\           //�\�ǉ��S���ҁF���쒼�o�\//
    private void CallCrackCreater()
    {
        // �l�C�����W���X�g��n��
        _creater.SetPointList(getCrackPoint.GetPointLest());
        // CrackCreater�����
        NewCrackObj = Instantiate(_crackCreaterObj);
        // �l�C�����W���X�g��������
        for (; 0 < NailsPoint.Count;)
        {
            NailsPoint.RemoveAt(0);
        }
    }
    //-----------------------------------------------------------------

}
