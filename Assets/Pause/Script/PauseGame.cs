//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�|�[�Y���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    public bool IsPause = false; // �|�[�Y��Ԃ��ǂ���
    private bool IgnoredFlg = false; // true�̎�if�̏�����������
    public int CursorY = 0; // Y�����̈ړ�������J�[�\���̔ԍ�
    const int CursorMax = 2; // �J�[�\���̈�ԉ�

    // ���j���[�̐��������邽�тɒǉ�
    private string[] PauseObj = {
        "Continue",
        "Retry",
        "Select" };

    // �O���擾
    private GameObject PlayerInputMana;
    private PlayerInputManager ScriptPIManager;
    private GameObject Cursor; // �J�[�\��
    private RectTransform cursorTransform; // �J�[�\���̍��W
    private GameObject Target; // �J�[�\���̈ʒu�̊�ƂȂ�obj
    private RectTransform targetTransform; // Target�̍��W�擾
    private RectTransform InitTransform; // �J�[�\�����ŏ��ɂ���ʒu��ۑ����Ă����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        // ��\���ɂ���
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;

        // �J�[�\���T��
        Cursor = GameObject.Find("Cursor");

        // �J�[�\���̍��W�擾
        cursorTransform = Cursor.GetComponent<RectTransform>();

        // �J�[�\���̈ʒu�̊�ƂȂ�obj�T��
        Target = GameObject.Find(PauseObj[CursorY]);

        // �����_�ł�Target�̍��W�擾
        targetTransform = Target.GetComponent<RectTransform>();

        // �J�[�\�������ʒu�ۑ�
        InitTransform = targetTransform;
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // �|�[�Y�{�^���������ꂽ�Ȃ�
        if (ScriptPIManager.GetPause() == true)
        {
            TimeOperate();

            ScriptPIManager.SetPause(false);
        }

        // �|�[�Y��Ԃ̎��̏���
        if (IsPause)
        {
            // �X�e�B�b�N�̓��͂Ƃ��Ă���
            float move = ScriptPIManager.GetCursorMove().y;

            // �J�[�\���̈ړ�
            // ���X�e�B�b�N or �\���{�^��
            // ���͂��������Ȃ�
            if (move != 0)
            {
                // ����͂��������Ȃ�
                if(move > 0)
                {
                    // �J�[�\�������
                    CursorY--;
                    if(CursorY < 0)
                    {
                        CursorY = CursorMax;
                    }
                }
                // �����͂��������Ȃ�
                else if(move < 0)
                {
                    // �J�[�\��������
                    CursorY++;
                    if (CursorY > CursorMax)
                    {
                        CursorY = 0;
                    }
                }

                // �^�[�Q�b�g�X�V
                Target = GameObject.Find(PauseObj[CursorY]);
                // �^�[�Q�b�g���W�X�V
                targetTransform = Target.GetComponent<RectTransform>();

                // �ʒu���ړ�
                cursorTransform.position = targetTransform.position;

                ScriptPIManager.SetCursorMove(Vector2.zero);
            }

            // �L�����Z���{�^�������͂��ꂽ
            if (ScriptPIManager.GetPressB() == true)
            {
                // �|�[�Y�I��
                TimeOperate();

                ScriptPIManager.SetPressB(false);
            }

            // ����{�^���������ꂽ
            if(ScriptPIManager.GetPressA() == true)
            {
                // �J�[�\���̈ʒu�ɂ���ď����ς��
                switch (CursorY)
                {
                    // ������
                    case 0:
                        // �|�[�Y�I��
                        TimeOperate();
                        break;

                    // ���g���C
                    case 1:
                        TimeOperate();

                        // ������V�[�������[�h���Ȃ���
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        break;

                    //�Z���N�g��
                    case 2:
                        // �X�e�[�W�Z���N�g�ɍs��
                        SceneManager.LoadScene("SelectScene");
                        break;
                }
                ScriptPIManager.SetPressA(false);
            }
        }
        else
        {
            if (CursorY != 0)
            {
                // �J�[�\���ʒu������
                cursorTransform.position = InitTransform.position;
                CursorY = 0;
            }
        }
    }

    // �Ăяo�����Ƃŋt�̏�Ԃɂ���֐�
    //���Ԏ~�߂��蓮�������肷��
    void TimeOperate()
    {
        // �|�[�Y��ԂȂ�ʏ�Đ��ɖ߂�
        if (IsPause)
        {
            // �ʏ�Đ��ɂ���
            Time.timeScale = 1f;
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;

            IsPause = false;
        }
        // �ʏ�Ȃ�|�[�Y��Ԃɂ���
        else
        {
            // �ꎞ��~����
            Time.timeScale = 0f;
            this.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;

            IsPause = true;
        }
    }
}
