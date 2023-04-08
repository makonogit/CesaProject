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

   
    public GameObject rightHand;// �E��
    [Header("����")]
    public GameObject leftHand; // ����
    [Header("�߂܂��鎞��")]
    public int gripTim = 1000;  // �S������

    int gripTimCnt;             // �S�����Ԃ��J�E���g
    Vector2 playerGripPos;      // �v���C���[���S��������W

    //-------------------------------------
    // *** �ړ��֘A ***

    [Header("��̈ړ����x")]
    public float moveSpeed = 0.01f;// ��̈ړ����x

    Vector2 startPos_R;            // �E��̏����ʒu
    Vector2 startPos_L;            // ����̏����ʒu

    //-------------------------------------
    // *** �O���I�u�W�F�N�g ***

    GameObject objPlayer;// �v���C���[

    //=====================================
    // *** ���������� ***
    //=====================================

    void Start()
    {
        //--------------------------------
        // *** �ϐ��̏����� ***

        // ���̃N���X�̃C���X�^���X�𐶐�
        if (instance == null)
        {
            instance = this;
        }

        // �v���C���[�̃I�u�W�F�N�g���擾
        objPlayer = GameObject.Find("player");
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
                nextGripPlayerState = GripPlayerStateID.ACCESS;
                return true;
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
                    nextGripPlayerState = GripPlayerStateID.MOVE;
                }
            }
        }
    }

    //=====================================
    // *** �ړ����� ***
    //=====================================

    void Move()
    {
        //--------------------------------------------------
        // *** �v���C���[�̍��W�܂ňړ� ***

        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        position_R.x += moveSpeed * 0.1f;
        position_L.x -= moveSpeed * 0.1f;
        position_R.y -= moveSpeed;
        position_L.y -= moveSpeed;

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
        //--------------------------------------------------
        // *** ����v���C���[�����ނ悤�Ɉړ����� ***

        Vector2 position_R = rightHand.transform.position;
        Vector2 position_L = leftHand.transform.position;

        position_R.x -= moveSpeed;
        position_L.x += moveSpeed;

        rightHand.transform.position = position_R;
        leftHand.transform.position = position_L;

        //--------------------------------------------------
        // *** �v���C���[��߂܂�������Ray�Ŕ��� ***

        if ((objPlayer.transform.position.x < position_L.x)||(objPlayer.transform.position.x > position_R.x))
        {
            foreach (RaycastHit2D hit_view in Physics2D.RaycastAll(leftHand.transform.position, Vector2.left, position_L.x - position_R.x))
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
                else
                {
                    //---------------------------------------------
                    // �߂܂����Ȃ�������RETURN��Ԃɂ���

                    nextGripPlayerState = GripPlayerStateID.RETURN;
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
            playerGripPos = objPlayer.transform.position;
        }

        // ���Ԃ��o������RETURN��Ԃɂ���
        if (gripTimCnt > gripTim)
        {
            nextGripPlayerState = GripPlayerStateID.RETURN;
            gripTimCnt = 0;
        }

        // �ړ��X�e�b�N����������J�E���g�𑁂߂�
        if(playerGripPos.x != objPlayer.transform.position.x)
        {
            gripTimCnt++;
        }

        // �v���C���[��ۑ��������W�Ɉړ�
        objPlayer.transform.position = playerGripPos;
    }

    //=====================================
    // *** ���̈ʒu�ɖ߂鏈�� ***
    //=====================================

    void Return()
    {
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
}
