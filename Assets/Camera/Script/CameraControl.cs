//---------------------------------------
//�S���ҁF���Ԑ^���q(�ǉ���{)
//���e�F�J�����̒Ǐ]
//�ǉ��F�J�����̈ړ��͈͐���
//---------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    public float LeftPos;  // ���[
    public float RightPos; // �E�[
    public float DownPos;  // ���[
    public float UpPos;    // ��[

    // �ȉ���_�J�����p�ϐ�
    private int ScreenNum = 0; // �����牽�ڂ̒�_�J�������@�z��̓Y�����ɗ��p����@�Y�����ၨ(2 * ScreenNum + 1)

    // �J�����̈ړ��͈͂̐����̑f�̖��O������
    private string[] CameraInfoName = 
        { "LeftDown1", "RightUp1",
          "LeftDown2", "RightUp2"}; 

    // �O���擾
    private GameObject target; // �J�������Ǐ]����Q�[���I�u�W�F�N�g��ێ�
    //private Vector3 offset; // �v���C���[�ƃJ�����̈ʒu�֌W��ێ�
    private Transform targetTransform; // �^�[�Q�b�g�̍��W
    private Transform thisTransform; // �J�����̌��ݍ��W������

    Camera Cam; // �J�������擾����ϐ�

    // �ړ��͈͂̐����̑f
    GameObject LeftDown; 
    GameObject RightUp;

    private GameObject CameraArea;
    private CameraZoom zoom;

    // �ȉ���_�J�����p�O���擾

    //------------------------------------------------------------------------------------------------------
    //* ���������� *
    //------------------------------------------------------------------------------------------------------
    void Start()
    {
        // �ŏ��̒Ǐ]�^�[�Q�b�g�̓v���C���[
        target = GameObject.Find("player");

        // �^�[�Q�b�g���ݒ肳���x�Ɏ擾���Ȃ���
        targetTransform = target.GetComponent<Transform>();

        // �Y�[���G���A�̃I�u�W�F�N�g�T��
        CameraArea = GameObject.Find("GoalArea");
        // �Y�[���X�N���v�g�擾
        zoom = CameraArea.GetComponent<CameraZoom>();

        //-----------------------------------------
        // �J�����̃T�C�Y���Ƃ邽�߂ɕK�v
        Cam = GetComponent<Camera>();

        //-----------------------------------------
        // �J�����̌��ݍ��W
        thisTransform = GetComponent<Transform>();

        // �Q�[���X�^�[�g���ł̃v���C���[�ƃJ�����̈ʒu�֌W���L��
        //offset = thisTransform.position - target.transform.position;

        // �ړ��͈͂̑f�ƂȂ�Q�[���I�u�W�F�N�g��T��
        LeftDown = GameObject.Find(CameraInfoName[2*ScreenNum]);    // ���̏ꍇLeftDown1
        RightUp = GameObject.Find(CameraInfoName[2*ScreenNum + 1]); // ���̏ꍇRightUp1
    }
    //------------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------------------
    // *�X�V�����̌�ɂ���鏈��*
    //------------------------------------------------------------------------------------------------------
    void LateUpdate()   // ���ׂẴQ�[���I�u�W�F�N�g��Update���\�b�h���Ăяo���ꂽ��Ɏ��s�����֐�
    {
        //------------------------------------------------------------------------------------------------------
        // �G���A���ɂ��邩���Ȃ����ŒǏ]�^�[�Q�b�g��ς���
        if (zoom.InArea == true)
        {
            // �G���A���Ń^�[�Q�b�g���v���C���[�Ȃ�
            if(target.name == "player")
            {
                // �^�[�Q�b�g��ύX
                target = GameObject.Find("GoalArea");
                targetTransform = target.transform;
            }
        }
        else
        {
            // �G���A�O�Ń^�[�Q�b�g���S�[���G���A�Ȃ�
            if (target.name == "GoalArea")
            {
                // �^�[�Q�b�g��ύX
                target = GameObject.Find("player");
                targetTransform = target.transform;
            }
        }

        //------------------------------------------------------------------------------------------------------
        // �J�����̒Ǐ] ���@�J�����؂�ւ�

        // �ړ��͈͐����ɕK�v�Ȏl���_�̍��W
        LeftPos = LeftDown.transform.position.x;
        DownPos = LeftDown.transform.position.y;
        RightPos = RightUp.transform.position.x;
        UpPos = RightUp.transform.position.y;

        // �^�[�Q�b�g���v���C���[�̎�
        if (target.name == "player")
        {
            // �v���C���[���J�����[����ł���ڂ��Ă���J�����؂�ւ�
            // �i���m�ɂ̓J�����̈ړ������͈̔͂�ς��Ă��邾��

            int temp = ScreenNum; // ��������p

            // �v���C���[�������Ă���͈͂̍��[����o���Ƃ�
            if (targetTransform.position.x < LeftPos)
            {
                // �J�����؂�ւ�
                ScreenNum--;
            }
            else if (targetTransform.position.x > RightPos)
            {
                // �J�����؂�ւ�
                ScreenNum++;
            }

            // �J�������؂�ւ���Ă�����
            if(temp != ScreenNum)
            {
                // �ړ������͈̔͂��؂�ւ�
                // �ړ��͈͂̑f�ƂȂ�Q�[���I�u�W�F�N�g��T��
                LeftDown = GameObject.Find(CameraInfoName[2 * ScreenNum]);
                RightUp = GameObject.Find(CameraInfoName[2 * ScreenNum + 1]);
            }
        }

        // �v���C���[�̌��݈ʒu����V�����J�����̈ʒu���쐬
        Vector3 vector = targetTransform.position;

        //------------------------------------------------------------------------------------------------------
        // ���[��荶�ɍs�����Ƃ�����J������x���W�Œ�
        if (vector.x - Cam.orthographicSize * 2 <= LeftPos)
        {
            vector.x = LeftPos + Cam.orthographicSize * 2;
        }
        // �E�[���E�ɍs�����Ƃ�����J������x���W�Œ�
        else if (vector.x + Cam.orthographicSize * 2 >= RightPos)
        {
            vector.x = RightPos - Cam.orthographicSize * 2;
        }

        // ���[��艺�ɍs�����Ƃ�����J������y���W�Œ�
        if (vector.y - Cam.orthographicSize <= DownPos)
        {
            vector.y = DownPos + Cam.orthographicSize;
        }
        // ��[����ɍs�����Ƃ�����J������y���W�Œ�
        else if (vector.y + Cam.orthographicSize >= UpPos)
        {
            vector.y = UpPos - Cam.orthographicSize;
        }

        // �c�����͌Œ�
        // vector.y = transform.position.y;
        // �J�����̈ʒu���ړ�
        thisTransform.position = new Vector3(vector.x,vector.y,thisTransform.position.z);
    }
}
