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
    Vector2 movement; // ���͗ʂ��擾����ϐ�

    [Header("�����Ƃ��͑���Ƃ��̂ǂꂭ�炢�̃X�s�[�h��")]
    public float magnification = 1.5f; // �����Ƃ��̃X�s�[�h�̔{��

    PlayerInputManager.DIRECTION oldDire; // �O�t���[���̌��������Ă������߂̕ϐ�

    public float ideal_IdleTime = 2.0f; //  �����~�܂��Ă���A�C�h����ԂɂȂ�܂ł̎���
    private float IdleTime = 0.0f; // �����~�܂��Ă���̌o�ߎ���

    enum MOVESTATUS
    {
        NONE,
        WALK,
        RUN,
        FRIEZE, // �A�C�h�������i�K 
    }

    private MOVESTATUS MoveSta = MOVESTATUS.NONE;

    // �O���擾
    private GameObject PlayerInputMana; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform thisTransform; // ���g��Transform���擾����ϐ�

    public LayerMask BlockLayer;

    private Animator anim; // �A�j���[�^�[���擾���邽�߂̕ϐ�

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
    }

    //----------------------------------------------------------------------------------------------------------
    // - �X�V���� -
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // ���ʂ̈ړ�
        //----------------------------------------------------------------------------------------------------------
        // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
        movement = ScriptPIManager.GetMovement();

        // ���̓������Ȃ����
        if(movement.x == 0.0f)
        {
            MoveSta = MOVESTATUS.FRIEZE;
            IdleTime += Time.deltaTime;
        }

        if(movement.x != 0.0f || !(anim.GetBool("frieze")))
        {
            IdleTime = 0.0f;        
        }

        Debug.Log(IdleTime);

        if ((movement.x > 0.0f && movement.x < 0.5f) || (movement.x < 0.0f && movement.x > -0.5f))
        {
            MoveSta = MOVESTATUS.WALK;
        }
        else if((movement.x >= 0.5f && movement.x <= 1.0f) || (movement.x <= -0.5f && movement.x >= -1.0f))
        {
            MoveSta = MOVESTATUS.RUN;
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

        //----------------------------------------------------------------------------------------------------------
        // �v���C���[��Transform�Ɉړ��ʂ�K������
        // �X�e�B�b�N�ŏ���͂���Ə���������肪���邽�߁AY,Z�ɂ͒��ڒl�����ďC��
        thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);

        //-----------------------------------------------------------------
        // �A�j���[�V�����֌W
        // movement��x�̒l�ɂ���Ă����
        anim.SetBool("walk", MoveSta == MOVESTATUS.WALK); // �X�e�B�b�N���͂̍��E�����܂łȂ����
        anim.SetBool("run", MoveSta == MOVESTATUS.RUN); // �X�e�B�b�N���͂̍��E�����ȏ�Ȃ瑖��
        anim.SetBool("frieze", MoveSta == MOVESTATUS.FRIEZE); // �X�e�B�b�N���͂�������Ώ������

        if (oldDire != ScriptPIManager.Direction)
        {
            // �v���C���[�̌��������Ƌt�ɂ���
            thisTransform.localScale = new Vector3(-thisTransform.localScale.x, thisTransform.localScale.y, thisTransform.localScale.z);
        }

        // �O�t���[���̌����Ƃ��ĕۑ�
        oldDire = ScriptPIManager.Direction;
    }
}
