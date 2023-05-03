//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�J�����̃Y�[��
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -
    
   [Header("��b������̕ω���")]
    public float ChangeVolume = 0.1f; // �J�����T�C�Y�̕ω���

    [Header("�Y�[����̕`��T�C�Y")]
    public float ZoomCameraSize = 2.5f;

    [Header("�ʏ펞�̕`��T�C�Y")]
    public float DefaultCameraSize = 5.0f; //�ʏ펞�̃J�����T�C�Y

    [Header("���݂̃J�����T�C�Y")]
    public float NowCameraSize = 5.0f; // ���݂̃J�����T�C�Y

    // �O���擾
    private GameObject Camera; // �Q�[���I�u�W�F�N�gMainCamera
    Camera Cam; // �J�����X�N���v�g���擾

    private Transform cameraTransform; // �J�����̍��W

    private GameObject player;      // �v���C���[
    private Transform playertans;   // �v���C���[��Transform
    public StageStatas stagestatas;    // �X�e�[�W�̃X�e�[�^�X���擾
    private ChangeResultScene resultScene;

    private bool First = false;

    public bool ZoomEnd = false;

    // �X�e�[�W�j�󉉏o�֌W�ϐ�
    private GameObject lastCameraTarget;
    private DirectingBreakStage breakstage;
    public bool SetBreak = false; // �j�󉉏o�Ăяo��������


    // Start is called before the first frame update
    void Start()
    {
        // MainCamera�T��
        Camera = GameObject.Find("Main Camera");
        // �J�����X�N���v�g���擾
        Cam = Camera.GetComponent<Camera>();

        player = GameObject.Find("player");
        playertans = player.transform;

        stagestatas = GetComponent<StageStatas>();
        resultScene = GameObject.Find("ChageResultScene").GetComponent<ChangeResultScene>();

        lastCameraTarget = GameObject.Find("CameraTarget_Start");
        breakstage = lastCameraTarget.GetComponent<DirectingBreakStage>();
        SetBreak = false;
    }

    // Update is called once per frame
    void Update()
    {
        //�@�S�Ĕj�󂳂ꂽ��
        if ((!resultScene.BossStage || (resultScene.BossStage && GameObject.Find("BossEnemy").transform.childCount == 0 && resultScene.WaitFlame > 0.2f))
            && stagestatas.GetStageCrystal() == 0)
        {
            // ���o�J�n�����Ă��Ȃ����
            if(SetBreak == false)
            {
                Debug.Log("�w�i�N���X�^���j��J�n");
                // �j�󉉏o�J�n
                breakstage.StartBreak();

                // �t���O
                SetBreak = true;
            }

            if (breakstage.GetBreakStage() == true)
            {
                Debug.Log("zoom");
                // �Y�[����̃J�����`��T�C�Y�ɂȂ�܂ŏ��X�ɃY�[���C�����Ă���
                if (NowCameraSize > ZoomCameraSize)
                {
                    // �`��T�C�Y�v�Z
                    NowCameraSize -= ChangeVolume * Time.deltaTime;
                }
                else
                {
                    NowCameraSize = ZoomCameraSize;
                    ZoomEnd = true;
                }

                // �J�����̈ʒu�̓v���C���[�̒��S�ɌŒ�
                //cameraTransform.position = playertans.position;

                //�J�����T�C�Y�ύX
                Cam.orthographicSize = NowCameraSize;
            }
        }
      
    }
}
