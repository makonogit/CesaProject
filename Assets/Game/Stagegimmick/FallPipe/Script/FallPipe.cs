//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�����̃p�C�v�̏���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPipe : MonoBehaviour
{
    // �ϐ��錾

    // �����蔻�莞�̕K�v�^�O��
    private string GroundTag = "Ground";
    private string PipeTag = "Pipe";
    private string PlayerTag = "Player";

    // �p�C�v�̏��
    public enum PIPESTATUS
    {
        NotBroken,       // �ǂ������Ă��Ȃ�
        LeftBroken,      // ���̃N���X�^��������ꂽ
        RightBroken,     // �E�̃N���X�^��������ꂽ
        AllBroken,       // �����̃N���X�^������ꂽ
        Fell             // ����������
    }

    // ���݂̃p�C�v�̏�ԁF����NotBroken
    [SerializeField]public PIPESTATUS pipeStatus = PIPESTATUS.NotBroken;

    private bool InPipe = false;

    // ���g�̃R���|�[�l���g�ϐ�
    private Transform thisTransform;
    private Rigidbody2D rigid2D;

    // �O���擾
    // �v���C���[
    private GameObject player;
    private Transform playerTransform;

    // �q�I�u�W�F�N�g
    private GameObject Child;
    private EdgeCollider2D ChildEdge;

    // �e�I�u�W�F�N�g(PipeSet)
    private GameObject Parent;
    // �N���X�^���}�l�[�W���[
    private GameObject CrystalManager;
    // ���̃N���X�^��
    private GameObject LeftUnionCrystal;
    // �E�̃N���X�^��
    private GameObject RightUnionCrystal;

    private BreakUnionCrystal UnionLeft;
    private BreakUnionCrystal UnionRight;

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�擾
        player = GameObject.Find("player");
        playerTransform = player.GetComponent<Transform>();

        // �g�����X�t�H�[���擾
        thisTransform = GetComponent<Transform>();

        // ���W�b�h�{�f�B�擾
        rigid2D = GetComponent<Rigidbody2D>();

        // �q�I�u�W�F�N�g�擾
        Child = transform.GetChild(1).gameObject;

        //�q�I�u�W�F�N�g�̃R���C�_�[�擾
        ChildEdge = Child.GetComponent<EdgeCollider2D>();

        // �e�擾
        Parent = transform.parent.gameObject;

        // �N���X�^���}�l�[�W���[�擾
        CrystalManager = Parent.transform.GetChild(3).gameObject;
        // ���̃N���X�^���擾
        LeftUnionCrystal = CrystalManager.transform.GetChild(0).gameObject;
        // �E�̃N���X�^���擾
        RightUnionCrystal = CrystalManager.transform.GetChild(1).gameObject;

        // �X�N���v�g�擾
        UnionLeft = LeftUnionCrystal.GetComponent<BreakUnionCrystal>();
        UnionRight = RightUnionCrystal.GetComponent<BreakUnionCrystal>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InPipe == false)
        {
            // �v���C���[���p�C�v����ɂ�����
            if (playerTransform.position.y > thisTransform.position.y)
            {
                // ���肠��
                ChildEdge.isTrigger = false;
            }
            else
            {
                // ���薳��
                ChildEdge.isTrigger = true;
            }
        }

        // �X�e�[�^�X�ɂ���ĈقȂ鏈�����s��
        switch (pipeStatus)
        {
            case PIPESTATUS.NotBroken:
                NotBroken();
                break;

            case PIPESTATUS.LeftBroken:
                LeftBroken();
                break;

            case PIPESTATUS.RightBroken:
                RightBroken();
                break;

            case PIPESTATUS.AllBroken:
                AllBroken();
                break;

            case PIPESTATUS.Fell:
                Fell();
                break;
        }
    }

    private void NotBroken()
    {
        // �Е��̃N���X�^�����󂳂ꂽ��Ή������ԂɑJ��

        // �󂳂ꂽ���擾
        bool left = UnionLeft.GetBreak();
        bool right = UnionRight.GetBreak();

        // �����󂳂ꂽ
        if (left)
        {
            pipeStatus = PIPESTATUS.LeftBroken;
        }

        // �E���󂳂ꂽ
        if (right)
        {
            pipeStatus = PIPESTATUS.RightBroken;
        }

        // ����Ȃ��Ȃ�Ǝv�����Ǘ�O����������
        if(left && right)
        {
            pipeStatus = pipeStatus = PIPESTATUS.AllBroken;
        }
    }

    private void LeftBroken()
    {
        bool right = UnionRight.GetBreak();

        if (right)
        {
            pipeStatus = PIPESTATUS.AllBroken;
        }
    }

    private void RightBroken()
    {
        bool left = UnionRight.GetBreak();

        if (left)
        {
            pipeStatus = PIPESTATUS.AllBroken;
        }
    }

    private void AllBroken()
    {
        // �ŏ��̃t���[������
        if (rigid2D.gravityScale != 1f)
        {
            rigid2D.gravityScale = 1.0f;
            rigid2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Fell()
    {
        // �ŏ��̃t���[������
        if(rigid2D.gravityScale != 0f)
        {
            // �����Ȃ�����
            rigid2D.gravityScale = 0f;
            rigid2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �n�ʂ��p�C�v�ɓ���������
        if(collision.gameObject.tag == GroundTag || 
            collision.gameObject.tag == PipeTag)
        {
            // �p�C�v�̏�Ԃ����������ԂȂ�
            if(pipeStatus == PIPESTATUS.AllBroken)
            {
                pipeStatus = PIPESTATUS.Fell;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PlayerTag)
        {
            ChildEdge.isTrigger = true;

            InPipe = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == PlayerTag)
        {
            ChildEdge.isTrigger = false;

            InPipe = false;
        }
    }
}
