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

    PlayerInputManager.DIRECTION oldDire; // �O�t���[���̌��������Ă������߂̕ϐ�

    // �O���擾
    private GameObject PlayerInputMana; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform thisTransform; // ���g��Transform���擾����ϐ�

    public LayerMask BlockLayer;

    private Animator anim; // �A�j���[�^�[���擾���邽�߂̕ϐ�

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

        oldDire = ScriptPIManager.Direction;

        //----------------------------------------------------------------------------------------------------------
        // ���g(player)�̎���Transform���擾����
        thisTransform = this.GetComponent<Transform>();

        // �A�j���[�^�[�擾
        anim = GetComponent<Animator>();
    }

    //----------------------------------------------------------------------------------------------------------
    // - �X�V���� -
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // ���ʂ̈ړ�
        //----------------------------------------------------------------------------------------------------------
        // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
        movement = ScriptPIManager.GetMovement();

        //----------------------------------------------------------------------------------------------------------
        // �v���C���[��Transform�Ɉړ��ʂ�K������
        // �X�e�B�b�N�ŏ���͂���Ə���������肪���邽�߁AY,Z�ɂ͒��ڒl�����ďC��
        thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);

        //-----------------------------------------------------------------
        // �A�j���[�V�����֌W
        // movement��x�̒l�ɂ����walk��run�ɂȂ�
        anim.SetBool("walk", (movement.x > 0.0f && movement.x < 0.5f) || (movement.x < 0.0f && movement.x > -0.5f)); // �X�e�B�b�N���͂̍��E�����܂łȂ����
        anim.SetBool("run", (movement.x >= 0.5f && movement.x <= 1.0f) || (movement.x <= -0.5f && movement.x >= -1.0f)); // �X�e�B�b�N���͂̍��E�����ȏ�Ȃ瑖��

        if (oldDire != ScriptPIManager.Direction)
        {
            // �v���C���[�̌��������Ƌt�ɂ���
            thisTransform.localScale = new Vector3(-thisTransform.localScale.x, thisTransform.localScale.y, thisTransform.localScale.z);
        }

        // �O�t���[���̌����Ƃ��ĕۑ�
        oldDire = ScriptPIManager.Direction;
    }
}
