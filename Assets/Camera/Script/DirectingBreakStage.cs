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

    // �O���擾
    private GameObject StageData; // �K�v�ƂȂ�I�u�W�F�N�g�̐e�I�u�W�F�N�g

    private GameObject CameraTarget_last; // �J�������ړ�����ŏI�I�ȍ��W�����I�u�W�F�N�g
    private Transform Last_Transform; // �ڕW�I�u�W�F�N�g�̍��W

    private GameObject StagePrefab; // �X�e�[�W���v���n�u
    private CameraZoom sc_cameraZoom; // �X�N���v�g

    private GameObject MainCamera;
    private CameraControl2 control;
    private GameObject player;

    [Header("�e�X�e�[�W���Ƃɗp�ӂ��ꂽBorderLine�}�e���A�����Z�b�g")]public Material Mat;

    public GameObject particle; // ��ꂽ�j�Ђ������Ă���p�[�e�B�N��
    private GameObject ParticleObj; // �쐬�����p�[�e�B�N�������ϐ�

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
    }

    // Update is called once per frame
    void Update()
    {
        if(startInit == false)
        {
            // �ڕW�I�u�W�F�N�g�擾
            CameraTarget_last = GameObject.Find("CameraTarget_Last");
            Last_Transform = CameraTarget_last.GetComponent<Transform>();
            Debug.Log(CameraTarget_last);
            Debug.Log(Last_Transform);

            // �X�e�[�W�̏������I�u�W�F�N�g�擾
            StageData = GameObject.Find("StageData");
            Debug.Log(StageData);

            // �X�e�[�W�v���n�u�@��)Stage1-1,Stage2-1
            StagePrefab = StageData.transform.GetChild(0).gameObject;
            Debug.Log(StagePrefab);

            // �J�����Y�[���X�N���v�g�擾
            sc_cameraZoom = StagePrefab.GetComponent<CameraZoom>();
            Debug.Log(sc_cameraZoom);

            // ������
            Mat.SetFloat("_Fader", 1f);  // �����l
            Mat.SetFloat("_Width", -9f); // �Œ�

            startInit = true;
        }

        if(startBreak == true && BreakStage == false)
        {
            // �J�����Ǐ]�^�[�Q�b�g��ύX
            if(control.GetTarget() != this.gameObject)
            {
                // �J�����Ǐ]�^�[�Q�b�g���X�e�[�W�̍ŏ��̕��ɂ��鎩�g�ɐݒ�
                control.SetTarget(this.gameObject);
                Debug.Log(control.GetTarget());

                // ���̃X�N���v�g�����I�u�W�F�N�g�̎q�I�u�W�F�N�g��
                ParticleObj = Instantiate(particle, this.gameObject.transform.GetChild(0).gameObject.transform);
            }

            // �J�����Ǐ]�^�[�Q�b�g�̈ʒu��ڕW�n�_�܂ŉ������Ȃ���ړ�������
            if (thisTransform.position.x < Last_Transform.position.x)
            {
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
                thisTransform.position = new Vector3(Last_Transform.position.x,
                    thisTransform.position.y, 
                    thisTransform.position.z);
            }

            // �w�i�N���X�^������������
            FaderRate -= Time.deltaTime * BreakSpeed;
            // �w�i�N���X�^���}�e���A����臒l���ő�l����ŏ��l�ɉ�����
            Mat.SetFloat("_Fader", FaderRate);

            if (FaderRate <= 1f - CrystalNum_X * 2f)
            {
                // �X�e�[�W�j�󊮗��t���O
                BreakStage = true;
                Debug.Log("�X�e�[�W�j�󊮗�");

                Destroy(ParticleObj);
            }

            // �J�E���g
            timer += Time.deltaTime;
        }
        else
        {
            thisTransform.position = InitPos;
            control.SetTarget(player);
            timer = 0f;

            FaderRate = 1f;
            Mat.SetFloat("_Fader", FaderRate);
        }
    }

    public bool GetBreakStage()
    {
        return BreakStage;
    }

    public void StartBreak()
    {
        startBreak = true;
    }
}