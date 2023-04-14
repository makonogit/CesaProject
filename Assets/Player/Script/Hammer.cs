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
    private GameObject Camera;                  // �J����
    private VibrationCamera vibration;          // �J�����U���X�N���v�g


    public bool AddCrackFlg = false;            // �Ђт��L�т�t���O
    private bool LongCrack = false;             // �L�тĂ���ЂтȂ̂�
    private float angle;                        // �Ђт�����p�x
    public Vector2 OldFirstPoint;              // �O��̎n�_���W
   
    [Header("�Ђт̒���")]
    public float CrackLength;

    [SerializeField,Header("���ߋZ�̂������")]
    private float CrackPower;
    private float MoveLength;                  // ������ێ�����ϐ�

    public List<Vector2> CrackPointList;       //�Ђт̃��X�g


    //��ԊǗ�
    public enum HammerState
    {
        NONE,       // �������Ă��Ȃ�   
        POWER,      // ���ߋZ
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

    private bool _isStartHaloAnimation;// �G�t�F�N�g�J�n�t���O
    private haloEffect _haloEffect;// �G�t�F�N�g
    //�\�\�\�\�\�\�\�\�\�\�\�\//

    // ��{�ǉ�
    private float stopTime; // �q�b�g�X�g�b�v���Ă��鎞��
    public float HitStopTime = 0.3f; // �q�b�g�X�g�b�v�I������

    private Animator anim;
    private PlayerStatas playerStatus;
    [SerializeField,Header("�n���}�[�ł����ނ܂ł̑ҋ@����")]
    private float WaitHammer;                  // �n���}�[��ł܂ł̑҂�����
    private float WaitHammerMeasure = 0.0f;    // �n���}�[�ł����ݎ��Ԃ��v������ϐ�

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

        MoveLength = CrackLength;   //�@�Ђт̒�����ێ�

        // �Ђт̃|�C���g�Ɏ����̍��W���w��
        CrackPointList.Add(transform.position);
        CrackPointList.Add(Vector2.zero);       //List��1�Ԗڂ��m��

        //--------------------------------------------
        //CrackCreater���擾  //�\�ǉ��S���ҁF���쒼�o�\//
        _creater = _crackCreaterObj.GetComponent<CrackCreater>();

        // �ړ��X�N���v�g���擾����
        Move = GetComponent<PlayerMove>();

        //--------------------------------------------
        //haloEffect���擾  //�\�ǉ��S���ҁF���쒼�o�\//
        _haloEffect = GameObject.Find("HaloObj").GetComponent<haloEffect>();
        if (_haloEffect == null) Debug.LogError("haloEffect�̃R���|�[�l���g���擾�ł��܂���ł����B");

        _isStartHaloAnimation = false;

        //----------------------------------------------
        // SE�Đ��p�X�N���v�g�擾
        seobj = GameObject.Find("SE");
        se = seobj.GetComponent<SEManager_Player>();

        //----------------------------------------------
        // �J�����U���X�N���v�g�̎擾
        Camera = GameObject.Find("Main Camera");
        vibration = Camera.GetComponent<VibrationCamera>();

        AngleTest = GameObject.Find("Target");
        TargtRenderer = AngleTest.GetComponent<SpriteRenderer>();
        Targetstate = AngleTest.GetComponent<TestTargetState>();
        AngleTest.transform.position = transform.position;

        // �A�j���[�^�[�擾
        anim = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatas>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //�@�Ђт̎n�_����Ɏ����̍��W�Ɏw��
        CrackPointList[0] = transform.position;

        //------------------------------------------------------
        // ��Ԃɂ���ď���
        switch (hammerstate)
        {
            case HammerState.NONE:

                // �p�x�̉���
                TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                // �O��̍��W�Ƃ̋��������߂�
                float Distance = Vector3.Magnitude(CrackPointList[0] - OldFirstPoint);
                if (Distance < 0.5f)
                {
                    if (CrackManager.transform.childCount > 0)
                    {
                        NowCrack = CrackManager.transform.GetChild(CrackManager.transform.childCount - 1).GetComponent<CrackCreater>();
                        AddCrackFlg = true;
                    }
                }
                else
                {
                    OldFirstPoint = Vector2.zero;
                    AddCrackFlg = false;
                }
                
                //�@���������ŗ��ߋZ
                if (InputManager.GetNail_Left() && InputManager.GetNail_Right())
                {
                    hammerstate = HammerState.POWER;
                }

                //�g���K�[�������������������
                if (InputManager.GetNail_Right() && !InputManager.GetNail_Left())
                {
                    // �O��̍��W�Ƃ̋��������߂�
                    //float Distance = Vector3.Magnitude(CrackPointList[0] - OldFirstPoint);
                    //-----------------------------------------------------------------------
                    // �O��̈ʒu�Ƃ��܂�ړ����Ă��Ȃ����āA�O�̂Ђт��c���Ă���|�C���g�ǉ�
                    if (Distance < 0.5f && CrackManager.transform.childCount > 0) 
                    {
                        ////�@�Ђт��擾
                        //NowCrack = CrackManager.transform.GetChild(CrackManager.transform.childCount - 1).GetComponent<CrackCreater>();
                        
                        //AddCrackFlg = true;

                    }
                    else
                    {
                        // �p�x�̉���
                        //TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }

                    //�@�Ə�(��)���ǂɂ߂肱��łȂ�������
                    if (!Targetstate.CheeckGround || (AddCrackFlg && Targetstate.CheeckGround))
                    {
                        hammerstate = HammerState.DIRECTION;
                    }
                }

                break;

            case HammerState.POWER:

                //�@�ړ��ł��Ȃ��悤�ɂ���
                Move.SetMovement(false);
                // �Ə��̔�\��
                TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                if (!AddCrackFlg)
                //-----------------------------------------------------------------------------
                // �p�x�Ƌ���������W���v�Z
                {
                    // ���X�e�B�b�N�̓��͂���p�x���擾����
                    Vector2 LeftStick = InputManager.GetMovement();

                    //----------------------------------------
                    //�@�X�e�B�b�N�̓��͂�����Ίp�x�v�Z
                    if (LeftStick != Vector2.zero)
                    {
                        angle = Mathf.Atan2(LeftStick.y, LeftStick.x) * Mathf.Rad2Deg;

                        // �p�x�𐳋K��
                        if (angle < 0)
                        {
                            angle += 360;
                        }

                        //�@�p�x��45�x���ŊǗ�
                        //angle = (((int)angle / 45)) * 45.0f;

                    }
                    else
                    {
                        // �Ȃ����
                        angle = angle;

                    }
                }


                // �p�x�Ƌ�������Point���W�����߂�
                CrackPointList[1] = new Vector2(CrackPointList[0].x + (MoveLength * Mathf.Cos(angle * (Mathf.PI / 180))), CrackPointList[0].y + (MoveLength * Mathf.Sin(angle * (Mathf.PI / 180))));

                //�f�o�b�O�p
                //AngleTest.transform.position = new Vector3(CrackPointList[1].x, CrackPointList[1].y, 0.0f);


                //----------------------------------------------
                //�@����������Ă����璷�����X�V
                if (InputManager.GetNail_Left() && InputManager.GetNail_Right())
                {
                    vibration.SetControlerVibration();
                    MoveLength += CrackPower * Time.deltaTime;
                    StartHaloAnimation();//���ǉ���:���쒼�o �A�j���[�V�����J�n
                }
                else
                {
                   
                    if (MoveLength > CrackLength)
                    {
                         //�@�����������߂�
                         int segment = (int)(MoveLength / CrackLength);

                         //�@�O���̕���
                         for (int i = 0; i < segment / 2; i++) {
                          
                             CrackPointList.Insert(1,Vector2.Lerp(CrackPointList[0],CrackPointList[1],0.5f));
                             //Debug.Log(CrackPointList[1]);
                         }

                         //�@����̒ǉ�
                         for (int i = 0; i < segment - (segment / 2); i++)
                         {
                             CrackPointList.Insert(CrackPointList.Count - 1,
                                 Vector2.Lerp(CrackPointList[CrackPointList.Count - 2], CrackPointList[CrackPointList.Count - 1], 0.5f));
                         }

                    }
                    
                    // SE�Đ�
                    vibration.SetVibration(0.5f);
                    se.PlaySE_Crack1();
                    se.PlayHammer();

                    // �q�b�g�X�g�b�v������
                    playerStatus.SetHitStop(true);
                    anim.speed = 0.02f;
                    stopTime = 0.0f;

                    MoveLength = CrackLength;   //�@�����̏�����
                                                //�@�����ꂽ��ł����ݏ�Ԃɂ���
                    hammerstate = HammerState.HAMMER;

                    EndHaloAnimation();//���ǉ���:���쒼�o �A�j���[�V������~
                }
                
                break;

            case HammerState.DIRECTION:

                //�@�ړ��ł��Ȃ��悤�ɂ���
                Move.SetMovement(false);

                //�@���������ꂽ���Ԃ�߂�
                if (InputManager.GetNail_Left())
                {
                    hammerstate = HammerState.POWER;
                }

                if(!AddCrackFlg) 
                //-----------------------------------------------------------------------------
                // �p�x�Ƌ���������W���v�Z
                {
                    // ���X�e�B�b�N�̓��͂���p�x���擾����
                    Vector2 LeftStick = InputManager.GetMovement();
                    
                    //----------------------------------------
                    //�@�X�e�B�b�N�̓��͂�����Ίp�x�v�Z
                    if (LeftStick != Vector2.zero)
                    {
                        angle = Mathf.Atan2(LeftStick.y, LeftStick.x) * Mathf.Rad2Deg;

                        // �p�x�𐳋K��
                        if (angle < 0)
                        {
                            angle += 360;
                        }

                        //�@�p�x��45�x���ŊǗ�
                        //angle = (((int)angle / 45)) * 45.0f;

                    }
                    else
                    {
                        // �Ȃ����
                        angle = angle;

                    }
                }


                // �p�x�Ƌ�������Point���W�����߂�
                CrackPointList[1] = new Vector2(CrackPointList[0].x + (CrackLength * Mathf.Cos(angle * (Mathf.PI / 180))), CrackPointList[0].y + (CrackLength * Mathf.Sin(angle * (Mathf.PI / 180))));

                if (AddCrackFlg)
                {
                    // �p�x�̔�\��
                    TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
                else
                {
                    // �p�x�̉���
                    TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }

                //�f�o�b�O�p
                AngleTest.transform.position = new Vector3(CrackPointList[1].x, CrackPointList[1].y, 0.0f);

                //�g���K�[�𗣂�����Ђѐ������(�A�j���[�V�����I��)
                if (!InputManager.GetNail_Right())
                {
                    //�@�Ə�(��)���ǂɂ߂肱��łȂ�������
                    if (!Targetstate.CheeckGround || (AddCrackFlg && Targetstate.CheeckGround))
                    {

                        // SE�Đ�
                        vibration.SetVibration(0.5f);
                        se.PlaySE_Crack1();
                        se.PlayHammer();


                        // �q�b�g�X�g�b�v������
                        playerStatus.SetHitStop(true);
                        anim.speed = 0.02f;
                        stopTime = 0.0f;
                       
                        hammerstate = HammerState.HAMMER;
                       

                    }
                    else
                    {
                        // Point���W��������
                        AngleTest.transform.position = CrackPointList[0];
                        // �Ə���\��
                        TargtRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                        //// Point���W��������
                        //AngleTest.transform.position = CrackPointList[0];
                        //�@�ړ���������
                        Move.SetMovement(true);
                        hammerstate = HammerState.NONE;
                    }
                   
                }

                
                break;
            case HammerState.HAMMER:

                //�@�ҋ@���Ԍv��
                WaitHammerMeasure += Time.deltaTime;

                //---------------------------------
                // �ҋ@���Ԍo�߂��Ă�����
                if (WaitHammerMeasure > WaitHammer)
                {
                    //-----------------------------------------------------
                    //�@�O��̈ʒu����ړ����Ă��Ȃ�������|�C���g��ǉ�
                    if (AddCrackFlg)
                    {

                        if (NowCrack != null)
                        {
                            if (NowCrack.GetState() == CrackCreater.CrackCreaterState.CRAETED)
                            {
                                NowCrack.SetState(CrackCreater.CrackCreaterState.ADD_CREATEBACK);
                            }
                        }
                        else
                        {
                            Debug.Log("�Ђт�������܂���");
                        }

                        AddCrackFlg = false;

                    }
                    else
                    {
                        CallCrackCreater();  //�Ђѐ���
                    }

                    OldFirstPoint = CrackPointList[0];  // �������̍��W��ۑ�


                    WaitHammerMeasure = 0.0f;       // �o�ߗp�ϐ�������
                                             
                    AngleTest.transform.position = CrackPointList[0];   // Point���W��������
                    Move.SetMovement(true);
                    hammerstate = HammerState.NONE;

                }

                break;
            default:
                Debug.Log("HammerState�ɐݒ�ł��Ȃ����l���������Ă��܂�");
                break;
        }
        
        // ��{�ǉ�
        // �q�b�g�X�g�b�v
        if(stopTime < HitStopTime)
        {
            stopTime += Time.deltaTime;
        }
        else
        {
            // �q�b�g�X�g�b�v�I��
            anim.speed = 1f;
            playerStatus.SetHitStop(false);
        }

        // �A�j���[�V�����֌W

        // ���߃A�j���[�V����
        anim.SetBool("accumulate", hammerstate == HammerState.POWER || hammerstate == HammerState.DIRECTION);
        // �ЂуA�j���[�V����
        anim.SetBool("crack", hammerstate == HammerState.HAMMER);
        // �L�����Z��
        anim.SetBool("cansel", hammerstate == HammerState.NONE);

        Debug.Log(hammerstate);

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
        for (int i = 1; i < 2; i++)
        {
            CrackPointList[i] = Vector2.zero;
        }

        //�@�|�C���g��2��葽��������폜
        if(CrackPointList.Count > 2)
        {
            CrackPointList.RemoveRange(2, CrackPointList.Count - 2);
        }

    }

    //
    // �֐��FStartHaloAnimation()
    //
    // �ړI�FHalo�A�j���[�V��������񂾂��Đ�����
    // 
    private void StartHaloAnimation()
    {

        if (_isStartHaloAnimation != true)
        {
            _haloEffect.Play();
            _isStartHaloAnimation = true;
        }
    }
    //
    // �֐��FEndHaloAnimation()
    //
    // �ړI�FHalo�A�j���[�V�����������I�ɏI������
    // 
    private void EndHaloAnimation() 
    {
        if(_isStartHaloAnimation == true) 
        {
            _haloEffect.End();
            _isStartHaloAnimation = false;
        }
    }

}

