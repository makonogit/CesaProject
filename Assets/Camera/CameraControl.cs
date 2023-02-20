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

    // �O���擾
    public GameObject player; // �v���C���[�̃Q�[���I�u�W�F�N�g��ێ�
    private Vector3 offset; // �v���C���[�ƃJ�����̈ʒu�֌W��ێ�
    private Transform thisTransform; // �J�����̌��ݍ��W������

    Camera Cam; // �J�������擾����ϐ�

    // �ړ��͈͂̐����̑f
    GameObject LeftDown; 
    GameObject RightUp;

    //------------------------------------------------------------------------------------------------------
    //* ���������� *
    //------------------------------------------------------------------------------------------------------
    void Start()
    {
        //-----------------------------------------
        // �J�����̃T�C�Y���Ƃ邽�߂ɕK�v
        Cam = GetComponent<Camera>();

        //-----------------------------------------
        // �J�����̌��ݍ��W
        thisTransform = GetComponent<Transform>();

        // �Q�[���X�^�[�g���ł̃v���C���[�ƃJ�����̈ʒu�֌W���L��
        offset = thisTransform.position - player.transform.position;

        // �ړ��͈͂̑f�ƂȂ�Q�[���I�u�W�F�N�g��T��
        LeftDown = GameObject.Find("LeftDown");
        RightUp = GameObject.Find("RightUp");

        // �ړ��͈͐����ɕK�v�Ȏl���_�̍��W
        LeftPos = LeftDown.transform.position.x + Cam.orthographicSize * 2;
        DownPos = LeftDown.transform.position.y + Cam.orthographicSize;
        RightPos = RightUp.transform.position.x - Cam.orthographicSize * 2;
        UpPos = RightUp.transform.position.y - Cam.orthographicSize;
    }
    //------------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------------------
    // *�X�V�����̌�ɂ���鏈��*
    //------------------------------------------------------------------------------------------------------
    void LateUpdate()   // ���ׂẴQ�[���I�u�W�F�N�g��Update���\�b�h���Ăяo���ꂽ��Ɏ��s�����֐�
    {
        // �v���C���[�̌��݈ʒu����V�����J�����̈ʒu���쐬
        Vector3 vector = player.transform.position + offset;

        //------------------------------------------------------------------------------------------------------
        // ���[��荶�ɍs�����Ƃ�����J������x���W�Œ�
        if (vector.x <= LeftPos)
        {
            vector.x = LeftPos;
        }
        // �E�[���E�ɍs�����Ƃ�����J������x���W�Œ�
        else if (vector.x >= RightPos)
        {
            vector.x = RightPos;
        }

        // ���[��艺�ɍs�����Ƃ�����J������y���W�Œ�
        if (vector.y <= DownPos)
        {
            vector.y = DownPos;
        }
        // ��[����ɍs�����Ƃ�����J������y���W�Œ�
        else if (vector.y >= UpPos)
        {
            vector.y = UpPos;
        }

        // �c�����͌Œ�
        // vector.y = transform.position.y;
        // �J�����̈ʒu���ړ�
        thisTransform.position = vector;

    }


}
