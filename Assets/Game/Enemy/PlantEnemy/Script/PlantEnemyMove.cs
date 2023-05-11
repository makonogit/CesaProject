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
    [SerializeField]private WhichPipe whichPipe = WhichPipe.Pipe1;

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
        Rage,         // �����΂�
        Death,        // ���S
    }

    // �G�s�����
    public AIState EnemyAI = AIState.Idle;

    // �v���C���[�����G�͈͂ɂ��邩
    public bool PlayerHit = false;

    // �U���p�ϐ�
    
    public float Timer = 0f; // �^�C�}�[
    private GameObject TargetObject; // ��яo������̃I�u�W�F�N�g
    private Vector3 TargetPosition; // �ڕW�n�_
    private Vector3 vector; // ���K�������x�N�g��

    [Header("�\�����삩��U���܂ł̑ҋ@����(s)")] public float AttackWaitTime = 1.0f; // ����o���Ă���U���ɓ���܂ł̑҂�����
    [Header("�U�����Ă��玟�̗\������Ɉڂ�܂ł̎���(s)")] public float AttackIntervalTime = 1.0f; // �U�����Ă��玟�̍U���Ɉڂ�܂ł̎���
    [Header("�\�����쎞�̃X�s�[�h")]public float PreSpeed = 3f; // �\�����쎞�̈ړ����x
    [Header("�U�����[�V�������X�s�[�h")]public float AttackSpeed = 10f; // �U������Ƃ��̈ړ����x

    // ��Ռ����p�ϐ�
    public bool CrackInPipe = false; // �p�C�v�ɂЂт�����������
    private Rigidbody2D rigid2D; // �G���g��rigidbody2D
    [Header("�ˏo���ɉ������")] public float ImpulsePower = 5f; // �ˏo���ɉ������
    [System.NonSerialized]public GameObject CrackInObject = null; // �Ђт����������p�C�v��ۑ�����
    private Vector3 CenterBetweenPipes; // �p�C�v�Ԃ̒��S���W
    [Header("�ˏo���Ă��獬����ԂɂȂ�܂ł̎���(s)")]public float FiringTime = 0.5f;
    private float Adjustment; // �ˏo���̖ڕW�ʒu���班����������
    [SerializeField] private float RageArea = 1f;
    private Vector3 RageStartPos; // �����΂��J�n���̏������W
    private float RageCount = 0f;
    [SerializeField] private float JumpInterval = 1.5f;
    private float RageImpulsePower = 2f;

    //�}�e���A��
    private Material mat;

    // �O���擾
    private Transform thisTransform; // ���̃I�u�W�F�N�g�̍��W�����ϐ�
    private GameObject player; // �v���C���[�̃Q�[���I�u�W�F�N�g�T���p
    private Transform playerTransform; // �v���C���[�̍��W

    [Header("���̓G����������PipeEnemyGroup�����Ă�")]public GameObject Parent; // �e�I�u�W�F�N�g
    private GameObject Pipe_1; // �p�C�v1
    private GameObject Pipe_2; // �p�C�v2
    private GameObject Child; // �G���g�̎q�I�u�W�F�N�g

    private void Init()
    {
        // ������

        Timer = 0f;
        RageCount = 0;
        EnemyAI = AIState.Idle;
        whichPipe = WhichPipe.Pipe1;
    }

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

        // rigidbody�擾
        rigid2D = GetComponent<Rigidbody2D>();

        // �p�C�v�Ԃ̒��S���W���擾
        if (Pipe_1.transform.localPosition.x > Pipe_2.transform.localPosition.x) 
        {
            CenterBetweenPipes = (Pipe_1.transform.localPosition - Pipe_2.transform.localPosition) / 2f;
        }
        else
        {
            CenterBetweenPipes = (Pipe_2.transform.localPosition - Pipe_1.transform.localPosition) / 2f;
        }

        // �q�I�u�W�F�N�g�擾
        Child = transform.Find("HitCollider").gameObject;

        // �}�e���A���擾
        mat = GetComponent<SpriteRenderer>().material;
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

            // �����΂�
            case AIState.Rage:
                Rage();
                break;

            // ���j
            case AIState.Death:
                Death();
                break;
        }


        //Debug.Log(Pipe_1.transform.localPosition);
        //Debug.Log(Pipe_2.transform.localPosition);
        //Debug.Log(CenterBetweenPipes);
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

        // �J�ڔ��f�֐�
        JudgeFiring();
    }

    private void Pre_Attack()
    {
        if (Timer == 0f)
        {
            SetTargetPipe();
        }

        // �����Ђ傱���Əo�����炢�܂ō��W�ύX
        thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, TargetPosition, Time.deltaTime * PreSpeed);

        // �J�E���g
        Timer += Time.deltaTime;

        // �w�莞�Ԍo�߂�����
        if (Timer >= AttackWaitTime)
        {
            EnemyAI = AIState.Attack;

            // ������
            Timer = 0f;
        }

        // �J�ڔ��f�֐�
        JudgeFiring();
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

        // �ڕW�̃p�C�v�̈ʒu�܂ňړ�
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

        // �J�ڔ��f�֐�
        JudgeFiring();
    }

    private void Firing()
    {
        if(Timer == 0f)
        {
            // �ڕW�n�_�����炷���߂̒l���
            if(whichPipe == WhichPipe.Pipe1)
            {
                Adjustment = CenterBetweenPipes.x * 0.2f;
            }
            else
            {
                Adjustment = -CenterBetweenPipes.x * 0.2f;
            }
        }

        // �w�莞�Ԃ��o�߂��Ă���d�͂̉e����^����
        if(Timer >= 0.1f)
        {
            rigid2D.gravityScale = 1f;
        }

        // �ŏI�ڕW�ʒu
        var target = new Vector3(
            CenterBetweenPipes.x + Adjustment,
            CenterBetweenPipes.y,
            CenterBetweenPipes.z
            );

        // y���W�͎��R����
        var PositionY = thisTransform.localPosition.y;

        // �p�C�v�̒��S�܂ňړ�
        thisTransform.localPosition = Vector3.MoveTowards(thisTransform.localPosition, target, Time.deltaTime * ImpulsePower);

        // y���W�𐳂������̂ɒu��������
        thisTransform.localPosition = new Vector3(
            thisTransform.localPosition.x,
            thisTransform.localPosition.y,
            thisTransform.localPosition.z
            );

        Timer += Time.deltaTime;

        // ��莞�Ԃ�������
        if (Timer >= FiringTime)
        {
            // ������Ԃ�
            EnemyAI = AIState.Confusion;
            Timer = 0f;
        }
    }

    private void Confusion()
    {
        // ��莞�Ԃ�������
        Timer += Time.deltaTime;
        if(Timer > 2f)
        {
            // �����΂�������
            EnemyAI = AIState.Rage;

            // �����΂��J�n���̍��W
            RageStartPos = thisTransform.position;

            RageCount = 0f;
            Timer = 0f;
        }
    }

    private void Rage()
    {
        // �\��n�߂��n�_�𒆐S��
        thisTransform.position = new Vector3(RageStartPos.x + Mathf.Sin(Timer) / 2f, thisTransform.position.y,RageStartPos.z);

        if(RageCount == 0f)
        {
            // ������̂ɒ��˂�
            rigid2D.AddForce(new Vector3(0f, 1f * RageImpulsePower, 0f), ForceMode2D.Impulse);
        }

        // �C���p���X�p
        RageCount += Time.deltaTime;
        if(RageCount > JumpInterval)
        {
            RageImpulsePower = Random.Range(2, 4); // �p���[�̓����_��
            RageCount = 0f;
        }

        Timer += Time.deltaTime;
    }

    private void Death()
    {
        // �v���C���[�Ƃ̓����蔻�������
        Child.GetComponent<CircleCollider2D>().enabled = false;

        // �d�͂����ĂȂ��Ȃ����
        if(rigid2D.gravityScale == 0f)
        {
            rigid2D.gravityScale = 1f;
        }
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

    // ���Ɍ������^�[�Q�b�g��ݒ�
    private void SetTargetPipe()
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
        var Vector_TargetEnemy = TargetObject.transform.localPosition - thisTransform.localPosition;

        // �x�N�g���𐳋K��
        vector = Vector_TargetEnemy.normalized;

        TargetPosition = new Vector3(
            thisTransform.localPosition.x + vector.x * 1.0f,
            thisTransform.localPosition.y + vector.y * 1.0f,
            0f);
    }

    // Firing��ԂɑJ�ڂ��邩���f����֐�
    private void JudgeFiring()
    {
        // �Ђт��G���g�̂���p�C�v�ɂ���������
        if (CrackInPipe == true)
        {
            // ���g������p�C�v���p�C�v1�̂Ƃ��Ђт����������p�C�v���p�C�v1��������
            if (whichPipe == WhichPipe.Pipe1 && CrackInObject == Pipe_1)
            {
                EnemyAI = AIState.Firing;
                Timer = 0f;
                //Debug.Log("�W���b�W�����g�ł���");

            }
            // ���g������p�C�v���p�C�v2�̂Ƃ��Ђт����������p�C�v���p�C�v2��������
            else if (whichPipe == WhichPipe.Pipe2 && CrackInObject == Pipe_2)
            {
                EnemyAI = AIState.Firing;
                Timer = 0f;
                //Debug.Log("�W���b�W�����g�ł���");
            }

            CrackInPipe = false;
        }
    }
}

