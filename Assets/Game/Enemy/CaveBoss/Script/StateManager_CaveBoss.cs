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
    // *** �s������֘A ***

    enum MainStateID   // ���C�����ID
    {
        NULL,          // ��ԂȂ�
        MOVE,          // �ړ����
        ATTACK,        // �U�����
        DEATH,         // �퓬�s�\
    }
    MainStateID oldMainState = MainStateID.NULL; // �O�̏��
    MainStateID nowMainState = MainStateID.MOVE; // ���݂̏��
    MainStateID nextMainState = MainStateID.NULL;// ���̏��

    enum AttackStateID// �U�����ID
    {
        NULL,         // ��ԂȂ�
        ENEMY_DROP,   // �G���~�点��
        GRIP_PLAYER   // �v���C���[��߂܂���
    }
    AttackStateID oldAttackState = AttackStateID.NULL;  // �O�̏��
    AttackStateID nowAttackState = AttackStateID.NULL;  // ���݂̏��
    AttackStateID nextAttackState = AttackStateID.NULL; // ���̏��

    //-------------------------------------
    // *** �s������֘A ***

    int mainStateDelay;                  // �s���Ԃ̊Ԋu
    bool isEndState;                     // �s���I���t���O

    // �}�e���A���֘A
    SpriteRenderer sr;// �F

    // �X�e�[�^�X�֘A
    int hp = 3;// �̗�

    //=====================================
    // *** ������ ***
    //=====================================

    void Start()
    {
        nowMainState = MainStateID.MOVE;

        // �}�e���A���֘A
        SpriteRenderer sr;// �F
    }

    //=====================================
    // *** �X�V���� ***
    //=====================================

    void Update()
    {
        //---------------------------------------
        // *** ���݂̏�Ԃɂ���ď����𕪊� ***

        if (nextMainState != MainStateID.NULL)
        {
            oldMainState = nowMainState;
            nowMainState = nextMainState;
            nextMainState = MainStateID.NULL;
        }

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
            // �퓬�s�\
            case MainStateID.DEATH:
                Death();
                break;
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

        mainStateDelay++;

        if (mainStateDelay >= 500)
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

            mainStateDelay = 0;
        }
    }

    //============================================================
    // *** �Փ˔��� ***
    //============================================================

    void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------------
        // �ЂтƂԂ�������퓬�s�\��ԂɑJ��
        //--------------------------------------------------------

        if (collision.gameObject.tag == "Crack")
        {
            hp--;

            if(hp <= 0)
            {
                nextMainState = MainStateID.DEATH;

            }

        }

    }

    //===========================================
    // *** �퓬�s�\��Ԃ̏��� ***
    //===========================================

    void Death()
    {
        //-------------------------------------------------------------------
        // ���X�ɓ����ɂ���B���S�ɓ����ɂȂ����炱�̃I�u�W�F�N�g��j������
        //-------------------------------------------------------------------

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - 0.005f);

        if (sr.color.a < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}