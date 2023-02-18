//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�v���C���[�̈ړ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    // �ړ��p
    public float Speed = 5f; // �ړ����x�p�ϐ�
    Vector2 movement; // ���͗ʂ��擾����ϐ�

    // �O���擾
    public GameObject PlayerInputManager; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform thisTransform; // ���g��Transform���擾����ϐ�

    public LayerMask BlockLayer;

    //----------------------------------------------------------------------------------------------------------
    // - ���������� -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

        //----------------------------------------------------------------------------------------------------------
        // ���g(player)�̎���Transform���擾����
        thisTransform = this.GetComponent<Transform>();
    }

    //----------------------------------------------------------------------------------------------------------
    // - �X�V���� -
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
        movement = ScriptPIManager.GetMovement();

        //----------------------------------------------------------------------------------------------------------
        // �v���C���[��Transform�Ɉړ��ʂ�K������
        // �X�e�B�b�N�ŏ���͂���Ə���������肪���邽�߁AY,Z�ɂ͒��ڒl�����ďC��
        thisTransform.Translate(movement.x * Speed * Time.deltaTime, 0.0f, 0.0f);

        //Vector3 origin = new Vector3(transform.position.x + 0.5f,transform.position.y,transform.position.z);
        //Vector3 Distance = Vector3.right * 10.0f;

        //RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, 10.0f,BlockLayer);

        //Debug.DrawRay(origin, Vector3.right,Color.red);

        //if (hit)
        //{
        //    Debug.Log(hit.collider);
        //}
        

    }
}
