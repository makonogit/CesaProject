//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�d���̈ړ��i�R���g���[���[�j�A�v���C���[�𒆐S�Ƃ������ar�̉~������ړ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailTargetMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    private string GroundTag = "Ground";

    [Header("�v���C���[�Ƃ̋���")]
    public float Speed = 5.0f; // �v���C���[�Ƃ̋���
    private Vector2 movement; // ���͗ʂ��擾����ϐ�
    public float Radius = 3.0f; // �v���C���[�Ɨ�����鋗��
    private float Distance; // �v���C���[�Ɨd���̋��������ϐ�
    private bool OldActive = false; // �O�t���[���̃A�N�e�B�u���
    [Header("�v���C���[�Ƃ̍�X")]
    public float AdjustX = 2.0f; // �A�N�e�B�u���̃v���C���[�Ƃ̍��W��X
    [Header("�v���C���[�Ƃ̍�Y")]
    public float AdjustY = 1.0f; // �A�N�e�B�u���̃v���C���[�Ƃ̍��W��Y

    // �O���擾
    private GameObject PlayerInputMana; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform thisTransform; // ���g��Transform���擾����ϐ�
    private GameObject player; // ���g��Transform���擾����ϐ�
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManager��T��
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

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
        // �Ə����[�h�̎��\���A�ړ�

        // �ŏ��̃t���[���̂ݓ���
        if (OldActive == false)
        {
            //// �o���ʒu�Œ�
            //thisTransform.position = new Vector3(
            //    playerTransform.position.x + AdjustX,
            //    playerTransform.position.y + AdjustY,
            //    playerTransform.position.z);
            // �Ə��\��
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        //----------------------------------------------------------------------------------------------------------
        Vector3 vector_FairyPlayer = playerTransform.position - thisTransform.position;

        // �d������v���C���[�̋���
        Distance = vector_FairyPlayer.magnitude;

        //----------------------------------------------------------------------------------------------------------
        // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
        movement = ScriptPIManager.GetRmove();

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

        if (OldActive == false)
        {
            //OldActive = true;
        }
        if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.AIM)
        {
           
        }
        else
        {
            // ���[�h���ς���čŏ��̃t���[���̎��ɓ���
            if (OldActive == true)
            {
                // ��\��
               // this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }

            if (OldActive == true)
            {
                OldActive = false;
            }
        }

        //Debug.Log(OldActive);
    }
}
