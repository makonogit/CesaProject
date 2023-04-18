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
    private float halfScale; 
    // ���C�̈ʒu����
    float AdjustX;

    // �ːi�p�ϐ�
    private float RammingSpeed = 5.0f;     // �ːi���̈ړ����x
    private float PreRammingTimer = 0f;    // �ːi��������
    private int SwitchBack;                // �؂�Ԃ���
    private float RammingWaitTimer;        // �ːi��̌�����

    // �������΂��p�ϐ�
    private float CreateShardsNeedTime = 0.6f;   // ����������̂ɂ����鎞��
    private float ShardCreateTimer;            // ������𐶐����n�߂Ă���̌o�ߎ���
    private int CreatedNum = 0; // ��������������
    public GameObject Shards_Prefab; // ������̃v���n�u�������Ă����ϐ�
    private GameObject[] shardObj = new GameObject[18]; // �쐬����������I�u�W�F�N�g�������z��
    private GameObject shardParent; // ������̐e�I�u�W�F�N�g
    private Vector3[] ShardVelocity = new Vector3[18]; // �������������ړ������邽�߂̒l������z��
    private bool AllAddVelocity = false; // �쐬�����S�Ă̂������Velocity�����Z������
    [Header("������̈ړ��X�s�[�h")]public float ShardSpeed = 5f;
    private float ShardWaitTime = 3f; // ��������΂�����̎��̃��[�V�����܂ł̑҂�����
    private float ShardThrowTimer = 0f; // ��������Ƃ΂��Ă���̌o�ߎ���
    private int ShardWaveNum = 0; // �Ȃ񂩂��̃E�F�[�u�����邩
    private int NowShardWave = 0; // �����E�F�[�u�ڂ�

    // ������z�u�p�ϐ�
    [Header("���x�Ԋu�ł������z�u���邩(�����l10)")]public float SpacingDeg = 10f; // ���x�Ԋu�Ŕz�u���邩
    private float shardDeg = -15f; // �p�x�i���x����n�܂�̂�)               
    private float radius = 4f;   // �{�X�����_�Ƃ����~�̔��a

    //���G
    [System.NonSerialized]public bool invincibility = false;

    public enum AIState
    {
        Lottery,         // �s�����I
        RammingInit,     // �ːi����
        Ramming,         // �ːi
        RammingWait,     // �ːi��̌�
        ThrowShardsInit, // �����珀��
        ThrowShards,     // �������΂�
        Death,           // ���j
    }

    // �G�s�����
    public AIState EnemyAI = AIState.Lottery;

    // �O���擾
    private Transform thisTransform; // ���̃I�u�W�F�N�g�̍��W�����ϐ�
    private GameObject player; // �v���C���[�̃Q�[���I�u�W�F�N�g�T���p
    private Transform playerTransform;
    private GameObject Colchild;
    private TownBossHealth BossHealth;
    private GameObject mainCam;
    private CameraControl2 cameraControl;   //�J�����Ǐ]
    private VibrationCamera vibration;

    private Material mat;

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �I�u�W�F�N�g��Transform���擾
        thisTransform = GetComponent<Transform>();

        // �v���C���[�I�u�W�F�N�g�T��
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        // ���g�̎q�I�u�W�F�N�g�擾
        //child = transform.Find("HitCollider").gameObject;
        Colchild = transform.GetChild(0).gameObject;
        // �{�X�̗̑̓X�N���v�g�擾
        BossHealth = Colchild.GetComponent<TownBossHealth>();

        // ShardParent�擾
        shardParent = transform.GetChild(1).gameObject;

        // �J�����T��
        mainCam = GameObject.Find("Main Camera");
        vibration = mainCam.GetComponent<VibrationCamera>();
        cameraControl = mainCam.GetComponent<CameraControl2>();

        // ���C�ʒu����
        halfScale = thisTransform.localScale.x / 2.0f;
        AdjustX = halfScale;

        // �T�C�Y��ۑ�
        sizeX = thisTransform.localScale.x;

        // �F�擾
        mat = GetComponent<SpriteRenderer>().material;

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

        // �{�X�̌����Ă���������Z�b�g
        SetDirection();

        // �i�s�����Ƀ��C���΂�
        CreateRay();

        switch (EnemyAI)
        {
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
                ThrowShards();
                break;

            // ���j
            case AIState.Death:
                Death();
                break;
        }
    }

    private void Lottery()
    {
        // ���̍s���𒊑I
        var Action = Random.Range(0, 2);

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
    }

    private void RammingInit()
    {
        // �ːi����
        // ���G
        invincibility = true;

        // �؂�Ԃ��񐔂����߂�
        SwitchBack = BossHealth.MaxBossHealth - BossHealth.BossHealth;

        // �ːi�������[�V�������ԕ��҂�
        PreRammingTimer += Time.deltaTime;

        // �ːi�����A�j���[�V�������I�������
        if (PreRammingTimer > 3f)
        {
            // �ːi�J�n
            EnemyAI = AIState.Ramming;

            // ������
            PreRammingTimer = 0f;
        }

        mat.color = new Color(1f, 0f, 0f);
    }

    private void Ramming()
    {
        // �ːi�U��
        // ���G

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

        mat.color = new Color(0f, 1f, 0f);

    }

    private void RammingWait()
    {
        // ���i�҂���Ă�j
        // ���G����
        invincibility = false;

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
        if (RammingWaitTimer > 3f)
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

        // �f�o�b�O�p�ɐF�̕ω��ōs���𔻕�
        mat.color = new Color(0f, 0f, 1f);

    }

    private void ThrowShardsInit()
    {
        // �����珀��
        // ���G
        invincibility = true;

        // �{�X��HP�ɂ���Ă������΂��̉񐔂��ς��
        ShardWaveNum = BossHealth.MaxBossHealth - BossHealth.BossHealth;

        if(ShardCreateTimer == 0)
        {
            // �p�xdeg���烉�W�A�����쐬
            var rad = (shardDeg + (CreatedNum % 6) * SpacingDeg) * Mathf.Deg2Rad;

            // ���W�A����p����sin��cos�����߂�
            var sin = Mathf.Sin(rad);
            var cos = Mathf.Cos(rad);

            var sign = 0f;
            // �����Ă�������ɂ���ĕ������ς��
            if (Direction == EnemyDirection.RIGHT)
            {
                sign = 1f;
            }
            else
            {
                sign = -1f;
            }

            // �{�X�𒆐S�Ƃ����~����̓_�����߂�
            Vector3 CreatePos = new Vector3(thisTransform.position.x + sign * cos * radius, thisTransform.position.y + sin * radius, 0f);

            // ���̂�����̉�]�p�x�����߂�
            // ������ ��]���������p�x
            // ������ ��]���������� right,up,forward
            Quaternion CreateRotate = Quaternion.AngleAxis((sign * (60f + SpacingDeg * (CreatedNum % 6))), Vector3.forward); 

            // ������ �쐬����I�u�W�F�N�g�̑f�ƂȂ�v���n�u
            // ������ �쐬����ʒu
            // ��O���� �쐬����Ƃ��̊p�x
            // ��l���� �쐬����I�u�W�F�N�g�̐e�I�u�W�F�N�g
            shardObj[CreatedNum] = Instantiate(Shards_Prefab, CreatePos, CreateRotate, shardParent.transform);

            // �쐬���������炩��{�X�̒��S���W�܂ł̃x�N�g�������߂�
            var Vector_Shrad_Boss = CreatePos - thisTransform.position;
            // ���Ƃ߂��x�N�g���̃I�u�W�F�N�g�̍쐬�ԍ��Ɣz��̓Y��������v����悤�Ƀx�N�g����ۑ�
            ShardVelocity[CreatedNum] = Vector_Shrad_Boss;
            // �傫������
            shardObj[CreatedNum].transform.localScale = new Vector3(0.01f, 0.01f, 0f);
        }

        // ������𐶐����Ă���̌o�ߎ���
        ShardCreateTimer += Time.deltaTime;

        if(ShardCreateTimer > CreateShardsNeedTime)
        {
            // �쐬���J�E���g
            CreatedNum++;

            // ���̂�����쐬�̂��ߏ�����
            ShardCreateTimer = 0f;
        }

        // ����������I������
        if (CreatedNum >= 6 + NowShardWave * 6)
        {
            // �������΂���
            EnemyAI = AIState.ThrowShards;

            // ������
            ShardCreateTimer = 0.0f;
        }
    }

    private void ThrowShards()
    {
        // �������΂�
        // ���G����
        invincibility = false;

        // ����AIState�ɂȂ����ŏ��̃t���[���̂ݓ���
        if (AllAddVelocity == false)
        {
            // ���W�b�h�{�f�B��velocity�ɑΉ������l�����Z
            for (int i = 0 + 6 * NowShardWave; i < CreatedNum; i++)
            {
                Rigidbody2D rigid2D = shardObj[i].GetComponent<Rigidbody2D>();
                rigid2D.velocity = ShardVelocity[i] * ShardSpeed;
            }

        }

        // HP�����Ȃ��Ȃ�΂Ȃ�قǂ�������΂��񐔂�������
        if(NowShardWave < ShardWaveNum)
        {
            // ���̃E�F�[�u�ɐi�߂�
            NowShardWave++;

            // ���̂����珀��
            EnemyAI = AIState.ThrowShardsInit;
        }
        else
        {
            AllAddVelocity = true;

            ShardThrowTimer += Time.deltaTime;
            if (ShardThrowTimer > ShardWaitTime)
            {
                // �s�����I
                EnemyAI = AIState.Lottery;

                // ������
                for (int i = 0; i < CreatedNum; i++)
                {
                    // �����������
                    Destroy(shardObj[i].gameObject);
                    shardObj[i] = null;

                    ShardVelocity[i] = Vector3.zero;
                }

                CreatedNum = 0;
                AllAddVelocity = false;
                ShardThrowTimer = 0f;
                NowShardWave = 0;
            }
        }

        mat.color = new Color(1f, 0f, 1f);
    }

    private void Death()
    {
        mat.color = new Color(2f, 2f, 2f);
    }

    private void CreateRay()
    {
        // �i�s�����Ƀ��C���΂��ĕǂɂԂ�������i�s������ς���
        Vector2 origin = new Vector2(
            thisTransform.position.x + AdjustX,
            thisTransform.position.y
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
            AdjustX = -halfScale;
        }
        else
        {
            // �E����
            thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            // ���C����
            AdjustX = halfScale;
        }
    }
}
