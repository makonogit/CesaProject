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

    public bool Clear = false;          //�X�e�[�W�N���A������
    public bool IsPause = false; // �|�[�Y��Ԃ��ǂ���
    public int CursorY = 0; // Y�����̈ړ�������J�[�\���̔ԍ�
    const int CursorMax = 3; // �J�[�\���̈�ԉ�
    public bool magnification = false; // �g��p�ϐ�
    public bool reduction = false;     // �k���p�ϐ�
    [Header("1 / ChangeSpeed �b�ŃT�C�Y���ς�肫��")]public float ChangeSpeed = 1.0f; // 
    private bool manual = false;

    private bool retry_Out = false; // ���g���C���I�����ꂽ��true
    private bool retry_In = false;  // �V�[�����ǂݍ��܂ꂽ��true

    private bool select_Out = false; // �Z���N�g���I�����ꂽ��true

    [Header("�t�F�[�h�A�E�g�ƃt�F�[�h�C���̊Ԋu")] public float OutInTime = 0.5f;
    private float WaitTimer = 0f;
    private bool fadeout = false;
    private bool fadein = false;

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
    private float InitPosX; // �J�[�\�����ŏ��ɂ���ʒu��ۑ����Ă����ϐ�
    private float InitPosY; // �J�[�\�����ŏ��ɂ���ʒu��ۑ����Ă����ϐ�

    private GameObject Manual;
    private RectTransform manualTransform;
    private Image manualImage;
    private GameObject black;
    private Image blackImage;

    [SerializeField] PauseSnap snap;    //�|�[�Y�p�ɉ����ϊ����邽�߂̃X�N���v�g

    //private GameObject player;
    //private Transform playerTransform;
    //private PlayerStatas playerStatus;

    // BGM
    [SerializeField] private BGMFadeManager _BGMFadeMana;

    // se�֌W
    private GameObject se;
    private SEManager_Pause seMana;

    // �t�F�[�h�֌W
    [Header("SceneManager"),SerializeField]private GameObject sceneManager;
    private Fade fade;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        // ��\���ɂ���
        //this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
        transform.localScale = new Vector3(0f, 0f, 0f);

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
        InitPosX = cursorTransform.anchoredPosition.x;
        InitPosY = cursorTransform.anchoredPosition.y;

        Manual = GameObject.Find("Manual");
        //manualTransform = Manual.GetComponent<RectTransform>();
        //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        manualImage = Manual.GetComponent<Image>();
        manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        // �w�i�Â�����
        black = GameObject.Find("Black");
        blackImage = black.GetComponent<Image>();
        blackImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        // �T�E���h�֌W
        se = GameObject.Find("SE");
        // Se�R���|�[�l���g�擾
        seMana = se.GetComponent<SEManager_Pause>();

        // ���X�|�[���֌W
        //player = GameObject.Find("player");
        //playerTransform = player.GetComponent<Transform>();
        //playerStatus = player.GetComponent<PlayerStatas>();



        // �t�F�[�h�֌W
        fade = sceneManager.GetComponent<Fade>();

        if(FadeAlpha.black == true)
        {
            retry_In = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------------------------------------------------
        // �|�[�Y�{�^���������ꂽ�Ȃ�(�X�e�[�W�N���A���Ă��Ȃ�������)
        if (!Clear)
        {
            if (ScriptPIManager.GetPause() == true)
            {
                //TimeOperate();

                if (IsPause == false)
                {
                    magnification = true;

                    snap.PauseSnapChange(); //�����Ă点��

                }
                else
                {
                    manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    // �q�I�u�W�F�N�g�̃��l��S�čX�V
                    for (int i = 0; i < Manual.transform.childCount; i++)
                    {
                        Manual.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    }
                    reduction = true;

                    snap.NormalSnapChange(); // ���ɖ߂�
                }

                ScriptPIManager.SetPause(false);

                seMana.PlaySE_OK();
            }
        }

        if (magnification)
        {
            // �|�[�Y���j���[���g��
            Magnification();
        }

        if (reduction)
        {
            // �|�[�Y���j���[���k��
            Reduction();
        }

        // ���g���C�t�F�[�h�A�E�g����
        if(retry_Out)
        {
            Retry_FadeOut();
        }

        // ���g���C�t�F�[�h�C��
        if (retry_In)
        {
            Retry_FadeIn();
        }

        // �Z���N�g�t�F�[�h�A�E�g
        if (select_Out)
        {
            Select_FadeOut();
        }

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
                    cursorTransform.anchoredPosition = new Vector2(InitPosX,targetTransform.anchoredPosition.y);

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
                    // �q�I�u�W�F�N�g�̃��l��S�čX�V
                    for(int i =0;i< Manual.transform.childCount; i++)
                    {
                        Manual.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    }

                    cursorImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
                    //TimeOperate();
                    reduction = true;

                    snap.NormalSnapChange();    //���̉��ɖ߂�
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
                            snap.NormalSnapChange();    //���̉��ɖ߂�
                            // �|�[�Y�I��
                            reduction = true;
                            break;

                        // ���g���C
                        case 1:
                            //reduction = true;

                            retry_Out = true;
                            
                            break;

                        // ������@��
                        case 2:
                            manual = true;
                            //manualTransform.localScale = new Vector3(ManualSizeX, ManualSizeY, 0.0f);
                            manualImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                            // �q�I�u�W�F�N�g�̃��l��S�čX�V
                            for (int i = 0; i < Manual.transform.childCount; i++)
                            {
                                Manual.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                            }
                            cursorImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                            break;

                        //�Z���N�g��
                        case 3:
                            select_Out = true;

                            
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
                cursorTransform.anchoredPosition = new Vector2(InitPosX, InitPosY);
                //cursorTransform.position = InitTransform.position;
                CursorY = 0;

                // �^�[�Q�b�g�X�V
                Target = GameObject.Find(PauseObj[CursorY]);
                // �^�[�Q�b�g���W�X�V
                targetTransform = Target.GetComponent<RectTransform>();

                //manualTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                manualImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                cursorImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
            //this.gameObject.GetComponent<CanvasGroup>().alpha = 0.0f;
            blackImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

            IsPause = false;
        }
        // �ʏ�Ȃ�|�[�Y��Ԃɂ���
        else
        {
            // �ꎞ��~����
            Time.timeScale = 0f;
            //this.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            blackImage.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            IsPause = true;
        }
    }

    // �|�[�Y���j���[�g�剉�o
    void Magnification()
    {
        // �X�P�[�������X�ɑ傫�����Ă���
        float scale = transform.localScale.x + ChangeSpeed * Time.unscaledDeltaTime;

        // ���
        if(scale > 1f)
        {
            scale = 1f;
        }

        // �v�Z���ʂ��X�P�[���ɑ��
        if (transform.localScale.x < 1.0f) 
        {
            transform.localScale = new Vector3(scale, scale, 0);
        }
        else
        {
            // �g�債��������
            magnification = false;

            TimeOperate();
        }
    }

    // �|�[�Y���j���[�k�����o
    void Reduction()
    {
        // �X�P�[�������X�ɏ��������Ă���
        float scale = transform.localScale.x - ChangeSpeed * Time.unscaledDeltaTime;

        // ����
        if (scale < 0f)
        {
            scale = 0f;
        }

        // �v�Z���ʂ��X�P�[���ɑ��
        if (transform.localScale.x > 0.0f)
        {
            transform.localScale = new Vector3(scale, scale, 0);
        }
        else
        {
            // �k������������
            reduction = false;

            TimeOperate();
        }
    }

    private void Retry_FadeOut()
    {
        Fade.FadeState fadestate = fade.GetFadeState();

        // �t�F�[�h�A�E�g���鏉��̂ݓ���
        if (fadestate != Fade.FadeState.FadeOut && fadeout == false)
        {
            // �t�F�[�h�A�E�g�J�n
            fade.FadeOut();

            // BGM�t�F�[�h�J�n
            _BGMFadeMana.SmallStageBGM();
            _BGMFadeMana.SmallBossBGM();

            fadeout = true;
        }

        if(fadestate == Fade.FadeState.FadeOut_Finish)
        {
            WaitTimer += Time.unscaledDeltaTime;

            if(WaitTimer > OutInTime)
            {
                //FadeAlpha.black = true;

                //Debug.Log("�V�[�������[�h");

                // ������V�[�������[�h���Ȃ���
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void Retry_FadeIn()
    {
        // ��ʂ̃t�F�[�h�C��
        fade.FadeIn();
        // BGM�t�F�[�h�C��(�V�[���̃��[�h������̂ŕK�v�͂Ȃ�)
        //_BGMFadeMana.BigStageBGM();

        retry_In = false;

        // �ʏ�Đ��ɂ���
        Time.timeScale = 1f;
    }

    private void Select_FadeOut()
    {
        Fade.FadeState fadestate = fade.GetFadeState();

        // �t�F�[�h�A�E�g���鏉��̂ݓ���
        if (fadestate != Fade.FadeState.FadeOut && fadeout == false)
        {
            // �t�F�[�h�A�E�g�J�n
            fade.FadeOut();

            // BGM�t�F�[�h�J�n
            _BGMFadeMana.SmallStageBGM();

            fadeout = true;
        }

        if (fadestate == Fade.FadeState.FadeOut_Finish)
        {
            WaitTimer += Time.unscaledDeltaTime;

            if (WaitTimer > OutInTime)
            {
                //FadeAlpha.black = true;

                TimeOperate();

                InMainScene.inMainScene = false;

                // �X�e�[�W�Z���N�g�ɍs��
                SceneManager.LoadScene("newSelectScene");
            }
        }
    }
}
