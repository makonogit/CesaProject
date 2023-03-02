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
    [Header("�v���C���[�Ƃ̋���")]
    public float Distance; // �v���C���[�Ɨd���̋��������ϐ�
    private bool OldActive = false; // �O�t���[���̃A�N�e�B�u���
    [Header("�v���C���[�Ƃ̍�X")]
    public float AdjustX = 2.0f; // �A�N�e�B�u���̃v���C���[�Ƃ̍��W��X
    [Header("�v���C���[�Ƃ̍�Y")]
    public float AdjustY = 1.0f; // �A�N�e�B�u���̃v���C���[�Ƃ̍��W��Y

    private Vector3 offset; // �Ə��𓮂����ĂȂ����̃v���C���[�ƏƏ��̃x�N�g���p�ϐ�
    bool Move = true; // �Ə��������Ă��邩�����Ă��Ȃ�������
    bool touchGround = false; // �Ə���Ground�^�O�̃I�u�W�F�N�g�ƐG��Ă��邩

    // �O���擾
    private GameObject PlayerInputMana; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    private Transform thisTransform; // ���g��Transform���擾����ϐ�
    private GameObject player; // ���g��Transform���擾����ϐ�
    private Transform playerTransform;
    private HammerNail hammerNail; // �B�łX�N���v�g�擾����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManager��T��
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        //----------------------------------------------------------------------------------------------------------
        // ���g(�Ə�)�̎���Transform���擾����
        thisTransform = this.GetComponent<Transform>();

        // �����͓���
        Color col = GetComponent<SpriteRenderer>().color;

        col.a = 0.0f;

        //----------------------------------------------------------------------------------------------------------
        // �v���C���[�T��
        player = GameObject.Find("player");

        playerTransform = player.GetComponent<Transform>();

        //----------------------------------------------------------------------------------------------------------
        // HammerNail�擾
        hammerNail = player.GetComponent<HammerNail>();
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
            //this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        //----------------------------------------------------------------------------------------------------------
        Vector3 vector_FairyPlayer = playerTransform.position - thisTransform.position;

        // �d������v���C���[�̋���
        Distance = vector_FairyPlayer.magnitude;

        //----------------------------------------------------------------------------------------------------------
        // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
        movement = ScriptPIManager.GetRmove();

        // �E�X�e�B�b�N�̓��͂��������
        if(movement.x == 0.0f && movement.y == 0.0f)
        {
            // �O�̃t���[���܂ŏƏ��������Ă����Ȃ�
            if(Move == true)
            {
                // �v���C���[�ƏƏ��̃x�N�g����ۑ�
                offset = vector_FairyPlayer;
                //Debug.Log(offset);

                // ����if���ɓ���Ȃ����߂�false
                Move = false;
            }

            // �e�q�֌W�̂Ƃ��̂悤�ȓ������Č�
            thisTransform.position = new Vector3(
                playerTransform.position.x - offset.x,
                playerTransform.position.y - offset.y,
                0.0f);
        }
        else
        {
            if(Move == false)
            {
                Move = true;
            }
        }

        // �v���C���[�ƏƏ��̋��������͈͈ȉ��Ȃ�
        if (Distance <= Radius)
        {
            //----------------------------------------------------------------------------------------------------------
            // �Ə��̌��݂̈ʒu�Ɉړ��ʂ����Z
            thisTransform.Translate(
            movement.x * Speed * Time.deltaTime,
            movement.y * Speed * Time.deltaTime,
            0.0f);

            if (touchGround == true)
            {
                // �B��łĂȂ��F:��
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            else
            {
                // �B��łĂ�F:�V�A��
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            // �Ə������ꂷ���Ȃ����߂̏���
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

        // �Ə��̃J���[���擾
        Color col = GetComponent<SpriteRenderer>().color;

        //---------------------------------------------------------------------
        // HammerNail��HammerState�̏�Ԃɂ����alpha�l�ς���
        // NAILSET�Ȃ�
        if(hammerNail._HammerState == global::HammerNail.HammerState.NAILSET)
        {

            
            if (col.a == 0.0f)
            {
                GetComponent<SpriteRenderer>().color = new Color(col.r, col.g, col.b,1.0f);


            }

            Debug.Log(col.a);
        }
        // NAILSET �ȊO�Ȃ�
        else
        {
            //Debug.Log("???????????????????????");
            if (col.a == 1.0f)
            {
                GetComponent<SpriteRenderer>().color = new Color(col.r, col.g, col.b, 0.0f);

            }
            Debug.Log(col.a);

        }


        Debug.Log(hammerNail._HammerState);

        //Debug.Log(OldActive);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == GroundTag)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            touchGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == GroundTag)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 1.0f, 1.0f);
            touchGround = false;
        }
    }
}
