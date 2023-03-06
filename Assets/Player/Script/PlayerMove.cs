//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�̈ړ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    // �ړ��p
    public float Speed = 5f; // �ړ����x�p�ϐ�
    Vector2 movement; // ���͗ʂ��擾����ϐ�

    // ���[�h
    public enum PLAYERMODE
    {
        FOLLOW, // �Ǐ]���[�h
        STOP // ��~���[�h
    }

    [System.NonSerialized]
    public PLAYERMODE mode = PLAYERMODE.FOLLOW; // �v���C���[�̈ړ����̃��[�h�i�Ǐ]�A��~�j
    private float LeaveTime; // ��苗������Ă���̌o�ߎ���
    //[Header("�d���ƃv���C���[���ۂ���")]
    [Header("�v���C���[���Ǐ]�ł��鋗��")]
    public float moveDistance = 3.0f; // �d���ƃv���C���[���ۂ���
    [Header("�v���C���[�������n�߂�܂ł̎���")]
    public float delayTime = 0.8f; // �����n�߂�܂ł̎���
    [Header("�v���C���[�Ɨd���̃\�[�V�����f�B�X�^���X")]
    public float socialDistance = 0.3f; // �v���C���[�Ɨd�����d�Ȃ�Ȃ��悤�ɂ���

    // �O���擾
    private GameObject PlayerInputMana; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform thisTransform; // ���g��Transform���擾����ϐ�

    private GameObject Fairy; // �Q�[���I�u�W�F�N�g�d����ێ�����
    private Transform fairyTransform; // �d���̍��W

    public LayerMask BlockLayer;

    //----------------------------------------------------------------------------------------------------------
    // - ���������� -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManager��T��
        PlayerInputMana = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        //----------------------------------------------------------------------------------------------------------
        // ���g(player)�̎���Transform���擾����
        thisTransform = this.GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // �d���T��
        Fairy = GameObject.Find("NailTarget");

        // �d����Transform���
        fairyTransform = Fairy.transform;
    }

    //----------------------------------------------------------------------------------------------------------
    // - �X�V���� -
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
        //movement = ScriptPIManager.GetMovement();

        //----------------------------------------------------------------------------------------------------------
        // �v���C���[��Transform�Ɉړ��ʂ�K������
        // �X�e�B�b�N�ŏ���͂���Ə���������肪���邽�߁AY,Z�ɂ͒��ڒl�����ďC��
        //thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);

        //Vector3 origin = new Vector3(transform.position.x + 0.5f,transform.position.y,transform.position.z);
        //Vector3 Distance = Vector3.right * 10.0f;

        //RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, 10.0f,BlockLayer);

        //Debug.DrawRay(origin, Vector3.right,Color.red);

        //if (hit)
        //{
        //    Debug.Log(hit.collider);
        //}

        //----------------------------------------------------------------------------------------------------------
        // ���ʂ̈ړ�
        //----------------------------------------------------------------------------------------------------------
        // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
        movement = ScriptPIManager.GetMovement();

        //----------------------------------------------------------------------------------------------------------
        // �v���C���[��Transform�Ɉړ��ʂ�K������
        // �X�e�B�b�N�ŏ���͂���Ə���������肪���邽�߁AY,Z�ɂ͒��ڒl�����ďC��
        thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);

        // �ړ����[�h�Ȃ�
        if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.MOVE)
        {
            


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //�d���ɒǏ]���鎞�̏���
            ////----------------------------------------------------------------------------------------------------------
            //// "�v���C���[�Ɨd���̋��������ȏ�"����"����������Ă����莞�Ԍo�߂���"�Ȃ�Ǐ]

            //// �v���C���[����d���ւ̃x�N�g�������߂�
            //Vector3 vector_PlayerFairy = fairyTransform.position - thisTransform.position;

            //// x�����̋��������K�v
            //vector_PlayerFairy.y = 0.0f;

            //// �d���ƃv���C���[�̋��������߂�
            //float Distance = vector_PlayerFairy.magnitude;

            //// �v���C���[�Ɨd���̋�������苗���ȏ�Ȃ�
            //if(Distance >= socialDistance && Distance <= moveDistance)  // ��苗���ȏ��苗�����ɂ���Ƃ��ɒǏ]
            ////if(Distance >= moveDistance) ��苗���ȏ�ɂ���Ƃ��ɒǏ]
            //{
            //    // ��莞�Ԍo�߂����瓮���n�߂�
            //    if (LeaveTime >= delayTime)
            //    {
            //        //----------------------------------------------------------------------------------------------------------
            //        // �v���C���[�̈ړ�
            //        thisTransform.Translate(vector_PlayerFairy.normalized.x * Speed * Time.deltaTime, 0.0f, 0.0f);
            //    }

            //    // ���ԉ��Z
            //    LeaveTime += Time.deltaTime;
            //}
            //else
            //{
            //    // ������
            //    LeaveTime = 0.0f;
            //}

            //// �Ǐ]���[�h���ɉE�N���b�N���ꂽ���~���[�h�ɂ���
            //if (Mouse.current.rightButton.wasPressedThisFrame)
            //{
            //    mode = PLAYERMODE.STOP;
            //}
        }
        // ��~���[�h�Ȃ�
        else if(ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.AIM)
        {
            // �ړ��͂��Ȃ�

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //�d���ɒǏ]���鎞�̏���
            //// ��~���[�h���ɉE�N���b�N���ꂽ��Ǐ]���[�h�ɂ���
            //if (Mouse.current.rightButton.wasPressedThisFrame)
            //{
            //    mode = PLAYERMODE.FOLLOW;

            //    // ������
            //    LeaveTime = 0.0f;
            //}
        }
    }
}
