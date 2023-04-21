//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�GAI(�v�����g��G��)
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEnemyMove : MonoBehaviour
{
    // �ϐ��錾

    // �G�������Ă������
    enum EnemyDirection
    {
        LEFT,
        RIGHT
    }

    private EnemyDirection Direction = EnemyDirection.RIGHT;

    // �G���ǂ���̃p�C�v�ɂ��邩
    enum WhichPipe
    {
        Pipe1,
        Pipe2, 
    }

    // ������Ԃ̓p�C�v1�̒�
    private WhichPipe whichPipe = WhichPipe.Pipe1;

    private float sizeX; // ���[�J���T�C�Y�ۑ�

    // �G�̃v���C���[�T�[�`�ϐ�
    private float SubX;    // ���߂�X���W�̍���ێ�����ϐ�
    private float SubY;    // ���߂�Y���W�̍���ێ�����ϐ�
    private float Distance; // ���߂�������ێ�����ϐ�

    public enum AIState
    {
        Idle,         // �A�C�h��
        Pre_Attack,   // �U������
        Attack,       // �U��
        Attack_Wait,  // �U����̑ҋ@
        Firing,       // ��яo��
        Confusion,    // ����
        Death,        // ���S
    }

    // �G�s�����
    public AIState EnemyAI = AIState.Idle;

    // �v���C���[�����G�͈͂ɂ��邩
    public bool PlayerHit = false;

    // �U���p�ϐ�
    
    private float Timer = 0f; // �^�C�}�[
    private GameObject TargetObject; // ��яo������̃I�u�W�F�N�g
    private Vector3 TargetPosition; // �ڕW�n�_
    private Vector3 vector; // ���K�������x�N�g��

    [Header("�\�����삩��U���܂ł̑ҋ@����")] public float AttackWaitTime = 1.0f; // ����o���Ă���U���ɓ���܂ł̑҂�����
    [Header("�U�����Ă��玟�̗\������Ɉڂ�܂ł̎���")] public float AttackIntervalTime = 1.0f; // �U�����Ă��玟�̍U���Ɉڂ�܂ł̎���
    [Header("�\�����쎞�̃X�s�[�h")]public float PreSpeed = 3f; // �\�����쎞�̈ړ����x
    [Header("�U�����[�V�������X�s�[�h")]public float AttackSpeed = 10f; // �U������Ƃ��̈ړ����x

    // �O���擾
    private Transform thisTransform; // ���̃I�u�W�F�N�g�̍��W�����ϐ�
    private GameObject player; // �v���C���[�̃Q�[���I�u�W�F�N�g�T���p
    private Transform playerTransform; // �v���C���[�̍��W

    [Header("���̓G����������PipeEnemyGroup�����Ă�")]public GameObject Parent; // �e�I�u�W�F�N�g
    private GameObject Pipe_1; // �p�C�v1
    private GameObject Pipe_2; // �p�C�v2

    // Start is called before the first frame update
    void Start()
    {
        // �s��������p�C�v�̃Q�[���I�u�W�F�N�g�擾
        Pipe_1 = Parent.transform.GetChild(0).gameObject;
        Pipe_2 = Parent.transform.GetChild(1).gameObject;

        // �v���C���[�擾
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        transform.localPosition = Pipe_1.transform.localPosition;

        // ���g�̃g�����X�t�H�[���ێ�
        thisTransform = transform;

        sizeX = thisTransform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // ���͈͓��Ƀv���C���[���N�����Ă�����X�e�[�^�X�ω�
        // �v���C���[�Ƃ̋��������Ƃ߂�
        SubX = thisTransform.localPosition.x - playerTransform.position.x; // x��
        SubY = thisTransform.localPosition.y - playerTransform.position.y; // y��

        // �O�����̒藝
        Distance = SubX * SubX + SubY * SubY; // �v���C���[�Ƃ̋��������܂���

        // �G�̌����Ă���������Z�b�g
        SetDirection();

        switch (EnemyAI)
        {
            // �A�C�h��
            case AIState.Idle:
                Idle();
                break;

            // �U������
            case AIState.Pre_Attack:
                Pre_Attack();
                break;

            // �U��
            case AIState.Attack:
                Attack();
                break;

            // �U����̌�
            case AIState.Attack_Wait:
                Attack_Wait();
                break;

            // ��яo���Ă���
            case AIState.Firing:
                Firing();
                break;

            // ����
            case AIState.Confusion:
                Confusion();
                break;

            // ���j
            case AIState.Death:
                Death();
                break;
        }

        //Debug.Log(EnemyAI);
        //Debug.Log("which");
        //Debug.Log(whichPipe);
        //Debug.Log("Target");
        //Debug.Log(TargetObject);
    }

    private void Idle()
    {
        if(Timer != 0f)
        {
            Timer = 0f;
        }

        // ����������p�C�v�̒��S�Ɍ�����
        if(whichPipe == WhichPipe.Pipe1)
        {
            // �ړ�
            thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, Pipe_1.transform.localPosition, Time.deltaTime);
        }
        else
        {
            // �ړ�
            thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, Pipe_2.transform.localPosition, Time.deltaTime);
        }

        // �v���C���[�����G�͈͂ɓ�������
        if(PlayerHit == true)
        {
            EnemyAI = AIState.Pre_Attack;
        }
    }

    private void Pre_Attack()
    {
        if (Timer == 0f)
        {
            // ���Α��̃p�C�v��ړI�ɂ���
            if (whichPipe == WhichPipe.Pipe1)
            {
                // �p�C�v2��ړI�n�ɂ���
                TargetObject = Pipe_2;
            }
            else
            {
                // �p�C�v1��ړI�n�ɂ���
                TargetObject = Pipe_1;
            }

            // ���ݒn����ړI�n�܂ł̃x�N�g�����擾
            var Vector_TargetEnemy = TargetObject.transform.position - thisTransform.position;

            // �x�N�g���𐳋K��
            vector = Vector_TargetEnemy.normalized;

            TargetPosition = new Vector3(
                thisTransform.localPosition.x + vector.x * 1.0f,
                thisTransform.localPosition.y + vector.y * 1.0f,
                0f);
        }

        // �����Ђ傱���Əo�����炢�܂ō��W�ύX
        thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, TargetPosition, Time.deltaTime);

        // �J�E���g
        Timer += Time.deltaTime;

        // �w�莞�Ԍo�߂�����
        if (Timer >= AttackWaitTime)
        {
            EnemyAI = AIState.Attack;

            // ������
            Timer = 0f;
        }
    }

    private void Attack()
    {
        // �ŏ��̂ݓ���
        if (Timer == 0f)
        {
            // ��яo���Ă����p�C�v�̍��W���擾
            TargetPosition = new Vector3(
                    TargetObject.transform.localPosition.x,
                    TargetObject.transform.localPosition.y,
                    0f);
        }

        // �����Ђ傱���Əo�����炢�܂ō��W�ύX
        thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, TargetPosition, Time.deltaTime * AttackSpeed);

        // �J�E���g
        Timer += Time.deltaTime;

        // �ڕW�n�_�ɓ��B������
        if (thisTransform.localPosition == TargetPosition)
        {
            // �C���^�[�o������
            EnemyAI = AIState.Attack_Wait;

            // ������
            Timer = 0f;
        }
    }

    private void Attack_Wait()
    {
        // �G�̎��f�[�^�X�V
        if(Timer == 0f)
        {
            // �����̂���p�C�v��؂�ւ�
            if (whichPipe == WhichPipe.Pipe1)
            {
                whichPipe = WhichPipe.Pipe2;
            }
            else
            {
                whichPipe = WhichPipe.Pipe1;
            }

            // �����؂�ւ�
            if (Direction == EnemyDirection.LEFT)
            {
                Direction = EnemyDirection.RIGHT;
            }
            else
            {
                Direction = EnemyDirection.LEFT;
            }
        }

        // �U���C���^�[�o�����Ƀv���C���[�����G�͈͂���O�ꂽ��
        if(Timer != 0f && PlayerHit == false)
        {
            // �A�C�h����Ԃɖ߂�
            EnemyAI = AIState.Idle;
        }

        // �J�E���g
        Timer += Time.deltaTime;

        // �C���^�[�o���^�C�����o�߂�����
        if(Timer >= AttackIntervalTime)
        {
            EnemyAI = AIState.Pre_Attack;

            // ������
            Timer = 0f;
        }
    }

    private void Firing()
    {

    }

    private void Confusion()
    {

    }

    private void Death()
    {

    }

    private void SetDirection()
    {
        if (Direction == EnemyDirection.LEFT)
        {
            // ������
            thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
        }
        else
        {
            // �E����
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
        }
    }
}

