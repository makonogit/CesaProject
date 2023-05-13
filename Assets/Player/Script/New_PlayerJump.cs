//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�W�����v������
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_PlayerJump : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -
    private float MoveX = 0f; // �g�p�̗\��Ȃ�
    public float MoveY = 0f; // ���̃t���[���ł̃v���C���[��y���ړ���

    // �t���O�ϐ�
    [Header("�f�o�b�O�p�ϐ�")]
    [Header("��Ԍn")]
    [SerializeField] private bool TriggerJumpflg = false; // �W�����v�����ĂȂ��n�ʂɂ���Ƃ��ɃW�����v�{�^���������ꂽ��
    [SerializeField] private bool PressJumpflg = false; // �W�����v�{�^����������Ă����true
    [SerializeField] private bool OldPressJumpflg = false; // �O�t���[���̉����󋵂�ێ�
    [SerializeField] private bool ReleaseJumpflg = false; // �W�����v�𗣂�����true
    [SerializeField] private bool ImFly = false; // �󒆂ɂ��邩
    [SerializeField] private bool ImDrop = false; // �W�����v���o�R���Ȃ�����

    // �ڒn����n
    [Header("�ڒn����n")]
    [SerializeField] private bool isGround = false; // �n�ʂɐG��Ă��邩
    [SerializeField] private bool isOverhead = false; // �V��ɐG��Ă��邩
    //[SerializeField] private int RayNum; // �������Ă��郌�C�̖{��

    // �����\�ϐ�
    [Header("�����p�ϐ�")]
    [SerializeField] private float JumpPower = 3.0f; // �W�����v��
    [SerializeField] private float gravity; // �W�����v�p���[��������Ă��d��
    [Header("�����������t���[����"),SerializeField] private int inertia = 5; // �����t���[����

    // �f�o�b�O�p�ϐ�
    private int count = 0;

    //----------------------------------------------------------------------------------------------------------
    // �O���擾

    [Header("�O���擾")]
    // ���͌n
    [SerializeField] private PlayerInputManager _playerInputManager; // ���V�[�����̃I�u�W�F�N�g�̂���public
    [SerializeField] private InputTrigger _trigger; // �g���K�[�𓾂�

    // ���W�n
    [SerializeField] private Transform _thisTransform;  //���g�̍��W
    [SerializeField] private Rigidbody2D _thisRigidbody2d; // ���W�b�h�{�f�B�[

    // �ڒn����n
    [SerializeField] private GroundCheck ground; // �ڒn����p�̃X�N���v�g���擾����ϐ�
    [SerializeField] private OverheadCheck overhead; // �ڒn����p�̃X�N���v�g���擾����ϐ�

    // �X�e�[�^�X�n
    [SerializeField] private PlayerStatas _playerStatus; // �v���C���[�̃X�e�[�^�X�������Ă���

    // �A�j���[�V�����n
    [SerializeField] private Animator anim;

    // �T�E���h�n
    [SerializeField] private SEManager_Player seMana;

    // �Ђьn
    //-------------��----------------
    [SerializeField] private CrackAutoMove crackmove;

    // �G�t�F�N�g�n
    //------------����---------------
    [SerializeField] private RunDustParticle _runDust;
    //-------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // ���͌n
        _trigger = _playerInputManager.GetComponent<InputTrigger>();

        // �X�e�[�^�X�n
        _playerStatus = GetComponent<PlayerStatas>();

        // ���W�n
        _thisTransform = GetComponent<Transform>();

        // ���̔䗦���悢 Deltatime���p�ɂ�萳�����l������悤�ɂ���5/11
        //gravity = JumpPower / 10f;

        //------------�ǉ��S���F��--------------------
        crackmove = GetComponent<CrackAutoMove>();

        //--------------------------------------
        // �ǉ��S����:���쒼�o
        //_runDust = GetComponent<RunDustParticle>();
        if (_runDust == null) Debug.LogError("RunDustParticle�̃R���|�[�l���g���擾�ł��܂���ł����B");
        //--------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------------------
        // �ǉ��S����:���쒼�o
        RunDust();


        //--------------------------------------
        // �q�b�g�X�g�b�v���łȂ��Ȃ���s
        if (!_playerStatus.GetHitStop())
        {

            // �d�͖�����
            _thisRigidbody2d.gravityScale = 0.0f;

            //--------------------------------------
            // �ڒn����

            // �ڒn����𓾂�
            isGround = ground.IsGroundCircle();
            // �n�ʂƐG��Ă��郌�C�̐����擾
            //RayNum = ground.GetRayNum();
            // �V��̏Փ˔���𓾂�
            isOverhead = overhead.IsOverHead();

            //-----------------------------------------
            // ���͎擾

            // �g���K�[
            TriggerJumpflg = _trigger.GetJumpTrigger(); // �����ꂽ�u�Ԃ�
            // �v���X
            PressJumpflg = _playerInputManager.GetJump(); // ������Ă��邩

            if (ReleaseJumpflg == false)
            {
                // �����[�X
                ReleaseJumpflg = OldPressJumpflg == true && PressJumpflg == false; // �����ꂽ�u�Ԃ�
            }

            //----------------------------------------------------
            // �W�����v�{�^���������ꂽ�u��

            // �n�ʂɂ���Ƃ���
            if (isGround == true)
            {
                // �W�����v���͂����ꂽ��
                if (TriggerJumpflg == true)
                {
                    // ���͔��
                    ImFly = true;

                    // 1�t���[���̃v���C���[�̈ړ��ʃZ�b�g
                    MoveY = JumpPower;

                    // �W�����vse�Đ�
                    seMana.PlaySE_Jump();

                    count = 0;
                }
            }

            // �R���痎������
            if (ImFly == false && isGround == false && ImDrop == false)
            {
                // ���͗�����
                ImDrop = true;

                // �������Ă�������
                MoveY = 0f;
            }

            // �W�����v�{�^����������Ă�����
            if (ImFly == true || ImDrop == true)
            {
                // �v���C���[Y���W�ω�
                _thisTransform.Translate(0f, MoveY * Time.deltaTime, 0f);

                // �d�͂̉e�����󂯂�����
                MoveY -= gravity * Time.deltaTime;
            }

            if (isGround == false && PressJumpflg == false)
            {
                // ����

                // �{�^���������ꂽ����y�ړ��ʂɂ���Ċ����𓭂�����
                if (MoveY > JumpPower / 100f * inertia)
                {
                    // ����
                    MoveY = JumpPower / 100f * inertia;
                }
            }

            // �V��ɓ���������
            if(MoveY > 0f && isOverhead == true && ImFly == true)
            {
                // �����J�n
                MoveY = 0f;
            }

            // �W�����v�̏������I����Ēn�ʂɂ����Ƃ�(�W�����v�����͂��ꂽ�t���[���ł͓���Ȃ�)
            if (isGround == true && ImFly == true && (TriggerJumpflg == false) && count > 50)
            {                                                                  
                // ��A�̃W�����v�I��                                          
                ImFly = false;                                                 
                MoveY = 0f;
                ReleaseJumpflg = false;
                //Debug.Log(count);                                            
            }                                                                  
                                                                               
            // �W�����v���o�R���Ȃ��������I���                                
            if (isGround == true && ImDrop == true)                            
            {                                                                  
                // �����I��                                                    
                ImDrop = false;                                                
                MoveY = 0f;                                                    
            }                                                                  
                                                                               
            // �o�O���p �����ǂł��Ȃ�������������������������������������
            count++;

            //if (TriggerJumpflg == true)
            //{
            //    Debug.Log("-----------------------------------------------------------------------------------");
            //}
            //if (MoveY != 0)
            //{
            //    Debug.Log(MoveY);
            //}

            //-------------------------------------------------------------------
            // �A�j���[�V�����֌W
            // �㏸�A�j���[�V����
            anim.SetBool("jump", MoveY > 0f);
            // �����A�j���[�V����
            anim.SetBool("drop", MoveY < 0f);

            // �O�t���[���̉����󋵂�ێ�
            OldPressJumpflg = PressJumpflg;

            // �d�͂��ǂ�
            _thisRigidbody2d.gravityScale = 1.0f;
        }
        else
        {
            // �q�b�g�X�g�b�v�����d��0
            if (_thisRigidbody2d.gravityScale == 1.0f)
            {
                _thisRigidbody2d.gravityScale = 0.0f;
            }

            Debug.Log(_thisRigidbody2d.gravityScale);
        }
    }

    ////�@�Z���N�g��ʂőI�����Ă��邩���f����֐�
    //public void SetSelected(bool select)
    //{
    //    Selected = select;
    //}

    private void RunDust()
    {
        _runDust.IsJump = !isGround;
    }
}
