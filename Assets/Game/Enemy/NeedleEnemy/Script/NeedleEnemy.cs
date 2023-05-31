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
    // *** �ϐ��錾 ***
    //=====================================

    //-------------------------------------
    // Ray�֘A
    //-------------------------------------

    [SerializeField] private LayerMask rayLayer;// Ray�̃����_�[

    //-------------------------------------
    // ��Ԋ֘A
    //-------------------------------------

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

    //-------------------------------------
    // �ړ��֘A
    //-------------------------------------

    [Header("[�ړ��֘A]")]
    [Header("�E�ړ��͈�")]
    public float moveRange = 3.0f; // �ړ��͈�
    [Header("�E�ړ����x")]
    public float moveSpeed = 0.01f;// �ړ����x
    [Header("�E�������x")]
    public float gravity = 0.02f;  // �������x
    float g = 0.02f;               // �����ɂ��ړ���
    Vector2 moveVector;            // �ړ�����
    Vector2 startPos;              // �����ʒu
    Vector2 center;                // ��]�̒��S���W
    float angle;                   // ��]�p�x
    float radius = 0.25f;          // �~�̔��a

    //-------------------------------------
    // �U���֘A
    //-------------------------------------

    [Header("[�U���֘A]")]
    [Header("�E���G�͈�")]
    public float attackRange = 6.0f;
    [Header("�E�U������܂ł̑ҋ@����")]
    public int attackDelay = 250;// �U������܂ł̑ҋ@����
    [Header("�E�U�����Ă��鎞��")]
    public int attackTime = 750; // �U�����Ă��鎞��
    int frameCount = 0;          // �t���[���J�E���g
    GameObject exclamationmark;  // �I�I�u�W�F�N�g

    // �A�j���[�V�����֘A
    [Header("[�A�j���[�V�����R���g���[���[]")]
    public Animator animator;// �A�j���[�V�����R���g���[���[
    float animSpeed = 1.0f;  // �A�j���[�V�����̑���
    
    // ���j�֘A
    [Header("[���j���G�t�F�N�g]")]
    public GameObject effect;  // �G�t�F�N�g
    SpriteRenderer sr;         // �F
    GameObject obj_hitcollider;// �v���C���[�Ƃ̓����蔻��

    // ��{�ǉ�
    private PlayEnemySound _playEnemySound;

    //=====================================
    // ����������

    void Start()
    {
        // �v���C���[�Ƃ̓����蔻����擾
        obj_hitcollider = transform.Find("HitCollider").gameObject;

        // �т�����}�[�N�̃I�u�W�F�N�g���擾
        exclamationmark = transform.Find("ExclamationMark").gameObject;

        // �X�^�[�g���̏�Ԃ�ݒ�
        nextState = StateID.MOVE;

        // �F���擾
        sr = GetComponent<SpriteRenderer>();

        // �����ʒu��ۑ�
        startPos = transform.position;

        // �ړ�������������
        moveVector.x = 1.0f;

        // �d�͂ɂ��ړ��ʂ�������
        g = gravity;

        // �Gse�擾
        _playEnemySound = GameObject.Find("EnemySE").GetComponent<PlayEnemySound>();
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

        //--------------------------
        // �d�͂ɂ�闎��
        //--------------------------

        Gravity();

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
            // ���������Ђт�CrackOrder���擾
            var order = collision.gameObject.GetComponent<CrackCreater>();

            //�������Ȃ�
            if (order != null)
            {
                if (order.State != CrackCreater.CrackCreaterState.CRAETED)
                {
                    nextState = StateID.DEATH;
                    Instantiate(effect, transform.position, Quaternion.identity);
                    // ��]�̒��S���W�ɏ����ʒu��ۑ�
                    center = transform.position;
                    obj_hitcollider.SetActive(false);

                    // ���炷
                    _playEnemySound.PlayEnemySE(PlayEnemySound.EnemySoundList.Destroy);
                    // �G����
                    Destroy(gameObject);
                }
            }
        }
    }

    //=====================================
    // �ړ�����

    void Move()
    {
        //---------------------------------
        // �ړ��͈͓������E�ɔ������Ĉړ�
        //---------------------------------

        if (g == 0)
        {

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
        }

        //---------------------------------
        // �ǂɏՓ˂��Ȃ��悤��Ray�Ŕ���
        //---------------------------------

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, moveVector, attackRange))
        {
            if (hit_view)
            {
                if (hit_view.collider.gameObject.CompareTag("Ground"))
                {
                    moveVector.x *= -1.0f;
                }
              
            }
        }

        //---------------------------------
        // �i�s�����Ƀv���C���[�����邩��Ray�Ŕ���
        //---------------------------------

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, moveVector, attackRange))
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

    //=====================================
    // *** �U���ҋ@���� ***
    //=====================================

    void AttackStandby()
    {
        //--------------------------------------------
        // �U���ҋ@���Ԃ��J�E���g
        //--------------------------------------------

        frameCount++;

        //--------------------------------------------
        // �т�����}�[�N���o��
        //--------------------------------------------

        if (attackDelay / 2 > frameCount)
        {
            exclamationmark.SetActive(true);

        }

        //--------------------------------------------
        // �ҋ@���Ԃ��I�������U����ԂɕύX
        //--------------------------------------------

        if (attackDelay == frameCount)
        {
            frameCount = 0;
            nextState = StateID.ATTACK;

            exclamationmark.SetActive(false);
        }
    }

    //=====================================
    // *** �U������ ***
    //=====================================

    void Attack()
    {
        //--------------------------------------------
        // �U�����Ԃ��J�E���g
        //--------------------------------------------

        frameCount++;

        //--------------------------------------------
        // �U���A�j���[�V�������Đ�
        //--------------------------------------------

        if (frameCount == 1)
        {
            animator.SetTrigger("AttackStart");
        }

        //--------------------------------------------
        // �U�����Ԃ��o�߂�����U���I��
        //--------------------------------------------

        if (attackTime == frameCount)
        {
            animator.SetTrigger("AttackEnd");
            nextState = StateID.MOVE;
            frameCount = 0;

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
        angle += 5.0f;

        //-------------------------------------------------------------------
        // ���X�ɓ����ɂ���B���S�ɓ����ɂȂ����炱�̃I�u�W�F�N�g��j������
        //-------------------------------------------------------------------

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - 0.005f);

        if (sr.color.a < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    //=========================================
    // *** �d�� ***
    //=========================================

    void Gravity()
    {
        Vector2 position = transform.position;
        position.y -= g;
        transform.position = position;

        g = gravity;

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, Vector2.down, 0.5f)){ 
            if (hit_view)
            {
                if (hit_view.collider.gameObject.CompareTag("Ground"))
                {
                    g = 0;
                }

            }
        }
    }

    //=====================================
    // *** ���������� ***
    //=====================================

    public void Init()
    {
        // ���W��������
        transform.position = startPos;

        // �X�^�[�g���̏�Ԃ�ݒ�
        nextState = StateID.MOVE;

        // �ړ�������������
        moveVector.x = 1.0f;

        // �d�͂ɂ��ړ��ʂ�������
        g = gravity;
    }
}

