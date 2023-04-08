//=========================================
// �S���F�����V�S
// ���e�F���A�̃{�X�̈ړ�����
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_CaveBoss: MonoBehaviour
{
    //=====================================
    // *** �ϐ��錾 ***
    //=====================================

    //-------------------------------------
    // *** �C���X�^���X ***

    public static Move_CaveBoss instance;// ���̃N���X�̃C���X�^���X

    //-------------------------------------
    // *** �ړ��֘A ***

    [Header("�ړ��͈�")]
    public float moveRange = 15.0f;// �ړ��͈�
    [Header("�ړ����x")]
    public float moveSpeed = 0.01f;// �ړ����x
    Vector2 moveVector;            // �ړ�����
    Vector2 startPos;              // �����ʒu

    //-------------------------------------
    // *** �A�j���[�V�����֘A ***

    [Header("�A�j���[�V�����R���g���[���[")]
    public Animator animator;// �A�j���[�V�����R���g���[���[
    float animSpeed = 1.0f;  // �A�j���[�V�����̑���

    //=====================================
    // *** ���������� ***
    //=====================================

    void Start()
    {
        //--------------------------------
        // *** �ϐ��̏����� ***

        // �����ʒu��ۑ�
        startPos = transform.position;

        // �ړ�������������
        moveVector.x = 1.0f;
        moveVector.y = 1.0f;

        // ���̃N���X�̃C���X�^���X�𐶐�
        if (instance == null)
        {
            instance = this;
        }
    }

    //=====================================
    // *** �X�V���� ***
    //=====================================

    void Update()
    {
        //----------------------------------------------------------
        // *** �A�j���[�V�����R���g���[���[�̒l�𐧌� ***

        //animator.SetFloat("Horizontal", moveVector.x);// ��
        //animator.SetFloat("Vertical", moveVector.y);  // �c
        animator.SetFloat("Speed", animSpeed);          // �Đ����x
    }

    //=====================================
    // *** �ړ����� ***
    //=====================================

    public void Move()
    {
        //-----------------------------------------
        // *** �ړ��͈͓������E�ɔ������Ĉړ� ***

        Vector2 position = transform.position;

        position.x += moveSpeed * moveVector.x;
        position.y += moveSpeed * moveVector.y;

        transform.position = position;

        // �E���̈ړ�����
        if (position.x > startPos.x + moveRange)
        {
            moveVector.x = -1.0f;
        }
        // �����̈ړ�����
        if (position.x < startPos.x - moveRange)
        {
            moveVector.x = 1.0f;
        }
        // �㑤�̈ړ�����
        if (position.y > startPos.y + 0.5f)
        {
            moveVector.y = -0.5f;
        }
        // �����̈ړ�����
        if (position.y < startPos.y - 0.5f)
        {
            moveVector.y = 0.5f;
        }

        //---------------------------------------
        // *** �ǂɏՓ˂��Ȃ��悤��Ray�Ŕ��� ***

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
    }
}
