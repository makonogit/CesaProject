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
    private HaveNails HaveNails;          //�B�̏��������擾

    private int HammerNails;              //�B��ǂɑł�����

    [Header("�B�̃v���n�u�I�u�W�F�N�g")]
    public GameObject NailPrehubObj;      //�B�̃v���n�u

    Transform NailsTrans;                 // �B��TransForm

    // �O���擾
    private GameObject PlayerInputManager;      // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    public GameObject CrackPrefab;              // �Ђѐ����p�v���n�u�I�u�W�F�N�g
    private CrackOrder Order;                   // �Ђѐ����p�X�N���v�g

    [SerializeField, Header("�B�������\�ȋ���")]
    private float NailAddArea;                   //�B���g�傷��͈�
    private float HammerNailArea;                //�B�𐶐��\�Ȕ͈͂�ۑ�
    private float NailsDistance;                 //�O��̓B�Ƃ̋���
    [SerializeField, Header("�B�������\��")]
    private bool NailsCreateFlg = true;           //�B�������\��
    //public bool MousePointaHit = false;         //�}�E�X�|�C���^���ǂɓ������Ă��邩

    [Header("�Ђт��������ꂽ��")]
    public bool CreateCrack = false;            //�Ђт��������ꂽ��
    bool CrackVibration = false;                //�U���ɂ��e��

    [Header("�B�̍��W")]
    public List<Vector2> NailsPoint;            //�B�̍��W���擾

    // private GameObject MousePointa;             //�}�E�X�|�C���^�p�I�u�W�F�N�g
    // private SpriteRenderer MousePointaRender;   //�}�E�X�|�C���^�̃X�v���C�g�����_�[

    private GameObject NailTarget;      //�B�Ə��I�u�W�F�N�g
    private Transform NailTargetTrans;  //�B�Ə��I�u�W�F�N�g��TransForm

    private NailTargetMove NailTargetMove;      //�B�̈ړ�

    private GameObject FallGage;        //�ǂ̕���xUI
    private FallWall fallwall;          //�ǂ̕���x�X�N���v�g

    //----------------------------
    //�Ђт̊g��p�R���C�_�[�֌W
    private GameObject ChilCrackArea;           //�Ђъg��p�q�I�u�W�F�N�g 
    private CircleCollider2D CircleCol;         //�q�I�u�W�F�N�g�̃R���C�_�[�̏��
    [SerializeField,Header("�R���C�_�[�̍ő�T�C�Y")]
    private float MaxColSize;                   //�R���C�_�[�̍ő�T�C�Y
    [SerializeField, Header("�R���C�_�[�̊g��X�s�[�h")]
    private float ColExtendSpeed;               //�R���C�_�[�̊g��X�s�[�h


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
        HaveNails = GetComponent<HaveNails>();

        NailsTrans = NailPrehubObj.transform;

        //--------------------------------------------
        //�@�Ђѐ����p�X�N���v�g���擾
        Order = CrackPrefab.GetComponent<CrackOrder>();

        //--------------------------------------------
        //�}�E�X�|�C���^�p�I�u�W�F�N�g���擾
        //MousePointa = GameObject.Find("Fairy");
        //MousePointaRender = MousePointa.GetComponent<SpriteRenderer>();

        //--------------------------------------------
        //�B�Ə��I�u�W�F�N�g�̎擾
        NailTarget = GameObject.Find("NailTarget");
        NailTargetTrans = NailTarget.transform;
        NailTargetMove = NailTarget.GetComponent<NailTargetMove>();

        HammerNailArea = NailTargetMove.Radius;

        //----------------------
        //�Ǖ���UI�̏��擾
        FallGage = GameObject.Find("Gage");
        fallwall = FallGage.GetComponent<FallWall>();

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
        //--------------------------------------------
        //CrackCreater���擾  //�\�ǉ��S���ҁF���쒼�o�\//
        _creater = _crackCreaterObj.GetComponent<CrackCreater>();


    }

    // Update is called once per frame
    void Update()
    {

        //�q�I�u�W�F�N�g�ƍ��W�𓯊�
        ChilCrackArea.transform.position = transform.position;
        ////�}�E�X�|�C���^�̍��W����ɍX�V
        //Vector3 MousePos = (Vector2)Camera.main.ScreenToWorldPoint(ScriptPIManager.GetMousePos());
        //MousePointa.transform.position = MousePos;

        //----------------------------------------
        //�B�������Ă�����B����ʂɑł�
        if (HaveNails.NailsNum > 0)
        {
            //---------------------------------------------------------------------------
            //�O��ł����B�Ƃ̋��������߂�(��������Ă��Ȃ�������v���C���[�Ƃ̋���)
            if (HammerNails > 0)
            {
                NailsDistance = Vector3.Distance(NailTargetTrans.position, NailsPoint[HammerNails - 1]);
            }
            else
            {
                NailsDistance = Vector3.Distance(NailTargetTrans.position, this.transform.position);
            }

            //----------------------------------------------------------------
            //�����������\�ȋ�����������Ă��邩�Ő����\�t���O���w��
            if (NailsDistance < NailTargetMove.Radius && ScriptPIManager.GetPlayerMode() == global::PlayerInputManager.PLAYERMODE.AIM)
            {
                NailsCreateFlg = true;
            }
            else
            {
                NailsCreateFlg = false;
            }

            //----------------------------------------------------------------
            //�B�����\���ǂ����Ń}�E�X�|�C���^�̐F��ύX
            // MousePointaRender.color = NailsCreateFlg == true ? Color.white : Color.red;

            //---------------------------------------------------------------------------
            //���X�e�B�b�N�������݌��m+�Ђѐ���������Ȃ�+�B��łĂ�͈͂Ȃ�B�𐶐�
            if (Gamepad.current.leftStickButton.wasPressedThisFrame && !CreateCrack && NailsCreateFlg)
            {

                NailsTrans.position = NailTargetTrans.position;
                // ���[���h���W�ɕϊ�Z�̃J�������W�����������Ȃ�̂�Vector2�^�ɃL���X�g�ϊ����đΏ�
                //NailsTrans.position = (Vector2)Camera.main.ScreenToWorldPoint(NailsTrans.position);

                //�|�C���g���W��ǉ�
                NailsPoint.Add(NailsTrans.position);
                obj = Instantiate(NailPrehubObj, NailsTrans.position, Quaternion.identity) as GameObject;
                obj.AddComponent<PolygonCollider2D>();

                NailTargetMove.Radius += NailAddArea;
                HammerNails++;
                HaveNails.NailsNum--;

                //---------------------------------------------------------------------------
                //�\�ǉ��S���ҁF��{��\//
                // �����ꂽ�u�Ԃ̏�Ԃ�ێ����ēG�̈ړ����~�߂�
                MomentHitNails = true;
                ColLimitTime = 0.0f;

                Debug.Log("?????????????????????????");

            }
            //�\�ǉ��S���ҁF��{��\//
            if(MomentHitNails)
            {
                //---------------------------------------------------------------------------
                // �ł��t����ꂽ�u�ԈȊO�̃t���[���ɓB�̓����蔻��͕K�v�Ȃ�

                // �ŐV�̃I�u�W�F�N�g�ɃR���C�_�[�����Ă��������
                if (ColLimitTime > DestroyNailColTime)
                {
                    if (obj.GetComponent<PolygonCollider2D>())
                    {
                        // �R���C�_�[����
                        Destroy(obj.GetComponent<PolygonCollider2D>());
                        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        // �B��łu�ԈȊO��{false
                       
                    }
                    MomentHitNails = false;
                }
                ColLimitTime += Time.deltaTime;

            }
        }

        //-------------------------------------------------
        //�ǂɑł����B��2�ȏ�œB�������[�h�����łЂѐ���
        if(HammerNails > 1 && Gamepad.current.bButton.wasPressedThisFrame)
        {
            HammerNails = 0;
            CreateCrack = true;
        }

        //------------------------------------
        //�Ђѐ���
        if (CreateCrack)
        {
            NailTargetMove.Radius = HammerNailArea;  //�B�����͈͂�������

            CallCrackCreater();//�\�ǉ��S���ҁF���쒼�o�\//

            fallwall.CreateCrackFlg = CreateCrack;  //�Ђт̐����������X�N���v�g�ɓn��
            fallwall.NowCrackObj = NewCrackObj;
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
        _creater.SetPointList(NailsPoint);
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
