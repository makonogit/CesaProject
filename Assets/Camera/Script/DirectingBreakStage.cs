//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�X�e�[�W�N���A��A�X�e�[�W�̍ŏ��̕���������Ă����A������f���J�����̈ړ�
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectingBreakStage : MonoBehaviour
{
    // �ϐ��錾
    public bool startInit = false; // �n�܂��Ĉ�x�����Ăяo��
    public bool startBreak = false; // ���o�J�n

    [Header("�J�����ړ����̃X�s�[�h")]public float MoveSpeed = 5.0f; // �J�����ړ����̃X�s�[�h
    private float timer = 0; // ���o���n�܂��Ă���̌o�ߎ���
    private Transform thisTransform; // ���g�̍��W
    private Vector3 InitPos; // �����ʒu
    private float FinalySpeed; // �v�Z��̍ŏI�I�ȃX�s�[�h
    [Header("����鎞�̃X�s�[�h")]public float BreakSpeed = 1.1f;

    [Header("�J�����ړ����ɉ������Ă�����")]public bool isAxel = true;

    private float FaderRate = 1f; // �ő�l����ŏ��l�̊Ԃ�ω�������

    [Header("�j�󂷂�N���X�^�������ɉ�������ł��邩"),SerializeField]
    private int CrystalNum_X; // �j�󂷂�N���X�^�������ɉ�������ł��邩

    private bool BreakStage; // �X�e�[�W�j�󉉏o���I�������true
    [SerializeField] private float StartFadeOutTime = 5f;
    private bool fadeOrder = false; // �t�F�[�h�A�E�g�̖��߂��o���Ă�����true

    // �N���A�̃t�F�[�h�A�E�g���n�܂��Ă���̌o�ߎ���
    private ResultManager _resultManager;

    // �O���擾
    private GameObject StageData; // �K�v�ƂȂ�I�u�W�F�N�g�̐e�I�u�W�F�N�g

    private GameObject StagePrefab; // �X�e�[�W���v���n�u
    private CameraZoom sc_cameraZoom; // �X�N���v�g

    private GameObject MainCamera;
    private CameraControl2 control;
    private GameObject player;
    private Transform _playerTransform;
    private PlayBgm bgm;

    [Header("�e�X�e�[�W���Ƃɗp�ӂ��ꂽBorderLine�}�e���A�����Z�b�g")]private Material Mat;

    public GameObject particle; // ��ꂽ�j�Ђ������Ă���p�[�e�B�N��
    private GameObject ParticleObj; // �쐬�����p�[�e�B�N�������ϐ�

    [SerializeField, Header("�����G�t�F�N�g")]
    private ScreenBreak _ScreenBreak;

    [SerializeField, Header("�w�i�N���X�^��")]
    private List<GameObject> Crystal;

    private BGMFadeManager _BGMFadeMana;
    private AudioSource SpecialBGM; // ����BGM
    private bool ClearBGMflg = false;
    private Fade _fade;

    GameObject Core;
    [SerializeField, Header("�q�r����N���X�^���Ǘ�")]
    private CrystalCrackManager _crystalmanager;

    // Start is called before the first frame update
    void Start()
    {
        // ���C���J�����擾
        MainCamera = GameObject.Find("Main Camera");
        // �J�����Ǐ]�X�N���v�g�擾
        control = MainCamera.GetComponent<CameraControl2>();
        thisTransform = GetComponent<Transform>();
        InitPos = thisTransform.position;

        player = GameObject.Find("player");

        startInit = false;
        startBreak = false;
        BreakStage = false;

        _BGMFadeMana = MainCamera.GetComponent<BGMFadeManager>();
        SpecialBGM = MainCamera.transform.Find("SpecialBGM").gameObject.GetComponent<AudioSource>();
        bgm = MainCamera.GetComponent<PlayBgm>();

        _fade = GameObject.Find("SceneManager").GetComponent<Fade>();

        _playerTransform = player.GetComponent<Transform>();

        _resultManager = GameObject.Find("Result_StageClear").GetComponent<ResultManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(startInit == false)
        {
            // �ڕW�I�u�W�F�N�g�擾

            // �X�e�[�W�̏������I�u�W�F�N�g�擾
            StageData = GameObject.Find("StageData");
            //Debug.Log(StageData);

            // �X�e�[�W�v���n�u�@��)Stage1-1,Stage2-1
            StagePrefab = StageData.transform.GetChild(0).gameObject;
            //Debug.Log(StagePrefab);

            // �J�����Y�[���X�N���v�g�擾
            sc_cameraZoom = StagePrefab.GetComponent<CameraZoom>();
            //Debug.Log(sc_cameraZoom);

            // ������
            //Mat.SetFloat("_Fader", 1f);  // �����l
            //Mat.SetFloat("_Width", -9f); // �Œ�

            startInit = true;
        }

        if(startBreak == true && BreakStage == false)
        {
            // �J�����Ǐ]�^�[�Q�b�g��ύX
            if(control.GetTarget() != this.gameObject)
            {
                // �J�����Ǐ]�^�[�Q�b�g���X�e�[�W�̍ŏ��̕��ɂ��鎩�g�ɐݒ�
                control.SetTarget(this.gameObject);
                //Debug.Log(control.GetTarget());

                // ���̃X�N���v�g�����I�u�W�F�N�g�̎q�I�u�W�F�N�g��
                // ParticleObj = Instantiate(particle, this.gameObject.transform.GetChild(0).gameObject.transform);

                _BGMFadeMana.SmallStageBGM();
                _BGMFadeMana.SmallBossBGM();
            }

            // �J�����Ǐ]�^�[�Q�b�g�̈ʒu��ڕW�n�_�܂ŉ������Ȃ���ړ�������
            if (thisTransform.position.x < _playerTransform.position.x)
            {
                bgm.CrackLoop();
                // ���W��ڕW�I�u�W�F�N�g�̕��܂ňړ�������
                // ��ɂȂ�΂Ȃ�قǑ����Ȃ�
                // ��������Ȃ�
                if (isAxel == true)
                {
                    FinalySpeed = MoveSpeed + MoveSpeed * timer / 2f;
                }
                // �����ňړ�������
                else
                {
                    FinalySpeed = MoveSpeed;
                }

               thisTransform.Translate(FinalySpeed * Time.deltaTime, 0f, 0f);
            }
            else
            {
                thisTransform.position = new Vector3(_playerTransform.position.x,
                    thisTransform.position.y, 
                    thisTransform.position.z);
            }

            // �w�i�N���X�^������������
            FaderRate -= Time.deltaTime * BreakSpeed;
            // �w�i�N���X�^���}�e���A����臒l���ő�l����ŏ��l�ɉ�����
            //Mat.SetFloat("_Fader", FaderRate);

            if (FaderRate <= 1f - CrystalNum_X * 2f)
            {
                if (!_ScreenBreak.enabled)
                {
                    // �R�A���擾�A����
                    Core = GameObject.Find("Core").transform.GetChild(0).gameObject;
                    Destroy(Core);

                    for (int i = 0; i < Crystal.Count; i++)
                    {
                        Destroy(Crystal[i]);
                    }


                    // ���߂̈��̂ݓ���
                    if (ClearBGMflg == false)
                    {
                        // �X�e�[�W�N���ABGM�Đ��J�n
                        _BGMFadeMana.StageClear();

                        ClearBGMflg = true;
                    }

                    // Destroy(ParticleObj);
                    _ScreenBreak.enabled = true;
                }
            }

            if(ClearBGMflg == true)
            {
                if(SpecialBGM.time > 2.5f) // ���Œ�Ȍ��ߑł��ł�
                {
                    // �X�e�[�W�j�󊮗��t���O
                    BreakStage = true;

                    ClearBGMflg = false;

                    Debug.Log("�X�e�[�W�j�󊮗�");
                }
            }

            // �J�E���g
            timer += Time.deltaTime;
        }
        else
        {
            // ���o���n�܂��ĂȂ��Ɠ���Ȃ� ���o�̓r���ŃZ���N�g�ɖ߂��Ă��܂��o�O����
            if (startBreak == true)
            {
                // ��莞�Ԍo�߂����ŏ��̃t���[���̂ݓ���
                if (SpecialBGM.time > StartFadeOutTime && fadeOrder == false)
                {
                    // ��ʈÂ����Ă���
                    _fade.FadeOut();

                    // �N���ABGM���������Ă���
                    _BGMFadeMana.smallSpecialBGM();

                    fadeOrder = true;
                }

                // �t�F�[�h�A�E�g���I�����Ă�����Z���N�g�ɑJ�ڂ��邽�߂̊֐��Ăяo��
                if (_fade.GetFadeState() == Fade.FadeState.FadeOut_Finish)
                {
                    _resultManager.GoSelectScene();
                }

                thisTransform.position = InitPos;
                control.SetTarget(player);
                timer = 0f;

                FaderRate = 1f;
                //Mat.SetFloat("_Fader", FaderRate);
            }
        }
    }

    public bool GetBreakStage()
    {
        return BreakStage;
    }

    public void StartBreak()
    {
        _crystalmanager.clear = true;
        startBreak = true;
    }
}
