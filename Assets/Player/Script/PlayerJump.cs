//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�̃W�����v
//�@�@�@�F�����Ă�ԍ������㏸����^�C�v
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    // �W�����v�p
    public float JumpPower = 20.0f; // �W�����v��
    public float JumpHeight = 5.0f; // �W�����v�ł��鍂��
    public float Gravity = 6.0f; // �d��
    private float JumpPos; // �W�����v����u�Ԃ̃v���C���[�̍���
    [SerializeField]
    private bool isGround = false; // �n�ʂɐG��Ă��邩
    private bool isOverhead = false; // �V��ɐG��Ă��邩
    [SerializeField]
    private bool isJump = false; // �W�����v�����ǂ���
    public float axel = 6.3f; // �d�͉����x  ���K���킹��5/10
    public float JumpTime = 0.0f; // �W�����v���n�܂��Ă��痎���n�߂�܂ł̌o�ߎ���
    public float FallTime = 0.0f; // �����n�߂Ă���̎���
    public int RayNum; // �������Ă��郌�C�̖{��
    public bool fall = false;

    // �u���b�N�̏�ɂ����Ԃł�FallTime������h��
    private float oldPosY; // �O�t���[����Y���W

    // �O���擾
    private GameObject PlayerInputManager; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform thisTransform; // ���g��Transform���擾����ϐ�
    private GroundCheck ground; // �ڒn����p�̃X�N���v�g���擾����ϐ�
    private OverheadCheck overhead; // �ڒn����p�̃X�N���v�g���擾����ϐ�
    private Rigidbody2D thisRigidbody2d; // rigidbody2d���擾����ϐ�
    private InputTrigger trigger;

    // �T�E���h�֌W
    private GameObject se;
    private SEManager_Player seMana;

    // �W�����vse����
    private bool playJumpSe = false;

    // �A�j���[�V�����֌W
    private Animator anim;

    private PlayerStatas playerStatus;

    //-------------��----------------
    private CrackAutoMove crackmove;        
    private Crack createcrack;      //�W�����v���Ђѐ����ł��Ȃ��悤�ɂ���
    private GameObject LineObj;
    private PredictionLine Line;    //�W�����v���̗\������������
    private bool Selected = false;  //�Z���N�g��ʗp

    // SE�̌��ʉ�-------�S���F����--------
    [Header("���ʉ�")]
    private AudioSource audioSource;  // �擾����AudioSource�R���|�[�l���g
    [SerializeField] AudioClip sound1; // �����t�@�C��

    //------------����---------------
    private RunDustParticle _runDust;
    //-------------------------------

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------------------------------------------------------
        // AudioSource�R���|�[�l���g���擾----�ǉ��S���F����-------
        audioSource = GetComponent<AudioSource>();

        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManager��T��
        PlayerInputManager = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

        trigger = PlayerInputManager.GetComponent<InputTrigger>();

        //----------------------------------------------------------------------------------------------------------
        // ���g(player)�̎���Transform���擾����
        thisTransform = this.GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // ���g��rigidbody2d�擾
        thisRigidbody2d = GetComponent<Rigidbody2D>();

        //----------------------------------------------------------------------------------------------------------
        //�����蔻�菈���擾
        ground = GetComponent<GroundCheck>();
        overhead = GetComponent<OverheadCheck>();

        //------------�ǉ��S���F��--------------------
       // createcrack = GetComponent<Crack>();
        crackmove = GetComponent<CrackAutoMove>();

        //  LineObj = GameObject.Find("Line");
        //  Line = LineObj.GetComponent<PredictionLine>();

        // Animator�擾
        anim = GetComponent<Animator>();

        // �T�E���h�֌W
        se = GameObject.Find("SE");
        // Se�R���|�[�l���g�擾
        seMana = se.GetComponent<SEManager_Player>();

        playerStatus = GetComponent<PlayerStatas>();
        //--------------------------------------
        // �ǉ��S����:���쒼�o
        _runDust = GetComponent<RunDustParticle>();
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
        if (!playerStatus.GetHitStop())
        {
            if(thisRigidbody2d.gravityScale == 0.0f && !Selected)
            {
                thisRigidbody2d.gravityScale = 1.0f;
            }

            //----------------------------------------------------------------------------------------------------------
            // �ڒn����𓾂�
            isGround = ground.IsGround();
            // �n�ʂƐG��Ă��郌�C�̐����擾
            RayNum = ground.GetRayNum();
            // �V��̏Փ˔���𓾂�
            isOverhead = overhead.IsOverHead();

            if (isJump == true)
            {
                isGround = false;
            }

            //Debug.Log(RayNum);

            //----------------------------------------------------------------------------------------------------------
            // �������͂���Ă��Ȃ���΂��̂܂܂̒l���v���C���[�̍��W�ɉ��Z���邱�ƂɂȂ�
            float xSpeed = 0.0f; //�v���C���[���W�ɉ��Z����Ƃ��ɕK�v�ȕϐ��A���ړ����̂�PlayerMove�Ŏ���
            float ySpeed = -Gravity; // �d�͂ɂ�鎩�R�����̒l�A�W�����v������Ώ㏑�������

            //----------------------------------------------------------------------------------------------------------
            // �W�����v����p�ϐ�
            bool Jump = false;

            //----------------------------------------------------------------------------------------------------------
            // ��x�ڂ̃W�����v�ȍ~���͂���߂�܂ŃW�����v�ł��Ȃ��悤�ɂ���
            if (ScriptPIManager.GetJumpTrigger() == true && isOverhead == false)
            {
                //----------------------------------------------------------------------------------------------------------
                // �W�����v�{�^����������Ă��邩�擾
                Jump = ScriptPIManager.GetJump();
            }

            //----------------------------------------------------------------------------------------------------------
            // �n�ʂɂ��Ă��鎞�ɃW�����v�����͂��ꂽ
            if (isGround)
            {
                //----------------------------------------------------------------------------------------------------------
                // �W�����v���͂���Ă����
                if (Jump)
                {
                    //----------------------------------------------------------------------------------------------------------
                    // �W�����v�ɂ���ď㏸����ʂ�ϐ��ɃZ�b�g
                    ySpeed = JumpPower * axel * JumpTime;

                    //----------------------------------------------------------------------------------------------------------
                    // �W�����v�����u�Ԃ̈ʒu���L��
                    JumpPos = thisTransform.position.y;

                    //----------------------------------------------------------------------------------------------------------
                    // ��Ԃ��W�����v���ɐݒ�
                    isJump = true;

                    if (playJumpSe == false)
                    {
                        // �W�����vse�Đ�
                        seMana.PlaySE_Jump();

                        playJumpSe = true;
                    }
                }
                //----------------------------------------------------------------------------------------------------------
                // �W�����v���͂���Ă��Ȃ����
                else
                {
                    //----------------------------------------------------------------------------------------------------------
                    // ��Ԃ��W�����v���ɐݒ�
                    isJump = false;
                }
            }
            //----------------------------------------------------------------------------------------------------------
            // �W�����v���ɃW�����v���͂�����
            else if (isJump)
            {
                //---------------------------------------------------------------------------------
                // �����t�@�C�����Đ�����-----�S���F����-------
                //if (!audioSource.isPlaying)
                //{
                //    audioSource.PlayOneShot(sound1);
                //}
                playJumpSe = false;

                //----------------------------------------------------------------------------------------------------------
                // �W�����v�{�^����������Ă���B����,���݂̍������W�����v�����ʒu���玩���̌��߂��ʒu��艺�Ȃ�W�����v�p��
                if (Jump == true/* && JumpPos + JumpHeight > transform.position.y*/)
                {
                    //----------------------------------------------------------------------------------------------------------
                    // �W�����v�ɂ���ď㏸����ʂ�ϐ��ɃZ�b�g
                    ySpeed = JumpPower - (axel * JumpTime);

                    if(ySpeed < 0f)
                    {
                        fall = true;
                        JumpTime = 0f;
                    }
                }
                else
                {
                    //----------------------------------------------------------------------------------------------------------
                    // ��Ԃ��W�����v���ɐݒ�
                    isJump = false;

                    fall = true;
                    JumpTime = 0f;

                    //---------------------------------------------------------------
                    // �W�����v����
                    ScriptPIManager.SetJumpTrigger(false);
                }
            }

            //----------------------------------------------------------------------------------------------------------
            // ���R�����i�����x�����j

            // �n�ʂɂ��ĂȂ����A�W�����v���͂��Ȃ�(PlayerInputManager�X�N���v�g�̕ϐ�Reset��true��������)��
            //if ((isGround == false && isJump == false && (crackmove.movestate == CrackAutoMove.MoveState.Walk || crackmove.movestate == CrackAutoMove.MoveState.CrackMoveEnd)))
            if(fall == true)
            {
                if (RayNum == 0)
                {
                    //----------------------------------------------------------------------------------------------------------
                    // ���R�����A������Ԃ̎��Ԃ��o�Ă΂��قǗ������x�㏸
                    ySpeed = -Gravity - (axel * FallTime);

                    if (thisTransform.position.y != oldPosY)
                    {
                        //----------------------------------------------------------------------------------------------------------
                        // ������Ԃł̌o�ߎ��Ԃ����Z
                        FallTime += Time.deltaTime;
                    }
                }
                else
                {
                    // ������Ԃł̌o�ߎ��Ԃ�������
                    FallTime = 0.0f;

                    fall = false;
                    // �n�ʂɒ��n����u�ԂɍĐ�
                    if (FallTime != 0.0f)
                    {
                        seMana.PlaySE_Drop();
                    }
                }
            }
            else
            {
                
                //----------------------------------------------------------------------------------------------------------
 
            }

            //----------------------------------------------------------------------------------------------------------
            // �W�����v���͂����邩�㏸���̎�
            if (isJump == true)
            {
                //----------------------------------------------------------------------------------------------------------
                // �o�ߎ��Ԃ����Z
                JumpTime += Time.deltaTime;
            }
            else
            {
                //----------------------------------------------------------------------------------------------------------
                // �o�ߎ��Ԃ�������
                JumpTime = 0.0f;
            }

            //----------------------------------------------------------------------------------------------------------
            // �v���C���[�̍��W�ɉ��Z?
            if (!Selected)
            {
                thisRigidbody2d.velocity = new Vector2(xSpeed, ySpeed);
            }

            //---------------------------------------------------------------
            // �A�j���[�V�����֌W
            // �W�����v���Ȃ�W�����v�A�j���[�V�����J�ڗp�ϐ���true
            if (!crackmove.HitFlg)
            {
                anim.SetBool("jump", isJump);
            }

            // �������Ȃ痎���A�j���[�V�����J�ڕϐ����Z�b�g
            anim.SetBool("drop", FallTime > 0.0f);


        }
        else
        {
            // ���̏�ŃX�g�b�v������
            thisRigidbody2d.velocity = new Vector2(0, 0);
        }

        // �O�t���[����y���W�Ƃ��ĕێ�
        oldPosY = thisTransform.position.y;
    }

    //�@�Z���N�g��ʂőI�����Ă��邩���f����֐�
    public void SetSelected(bool select)
    {
        Selected = select;
    }
    private void RunDust ()
    {
        _runDust.IsJump = !isGround;
    }
}
