//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�GAI
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    public bool Stop = false; // �f�o�b�O�p �G�����̏�ɂƂǂ܂�

    // �G�̏���J�n�ʒu
    private Vector3 start;
    // �G�̈ړ���
    private Vector3 target;
    // �G�̈ړ�����
    private bool Outbound = true; // true:���H false:���H
    [Header("�G�̈ړ��͈�")]
    public float MoveArea = 5.0f; // �G�̈ړ��͈�
    // �ړ������̊�����\�� 0����1
    private float timer;
    // ���C�̏Փ˔��茋�ʗp�ϐ�
    RaycastHit2D hit;
    // ���C�̈ʒu����
    float AdjustX;
    private float sizeX; // ���[�J���T�C�Y�ۑ�

    // �G�̃v���C���[�T�[�`�ϐ�
    private float SubX;    // ���߂�X���W�̍���ێ�����ϐ�
    private float SubY;    // ���߂�Y���W�̍���ێ�����ϐ�
    private float Distance; // ���߂�������ێ�����ϐ�
    [Header("�v���C���[�����m����͈�")]
    public float senserDistance = 6.0f; // ������Ƃ�͈�

    // �G���U�����n�߂鋗��
    [Header("�U�����[�V�����ɓ��鋗��")]
    public float attackDistance = 2.0f; // �U�����鋗��

    [Header("�v���C���[��ǂ������鑬�x")]
    public float TrackingSpeed = 3.0f; // �ǐՃX�s�[�h

    private enum AIState
    {
        INIT_PATROL,
        PATROL,
        INIT_TRACKING,
        TRACKING,
        INIT_ATTACK,
        ATTACK
    }

    // �G�s�����
    AIState EnemyAI = AIState.INIT_PATROL;

    // �O���擾
    private Transform thisTransform; // ���̃I�u�W�F�N�g�̍��W�����ϐ�
    private GameObject player; // �v���C���[�̃Q�[���I�u�W�F�N�g�T���p
    private Transform playerTransform;
    private HammerNail hammer; // HammerNail���擾

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �I�u�W�F�N�g��Transform���擾
        thisTransform = GetComponent<Transform>();

        // �v���C���[�I�u�W�F�N�g�T��
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        AdjustX = thisTransform.localScale.x / 2.0f;
        sizeX = thisTransform.localScale.x;

        // Hammer�X�N���v�g�擾
        hammer = player.GetComponent<HammerNail>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Stop == false)
        {
            // ���͈͓��Ƀv���C���[���N�����Ă�����X�e�[�^�X�ω�
            // �v���C���[�Ƃ̋��������Ƃ߂�
            SubX = thisTransform.position.x - playerTransform.position.x; // x��
            SubY = thisTransform.position.y - playerTransform.position.y; // y��

            // �O�����̒藝
            Distance = SubX * SubX + SubY * SubY; // �v���C���[�Ƃ̋��������܂���

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
            }
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
        }
        // ���C�̏Փ˂ɂ��Đݒ�
        else
        {
            // �E�ǂƂ̏Փˌ�
            if (Outbound)
            {
                // �G�̊J�n�ʒu����
                start = new Vector3(thisTransform.position.x - MoveArea, thisTransform.position.y, 0.0f);
                // �G�̖ړI�n
                target = thisTransform.position;
                // ������
                timer = 1.0f;

                AdjustX = -1 * thisTransform.localScale.x / 2.0f;
                thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z); // ������


                Outbound = false;
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

                AdjustX = thisTransform.localScale.x / 2.0f;
                thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z); // �E����


                Outbound = true;
            }
        }

        EnemyAI = AIState.PATROL;
    }
    void Patrol()
    {
        // 2�b�ňړ������ύX

        // �E�Ɉړ����Ă���Ȃ�
        if (Outbound)
        {
            timer += Time.deltaTime / 2;

            if (timer >= 1.0f)
            {
                Outbound = false;
                AdjustX = -AdjustX;

                thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            }
        }
        // ���Ɉړ����Ă���Ȃ�
        else
        {
            timer -= Time.deltaTime / 2;

            if (timer <= 0.0f)
            {
                Outbound = true;
                AdjustX = -AdjustX;

                thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);

            }
        }

        //---------------------------------------------------------------------
        // �ǂɌ������Ẵ��C

        // �i�s�����Ƀ��C���΂��ĕǂɂԂ�������i�s������ς���
        Vector2 origin = new Vector2(
            thisTransform.position.x + AdjustX,
            thisTransform.position.y
            );

        // ���C���΂�����
        Vector2 RayDirection;
        if (Outbound)
        {
            RayDirection = new Vector2(1, 0); // �E����
        }
        else
        {
            RayDirection = new Vector2(-1, 0); // ������
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

        Debug.DrawRay(origin, distance, Color.red);
        //-----------------------------------------------------------------------------------------------

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
            // �p�g���[���̎�����������ς��Ȃ�i�G�̌�납����G�͈͂ɓ�������j
            if (SubX < 0.0f)
            {
                // �������ς��Ȃ�
                if (Outbound == false)
                {
                    AdjustX = -AdjustX;
                }
                Outbound = true;
            }
            else if (SubX > 0.0f)
            {
                if (Outbound == true)
                {
                    AdjustX = -AdjustX;
                }
                Outbound = false;
            }

            //---------------------------------------------------------------------
            // �ǂɌ������Ẵ��C

            // �i�s�����Ƀ��C���΂��ĕǂɂԂ�������i�s������ς���
            Vector2 origin = new Vector2(
                thisTransform.position.x + AdjustX,
                thisTransform.position.y
                );

            // ���C���΂�����
            Vector2 RayDirection;
            if (Outbound)
            {
                RayDirection = new Vector2(1, 0); // �E����
                // �E����
                thisTransform.localScale = new Vector3(sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
            }
            else
            {
                RayDirection = new Vector2(-1, 0); // ������
                // ������
                thisTransform.localScale = new Vector3(-sizeX, thisTransform.localScale.y, thisTransform.localScale.z);
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

            Debug.DrawRay(origin, distance, Color.red);

            // �ǂɃ��C���ڐG������ǂ�Ȃ�
            if (!hit)
            {
                // �v���C���[�Ɍ������Đi��
                // �v���C���[���G���g���E�ɂ���Ȃ�
                if (SubX < 0.0f)
                {
                    thisTransform.Translate(TrackingSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
                // �v���C���[���G���g��荶�ɂ���Ȃ�
                else if (SubX > 0.0f)
                {
                    thisTransform.Translate(-1 * TrackingSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
            }

            // �߂Â���������
            if (Distance < attackDistance)
            {
                // �U����Ԃɕω�
                EnemyAI = AIState.ATTACK;
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
        EnemyAI = AIState.ATTACK;
    }
    void Attack()
    {
        if (Distance > attackDistance)
        {
            EnemyAI = AIState.TRACKING;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == NailTag)
    //    {
    //        Debug.Log("tag");

    //        if(hammer.MomentHitNails == true)
    //        {
    //            Destroy(this.gameObject);
    //        }
    //    }
    //}
}