//-----------------------------
//�S���F�����S
//���e�F�Ђт�����ver2
//-----------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{

    //---------------------------------
    //�@�ϐ��錾

    //---------------------------------
    //�O���擾
    private GameObject InputanagerObj;          // InputManager�����I�u�W�F�N�g
    private PlayerInputManager InputManager;    // InputManager
    private GameObject AngleTest;               // �p�x���������邽��
    private SpriteRenderer TargtRenderer;       // �p�x�����p�̃����_�[
    private TestTargetState Targetstate;        // �Ђт����邩���f
    private PlayerMove Move;    �@              // �ړ��X�N���v�g
    private GameObject CrackManager;            // �S�Ă̂Ђт̐e�I�u�W�F�N�g
    private CrackCreater NowCrack;              // ���݂̂Ђт�Creater
    private GameObject seobj;                   // SE�I�u�W�F�N�g
    private SEManager_Player se;                // SE�Đ��p

    public bool AddCrackFlg = false;            // �Ђт��L�т�t���O
    private bool LongCrack = false;             // �L�тĂ���ЂтȂ̂�
    private float angle;                        // �Ђт�����p�x
    private Vector2 OldFirstPoint;              // �O��̎n�_���W
   
    [Header("�Ђт̒���")]
    public float CrackLength;            

    public List<Vector2> CrackPointList;       //�Ђт̃��X�g


    //��ԊǗ�
    public enum HammerState
    {
        NONE,       // �������Ă��Ȃ�   
        DIRECTION,  // ��������
        HAMMER,     // �@��
    }

    public HammerState hammerstate;     // ��ԊǗ��p�ϐ�

    //�\�ǉ��S���ҁF���쒼�o�\//
    [Header("�Ђт����obj")]
    public GameObject _crackCreaterObj;
    [System.NonSerialized]
    public GameObject NewCrackObj;  //�V�����q�r�̃I�u�W�F�N�g
    CrackCreater _creater;
    //�\�\�\�\�\�\�\�\�\�\�\�\//


    // Start is called before the first frame update
    void Start()
    {
        //-----------------------------------
        // InputManager���擾����
        InputanagerObj = GameObject.Find("PlayerInputManager");
        InputManager = InputanagerObj.GetComponent<PlayerInputManager>();

        //-----------------------------------
        // �Ђт̐e�I�u�W�F�N�g���擾
        CrackManager = GameObject.Find("CrackManager");

        // �ŏ��͉������Ă��Ȃ���Ԃɂ���
        hammerstate = HammerState.NONE;

        // �Ђт̃|�C���g�Ɏ����̍��W���w��
        CrackPointList.Add(transform.position);
        CrackPointList.Add(Vector2.zero);       //List��1�Ԗڂ��m��

        //--------------------------------------------
        //CrackCreater���擾  //�\�ǉ��S���ҁF���쒼�o�\//
        _creater = _crackCreaterObj.GetComponent<CrackCreater>();

        // �ړ��X�N���v�g���擾����
        Move = GetComponent<PlayerMove>();

        //----------------------------------------------
        // SE�Đ��p�X�N���v�g�擾
        seobj = GameObject.Find("SE");
        se = seobj.GetComponent<SEManager_Player>();

        AngleTest = GameObject.Find("Target");
        TargtRenderer = AngleTest.GetComponent<SpriteRenderer>();
        Targetstate = AngleTest.GetComponent<TestTargetState>();
    }

    // Update is called once per frame
    void Update()
    {

        //------------------------------------------------------
        // ��Ԃɂ���ď���
        switch (hammerstate)
        {
            case HammerState.NONE:

                // �p�x�̉���
                TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                //�@�Ђт̎n�_����Ɏ����̍��W�Ɏw��
                CrackPointList[0] = transform.position;

                //�g���K�[�������������������
                if (InputManager.GetNail_Right() && !InputManager.GetNail_Left())
                {
                    //---------------------------------------------
                    // �O��̈ʒu�ƈړ����Ă��Ȃ�������|�C���g�ǉ�
                    if (CrackPointList[0] == OldFirstPoint)
                    {
                        AddCrackFlg = true;
                    }
                    
                    hammerstate = HammerState.DIRECTION;
                }
                break;
            case HammerState.DIRECTION:

                // �p�x�̉���
                TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                //�@���������ꂽ���Ԃ�߂�
                if (InputManager.GetNail_Left())
                {
                    hammerstate = HammerState.NONE;
                }

                //�@�ړ��ł��Ȃ��悤�ɂ���
                Move.SetMovement(false);

                //�f�o�b�O�p
                AngleTest.transform.position = new Vector3(CrackPointList[1].x, CrackPointList[1].y, 0.0f);

                if(!AddCrackFlg) 
                //-----------------------------------------------------------------------------
                // �p�x�Ƌ���������W���v�Z
                {
                    // ���X�e�B�b�N�̓��͂���p�x���擾����
                    Vector2 LeftStick = InputManager.GetMovement();
                    angle = Mathf.Atan2(LeftStick.y, LeftStick.x) * Mathf.Rad2Deg;

                    // �p�x�𐳋K��
                    if (angle < 0)
                    {
                        angle += 360;
                    }

                    //�@�p�x��45�x���ŊǗ�
                    angle = (((int)angle / 45)) * 45.0f;

                    //----------------------------------------
                    //�@�X�e�B�b�N�̓��͂�����΍��W�v�Z
                    if (LeftStick != Vector2.zero)
                    {
                        // �p�x�Ƌ�������Point���W�����߂�
                        CrackPointList[1] = new Vector2(CrackPointList[0].x + (CrackLength * Mathf.Cos(angle * (Mathf.PI / 180))), CrackPointList[0].y + (CrackLength * Mathf.Sin(angle * (Mathf.PI / 180))));
                    }
                    else
                    {
                        // �Ȃ���ΑO��̍��W
                        CrackPointList[1] = CrackPointList[1];

                    }
                }

                //�g���K�[�𗣂�����Ђѐ������
                if (!InputManager.GetNail_Right())
                {
                    //�@�Ə�(��)���ǂɂ߂肱��łȂ�������
                    if (!Targetstate.CheeckGround || (AddCrackFlg && Targetstate.CheeckGround))
                    {
                        // SE�Đ�
                        se.PlaySE_Crack1();
                    }
                    hammerstate = HammerState.HAMMER;
                }

                break;
            case HammerState.HAMMER:

                //----------------------------------------------------
                //�@�O��̈ʒu����ړ����Ă��Ȃ�������|�C���g��ǉ�
                if (AddCrackFlg)
                {
                    NowCrack = CrackManager.transform.GetChild(CrackManager.transform.childCount - 1).GetComponent<CrackCreater>();
                    NowCrack.SetState(CrackCreater.CrackCreaterState.ADD_CREATEBACK);
                    AddCrackFlg = false;

                }
                else if(CrackPointList[1] != Vector2.zero)
                {
                    CallCrackCreater();  //�Ђѐ���
                }

                OldFirstPoint = CrackPointList[0];  // �������̍��W��ۑ�
              
                Move.SetMovement(true);
                hammerstate = HammerState.NONE;

                break;
            default:
                Debug.Log("HammerState�ɐݒ�ł��Ȃ����l���������Ă��܂�");
                break;
        }

        //----------------------------------------
        //�@�����I��������ړ�����
      
    }

    //-----------------------------------------------------------------
    //�\CrackCreater���ĂԊ֐��\           //�\�ǉ��S���ҁF���쒼�o�\//
    private void CallCrackCreater()
    {
        // �l�C�����W���X�g��n��
        _creater.SetPointList(CrackPointList);
        // CrackCreater�����
        NewCrackObj = Instantiate(_crackCreaterObj);

        // CrackManager�̎q�I�u�W�F�N�g�ɂ���
        NewCrackObj.transform.parent = CrackManager.transform;

        // �l�C�����W���X�g��������
        for (int i = 0; i < 2; i++)
        {
            CrackPointList[i] = Vector2.zero;
        }

    }
    //-----------------------------------------------------------------


}

