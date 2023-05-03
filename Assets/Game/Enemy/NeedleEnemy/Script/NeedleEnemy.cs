//-----------------------------------------
// �S���F�����V�S
// ���e�F����L�΂��G
//-----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleEnemy : MonoBehaviour
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
        DEATH,         // �퓬�s�\���
    }
    StateID oldState = StateID.NULL; // �O�̏��
    StateID nowState = StateID.NULL; // ���݂̏��
    StateID nextState = StateID.NULL;// ���̏��

    // �ړ��֘A
    [Header("�ړ��͈�")]
    public float moveRange = 3.0f; // �ړ��͈�
    [Header("�ړ����x")]
    public float moveSpeed = 0.01f;// �ړ����x
    Vector2 moveVector;            // �ړ�����
    Vector2 startPos;              // �����ʒu

    // �U���֘A
    [Header("�U������܂ł̑ҋ@����")]
    public int attackDelay = 500;// �U������܂ł̑ҋ@����
    [Header("�U�����Ă��鎞��")]
    public int attackTime = 500; // �U�����Ă��鎞��
    int frameCount = 0;          // �t���[���J�E���g

    // �A�j���[�V�����֘A
    [Header("�A�j���[�V�����R���g���[���[")]
    public Animator animator;// �A�j���[�V�����R���g���[���[
    float animSpeed = 1.0f;  // �A�j���[�V�����̑���

    // �}�e���A���֘A
    SpriteRenderer sr;// �F

    //=====================================
    // ����������

    void Start()
    {
        //--------------------------------
        // �ϐ��̏�����

        // �X�^�[�g���̏�Ԃ�ݒ�
        nextState = StateID.MOVE;

        // �F���擾
        sr = GetComponent<SpriteRenderer>();

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
            // �퓬�s�\���
            case StateID.DEATH:
                Death();
                break;

        }

        //---------------------------------
        // �A�j���[�V�����R���g���[���[�̒l�𐧌�

        animator.SetFloat("Horizontal", moveVector.x);// ��
        animator.SetFloat("Vertical", moveVector.y);  // �c
        animator.SetFloat("Speed", animSpeed);        // �Đ����x
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
            nextState = StateID.DEATH;
        }

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

        if(position.x > startPos.x + moveRange)
        {
            moveVector.x = -1.0f;
        }

        if(position.x < startPos.x - moveRange)
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

        for(float x = -1.0f; x < 1.0f; x += 0.1f)
        {
            for (float y = 0.0f; y < 1.0f; y += 0.1f)
            {
                Vector2 vector = new Vector2(x,y);

                foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, vector, transform.localScale.x * 0.5f))
                {
                    if (hit_view)
                    {
                        if (hit_view.collider.gameObject.CompareTag("Player"))
                        {
                            nextState = StateID.ATTACK_STANDBY;
                            
                        }
                    }
                }
            }
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
        //--------------------------------------------
        // �U�����Ԃ��J�E���g

        frameCount++;

        //--------------------------------------------
        // �̂�傫������

        if (frameCount == 1)
        {
            Transform objTransform = transform;

            // �傫���̎w��
            Vector3 scale = objTransform.localScale;
            scale.x *= 2;
            scale.y *= 2;

            // �傫����G�p
            objTransform.localScale = scale;
        }

        //--------------------------------------------
        // �̂����̑傫���ɖ߂��āA�ړ���ԂɕύX

        if (attackTime == frameCount)
        {
            nextState = StateID.MOVE;
            frameCount = 0;

            Transform objTransform = transform;

            // �傫���̎w��
            Vector3 scale = objTransform.localScale;
            scale.x /= 2;
            scale.y /= 2;

            // �傫����G�p
            objTransform.localScale = scale;
        }
    }

    //===========================================
    // *** �퓬�s�\��Ԃ̏���
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

