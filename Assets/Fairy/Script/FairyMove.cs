//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�d���̈ړ��i�R���g���[���[�j�A�v���C���[�𒆐S�Ƃ������ar�̉~������ړ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    private string GroundTag = "Ground";

    [Header("�v���C���[�Ƃ̋���")]
    public float Speed = 5.0f; // �v���C���[�Ƃ̋���
    private Vector2 movement; // ���͗ʂ��擾����ϐ�
    public float Radius = 3.0f; // �v���C���[�Ɨ�����鋗��
    private float Distance; // �v���C���[�Ɨd���̋��������ϐ�

    // �O���擾
    private GameObject PlayerInputManager; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform thisTransform; // ���g��Transform���擾����ϐ�
    private GameObject player; // ���g��Transform���擾����ϐ�
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManager��T��
        PlayerInputManager = GameObject.Find("PlayerInputManager");
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

        //----------------------------------------------------------------------------------------------------------
        // ���g(�d��)�̎���Transform���擾����
        thisTransform = this.GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // �v���C���[�T��
        player = GameObject.Find("player");

        playerTransform = player.GetComponent<Transform>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        Vector3 vector_FairyPlayer = playerTransform.position - thisTransform.position;

        // �d������v���C���[�̋���
        Distance = vector_FairyPlayer.magnitude;

        //----------------------------------------------------------------------------------------------------------
        // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
        movement = ScriptPIManager.GetMovement();

        if (Distance <= Radius)
        {
            //----------------------------------------------------------------------------------------------------------
            // �v���C���[�̍��W����ɗd���̈ʒu���v�Z
            thisTransform.Translate(movement.x * Speed * Time.deltaTime, movement.y * Speed * Time.deltaTime, 0.0f);
        }
        else
        {
            thisTransform.Translate(
                vector_FairyPlayer.normalized.x * Speed * Time.deltaTime,
                vector_FairyPlayer.normalized.y * Speed * Time.deltaTime,
                0.0f);
        }
    }
}
