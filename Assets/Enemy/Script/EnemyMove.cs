//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�G�̈ړ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    private string NailTag = "UsedNail";

    public float MoveDistance = 1; // �ړ��͈�
    public float MoveSpeed = 0.05f; // �ړ����x
    private Vector3 StartPosition; // �G�̊J�n�ʒu
    private float StartTime = 0.0f; // �G����������Ă���̌o�ߎ���
    public bool Stop = false; // �f�o�b�O�p �G�����̏�ɂƂǂ܂�

    // �O���擾
    private Transform thisTranform; // ���̃I�u�W�F�N�g�̍��W�����ϐ�
    private GameObject player; // �v���C���[�̃Q�[���I�u�W�F�N�g�T���p
    private HammerNail hammer; // HammerNail���擾

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �I�u�W�F�N�g��Transform���擾
        thisTranform = GetComponent<Transform>();

        //---------------------------------------------------------
        // �G�̊J�n�ʒu���擾
        StartPosition = thisTranform.position;

        // �v���C���[�I�u�W�F�N�g�T��
        player = GameObject.Find("player");
        // Hammer�X�N���v�g�擾
        hammer = player.GetComponent<HammerNail>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Stop == false)
        {
            //-------------------------------------------------------------------------------------------
            // �G�̊J�n�ʒu����distance * MoveSpeed�͈̔͂ō��E�ړ�
            thisTranform.position = new Vector3(StartPosition.x + Mathf.Sin(StartTime) * MoveSpeed * MoveDistance, StartPosition.y, StartPosition.z);

            //-------------------------------------------------------------------------------------------
            //���Ԍo��
            StartTime += Time.deltaTime;
        }
        else
        {
            thisTranform.position = new Vector3(thisTranform.position.x, StartPosition.y, StartPosition.z);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == NailTag)
        {
            Debug.Log("tag");

            if(hammer.MomentHitNails == true)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
