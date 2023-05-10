//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�̈ړ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    // �ړ��p
    public float BaseSpeed = 5f; // �ړ����x�p�ϐ�
    private Vector2 movement; // ���͗ʂ��擾����ϐ�
    bool Moveflg = true;    //�ړ��t���O�@�ǉ��S���F��

    [Header("�����Ƃ��͑���Ƃ��̂ǂꂭ�炢�̃X�s�[�h��")]
    public float magnification = 1.5f; // �����Ƃ��̃X�s�[�h�̔{��

    PlayerInputManager.DIRECTION oldDire = PlayerInputManager.DIRECTION.RIGHT; // �O�t���[���̌��������Ă������߂̕ϐ�

    public float ideal_IdleTime = 2.0f; //  �����~�܂��Ă���A�C�h����ԂɂȂ�܂ł̎���
    private float IdleTime = 0.0f; // �����~�܂��Ă���̌o�ߎ���

    public enum MOVESTATUS
    {
        NONE,
        WALK,
        RUN,
        FRIEZE, // �A�C�h�������i�K 
    }

    public MOVESTATUS MoveSta = MOVESTATUS.NONE;

    public bool debug = false;

    // �O���擾
    private GameObject PlayerInputMana; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform thisTransform; // ���g��Transform���擾����ϐ�

    public LayerMask BlockLayer;

    private Animator anim; // �A�j���[�^�[���擾���邽�߂̕ϐ�

    private GameObject se;
    private SEManager_Player seMana;

    private PlayerStatas playerStatus;

    // �v���C���[�v���v��
    private WallCheck _wallCheck;

    //--------------------------------------
    // �ǉ��S����:���쒼�o
    private RunDustParticle _runDust;
    //--------------------------------------

    //----------------------------------------------------------------------------------------------------------
    // - ���������� -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManager��T��
        PlayerInputMana = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        oldDire = ScriptPIManager.Direction;

        //----------------------------------------------------------------------------------------------------------
        // ���g(player)�̎���Transform���擾����
        thisTransform = this.GetComponent<Transform>();

        // �A�j���[�^�[�擾
        anim = GetComponent<Animator>();

        se = GameObject.Find("SE");
        // Se�R���|�[�l���g�擾
        seMana = se.GetComponent<SEManager_Player>();

        playerStatus = GetComponent<PlayerStatas>();
        //--------------------------------------
        // �ǉ��S����:���쒼�o
        _runDust = GetComponent<RunDustParticle>();
        if(_runDust == null) Debug.LogError("RunDustParticle�̃R���|�[�l���g���擾�ł��܂���ł����B");
        //--------------------------------------

        // �ǂƂ̐ڐG����X�N���v�g
        _wallCheck = GetComponent<WallCheck>();
    }

    //----------------------------------------------------------------------------------------------------------
    // - �X�V���� -
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // ���ʂ̈ړ�
        //----------------------------------------------------------------------------------------------------------

        if (Moveflg && !playerStatus.GetHitStop()) // �ړ��t���O�������Ă��邩�A�q�b�g�X�g�b�v������Ȃ�
        {
            // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
            movement = ScriptPIManager.GetMovement();
        }
        else
        {
            movement = Vector2.zero;
        }

        // ���̓������Ȃ����
        if (movement.x == 0.0f)
        {
            MoveSta = MOVESTATUS.FRIEZE;
            IdleTime += Time.deltaTime;

            // �Ō��se�Đ��ϐ��Z�b�g
            seMana.SetMoveFinish();
        }

        if(movement.x != 0.0f || !(anim.GetBool("frieze")))
        {
            IdleTime = 0.0f;        
        }

        if ((movement.x > 0.0f && movement.x < 0.5f) || (movement.x < 0.0f && movement.x > -0.5f))
        {
            MoveSta = MOVESTATUS.WALK;

            // se�Đ��J�n
            seMana.SetMoveStart();
        }
        else if((movement.x >= 0.5f && movement.x <= 1.0f) || (movement.x <= -0.5f && movement.x >= -1.0f))
        {
            MoveSta = MOVESTATUS.RUN;
                
            // se�Đ��J�n
            seMana.SetMoveStart();
        }
        else if(movement.x == 0)
        {
            // �����~�܂��Ă���̌o�ߎ��Ԃ��w��̎��Ԉȏ�Ȃ�A�C�h����ԂɂȂ�
            if (IdleTime >= ideal_IdleTime)
            {
                MoveSta = MOVESTATUS.NONE;
            }
        }

        float Speed = 0.0f;
        switch (MoveSta)
        {
            case MOVESTATUS.WALK:
                Speed = BaseSpeed * magnification;
                
                break;

            case MOVESTATUS.RUN:
                Speed = BaseSpeed;
                break;
        }

        //-----------------------------------------------------------------
        // �A�j���[�V�����֌W
        // movement��x�̒l�ɂ���Ă����
        anim.SetBool("walk", MoveSta == MOVESTATUS.WALK); // �X�e�B�b�N���͂̍��E�����܂łȂ����
        anim.SetBool("run", MoveSta == MOVESTATUS.RUN); // �X�e�B�b�N���͂̍��E�����ȏ�Ȃ瑖��
        anim.SetBool("frieze", MoveSta == MOVESTATUS.FRIEZE); // �X�e�B�b�N���͂�������Ώ������

        //-------------------------------------------------------------------
        // ������艺�͋����I��movement�̒l�������鎞����
        // �X�N���v�g�����ĂȂ��Ă��G���[�ɂȂ�Ȃ�
        if (_wallCheck != null)
        {
            // �ǂƂ̐ڐG����
            bool LeftWall = _wallCheck.LeftCheck();
            bool RightWall = _wallCheck.RightCheck();

            // �ǂƐڐG�������C�̕����ɐi�����Ƃ�����movement�������I��0�ɂ��ăv���v������

            // ���ǂƐڐG
            if (LeftWall == true && RightWall == false)
            {
                // ���ɐi�s���悤�Ƃ��Ă���
                if (movement.x < 0f)
                {
                    movement.x = 0f;
                }
            }
            // �E�ǂƐڐG
            else if (LeftWall == false && RightWall == true)
            {
                // �E�ɐi�s���悤�Ƃ��Ă���
                if (movement.x > 0f)
                {
                    movement.x = 0f;
                }
            }
            // ����true�������͗���false�Ȃ牽�����Ȃ�
        }

        //----------------------------------------------------------------------------------------------------------
        // �v���C���[��Transform�Ɉړ��ʂ�K������
        // �X�e�B�b�N�ŏ���͂���Ə���������肪���邽�߁AY,Z�ɂ͒��ڒl�����ďC��
        thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);
        
        if (oldDire != ScriptPIManager.Direction)
        {
            // �v���C���[�̌��������Ƌt�ɂ���
            thisTransform.localScale = new Vector3(-thisTransform.localScale.x, thisTransform.localScale.y, thisTransform.localScale.z);
        }
        //--------------------------------------
        // �ǉ��S����:���쒼�o
        RunDust();
        //--------------------------------------
        // �O�t���[���̌����Ƃ��ĕۑ�
        oldDire = ScriptPIManager.Direction;
    }

    //-------------------------------------
    //�@�ړ��ݒ�֐�
    //�@�����Ftrue �ړ��@false �ړ����Ȃ�
    //�@�߂�l�F�Ȃ�
    //�@�ǉ��S���F��
    public void SetMovement(bool moveflg)
    {
        Moveflg = moveflg;
    }

    //-------------------------------------
    //�@�ړ��ݒ�擾�֐�
    //�@�����F�Ȃ�
    //�@�߂�l�F�ړ��t���O
    //�@�ǉ��S���F��
    public bool GetMovementFlg()
    {
        return Moveflg;
    }


    //-------------------------------------
    //�@���͗ʎ擾�֐�
    //�@�����F�Ȃ�
    //�@�߂�l�F���͗�
    //�@�ǉ��S���F��
    public Vector2 GetMovement()
    {
        return movement;
    }

    //--------------------------------------
    // �ǉ��S����:���쒼�o
    // �֐��FRunDust() 
    // �ړI�F���������̃G�t�F�N�g�̃I���I�t
    private void RunDust() 
    {
        _runDust.IsMove = (MoveSta != MOVESTATUS.FRIEZE);
    }
    //--------------------------------------
}
