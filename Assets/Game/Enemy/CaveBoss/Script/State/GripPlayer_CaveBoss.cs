//=========================================
// �S���F�����V�S
// ���e�F���A�̃{�X�̃v���C���[��͂ލU��
//=========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripPlayer_CaveBoss : MonoBehaviour
{
    //=====================================
    // *** �ϐ��錾 ***
    //=====================================

    //-------------------------------------
    // *** �C���X�^���X ***

    public static GripPlayer_CaveBoss instance;// ���̃N���X�̃C���X�^���X

    //-------------------------------------
    // *** ��Ԋ֘A ***

    enum GripPlayerStateID // ���ID
    {
        NULL,          // ��ԂȂ�
        ACCESS,        // �T�����
        MOVE,          // �ړ����
        APPEAR,        // �o�����
        ATTACK,        // �U�����
        GRIP,          // �߂܂������
        RETURN,        // �߂���
        END            // �s���I��
    }
    GripPlayerStateID oldGripPlayerState = GripPlayerStateID.NULL;  // �O�̏��
    GripPlayerStateID nowGripPlayerState = GripPlayerStateID.ACCESS;// ���݂̏��
    GripPlayerStateID nextGripPlayerState = GripPlayerStateID.NULL; // ���̏��

    //-------------------------------------
    // *** �U���֘A ***

    [Header("�E��")]
    public GameObject rightHand;// �E��
    [Header("����")]
    public GameObject leftHand; // ����
    [Header("�߂܂��鎞��")]
    public int gripTim = 1000;     // �S������
    [Header("�S���_���[�W���󂯂�܂ł̎���")]
    public int gripDamageTim = 500;// �S���_���[�W���󂯂�܂ł̎���

    int gripTimCnt;             // �S�����Ԃ��J�E���g
    Vector2 playerGripPos;      // �v���C���[���S��������W

    //-------------------------------------
    // *** �ړ��֘A ***

    [Header("��̈ړ����x")]
    public float moveSpeed = 0.01f;// ��̈ړ����x

    Vector2 startPos_R;            // �E��̏����ʒu
    Vector2 startPos_L;            // ����̏����ʒu

    // �傫��
    Vector3 startScale;

    // ��]
    Vector2 center_R;      // ��]�̒��S���W
    Vector2 center_L;      // ��]�̒��S���W
    float angle;         // ��]�p�x
    float radius = 0.25f;// �~�̔��a

    //-------------------------------------
    // �o�����

    [Header("�o�����x")]
    public float appearSpeed = 0.01f;

    //-------------------------------------
    // *** �O���I�u�W�F�N�g ***

    GameObject objPlayer;      // �v���C���[
    private HitEnemy _hitEnemy;// ���G���Ԋ֌W�X�N���v�g

    Animator animLeft; // ����̃A�j���[�^�[
    Animator animRight;// �E��̃A�j���[�^�[

    //-------------------------------------
    // �}�e���A���֘A 

    // �F
    SpriteRenderer sr_boss;     // �{�X�̐F
    SpriteRenderer sr_lefthand; // ����̐F
    SpriteRenderer sr_righthand;// �E��̐F

    float alpha = 0.0f;// �����x

    //=====================================
    // *** ���������� ***
    //=====================================

    void Start()
    {

        // �F���擾
        sr_boss = GetComponent<SpriteRenderer>();
        sr_lefthand = GameObject.Find("LeftHand").GetComponent<SpriteRenderer>();
        sr_righthand = GameObject.Find("RightHand").GetComponent<SpriteRenderer>();


        animLeft = GameObject.Find("LeftHand").GetComponent<Animator>();
        animRight = GameObject.Find("RightHand").GetComponent<Animator>();

        //--------------------------------
        // *** �ϐ��̏����� ***

        // ���̃N���X�̃C���X�^���X�𐶐�
        if (instance == null)
        {
            instance = this;
        }

        // �v���C���[�̃I�u�W�F�N�g���擾
        objPlayer = GameObject.Find("player");
        _hitEnemy = objPlayer.GetComponent<HitEnemy>();

        startScale = rightHand.transform.localScale;
    }

    //=====================================
    // *** �v���C���[��͂ޏ��� ***
    //
    // �����@�F����
    // �߂�l�F�U�����I���������itrue�F�I���Afalse�F�U�����j
    //=====================================

    public bool GripPlayer()
    {
        //---------------------------------------
        // *** ���݂̏�Ԃɂ���ď����𕪊� ***

        if (nextGripPlayerState != GripPlayerStateID.NULL)
        {
            oldGripPlayerState = nowGripPlayerState;
            nowGripPlayerState = nextGripPlayerState;
            nextGripPlayerState = GripPlayerStateID.NULL;
        }

        switch (nowGripPlayerState)
        {

            // ���G���
            case GripPlayerStateID.ACCESS:
                Access();
                break;
            // �o�����
            case GripPlayerStateID.APPEAR:
                Appear();
                break;
            // �ړ����
            case GripPlayerStateID.MOVE:
                Move();
                break;
            // �U�����
            case GripPlayerStateID.ATTACK:
                Attack();
                break;
            // �߂܂������
            case GripPlayerStateID.GRIP:
                Grip();
                break;
            // �߂���
            case GripPlayerStateID.RETURN:
                Return();
                break;
            // �I�����
            case GripPlayerStateID.END:
                return End();
                break;
        }

        return false;
    }

    //=====================================
    // *** ���G���� ***
    //=====================================

    void Access()
    {
        //-----------------------------------------
        // *** �ړ��͈͓������E�ɔ������Ĉړ� ***

        Move_CaveBoss.instance.Move();

        //-----------------------------------------
        // *** Player���^���ɋ��邩��Ray�Ŕ��� ***

        foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(transform.position, Vector2.down))
        {
            if (hit_view)
            {
                if (hit_view.collider.gameObject.CompareTag("Player"))
                {
                    // ��̏����ʒu��ۑ�
                    startPos_R = rightHand.transform.position;
                    startPos_L = leftHand.transform.position;

                    // ����ړ���ԂɕύX
                    nextGripPlayerState = GripPlayerStateID.APPEAR;
                }
            }
        }
    }

    //=====================================
    // *** �o����� ***
    //=====================================

    void Appear()
    {
        // �����x�����Z
        alpha += appearSpeed;

        // �����x��G�p
        sr_boss.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        sr_lefthand.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        sr_righthand.color = new Color(1.0f, 1.0f, 1.0f, alpha);

        // ���S�ɏo��������U����ԂɑJ��
        if (alpha >= 1.0f)
        {
            nextGripPlayerState = GripPlayerStateID.MOVE;
        }
    }

    //=====================================
    // *** �ړ����� ***
    //=====================================

    void Move()
    {
        // �傫���̎w��
        Vector3 scale = rightHand.transform.localScale;
        scale.x += 0.001f;
        scale.y += 0.001f;
        rightHand.transform.localScale = scale;
        leftHand.transform.localScale = scale;

        //--------------------------------------------------
        // *** �v���C���[�̍��W�܂ňړ� ***

        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        position_R.x += moveSpeed * 0.5f;
        position_L.x -= moveSpeed * 0.5f;
        position_R.y -= moveSpeed * 1.5f;
        position_L.y -= moveSpeed * 1.5f;

        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;

        //--------------------------------------------------
        // *** �v���C���[�̍��W�ɒ�������U����Ԃɂ��� ***

        if (objPlayer.transform.position.y > position_R.y)
        {
            nextGripPlayerState = GripPlayerStateID.ATTACK;
        }
    }

    //=====================================
    // *** �U������ ***
    //=====================================

    void Attack()
    {
        // �傫���̎w��
        Vector3 scale = rightHand.transform.localScale;
        scale.x += 0.001f;
        scale.y += 0.001f;

        rightHand.transform.localScale = scale;
        leftHand.transform.localScale = scale;

        //--------------------------------------------------
        // *** ����v���C���[�����ނ悤�Ɉړ����� ***

        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        position_R.x -= moveSpeed * 2.0f;
        position_L.x += moveSpeed * 2.0f;

        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;

        //--------------------------------------------------
        // *** �v���C���[��߂܂�������Ray�Ŕ��� ***

        if ((objPlayer.transform.position.x < position_L.x + 0.0f) || (objPlayer.transform.position.x > position_R.x + 0.0f))
        {

            nextGripPlayerState = GripPlayerStateID.RETURN;

            // ����
            foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(leftHand.transform.position, Vector2.right, 2.0f))
            {
             
                if (hit_view)
                {
                    //---------------------------------------------
                    // �߂܂�����GRIP��Ԃɂ���

                    if (hit_view.collider.gameObject.CompareTag("Player"))
                    {
                        nextGripPlayerState = GripPlayerStateID.GRIP;
                    }
                }
               
            }

            // �E��
            foreach (RaycastHit2D hit_R in Physics2D.RaycastAll(rightHand.transform.position, Vector2.left, 2.0f))
            {

                if (hit_R)
                {
                    //---------------------------------------------
                    // �߂܂�����GRIP��Ԃɂ���

                    if (hit_R.collider.gameObject.CompareTag("Player"))
                    {
                        nextGripPlayerState = GripPlayerStateID.GRIP;
                    }
                }

            }
        }
            
    }

    //=====================================
    // *** �߂܂��鏈�� ***
    //=====================================

    void Grip()
    {
        //---------------------------------
        // *** �v���C���[��߂܂��� ***

        // �߂܂��Ă��鎞�Ԃ��J�E���g
        gripTimCnt++;

        // �߂܂������W��ۑ�
        if (gripTimCnt == 1)
        {
            animLeft.SetTrigger("Trigger");
            animRight.SetTrigger("Trigger");

            center_L = leftHand.transform.position;
            center_R = rightHand.transform.position;

            playerGripPos = objPlayer.transform.position;
        }

        //---------------------------------------------------
        //  �㉺�ɂӂ�ӂ킳����
        //---------------------------------------------------

        // ���݂̃g�����X�t�H�[�����擾
        Vector3 pos = rightHand.transform.position;
        // �p�x�����W�A���ɕϊ�
        float rd = -angle * Mathf.PI / 180.0f;
        // ��]��̍��W���v�Z
        //pos.x = center.x + (Mathf.Sin(rd) * radius) + radius + 0.1f;
        pos.y = center_R.y + (Mathf.Cos(rd) * radius) + radius + 0.1f;
        // �ύX�𔽉f
        rightHand.transform.position = pos;
        pos = leftHand.transform.position;
        // �p�x�����W�A���ɕϊ�
        rd = -angle * Mathf.PI / 180.0f;
        // ��]��̍��W���v�Z
        //pos.x = center.x + (Mathf.Sin(rd) * radius) + radius + 0.1f;
        pos.y = center_L.y + (Mathf.Cos(rd) * radius) + radius + 0.1f;
        // �ύX�𔽉f
        leftHand.transform.position = pos;
        // �p�x�����Z
        angle += 0.2f;

        // �ړ��X�e�b�N����������_���[�W���󂯂�܂ł̎��Ԃ�����
        if (playerGripPos.x != objPlayer.transform.position.x)
        {
            //gripTimCnt++;
            gripDamageTim++;
        }

        // �_���[�W���󂯂�܂ł̎��Ԃ��߂�����_���[�W��^����
        if (gripDamageTim < gripTimCnt)
        {
            // �G�ƃv���C���[���ڐG�����Ƃ��̏����֐��Ăяo��
            _hitEnemy.HitPlayer(objPlayer.transform);
        }

        // ���Ԃ��o������RETURN��Ԃɂ���
        if (gripTimCnt > gripTim)
        {
            nextGripPlayerState = GripPlayerStateID.RETURN;
            gripTimCnt = 0;
            
            animRight.SetTrigger("ResetTrigger");
            animLeft.SetTrigger("ResetTrigger");
        }

        // �v���C���[��ۑ��������W�Ɉړ�
        objPlayer.transform.position = playerGripPos;
    }

    //=====================================
    // *** ���̈ʒu�ɖ߂鏈�� ***
    //=====================================

    void Return()
    {
       
        if(startScale.x < rightHand.transform.localScale.x)
        {
            // �傫���̎w��
            Vector3 scale = rightHand.transform.localScale;
            scale.x -= 0.0015f;
            scale.y -= 0.0015f;

            rightHand.transform.localScale = scale;
            leftHand.transform.localScale = scale;
        }

        //---------------------------------------
        // *** ������̍��W�Ɉړ� ***

        // ��̍��W���擾
        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        // �E��
        if (position_R.x > startPos_R.x)
        {
            position_R.x -= moveSpeed;
        }
        if (position_R.x < startPos_R.x)
        {
            position_R.x += moveSpeed;
        }
        if (position_R.y < startPos_R.y)
        {
            position_R.y += moveSpeed;
        }

        // ����
        if (position_L.x > startPos_L.x)
        {
            position_L.x -= moveSpeed;
        }
        if (position_L.x < startPos_L.x)
        {
            position_L.x += moveSpeed;
        }
        if (position_L.y < startPos_L.y)
        {
            position_L.y += moveSpeed;
        }

        //---------------------------------------
        // *** �ړ����I�������END��Ԃɂ��� ***

        else
        {
            nextGripPlayerState = GripPlayerStateID.END;

            
        }

        // ���W�̕ύX��K�p
        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;
    }

    //=====================================
    // *** �I������ ***
    //=====================================

    bool End()
    {
        // �����x�����Z
        alpha -= appearSpeed;

        // �����x��G�p
        sr_boss.color = new Color(1.0f, 1.0f, 1.0f,alpha);
        sr_lefthand.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        sr_righthand.color = new Color(1.0f, 1.0f, 1.0f, alpha);

        // ���S�ɏ�������U����ԂɑJ��
        if (alpha <= 0.0f)
        {
            nextGripPlayerState = GripPlayerStateID.ACCESS;
            return true;
        }

        return false;
    }
}
