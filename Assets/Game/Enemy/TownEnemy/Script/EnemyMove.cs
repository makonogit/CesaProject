//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�GAI(���G��)
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    public bool Stop = false; // �f�o�b�O�p �G�����̏�ɂƂǂ܂�

    enum EnemyDirection
    {
        LEFT,
        RIGHT
    }

    private EnemyDirection Direction = EnemyDirection.RIGHT;

    // ���C�̏Փ˔��茋�ʗp�ϐ�
    RaycastHit2D hit;
    public bool test;
    // ���C������l�F���@�E�ɂ��炷,�@���@���ɂ��炷
    private float Scale;
    // ���C�̈ʒu����
    public float AdjustX = 0f;
    public float AdjustY = 0f;

    // �G�̏���J�n�ʒu
    private Vector3 start;
    // �G�̈ړ���
    private Vector3 target;
    //// �G�̈ړ�����
    //private bool Outbound = true; // true:���H false:���H
    [Header("�G�̈ړ��͈�")]
    public float MoveArea = 5.0f; // �G�̈ړ��͈�
    // �ړ������̊�����\�� 0����1
    private float timer;
    private float sizeX; // ���[�J���T�C�Y�ۑ�

    // �G�̃v���C���[�T�[�`�ϐ�
    private float SubX;    // ���߂�X���W�̍���ێ�����ϐ�
    private float SubY;    // ���߂�Y���W�̍���ێ�����ϐ�
    public float Distance; // ���߂�������ێ�����ϐ�
    [Header("�v���C���[�����m����͈�")]
    public float senserDistance = 6.0f; // ������Ƃ�͈�

    // �G���U�����n�߂鋗��
    [Header("�U�����[�V�����ɓ��鋗��")]
    public float attackDistance = 2.0f; // �U�����鋗��

    // �U���p�ϐ�
    private float AttackStartPositionX; // �U����ԂɂȂ������̍��W
    [Header("���̍U���̃��[�v�ɕK�v�Ȏ���")]
    [SerializeField]private float OneAttackNeedTime = 0.5f; // ���̍U���̃��[�v�ɕK�v�Ȏ���
    private float AttackTimer = 0f; // �U�����n�܂��Ă���̌o�ߎ���(�U��1���[�v���Ƃɏ�����)
    [Header("�U�����͂�����")]
    [SerializeField]private float AttackDistance = 0.5f; // �U�����͂�����
    private float AttackSign = 1f; // ��������
    private bool Return = false;
    private float NowEnemyPos; // �U�����̍��W�ω��̌��ʗp�ϐ�
    [Header("�U����̍d������")]
    [SerializeField] private float AttackWaitTime = 0.3f; // �U����̑ҋ@����
    private float AttackWaitTimer = 0f;

    // �R���[�`���֌W
    private bool InCoroutine = false; // �R���[�`���ɓ�������
    private bool AttackStart = false; // �U���Ƀf�B���C���|����
    [Header("�U�������b�����点�邩")]public float InCoroutineWaitTime = 0.5f; // ���b�x�点�邩

    // ���S���̏������I������������ϐ�
    private bool death = false;


    [Header("�v���C���[��ǂ������鑬�x")]
    public float TrackingSpeed = 3.0f; // �ǐՃX�s�[�h

    public enum AIState
    {
        INIT_PATROL,   // ���񏀔�
        PATROL,        // ����
        INIT_TRACKING, // �ǐՏ���
        TRACKING,      // �ǐ�
        INIT_ATTACK,   // �U������
        ATTACK,        // �U��
        ATTACK_WAIT,   // �U���ҋ@
        DEATH,         // ���S
    }

    // �G�s�����
    public AIState EnemyAI = AIState.INIT_PATROL;

    private CircleCollider2D circleCol;
    private float ColRadius;

    // �O���擾
    private Transform thisTransform; // ���̃I�u�W�F�N�g�̍��W�����ϐ�
    private GameObject player; // �v���C���[�̃Q�[���I�u�W�F�N�g�T���p
    private Transform playerTransform;
    private HammerNail hammer; // HammerNail���擾
    private GameObject Child; // �G���g�̎q�I�u�W�F�N�g


    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �I�u�W�F�N�g��Transform���擾
        thisTransform = GetComponent<Transform>();

        // �v���C���[�I�u�W�F�N�g�T��
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        sizeX = thisTransform.localScale.x;

        // Hammer�X�N���v�g�擾
        hammer = player.GetComponent<HammerNail>();


        // �q�I�u�W�F�N�g�擾
        Child = transform.Find("HitCollider").gameObject;

        // �R���C�_�[�擾
        circleCol = GetComponent<CircleCollider2D>();
        ColRadius = circleCol.radius;
        AdjustX = ColRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (Stop == false)
        {
            // ���͈͓��Ƀv���C���[���N�����Ă�����X�e�[�^�X�ω�
            // �v���C���[�Ƃ̋��������Ƃ߂�
            var Vec = thisTransform.position - playerTransform.position;

            // �O�����̒藝
            Distance = Vec.magnitude; // �v���C���[�Ƃ̋��������܂���

            if (gameObject.name == "Enemy")
            {
                //Debug.Log(Distance);
                //Debug.Log(senserDistance);
                //Debug.Log(playerTransform.position);
                //Debug.Log(thisTransform.position);
            }

            // �����Ă���������Z�b�g
            SetDirection();

            // �i�s�����Ƀ��C���΂�
            CreateRay();

            if (death == false)
            {
                switch (EnemyAI)
                {
                    case AIState.INIT_PATROL:
                        Init_Patrol();
                        break;

                    case AIState.PATROL:
                        Patrol();
                        break;

                    case AIState.TRACKING:
                        Tracking();
                        break;

                    case AIState.INIT_TRACKING:
                        Init_Tracking();
                        break;

                    case AIState.INIT_ATTACK:
                        Init_Attack();
                        break;

                    case AIState.ATTACK:
                        Attack();
                        break;

                    case AIState.ATTACK_WAIT:
                        Attack_Wait();
                        break;

                    case AIState.DEATH:
                        Death();
                        break;
                }
            }
        }
        else
        {
            // �����Ă���������Z�b�g
            SetDirection();

            // �i�s�����Ƀ��C���΂�
            CreateRay();
        }
    }

    void Init_Patrol()
    {
        // ���C�̏Փ˂������������̍Đݒ�
        if (hit == false)
        {
            // �G�̊J�n�ʒu����
            start = thisTransform.position;

            // �G�̖ړI�n
            target = new Vector3(thisTransform.position.x + MoveArea, thisTransform.position.y, 0.0f);

            // ������
            timer = 0;

            Debug.Log("AAAAAAAAAAAAAAA");
        }
        // ���C�̏Փ˂ɂ��Đݒ�
        else
        {
            // �E�ǂƂ̏Փˌ�
            if (Direction == EnemyDirection.RIGHT)
            {
                // �G�̊J�n�ʒu����
                start = new Vector3(thisTransform.position.x - MoveArea, thisTransform.position.y, 0.0f);
                // �G�̖ړI�n
                target = thisTransform.position;
                // ������
                timer = 1.0f;

                // �������ɂ���
                Direction = EnemyDirection.LEFT;
            }
            // ���ǂƂ̏Փˌ�
            else
            {
                // �G�̊J�n�ʒu����
                start = thisTransform.position;
                // �G�̖ړI�n
                target = new Vector3(thisTransform.position.x + MoveArea, thisTransform.position.y, 0.0f);
                // ������
                timer = 0;

                // �E�����ɂ���
                Direction = EnemyDirection.RIGHT;
            }
        }

        EnemyAI = AIState.PATROL;
    }
    void Patrol()
    {
        // 2�b�ňړ������ύX

        // �E�Ɉړ����Ă���Ȃ�
        if (Direction == EnemyDirection.RIGHT)
        {
            timer += Time.deltaTime / 2;

            if (timer >= 1.0f)
            {
                Direction = EnemyDirection.LEFT;
            }
        }
        // ���Ɉړ����Ă���Ȃ�
        else
        {
            timer -= Time.deltaTime / 2;

            if (timer <= 0.0f)
            {
                Direction = EnemyDirection.RIGHT;
            }
        }

        // ���C���X�e�[�W�ɏՓ˂��ĂȂ���Έړ�����
        if (!hit)
        {
            float positonY = thisTransform.position.y;

            // start��target�̈ʒu�Ԃ��ړ�
            thisTransform.position = Vector3.Lerp(start, target, timer);

            thisTransform.position = new Vector3(thisTransform.position.x, positonY, thisTransform.position.z);
        }
        else
        {
            // ����Đݒ�
            EnemyAI = AIState.INIT_PATROL;

            Debug.Log("BBBBBBBBBBBBBB");
        }

        // ��苗�����Ƀv���C���[������
        if (Distance < senserDistance)
        {
            // �ǐՏ���
            EnemyAI = AIState.INIT_TRACKING;
        }
    }

    void Init_Tracking()
    {
        // �ǐ�
        EnemyAI = AIState.TRACKING;
    }

    void Tracking()
    {
        // ��苗�����Ƀv���C���[������
        if (Distance < senserDistance)
        {

            Debug.Log("CCCCCCCCCCCCCC");
            // �p�g���[���̎�����������ς��Ȃ�i�G�̌�납����G�͈͂ɓ�������j
            if (thisTransform.position.x < playerTransform.position.x)
            {
                Direction = EnemyDirection.RIGHT;
            }
            else if (thisTransform.position.x > playerTransform.position.x)
            {
                Direction = EnemyDirection.LEFT;
            }

            // �ǂɃ��C���ڐG������ǂ�Ȃ�
            if (!hit)
            {
                // �v���C���[�Ɍ������Đi��
                // �v���C���[���G���g���E�ɂ���Ȃ�
                if (thisTransform.position.x < playerTransform.position.x)
                {
                    thisTransform.Translate(TrackingSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
                // �v���C���[���G���g��荶�ɂ���Ȃ�
                else if (thisTransform.position.x > playerTransform.position.x)
                {
                    thisTransform.Translate(-1 * TrackingSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
            }

            // �߂Â���������
            if (Distance < attackDistance)
            {
                // �U����Ԃɕω�
                EnemyAI = AIState.INIT_ATTACK;
            }
        }
        // �ǐՔ͈͊O�Ƀv���C���[���ł��珄��ɖ߂�
        else
        {
            EnemyAI = AIState.INIT_PATROL;
        }
    }
    void Init_Attack()
    {
        // �̓����菀��

        // �v���C���[���G���g���E�ɂ���Ȃ�
        if (thisTransform.position.x < playerTransform.position.x)
        {
            AttackSign = 1f;
        }
        // �v���C���[���G���g��荶�ɂ���Ȃ�
        else if (thisTransform.position.x > playerTransform.position.x)
        {
            AttackSign = -1f;
        }

        // �U���ɂȂ�������X���W���擾
        AttackStartPositionX = NowEnemyPos = thisTransform.localPosition.x;

        // ������
        AttackTimer = 0f;

        EnemyAI = AIState.ATTACK;

    }
    void Attack()
    {
        // �^�b�N��

        // �ŏ��̃t���[���Ńf�B���C������
        if(InCoroutine == false)
        {
            StartCoroutine(WaitTimer());
        }

        if (AttackStart == true)
        {
            if (Return == false)
            {
                // ���W�v�Z
                NowEnemyPos += AttackSign * AttackDistance * Time.deltaTime * 2f;

                // AttackTimer�̒l�ɂ���č��W�ω�
                thisTransform.localPosition = new Vector3(
                    NowEnemyPos,
                    thisTransform.localPosition.y,
                    thisTransform.localPosition.z);
            }
            else
            {
                // ���W�v�Z
                // 0�ɋ߂Â��Ă���
                NowEnemyPos -= AttackSign * AttackDistance * Time.deltaTime * 2f;

                // ���̈ʒu��ʂ�߂��Ȃ��悤�ɂ���
                if (AttackSign * NowEnemyPos < 0)
                {
                    NowEnemyPos = AttackStartPositionX;
                }

                thisTransform.localPosition = new Vector3(
                    NowEnemyPos,
                    thisTransform.localPosition.y,
                    thisTransform.localPosition.z);
            }

            AttackTimer += Time.deltaTime;

            //  ���Ԍo�߂ł��Ƃ̒n�_�ɖ߂�悤�̕ϐ�
            if (AttackTimer > OneAttackNeedTime / 2)
            {
                Return = true;
            }

            // ���̍U�����[�v�ɂ����鎞�Ԃ��o�߂�����
            if (AttackTimer > OneAttackNeedTime)
            {
                // �J��Ԃ�
                EnemyAI = AIState.ATTACK_WAIT;

                // ������
                Return = false;
                InCoroutine = false;
                AttackStart = false;
            }
        }

        if (Distance > attackDistance)
        {
            //EnemyAI = AIState.TRACKING;
        }
    }

    private void Attack_Wait()
    {
        // �ҋ@
        AttackWaitTimer += Time.deltaTime;

        // ��莞�Ԍo�߂�����
        if(AttackWaitTimer > AttackWaitTime)
        {
            // �U���Ɉڍs
            EnemyAI = AIState.INIT_ATTACK;

            AttackWaitTimer = 0;
        }

        if (Distance > attackDistance)
        {
            EnemyAI = AIState.TRACKING;

            AttackWaitTimer = 0;
        }
    }

    private void Death()
    {
        // ���S���

        if (death == false) 
        {
            // �v���C���[�Ƃ̓����蔻���
            Child.GetComponent<CircleCollider2D>().enabled = false;

            death = true;
        }
    }

    IEnumerator WaitTimer()
    {
        InCoroutine = true;
        // �w�莞�ԑҋ@
        yield return new WaitForSeconds(InCoroutineWaitTime);

        AttackStart = true;
    }

    private void CreateRay()
    {
        // �i�s�����Ƀ��C���΂��ĕǂɂԂ�������i�s������ς���
        Vector2 origin = new Vector2(
            thisTransform.position.x + AdjustX,
            thisTransform.position.y + AdjustY
            );

        // ���C���΂�����
        Vector2 RayDirection = Vector2.zero;

        // �{�X�̌����ɂ���ă��C���Ƃ΂��������ω�
        switch (Direction)
        {
            case EnemyDirection.LEFT:
                RayDirection = new Vector2(-1, 0); // ������
                break;

            case EnemyDirection.RIGHT:
                RayDirection = new Vector2(1, 0); // �E����
                break;
        }

        // ����
        float length = 0.1f;
        // ����
        Vector2 distance = RayDirection * length;
        // ����̃��C���[�̃��m�Ƃ����Փ˔�����Ƃ�
        // ���C���[�}�X�N�͓�i���𗘗p
        // ��:layerMask = 1 << 2 �͓�i���\����100�B�ォ��O�ڂ̃��C���[�Ƃ����Ƃ����Ӗ�
        LayerMask layerMask = 1 << 10; // ���V�t�g���Z�A1��<<�̉E�̐��������ɃV�t�g

        // ���C��΂��ăX�e�[�W�ƂԂ������琶����߂�
        hit = Physics2D.Raycast(origin, RayDirection, length, layerMask); // ��O���� ���C���� �A��l���� ���C���[ -1�͑S�Ẵ��C���[
        test = hit;

        // �`��
        Debug.DrawRay(origin, distance, Color.red);
        //-----------------------------------------------------------------------------------------------
    }

    private void SetDirection()
    {
        if (Direction == EnemyDirection.LEFT)
        {
            // ������
            thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            AdjustX = -ColRadius * 0.5f;
        }
        else
        {
            // �E����
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            AdjustX = ColRadius * 0.5f;
        }
    }
}
