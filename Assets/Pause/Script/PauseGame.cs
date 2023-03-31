//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�|�[�Y���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - �ϐ��錾 -

    public bool IsPause = false; // �|�[�Y��Ԃ��ǂ���
    public int CursorY = 0; // Y�����̈ړ�������J�[�\���̔ԍ�
    const int CursorMax = 3; // �J�[�\���̈�ԉ�

    //private float ManualSizeX = 3.8f;
    //private float ManualSizeY = 10.2f;
    public bool manual = false;

    // ���j���[�̐��������邽�тɒǉ�
    private string[] PauseObj = {
        "Continue",
        "Retry",
        "HowTo",
        "Select" };

    // �O���擾
    private GameObject PlayerInputMana;
    private PlayerInputManager ScriptPIManager;
    private GameObject Cursor; // �J�[�\��
    private RectTransform cursorTransform; // �J�[�\���̍��W
    private Image cursorImage;
    private GameObject Target; // �J�[�\���̈ʒu�̊�ƂȂ�obj
    private RectTransform targetTransform; // Target�̍��W�擾
    private RectTransform InitTransform; // �J�[�\�����ŏ��ɂ���ʒu��ۑ����Ă����ϐ�

    private GameObject Manual;
    //private RectTransform manualTransform;
    private Image manualImage;

    // se�֌W
    private GameObject se;
    private SEManager_Pause seMana;

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

        // �J�[�\���̃C���[�W�R���|�[�l���g�擾
        cursorImage = Cursor.GetComponent<Image>();

        // �J�[�\���̈ʒu�̊�ƂȂ�obj�T��
        Target = GameObject.Find(PauseObj[CursorY]);

        // �����_�ł�Target�̍��W�擾
        targetTransform = Target.GetComponent<RectTransform>();

        // �J�[�\�������ʒu�ۑ�
        InitTransform = targetTransform;

        Manual = GameObject.Find("Manual");
        //manualTransform = Manual.GetComponent<RectTransform>();
        //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        manualImage = Manual.GetComponent<Image>();
        manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        // �T�E���h�֌W
        se = GameObject.Find("SE");
        // Se�R���|�[�l���g�擾
        seMana = se.GetComponent<SEManager_Pause>();

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

            seMana.PlaySE_OK();
        }

        //Debug.Log(manual);

        // �|�[�Y��Ԃ̎��̏���
        if (IsPause)
        {
            // �X�e�B�b�N�̓��͂Ƃ��Ă���
            float move = ScriptPIManager.GetCursorMove().y;

            if (manual == false)
            {
                // �J�[�\���̈ړ�
                // ���X�e�B�b�N or �\���{�^��
                // ���͂��������Ȃ�
                if (move != 0)
                {
                    // ����͂��������Ȃ�
                    if (move > 0)
                    {
                        // �J�[�\�������
                        CursorY--;
                        if (CursorY < 0)
                        {
                            CursorY = CursorMax;
                        }
                    }
                    // �����͂��������Ȃ�
                    else if (move < 0)
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

                    seMana.PlaySE_Select();
                }
            }

            if (manual == true)
            {
                // �L�����Z���{�^�������͂��ꂽ
                if (ScriptPIManager.GetPressB() == true)
                {
                    //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    cursorImage.color = new Color(0.0f, 0.0f, 0.0f, 0.31f);
                    manual = false;

                    ScriptPIManager.SetPressB(false);
                    ScriptPIManager.SetPressA(false);

                    seMana.PlaySE_Cansel();
                }
            }
            else
            {
                // �L�����Z���{�^�������͂��ꂽ
                if (ScriptPIManager.GetPressB() == true)
                {
                    // �|�[�Y�I��
                    TimeOperate();

                    ScriptPIManager.SetPressB(false);

                    seMana.PlaySE_Cansel();

                }
            }

            if (manual == false)
            {
                // ����{�^���������ꂽ
                if (ScriptPIManager.GetPressA() == true)
                {
                    seMana.PlaySE_OK();

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

                        // ������@��
                        case 2:
                            manual = true;
                            //manualTransform.localScale = new Vector3(ManualSizeX, ManualSizeY, 0.0f);
                            manualImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                            cursorImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                            break;

                        //�Z���N�g��
                        case 3:
                            TimeOperate();

                            // �X�e�[�W�Z���N�g�ɍs��
                            SceneManager.LoadScene("newSelectScene");
                            break;
                    }
                    ScriptPIManager.SetPressA(false);
                }
            }
        }
        else
        {
            if (CursorY != 0)
            {
                // �J�[�\���ʒu������
                cursorTransform.position = InitTransform.position;
                CursorY = 0;

                //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                cursorImage.color = new Color(0.0f, 0.0f, 0.0f, 0.31f);
                manual = false;
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
