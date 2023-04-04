//-----------------------------------------
// �S���F�����V�S
// ���e�F���A�̃{�X
//-----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveBoss : MonoBehaviour
{
    //=====================================
    // �ϐ��錾

    // Ray�֘A
    [SerializeField] private LayerMask rayLayer;// Ray�̃����_�[

    // ��Ԋ֘A
    enum StateID// ���ID
    {
        NULL,          // ��ԂȂ�
        MOVE,          // �ړ����
        ATTACK_STANDBY,// �U���ҋ@���
        ATTACK,        // �U�����
    }
    StateID oldState = StateID.NULL; // �O�̏��
    StateID nowState = StateID.MOVE; // ���݂̏��
    StateID nextState = StateID.NULL;// ���̏��

    // �ړ��֘A
    [Header("�ړ��͈�")]
    public float moveRange = 15.0f;// �ړ��͈�
    [Header("�ړ����x")]
    public float moveSpeed = 0.01f;// �ړ����x
    Vector2 moveVector;            // �ړ�����
    Vector2 startPos;              // �����ʒu

    // �U���֘A
    [Header("�U������܂ł̑ҋ@����")]
    public int attackDelay = 500; // �U������܂ł̑ҋ@����
    [Header("�~�点��G")]
    public GameObject needleEnemy;// �~�点��G
    int frameCount = 0;           // �t���[���J�E���g

    // �A�j���[�V�����֘A
    [Header("�A�j���[�V�����R���g���[���[")]
    public Animator animator;// �A�j���[�V�����R���g���[���[
    float animSpeed = 1.0f;  // �A�j���[�V�����̑���

    //=====================================
    // ����������

    void Start()
    {
        //--------------------------------
        // �ϐ��̏�����

        // �����ʒu��ۑ�
        startPos = transform.position;

        // �ړ�������������
        moveVector.x = 1.0f;

    }

    //=====================================
    // �X�V����

    void Update()
    {
        //---------------------------------
        // ���݂̏�Ԃɂ���ď����𕪊�

        if (nextState != StateID.NULL)
        {
            oldState = nowState;
            nowState = nextState;
            nextState = StateID.NULL;
        }

        switch (nowState)
        {
            // �ړ����
            case StateID.MOVE:
                Move();
                break;
            // �U���ҋ@���
            case StateID.ATTACK_STANDBY:
                AttackStandby();
                break;
            // �U�����
            case StateID.ATTACK:
                Attack();
                break;
        }

        //---------------------------------
        // �A�j���[�V�����R���g���[���[�̒l�𐧌�

        //animator.SetFloat("Horizontal", moveVector.x);// ��
        //animator.SetFloat("Vertical", moveVector.y);  // �c
        animator.SetFloat("Speed", animSpeed);        // �Đ����x
    }

    //=====================================
    // �ړ�����

    void Move()
    {
        //---------------------------------
        // �ړ��͈͓������E�ɔ������Ĉړ�

        Vector2 position = transform.position;

        position.x += moveSpeed * moveVector.x;

        transform.position = position;

        if (position.x > startPos.x + moveRange)
        {
            moveVector.x = -1.0f;
        }

        if (position.x < startPos.x - moveRange)
        {
            moveVector.x = 1.0f;
        }

        //---------------------------------
        // �ǂɏՓ˂��Ȃ��悤��Ray�Ŕ���

        // �E��
        foreach (RaycastHit2D hit_R in Physics2D.RaycastAll(transform.position, Vector2.right, transform.localScale.x * 0.2f))
        {
            if (hit_R)
            {
                if (hit_R.collider.gameObject.CompareTag("Ground"))
                {
                    moveVector.x = -1.0f;
                }
            }
        }

        // ����
        foreach (RaycastHit2D hit_L in Physics2D.RaycastAll(transform.position, Vector2.left, transform.localScale.x * 0.2f))
        {
            if (hit_L)
            {
                if (hit_L.collider.gameObject.CompareTag("Ground"))
                {
                    moveVector.x = 1.0f;
                }
            }
        }

        //---------------------------------
        // Player������ɋ��邩��Ray�Ŕ���

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, Vector2.down))
        {
            if (hit_view)
            {
                if (hit_view.collider.gameObject.CompareTag("Player"))
                {
                    //nextState = StateID.ATTACK_STANDBY;

                }
            }
        }

        //---------------------------------
        // �����_���ɍs��������

        frameCount++;

        if (frameCount == 500)
        {
            int rnd = Random.Range(1, 100 + 1);

            if (rnd >= 70)
            {
                nextState = StateID.ATTACK_STANDBY;
            }

            frameCount = 0;
        }
    }

    //=====================================
    // �U���ҋ@����

    void AttackStandby()
    {
        //--------------------------------------------
        // �U���ҋ@���Ԃ��J�E���g

        frameCount++;

        //--------------------------------------------
        // �ҋ@���Ԃ��I�������U����ԂɕύX

        if (attackDelay == frameCount)
        {
            frameCount = 0;
            nextState = StateID.ATTACK;
        }
    }

    //=====================================
    // �U������

    void Attack()
    {
        //---------------------------------
        // �G���~�点��

        for (int i = 0; i < 3; i++)
        {
            // �G�𐶐�
            GameObject obj = Instantiate(needleEnemy, transform.position, Quaternion.identity);

            int rndX = Random.Range(1, 20);

            // ���W��ύX
            Transform objTransform = obj.transform;
            Vector3 pos = objTransform.position;
            pos.x = startPos.x + - 8.0f + 0.8f * rndX;
            pos.y = transform.position.y;

            // �ύX��G�p����
            objTransform.position = pos;    // ���W
        }

        nextState = StateID.MOVE;
    }
}
