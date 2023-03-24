//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�Ə��̈ړ��i�R���g���[���[R�X�e�B�b�N�j�A�v���C���[�𒆐S�Ƃ������ar�̉~������ړ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailTargetMove : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    private string GroundTag = "Ground";

    [Header("�Ə��̈ړ����x")]
    public float Speed = 5.0f; // �v���C���[�Ƃ̋���
    private Vector2 movement; // ���͗ʂ��擾����ϐ�
    public float Radius = 3.0f; // �v���C���[�Ɨ�����鋗��
    [Header("�v���C���[�Ƃ̋���")]
    public float Distance; // �v���C���[�Ɨd���̋��������ϐ�
    [Header("�Ə��������Ƃ��̃v���C���[�Ƃ̍��̊")]
    public float InitPosDistance = 2.0f; // �Ə��������Ƃ��̃v���C���[�Ƃ̍��̊

    private Vector3 offset_TargetStop; // �Ə��𓮂����ĂȂ����̃v���C���[�ƏƏ��̃x�N�g���p�ϐ�
    bool Move = true; // �Ə��������Ă��邩�����Ă��Ȃ�������
    bool touchGround = false; // �Ə���Ground�^�O�̃I�u�W�F�N�g�ƐG��Ă��邩

    [System.NonSerialized]
    public bool CreateCrack = false;

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

        //-----------------------------------------------------------------------------------------------------------
        // �Ə��̈ʒu��������
        thisTransform.position = new Vector3(playerTransform.position.x + 0.3f,playerTransform.position.y + 0.3f,0.0f);

    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------------------------------------------------------
        // �Ə��̕\���A�ړ�

        //----------------------------------------------------------------------------------------------------------
        Vector3 vector_PlayerFairy = thisTransform.position - playerTransform.position;

        // �Ə�����v���C���[�̋���
        Distance = vector_PlayerFairy.magnitude;

        //----------------------------------------------------------------------------------------------------------
        // �ړ��ʂ�PlayerInputManager����Ƃ��Ă���
        movement = ScriptPIManager.GetRmove();

        // �E�X�e�B�b�N�̓��͂��������
        if(movement.x == 0.0f && movement.y == 0.0f)
        {
            // �O�̃t���[���܂ŏƏ��������Ă����Ȃ�
            if (Move == true)
            {
                // �v���C���[�ƏƏ��̃x�N�g����ۑ�
                offset_TargetStop = vector_PlayerFairy;

                // ����if���ɓ���Ȃ����߂�false
                Move = false;
            }

            // �e�q�֌W�̂Ƃ��̂悤�ȓ������Č�
            thisTransform.position = new Vector3(
                playerTransform.position.x + offset_TargetStop.x,
                playerTransform.position.y + offset_TargetStop.y,
                0.0f);
        }
        else
        {
            if(Move == false)
            {
                Move = true;
            }
        }

        // �\�����Ă��Ȃ����ɏƏ��̈ʒu��������ێ������܂܋߂Â���
        // �\�����Ȃ���ԂȂ�
        if (CreateCrack == true)
        {
            // �Ə����x�N�g�����ێ������܂܎w�肵�������̈ʒu�Ɉړ�������
            thisTransform.position = new Vector3(
                playerTransform.position.x + offset_TargetStop.normalized.x * InitPosDistance,
                playerTransform.position.y + offset_TargetStop.normalized.y * InitPosDistance,
                0.0f);

            // ���W���X�V���ꂽ�̂ŃI�t�Z�b�g���X�V
            offset_TargetStop = thisTransform.position - playerTransform.position;

            // ���Ђт����������܂�false
            CreateCrack = false;
        }
        
        if (hammerNail._HammerState == global::HammerNail.HammerState.NAILSET)
        {
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
                    -vector_PlayerFairy.normalized.x * Speed * Time.deltaTime,
                    -vector_PlayerFairy.normalized.y * Speed * Time.deltaTime,
                    0.0f);
            }

            //Debug.Log(thisTransform.position);
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
        }
        // NAILSET �ȊO�Ȃ�
        else
        {
            if (col.a == 1.0f)
            {
                GetComponent<SpriteRenderer>().color = new Color(col.r, col.g, col.b, 0.0f);

            }
        }
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
