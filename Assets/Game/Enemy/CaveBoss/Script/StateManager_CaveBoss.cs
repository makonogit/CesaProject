//=========================================
// �S���F�����V�S
// ���e�F���A�̃{�X�̍s���𐧌�
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_CaveBoss : MonoBehaviour
{
    //=====================================
    // *** �ϐ��錾 ***
    //=====================================

    //-------------------------------------
    // �������֘A 
    //-------------------------------------

    // ���W
    Vector3 init_boss_pos;    // �{�X�̏����ʒu
    Vector3 init_lefthand_pos;// ����̏����ʒu
    Vector3 init_right_pos;   // �E��̏����ʒu

    // �X�e�[�^�X
    int init_boss_hp;         // �{�X�̏���HP

    //-------------------------------------
    // �s������֘A 
    //-------------------------------------

    // ���C�����ID
    enum MainStateID   
    {
        NULL,          // ��ԂȂ�
        MOVE,          // �ړ����
        ATTACK,        // �U�����
        DAMAGE,        // ��e���
        DEATH,         // �퓬�s�\
    }

    // ���C�����
    MainStateID oldMainState = MainStateID.NULL; // �O�̏��
    MainStateID nowMainState = MainStateID.NULL; // ���݂̏��
    MainStateID nextMainState = MainStateID.NULL;// ���̏��

    // �U�����ID
    enum AttackStateID
    {
        NULL,         // ��ԂȂ�
        ENEMY_DROP,   // �G���~�点��
        GRIP_PLAYER   // �v���C���[��߂܂���
    }

    // �U�����
    AttackStateID oldAttackState = AttackStateID.NULL;  // �O�̏��
    AttackStateID nowAttackState = AttackStateID.NULL;  // ���݂̏��
    AttackStateID nextAttackState = AttackStateID.NULL; // ���̏��
    bool isEndState;                                    // �U���I���t���O

    // �ҋ@����
    [Header("[�s���ҋ@����]")]
    public int mainStateDelay = 500;// �s���ҋ@����
    int mainStateDelay_Cnt;         // �s���ҋ@���Ԃ��J�E���g

    //-------------------------------------
    // �}�e���A���֘A 
    //-------------------------------------

    // �F
    SpriteRenderer sr_boss;     // �{�X�̐F
    SpriteRenderer sr_lefthand; // ����̐F
    SpriteRenderer sr_righthand;// �E��̐F

    //-------------------------------------
    // ��e��Ԋ֘A 
    //-------------------------------------

    // ��e���
    [Header("[��e���]")]
    [Header("�EHP")]
    public int hp = 3;               // �̗�
    [Header("�E���j���G�t�F�N�g")]
    public GameObject effect;        // �G�t�F�N�g
    [Header("�E���G����")]
    public int invincible_time = 100;// ���G����
    int invincible_time_cnt;         // ���G���ԃJ�E���g

    //-------------------------------------
    // �ړ��֘A 
    //-------------------------------------

    // ��]
    Vector2 center;      // ��]�̒��S���W
    float angle;         // ��]�p�x
    float radius = 0.25f;// �~�̔��a
   
    //=====================================
    // *** ������ ***
    //=====================================

    void Start()
    {
        // ���݂̏�Ԃ��ړ���Ԃɏ�����
        nowMainState = MainStateID.MOVE;

        // �F���擾
        sr_boss = GetComponent<SpriteRenderer>();
        sr_lefthand = GameObject.Find("LeftHand").GetComponent<SpriteRenderer>();
        sr_righthand = GameObject.Find("RightHand").GetComponent<SpriteRenderer>();

        // �����ʒu��ۑ�
        init_boss_pos = this.transform.position;
        init_lefthand_pos = transform.Find("LeftHand").gameObject.transform.position;
        init_right_pos = transform.Find("RightHand").gameObject.transform.position;

        // ����HP��ۑ�
        init_boss_hp = hp;
    }

    //=====================================
    // *** �X�V���� ***
    //=====================================

    void Update()
    {
        //---------------------------------------
        // ��Ԃ��X�V
        //--------------------------------------

        if (nextMainState != MainStateID.NULL)
        {
            oldMainState = nowMainState;
            nowMainState = nextMainState;
            nextMainState = MainStateID.NULL;
        }

        //---------------------------------------
        // ���݂̏�Ԃɂ���ď����𕪊�
        //---------------------------------------

        switch (nowMainState)
        {
            // �ړ����
            case MainStateID.MOVE:
                Move_CaveBoss.instance.Move();
                RandomMainState();
                break;
            // �U�����
            case MainStateID.ATTACK:
                Attack();
                break;
            // ��e���
            case MainStateID.DAMAGE:
                Damage();
                break;
            // �퓬�s�\
            case MainStateID.DEATH:
                Death();
                break;
        }
    }

    //============================================================
    // *** �Փ˔��� ***
    //============================================================

    void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------------
        // ��e��ԈȊO�łЂтƏՓ˂������e��ԂɑJ��
        //--------------------------------------------------------

        if (nowMainState != MainStateID.DAMAGE)
        {
            if (collision.gameObject.tag == "Crack")
            {
                // ��e��ԂɑJ��
                nextMainState = MainStateID.DAMAGE;

                // �Փ˂����Ђт�j��
                Destroy(collision.gameObject);
            }
        }
    }

    //=====================================
    // *** �U������ ***
    //=====================================

    void Attack()
    {
        //---------------------------------------
        // *** ���݂̏�Ԃɂ���ď����𕪊� ***

        if (nextAttackState != AttackStateID.NULL)
        {
            oldAttackState = nowAttackState;
            nowAttackState = nextAttackState;
            nextAttackState = AttackStateID.NULL;
        }

        switch (nowAttackState)
        {
            // �G���~�点��
            case AttackStateID.ENEMY_DROP:
                isEndState = EnemyDrop_CaveBoss.instance.EnemyDrop();
                break;
            // �v���C���[��͂�
            case AttackStateID.GRIP_PLAYER:
                isEndState = GripPlayer_CaveBoss.instance.GripPlayer();
                break;
        }

        //---------------------------------------
        // *** �U�����I��������ʏ��Ԃɖ߂� ***

        if (isEndState == true)
        {
            nextMainState = MainStateID.MOVE;
            isEndState = false;
        }
    }

    //=============================================
    // *** �����_���Ƀ��C���s�������肷�鏈�� ***
    //=============================================

    void RandomMainState()
    {
        //---------------------------------
        // *** �����_���Ɏ��̍s�������� ***

        mainStateDelay_Cnt++;

        if (mainStateDelay_Cnt >= mainStateDelay)
        {
            //---------------------------------
            // �G���~�点��

            int rnd = Random.Range(1, 100 + 1);

            if (rnd > 50)
            {
                nextMainState = MainStateID.ATTACK;
                nextAttackState = AttackStateID.ENEMY_DROP;
            }

            //---------------------------------
            // �v���C���[��߂܂���

            else
            {
                nextMainState = MainStateID.ATTACK;
                nextAttackState = AttackStateID.GRIP_PLAYER;
            }

            mainStateDelay_Cnt = 0;
        }
    }

    //===========================================
    // *** �퓬�s�\��Ԃ̏��� ***
    //===========================================

    void Death()
    {
        //---------------------------------------------------
        //  �G�����E�ɐU��
        //---------------------------------------------------

        // ���݂̃g�����X�t�H�[�����擾
        Vector3 pos = this.transform.position;
        // �p�x�����W�A���ɕϊ�
        float rd = -angle * Mathf.PI / 180.0f;
        // ��]��̍��W���v�Z
        pos.x = center.x + (Mathf.Cos(rd) * radius) + radius;
        // �ύX�𔽉f
        this.transform.position = pos;
        // �p�x�����Z
        angle += 10.0f;

        //-------------------------------------------------------------------
        // ���X�ɓ����ɂ���B���S�ɓ����ɂȂ����炱�̃I�u�W�F�N�g��j������
        //-------------------------------------------------------------------

        // ���̒l�����炷
        sr_boss.color = new Color(sr_boss.color.r, sr_boss.color.g, sr_boss.color.b, sr_boss.color.a - 0.002f);
        sr_lefthand.color = new Color(sr_boss.color.r, sr_boss.color.g, sr_boss.color.b, sr_boss.color.a - 0.002f);
        sr_righthand.color = new Color(sr_boss.color.r, sr_boss.color.g, sr_boss.color.b, sr_boss.color.a - 0.002f);

        // ����0�ɂȂ����炱�̃I�u�W�F�N�g��j��
        if (sr_boss.color.a < 0.0f)
        {
            // ���̃I�u�W�F�N�g��j��
            Destroy(gameObject);
        }
    }

    //===========================================
    // *** ��e��Ԃ̏��� ***
    //===========================================

    void Damage()
    {
       
        // ���G���Ԃ��J�E���g
        invincible_time_cnt++;

        //---------------------------------------
        // HP��1���炷�B0�Ȃ�퓬�s�\��ԂɑJ��
        //---------------------------------------

        if (invincible_time_cnt == 1)
        {
            // HP��1���炷
            hp--;

            // 0�Ȃ�퓬�s�\��ԂɑJ��
            if (hp <= 0)
            {
                // �F���f�t�H���g�ɖ߂�
                sr_boss.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                sr_lefthand.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                sr_righthand.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

                // �퓬�s�\��ԂɑJ��
                nextMainState = MainStateID.DEATH;

                // �G�t�F�N�g���Đ�
                Instantiate(effect, transform.position, Quaternion.identity);

                // ��]�̒��S���W�Ɍ��݈ʒu��ۑ�
                center = transform.position;
            }
        }

        //---------------------------------------
        // ���G���ԓ��Ȃ�{�X��ԐF�ɕύX
        //---------------------------------------

        if (invincible_time_cnt < invincible_time)
        {
            // �ԐF�ɕύX
            sr_boss.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            sr_lefthand.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            sr_righthand.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        //--------------------------------------------
        // ���G���Ԃ��I�������Ȃ�{�X�����̏�Ԃɖ߂�
        //--------------------------------------------

        else
        {
            // ���G���Ԃ̃J�E���g�����Z�b�g
            invincible_time_cnt = 0;

            // �F���f�t�H���g�ɖ߂�
            sr_boss.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            sr_lefthand.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            sr_righthand.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            // �_���[�W���󂯂�O�̏�Ԃɖ߂�
            nextMainState = oldMainState;
        }

    }

    //===========================================
    // *** ���������� ***
    //===========================================

    public void Init()
    {
        // hp��������
        hp = init_boss_hp;

        // ���W��������
        this.transform.position = init_boss_pos;
        transform.Find("LeftHand").gameObject.transform.position = init_lefthand_pos;
        transform.Find("RightHand").gameObject.transform.position = init_right_pos;

        // ���݂̏�Ԃ��ړ���Ԃɏ�����
        nowMainState = MainStateID.MOVE;

        // �p�x��������
        angle = 0.0f;
    }
}