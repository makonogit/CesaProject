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
    private float NailsDistance;                 //�O��̓B�Ƃ̋���
    [SerializeField, Header("�B�������\��")]
    private bool NailsCreateFlg = true;         //�B�������\��
    //public bool MousePointaHit = false;         //�}�E�X�|�C���^���ǂɓ������Ă��邩

    bool CreateCrack = false;                   //�Ђт��������ꂽ��
    [Header("�B�̍��W")]
    public List<Vector2> NailsPoint;            //�B�̍��W���擾

    // private GameObject MousePointa;             //�}�E�X�|�C���^�p�I�u�W�F�N�g
    // private SpriteRenderer MousePointaRender;   //�}�E�X�|�C���^�̃X�v���C�g�����_�[

    private GameObject NailTarget;      //�B�Ə��I�u�W�F�N�g
    private Transform NailTargetTrans;  //�B�Ə��I�u�W�F�N�g��TransForm

    private NailTargetMove NailTargetMove;         //�B�̈ړ�

    //�\�ǉ��S���ҁF���쒼�o�\//
    [Header("�Ђт����obj")]
    public GameObject _crackCreaterObj;
    CrackCreater _creater;
    //�\�\�\�\�\�\�\�\�\�\�\�\//

    //�\�ǉ��S���ҁF��{��\//
    [Header("�Ђт������ꂽ�u�Ԃ̂�true�ɂȂ�")]
    public bool MomentHitNails = false;

    public float ColLimitTime; // �g�p�ςݓB���������ꂽ�u�Ԃ���̎���
    private float DestroyNailColTime = 1.0f; // �g�p�ς݂̓B�̃R���C�_�[�������鎞�� 

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

        NailsDistance = NailTargetMove.Radius;

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

        //------------------------------------
        //�Ђѐ���
        if (CreateCrack)
        {
            NailTargetMove.Radius = NailsDistance;
            CallCrackCreater();//�\�ǉ��S���ҁF���쒼�o�\//
        }
    }


    
    //-----------------------------------------------------------------
    //�\CrackCreater���ĂԊ֐��\           //�\�ǉ��S���ҁF���쒼�o�\//
    private void CallCrackCreater()
    {
        // �l�C�����W���X�g��n��
        _creater.SetPointList(NailsPoint);
        // CrackCreater�����
        GameObject obj = Instantiate(_crackCreaterObj);
        // �l�C�����W���X�g��������
        for (; 0 < NailsPoint.Count;)
        {
            NailsPoint.RemoveAt(0);
        }
    }
    //-----------------------------------------------------------------
}
