//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F1�ʃ{�XAI
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBossMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    // �G�������Ă������
    enum EnemyDirection
    {
        LEFT,
        RIGHT
    }

    private EnemyDirection Direction = EnemyDirection.LEFT;

    private float sizeX; // ���[�J���T�C�Y�ۑ�

    // �G�̃v���C���[�T�[�`�ϐ�
    private float SubX;    // ���߂�X���W�̍���ێ�����ϐ�
    private float SubY;    // ���߂�Y���W�̍���ێ�����ϐ�
    private float Distance; // ���߂�������ێ�����ϐ�

    // ���C�̏Փ˔��茋�ʗp�ϐ�
    RaycastHit2D hit;
    // ���C������l�F���@�E�ɂ��炷,�@���@���ɂ��炷
    private float Scale; 
    // ���C�̈ʒu����
    float AdjustX;
    float AdjustY;

    // ���C��L�΂��p�̃^�C�}�[�ϐ�
    private float RayLengthTimer = 0.0f;
    [Header("�������̃X�s�[�h")] public float WalkSpeed = 2.0f;
    [Header("��������")] public float WalkTime = 2.0f;
    private float WalkTimer = 0f;
    bool raycast = false; // ���C���΂����ǂ���

    // �v���C���[�ɋC�Â�����
    [SerializeField] private float MoveStartDistance = 120f;
    
    // �ːi�p�ϐ�
    [Header("�ːi���̃X�s�[�h")]public float RammingSpeed = 5.0f;     // �ːi���̈ړ����x
    [Header("�ːi��������")] public float PreRammingTime = 3.0f;      // �ːi��������
    private float PreRammingTimer = 0f;                               // �ːi�����^�C�}�[
    private int SwitchBack;                                           // �؂�Ԃ���
    [Header("�ːi��̌㌄")] public float RammingWaitTime = 3.0f;     // �ːi��̌㌄����
    private float RammingWaitTimer;                                   // �ːi��̌����ԃ^�C�}�[
    private bool HitPlayer = false;
    [SerializeField] private GameObject SandSmoke; // �ːi�������̍����p�[�e�B�N��
    [SerializeField,Header("���W�����p(����)")] private Vector2 sandSmokeOffset_left; // ���W�����p(����)
    private float sandSmokeAdjustX_left;   // �{�X�̌����Œl��ς���K�v�����邽��
    [SerializeField,Header("���W�����p(�E��)")] private Vector2 sandSmokeOffset_right; // ���W�����p(�E��)
    private float sandSmokeAdjustX_right;  // �{�X�̌����Œl��ς���K�v�����邽��

    //// �������΂��p�ϐ�
    //private float CreateShardsNeedTime = 0.6f;   // ����������̂ɂ����鎞��
    //private float ShardCreateTimer;            // ������𐶐����n�߂Ă���̌o�ߎ���
    //private int CreatedNum = 0; // ��������������
    //public GameObject Shards_Prefab; // ������̃v���n�u�������Ă����ϐ�
    //private GameObject[] shardObj = new GameObject[18]; // �쐬����������I�u�W�F�N�g�������z��
    //private GameObject shardParent; // ������̐e�I�u�W�F�N�g
    //private Vector3[] ShardVelocity = new Vector3[18]; // �������������ړ������邽�߂̒l������z��
    //private bool AllAddVelocity = false; // �쐬�����S�Ă̂������Velocity�����Z������
    //[Header("������̈ړ��X�s�[�h")]public float ShardSpeed = 5f;
    //private float ShardWaitTime = 3f; // ��������΂�����̎��̃��[�V�����܂ł̑҂�����
    //private float ShardThrowTimer = 0f; // ��������Ƃ΂��Ă���̌o�ߎ���
    //private int ShardWaveNum = 0; // �Ȃ񂩂��̃E�F�[�u�����邩
    //private int NowShardWave = 0; // �����E�F�[�u�ڂ�
    //[Header("1�E�F�[�u�ɉ���΂���(�ő�6��)"),SerializeField] private int CreateShardNum = 6;
    //// ���А������̃G�t�F�N�g
    //public GameObject ChargeEffect;

    //// ������z�u�p�ϐ�
    //[Header("���x�Ԋu�ł������z�u���邩(�����l10)")]public float SpacingDeg = 10f; // ���x�Ԋu�Ŕz�u���邩
    //private float shardDeg = 0f; // �p�x�i���x����n�܂�̂�)               
    //private float radius = 2.5f;   // �{�X�����_�Ƃ����~�̔��a

    //���G
    //[System.NonSerialized]public bool invincibility = false;
    //[System.NonSerialized] public bool Damaged = false;
    //private GameObject Barrier; // �o���A�I�u�W�F�N�g
    //private Material BariMat;   // �}�e���A��

    // ���S
    private bool death = false;
    private Directing_BossLight bossDirecting;

    // �������p�ϐ�
    private Vector3 InitPosition; // �����ʒu

    public enum AIState
    {
        None,            // �ҋ@
        Walk,            // �U��
        WalkInit,        // �U������
        Lottery,         // �s�����I
        RammingInit,     // �ːi����
        Ramming,         // �ːi
        RammingWait,     // �ːi��̌�
        ThrowShardsInit, // �����珀��
        ThrowShards,     // �������΂�
        Death,           // ���j
    }

    // �G�s�����
    public AIState EnemyAI = AIState.None;

    // �O���擾
    private Transform thisTransform; // ���̃I�u�W�F�N�g�̍��W�����ϐ�
    private GameObject player; // �v���C���[�̃Q�[���I�u�W�F�N�g�T���p
    private Transform playerTransform;
    private GameObject Colchild;
    private TownBossHealth BossHealth;
    private GameObject mainCam;
    private CameraControl2 cameraControl;   //�J�����Ǐ]
    private VibrationCamera vibration;
    private BGMFadeManager _BGMfadeMana;

    public void Init()
    {
        // ���W������
        thisTransform.position = InitPosition;

        // ��ԃZ�b�g
        EnemyAI = AIState.None;

        // ����
        Direction = EnemyDirection.LEFT;

        // �̗͍ő�ɂ���
        BossHealth.BossHealth = BossHealth.MaxBossHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �I�u�W�F�N�g��Transform���擾
        thisTransform = GetComponent<Transform>();

        // �������p�̍��W�ۑ�
        InitPosition = thisTransform.position;

        // �v���C���[�I�u�W�F�N�g�T��
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        // ���g�̎q�I�u�W�F�N�g�擾
        Colchild = transform.GetChild(0).gameObject;
        // �{�X�̗̑̓X�N���v�g�擾
        BossHealth = Colchild.GetComponent<TownBossHealth>();

        // �o���A�I�u�W�F�N�g�擾
        //Barrier = transform.GetChild(2).gameObject;
        //BariMat = Barrier.GetComponent<SpriteRenderer>().material;
        //BariMat.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        // ShardParent�擾
        //shardParent = transform.GetChild(1).gameObject;

        // �J�����T��
        mainCam = GameObject.Find("Main Camera");
        vibration = mainCam.GetComponent<VibrationCamera>();
        cameraControl = mainCam.GetComponent<CameraControl2>();

        // ���C�ʒu����
        Scale = thisTransform.localScale.x * 1.3f;
        AdjustX = Scale;
        AdjustY = -0.9f;

        // �T�C�Y��ۑ�
        sizeX = thisTransform.localScale.x;

        // �{�X���j�p���o
        bossDirecting = transform.GetChild(3).gameObject.GetComponent<Directing_BossLight>();
        _BGMfadeMana = mainCam.GetComponent<BGMFadeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // ���͈͓��Ƀv���C���[���N�����Ă�����X�e�[�^�X�ω�
        // �v���C���[�Ƃ̋��������Ƃ߂�
        SubX = thisTransform.position.x - playerTransform.position.x; // x��
        SubY = thisTransform.position.y - playerTransform.position.y; // y��

        // �O�����̒藝
        Distance = SubX * SubX + SubY * SubY; // �v���C���[�Ƃ̋��������܂���

        //Debug.Log(Distance);

        // �{�X�̌����Ă���������Z�b�g
        SetDirection();

        // �i�s�����Ƀ��C���΂�
        CreateRay();

        switch (EnemyAI)
        {
            // �������Ȃ�
            case AIState.None:
                None();
                break;

            // �U������
            case AIState.WalkInit:
                WalkInit();
                break;

            // �U��
            case AIState.Walk:
                Walk();
                break;

            // �s�����I
            case AIState.Lottery:
                Lottery();
                break;

            // �ːi����
            case AIState.RammingInit:
                RammingInit();
                break;

            // �ːi
            case AIState.Ramming:
                Ramming();
                break;

            // �ːi��̌�
            case AIState.RammingWait:
                RammingWait();
                break;

            // �����珀��
            case AIState.ThrowShardsInit:
                ThrowShardsInit();
                break;

            // �������΂�
            case AIState.ThrowShards:
                //ThrowShards();
                break;

            // ���j
            case AIState.Death:
                Death();
                break;
        }

        //Debug.Log(invincibility);
        // �o���A�`�� ����Ȃ��̂͂Ȃ�
        //DrawBarrier();
    }

    private void Lottery()
    {
        // ���̍s���𒊑I
        var Action = Random.Range(0, 1);

        switch (Action)
        {
            // �ːi
            case 0:
                EnemyAI = AIState.RammingInit;
                break;

            // �������΂�
            case 1:
                EnemyAI = AIState.ThrowShardsInit;
                break;
        }

        //Damaged = false;
        //invincibility = false;
    }

    private void RammingInit()
    {
        // �ːi����
        // ���G
        //invincibility = true;

        // �؂�Ԃ��񐔂����߂�
        SwitchBack = BossHealth.MaxBossHealth - BossHealth.BossHealth;

        // �ːi�������[�V�������ԕ��҂�
        PreRammingTimer += Time.deltaTime;

        // �ːi�����A�j���[�V�������I�������
        if (PreRammingTimer > PreRammingTime)
        {
            // �ːi�J�n
            EnemyAI = AIState.Ramming;

            // ������
            PreRammingTimer = 0f;
        }
    }

    private void Ramming()
    {
        // �ːi�U��
        // ���G

        // �ǂɂԂ���O�Ƀv���C���[�ɂԂ�������
        if (HitPlayer == true)
        {
            EnemyAI = AIState.WalkInit;

            // ������
            RayLengthTimer = 0f;

            HitPlayer = false;

            // ���G����
            //invincibility = false;
        }

        float sign = 0.0f; // ����

        // �����Ă�������ɂ���ĕ������ς��
        if(Direction == EnemyDirection.RIGHT)
        {
            sign = 1f;
        }
        else
        {
            sign = -1f;
        }

        // �ړ����������߂�
        float MoveDistance = sign * RammingSpeed * Time.deltaTime; // ��b�Ԃ�RammmingSpeed���ړ�

        // �ǂɂԂ���܂œːi
        if (hit == false)
        {
            // �{�X�̌��݈ʒu�ɉ��Z
            thisTransform.Translate(MoveDistance, 0.0f, 0.0f);
        }
        // �ǂɂԂ���
        else
        {
            // �؂�Ԃ��񐔂�1�ȏ゠��ΐ؂�Ԃ��ċt�����ɓːi
            if (SwitchBack > 0) 
            {
                if(Direction == EnemyDirection.LEFT)
                {
                    Direction = EnemyDirection.RIGHT;
                }
                else
                {
                    Direction = EnemyDirection.LEFT;
                }

                // �؂�Ԃ��񐔌��炷
                SwitchBack--;
            }
            else
            {
                EnemyAI = AIState.RammingWait;
            }
        }
    }

    private void RammingWait()
    {
        //// ���i�҂���Ă�j
        //if (Damaged == false)
        //{
        //    // ���G����
        //    invincibility = false;
        //}
        //else
        //{
        //    // �_���[�W����������疳�G
        //    invincibility = true;
        //}

        float vibTime = 1f;
        if(RammingWaitTimer == 0)
        {
            cameraControl.enabled = false;
            // ��b�ԐU��
            vibration.SetVibration(vibTime);
        }

        RammingWaitTimer += Time.deltaTime;

        if(RammingWaitTimer > vibTime)
        {
            cameraControl.enabled = true;
        }

        // �w�莞�Ԃ҂�����������ς���AI�ω�
        if (RammingWaitTimer > RammingWaitTime)
        {
            if (Direction == EnemyDirection.LEFT)
            {
                Direction = EnemyDirection.RIGHT;
            }
            else
            {
                Direction = EnemyDirection.LEFT;
            }

            // �s�����I
            EnemyAI = AIState.Lottery;

            // ������
            RammingWaitTimer = 0;
        }
    }

    //Vector3 CreatePos;
    //Quaternion CreateRotate;
    //GameObject effect;
    private void ThrowShardsInit()
    {
        // �����珀��
        // ���G
        //invincibility = true;

    //    // �{�X��HP�ɂ���Ă������΂��̉񐔂��ς��
    //    ShardWaveNum = BossHealth.MaxBossHealth - BossHealth.BossHealth;

    //    if(ShardCreateTimer == 0)
    //    {
    //        // �p�xdeg���烉�W�A�����쐬
    //        var rad = (shardDeg + (CreatedNum % CreateShardNum) * SpacingDeg) * Mathf.Deg2Rad;

    //        // ���W�A����p����sin��cos�����߂�
    //        var sin = Mathf.Sin(rad);
    //        var cos = Mathf.Cos(rad);

    //        var sign = 0f;
    //        // �����Ă�������ɂ���ĕ������ς��
    //        if (Direction == EnemyDirection.RIGHT)
    //        {
    //            sign = 1f;
    //        }
    //        else
    //        {
    //            sign = -1f;
    //        }

    //        // �{�X�𒆐S�Ƃ����~����̓_�����߂�
    //        CreatePos = new Vector3(thisTransform.position.x + sign * cos * radius, thisTransform.position.y + AdjustY + sin * radius, 0f);

    //        // ���̂�����̉�]�p�x�����߂�
    //        // ������ ��]���������p�x
    //        // ������ ��]���������� right,up,forward
    //        CreateRotate = Quaternion.AngleAxis(
    //            (sign *               // �����i�ǂ�������ɔ�΂��̂��j
    //            (90f +                // �Ăяo�����Ђ��΂������ɂނ����邽�߂̊p�x
    //            SpacingDeg *          // ���x�Ԋu�Ŕz�u����̂�
    //            (CreatedNum % CreateShardNum)))    // ���̃E�F�[�u�̒��ŉ��Ԗڂɐ�������郂�m��(1�E�F�[�u6��)
    //            , Vector3.forward);   // z����]��������

    //        // ���S�ɏW�܂��Ă���G�t�F�N�g����
    //        effect = Instantiate(ChargeEffect);
    //        effect.transform.position = CreatePos;

    //        // �{�X�̒��S���W��AdjustY�ł��炵���̂ł��炵�����W�������Ă���
    //        Vector3 AdjustYThisPos = new Vector3(thisTransform.position.x, thisTransform.position.y + AdjustY, thisTransform.position.z);

    //        // �쐬���������炩��{�X�̒��S���W�܂ł̃x�N�g�������߂�
    //        var Vector_Shrad_Boss = CreatePos - AdjustYThisPos;
    //        // ���Ƃ߂��x�N�g���̃I�u�W�F�N�g�̍쐬�ԍ��Ɣz��̓Y��������v����悤�Ƀx�N�g����ۑ�
    //        ShardVelocity[CreatedNum] = Vector_Shrad_Boss;
    //    }

    //    // ������𐶐����Ă���̌o�ߎ���
    //    ShardCreateTimer += Time.deltaTime;

    //    if(ShardCreateTimer > CreateShardsNeedTime)
    //    {
    //        // ������ �쐬����I�u�W�F�N�g�̑f�ƂȂ�v���n�u
    //        // ������ �쐬����ʒu
    //        // ��O���� �쐬����Ƃ��̊p�x
    //        // ��l���� �쐬����I�u�W�F�N�g�̐e�I�u�W�F�N�g
    //        shardObj[CreatedNum] = Instantiate(Shards_Prefab, CreatePos, CreateRotate, shardParent.transform);

    //        // �傫������
    //        shardObj[CreatedNum].transform.localScale = new Vector3(1f, 1f, 1f);

    //        // �p�[�e�B�N���̃Q�[���I�u�W�F�N�g����
    //        Destroy(effect);

    //        // �쐬���J�E���g
    //        CreatedNum++;

    //        // ���̂�����쐬�̂��ߏ�����
    //        ShardCreateTimer = 0f;
    //    }

    //    // ����������I������
    //    if (CreatedNum >= CreateShardNum + NowShardWave * CreateShardNum)
    //    {
    //        // �������΂���
    //        EnemyAI = AIState.ThrowShards;

    //        // ������
    //        ShardCreateTimer = 0.0f;
    //    }
    //}

    //private void ThrowShards()
    //{
    //    // �������΂�

    //    if (Damaged == false)
    //    {
    //        // ���G����
    //        //invincibility = false;
    //    }

    //    // ����AIState�ɂȂ����ŏ��̃t���[���̂ݓ���
    //    if (AllAddVelocity == false)
    //    {
    //        // ���W�b�h�{�f�B��velocity�ɑΉ������l�����Z
    //        for (int i = 0 + CreateShardNum * NowShardWave; i < CreatedNum; i++)
    //        {
    //            if (shardObj[i] != null)
    //            {
    //                Rigidbody2D rigid2D = shardObj[i].GetComponent<Rigidbody2D>();
    //                rigid2D.velocity = ShardVelocity[i] * ShardSpeed;
    //            }
    //        }
    //    }

    //    // HP�����Ȃ��Ȃ�΂Ȃ�قǂ�������΂��񐔂�������
    //    if(NowShardWave < ShardWaveNum)
    //    {
    //        // ���̃E�F�[�u�ɐi�߂�
    //        NowShardWave++;

    //        // ���̂����珀��
    //        EnemyAI = AIState.ThrowShardsInit;
    //    }
    //    else
    //    {
    //        AllAddVelocity = true;

    //        ShardThrowTimer += Time.deltaTime;
    //        if (ShardThrowTimer > ShardWaitTime)
    //        {
    //            // �s�����I
    //            EnemyAI = AIState.Lottery;

    //            // ������
    //            for (int i = 0; i < CreatedNum; i++)
    //            {
    //                if (shardObj[i] != null)
    //                {
    //                    // �����������
    //                    Destroy(shardObj[i].gameObject);
    //                    shardObj[i] = null;
    //                }
    //                ShardVelocity[i] = Vector3.zero;
    //            }

    //            CreatedNum = 0;
    //            AllAddVelocity = false;
    //            ShardThrowTimer = 0f;
    //            NowShardWave = 0;
    //        }
    //    }
    }

    private void Death()
    {
        if (death == false)
        {
            // �v���C���[�Ƃ̓����蔻���
            Colchild.GetComponent<CircleCollider2D>().enabled = false;

            death = true;

            bossDirecting.Flash();

            // �{�XBGM������
            _BGMfadeMana.SmallBossBGM();

            //// �o���A�����ɂ���
            //BariMat.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void None()
    {
        // ��苗���܂Ńv���C���[���߂Â�����
        if(Distance < MoveStartDistance)
        {
            EnemyAI = AIState.Lottery;
        }
    }

    private void WalkInit()
    {
        // �ǂ���Ɉړ����邩���C���΂��Č��߂�

        // �J�E���g
        RayLengthTimer += Time.deltaTime;

        // �v���C���[�Ƀq�b�g���Ă���w�莞�ԑ҂�����
        if (RayLengthTimer > 1f && raycast == false)
        {
            // ���C�쐬�J�n
            raycast = true;
            RayLengthTimer = 0f + Time.deltaTime;
        }

        if (raycast)
        {
            // ���C�����E�ɔ�΂�
            // �����ʒu
            Vector2 origin = new Vector2(
                thisTransform.position.x,
                thisTransform.position.y + AdjustY
                );

            // ���C���΂�����
            Vector2 LeftRay = Vector2.left;
            Vector2 RightRay = Vector2.right;

            // ����
            float length = 10f * RayLengthTimer;
            // ����
            Vector2 DisLeft = LeftRay * length;
            Vector2 DisRight = RightRay * length;

            LayerMask layerMask = 1 << 10; // Ground�̂�

            // ���C��΂��ăX�e�[�W�ƂԂ������琶����߂�
            bool hit_left = Physics2D.Raycast(origin, LeftRay, length, layerMask);    // ��������
            bool hit_right = Physics2D.Raycast(origin, RightRay, length, layerMask);  // �E������

            // �`��
            Debug.DrawRay(origin, DisLeft, Color.red);
            Debug.DrawRay(origin, DisRight, Color.blue);

            // ���C����ɂԂ��������Ɣ��Ε����ɕ���
            // ���������ɂԂ���΃����_��
            if (hit_left == true && hit_right == true)
            {
                // �O�`�P�̗����擾
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    Direction = EnemyDirection.LEFT;
                }
                else
                {
                    Direction = EnemyDirection.RIGHT;
                }
            }
            else if (hit_left == true)
            {
                // �E�ɕ���
                Direction = EnemyDirection.RIGHT;
            }
            else if (hit_right == true)
            {
                // ���ɕ���
                Direction = EnemyDirection.LEFT;
            }

            // �����ǂ��炩���Ԃ����Ă���Ώ�Ԃ�J��
            if (hit_left || hit_right)
            {
                EnemyAI = AIState.Walk;

                // ������
                RayLengthTimer = 0f;
                raycast = false;
            }
        }

        //Debug.Log("walkInit");
        //Debug.Log(HitPlayer);     
    }

    private void Walk()
    {
        //if(Damaged == true)
        //{
        //    //invincibility = true;
        //}

        // �w�莞�ԉ��ɕ���
        if(Direction == EnemyDirection.LEFT)
        {
            // ���Ɉړ�
            thisTransform.Translate(-WalkSpeed * Time.deltaTime, 0f, 0f);
        }
        else
        {
            // �E�Ɉړ�
            thisTransform.Translate(WalkSpeed * Time.deltaTime, 0f, 0f);
        }

        WalkTimer += Time.deltaTime;
        if(WalkTimer > WalkTime)
        {
            EnemyAI = AIState.Lottery;

            // ������
            WalkTimer = 0f;

            // �v���C���[�̈ʒu�Ɍ������čU�����邽�߂̏���
            if(playerTransform.position.x > thisTransform.position.x)
            {
                Direction = EnemyDirection.RIGHT;
            }
            else
            {
                Direction = EnemyDirection.LEFT;
            }
        }

        if (hit == true)
        {
            EnemyAI = AIState.Lottery;
        }
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

        // �`��
        Debug.DrawRay(origin, distance, Color.red);
        //-----------------------------------------------------------------------------------------------
    }

    private void SetDirection()
    {
        if (Direction == EnemyDirection.LEFT) {
            // ������
            thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            // ���C����
            AdjustX = Scale;

            // �p�[�e�B�N���ʒu����
            sandSmokeAdjustX_left = -sandSmokeOffset_left.x;
            sandSmokeAdjustX_right = -sandSmokeOffset_right.x;
        }
        else
        {
            // �E����
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            // ���C����
            AdjustX = -Scale;

            // �p�[�e�B�N���ʒu����
            sandSmokeAdjustX_left = sandSmokeOffset_left.x;
            sandSmokeAdjustX_right = sandSmokeOffset_right.x;
        }
    }

    //private void DrawBarrier()
    //{
    //    // ���G�Ȃ�o���A�`��
    //    if(invincibility == true)
    //    {
    //        BariMat.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //    }
    //    else
    //    {
    //        BariMat.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    //    }
    //}

    public void SetHitPlayer(bool _hit)
    {
        HitPlayer = _hit;
    }

    public void CreateSandSmokeLeft()
    {
         // �p�[�e�B�N������
         var obj = Instantiate(SandSmoke);
         // �{�X�̍��W�擾
         var pos = GetComponent<Transform>().position;

         // �����ɐ���
         obj.transform.position = new Vector3(pos.x + sandSmokeAdjustX_left, pos.y + sandSmokeOffset_left.y, pos.z);
    }

    public void CreateSandSmokeRight()
    {
         // �p�[�e�B�N������
         var obj = Instantiate(SandSmoke);
         // �{�X�̍��W�擾
         var pos = GetComponent<Transform>().position;

         // �����ɐ���
         obj.transform.position = new Vector3(pos.x + sandSmokeAdjustX_right, pos.y + sandSmokeOffset_right.y, pos.z);
    }
}
