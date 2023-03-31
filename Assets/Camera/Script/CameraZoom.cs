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
    private string playerTag = "Player"; // �v���C���[�̃^�O

    //[System.NonSerialized]
    public bool InArea = false; // �G���A���ɂ��邩

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

    private GameObject goal; // �S�[���I�u�W�F�N�g
    private Transform goalTransform;   // �S�[���̍��W
    private Transform cameraTransform; // �J�����̍��W

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[�Ƃ̓����蔻��̂ݍl��
        if (collision.gameObject.tag == playerTag)
        {
            // ��ԁ��G���A���ɂ���
            InArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �v���C���[�Ƃ̓����蔻��̂ݍl��
        if (collision.gameObject.tag == playerTag)
        {
            // ��ԁ��G���A���ɂ��Ȃ�
            InArea = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // MainCamera�T��
        Camera = GameObject.Find("Main Camera");
        // �J�����X�N���v�g���擾
        Cam = Camera.GetComponent<Camera>();

        // Goal�T��
        goal = GameObject.Find("Goal");

        // Goal�̍��W�擾
        goalTransform = goal.GetComponent<Transform>();
        cameraTransform = Camera.GetComponent<Transform>();
        //Debug.Log(Camera);
    }

    // Update is called once per frame
    void Update()
    {
        // �S�[���C�x���g�͈͓̔��ɂ����
        if (InArea == true)
        {
            // �Y�[����̃J�����`��T�C�Y�ɂȂ�܂ŏ��X�ɃY�[���C�����Ă���
            if (NowCameraSize > ZoomCameraSize)
            {
                // �`��T�C�Y�v�Z
                NowCameraSize -= ChangeVolume * Time.deltaTime;
            }
            else
            {
                NowCameraSize = ZoomCameraSize;
            }

            // �J�����̈ʒu�̓G���A�̒��S�ɌŒ�
            cameraTransform = goalTransform;
        }
        else
        {
            // �f�t�H���g�̃J�����`��T�C�Y�ɂȂ�܂ŏ��X�ɃY�[���A�E�g���Ă���
            if (NowCameraSize < DefaultCameraSize)
            {
                // �`��T�C�Y�v�Z
                NowCameraSize += ChangeVolume * Time.deltaTime;
            }
            else
            {
                NowCameraSize = DefaultCameraSize;
            }
        }

        //�J�����T�C�Y�ύX
        Cam.orthographicSize = NowCameraSize;
    }
}
