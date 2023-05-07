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

    public float WaitFallTimer = 0f;
    [Header("�v���C���[����ɏ���Ă��痎����܂ł̑҂�����")]public float WaitFallTime = 0f;
    private bool OnThePipe = false; // ����̏����̎��Ƀp�C�v�̏�ɂ̂�����

    // ���g�̃R���|�[�l���g�ϐ�
    private Transform thisTransform;
    private Rigidbody2D rigid2D;

    // �O���擾
    // �v���C���[
    private GameObject player;
    private Transform playerTransform;


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

    private GameObject SEObj;               //SE�Đ��p�I�u�W�F�N�g
    private GimmickPlay_2 PlaySound;     //SE�Đ��p�X�N���v�g


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

        SEObj = GameObject.Find("BlockSE");
        PlaySound = SEObj.GetComponent<GimmickPlay_2>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if(OnThePipe == true)
        {
            WaitTimeOnPipe();
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
        if (left && right)
        {
            pipeStatus = pipeStatus = PIPESTATUS.AllBroken;
        }
    }

    private void LeftBroken()
    {
        if (OnThePipe == false)
        {
            // ���ĂȂ�������ꂽ��
            bool right = UnionRight.GetBreak();

            if (right)
            {
                pipeStatus = PIPESTATUS.AllBroken;
            }
        }
    }

    private void RightBroken()
    {
        if (OnThePipe == false)
        {
            bool left = UnionLeft.GetBreak();

            if (left)
            {
                pipeStatus = PIPESTATUS.AllBroken;
            }
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
            
            if (!PlaySound.IsPlay())
            {
                PlaySound.PlayerGimmickSE(GimmickPlay_2.GimmickSE2List.PIPEFALL);
            }

        }
    }

    private void WaitTimeOnPipe()
    {
        // ���߂̈��̂�
        if(WaitFallTimer == 0f)
        {
            // �����ĂȂ����̃N���X�^����j��
            if (UnionLeft.GetBreak() == false)
            {
                UnionLeft.Func_BreakBlock();
            }
            if (UnionRight.GetBreak() == false)
            {
                UnionRight.Func_BreakBlock();
            }
        }

        WaitFallTimer += Time.deltaTime;

        if (WaitFallTimer > WaitFallTime)
        {
            OnThePipe = false;
            pipeStatus = PIPESTATUS.AllBroken;
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

        // �v���C���[���p�C�v����ɂ���Ƃ��ɓ���������
        if(collision.gameObject.tag == PlayerTag)
        {
            if (pipeStatus == PIPESTATUS.LeftBroken || pipeStatus == PIPESTATUS.RightBroken)
            {
                // �v���C���[�̋r�̕������p�C�v�̏�\�ʂ���Ȃ�
                if (playerTransform.position.y - playerTransform.localScale.y / 2.0f > thisTransform.position.y + thisTransform.localScale.y / 2.0f)
                {
                    // �҂����Ԃ�݂��邽�߃t���O���Ă邾��
                    OnThePipe = true;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // �n�ʂ��p�C�v�ɓ���������
        if (collision.gameObject.tag == GroundTag ||
            collision.gameObject.tag == PipeTag)
        {
            // �p�C�v�̏�Ԃ����������ԂȂ�
            if (pipeStatus == PIPESTATUS.AllBroken || pipeStatus == PIPESTATUS.LeftBroken || pipeStatus == PIPESTATUS.RightBroken)
            {
                pipeStatus = PIPESTATUS.Fell;
            }
        }

        // �v���C���[���p�C�v����ɂ���Ƃ��ɓ���������
        if (collision.gameObject.tag == PlayerTag)
        {
            if (pipeStatus == PIPESTATUS.LeftBroken || pipeStatus == PIPESTATUS.RightBroken)
            {
                // �v���C���[�̋r�̕������p�C�v�̏�\�ʂ���Ȃ�
                if (playerTransform.position.y - playerTransform.localScale.y / 2.0f > thisTransform.position.y + thisTransform.localScale.y / 2.0f)
                {
                    // �҂����Ԃ�݂��邽�߃t���O���Ă邾��
                    OnThePipe = true;
                }
            }
        }
    }
}
