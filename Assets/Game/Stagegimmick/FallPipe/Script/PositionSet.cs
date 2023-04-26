//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F��ԍ��̃p�C�v����Ƀp�C�v�ƃ��j�I���N���X�^���̍��W���Z�b�g
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSet : MonoBehaviour
{
    // �ϐ��錾

    // �O���擾

    // ���̃p�C�v���
    private GameObject LeftPipe;
    private Transform LeftPipeTransform;

    // �����̃p�C�v���
    private GameObject MiddlePipe;
    private Transform MiddlePipeTransform;

    // �E�̃p�C�v���
    private GameObject RightPipe;
    private Transform RightPipeTransform;

    // �ڍ����N���X�^���}�l�[�W���[
    private GameObject CrystalManager;

    // ���̐ڍ����N���X�^�����
    private GameObject LeftUnionCrystal;
    private Transform LeftUnionCrystalTransform;

    // �E�̐ڍ����N���X�^�����
    private GameObject RightUnionCrystal;
    private Transform RightUnionCrystalTransform;

    // Start is called before the first frame update
    void Start()
    {
        // �q�I�u�W�F�N�g�����ԂɎ擾

        // ���p�C�v
        LeftPipe = transform.GetChild(0).gameObject;
        LeftPipeTransform = LeftPipe.GetComponent<Transform>();
        //Debug.Log(LeftPipe);

        // �E�p�C�v
        RightPipe = transform.GetChild(1).gameObject;
        RightPipeTransform = RightPipe.GetComponent<Transform>();
        //Debug.Log(RightPipe);

        // �����p�C�v
        MiddlePipe = transform.GetChild(2).gameObject;
        MiddlePipeTransform = MiddlePipe.GetComponent<Transform>();
        //Debug.Log(MiddlePipe);

        // �N���X�^���}�l�[�W���[
        CrystalManager = transform.GetChild(3).gameObject;
        //Debug.Log(CrystalManager);

        // �ڍ����N���X�^���i���j
        LeftUnionCrystal = CrystalManager.transform.GetChild(0).gameObject;
        LeftUnionCrystalTransform = LeftUnionCrystal.GetComponent<Transform>();
        //Debug.Log(LeftUnionCrystal);

        // �ڍ����N���X�^���i�E�j
        RightUnionCrystal = CrystalManager.transform.GetChild(1).gameObject;
        RightUnionCrystalTransform = RightUnionCrystal.GetComponent<Transform>();
        //Debug.Log(RightUnionCrystal);

        // ���W�Z�b�g
        Init_Position();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init_Position()
    {
        // ��ԍ��̃p�C�v����ɔz�u���Ă���

        // �����̃p�C�v�z�u
        MiddlePipeTransform.localPosition = new Vector3(
            LeftPipeTransform.localPosition.x + LeftPipeTransform.localScale.x,
            LeftPipeTransform.localPosition.y,
            LeftPipeTransform.localPosition.z);

        // �E�̃p�C�v�z�u
        RightPipeTransform.localPosition = new Vector3(
            MiddlePipeTransform.localPosition.x + MiddlePipeTransform.localScale.x,
            MiddlePipeTransform.localPosition.y,
            MiddlePipeTransform.localPosition.z);

        // ���̃N���X�^���z�u
        LeftUnionCrystalTransform.localPosition = new Vector3(
            LeftPipeTransform.localPosition.x + LeftPipeTransform.localScale.x / 2,
            LeftPipeTransform.localPosition.y,
            LeftPipeTransform.localPosition.z);

        // �E�̃N���X�^���z�u
        RightUnionCrystalTransform.localPosition = new Vector3(
            MiddlePipeTransform.localPosition.x + MiddlePipeTransform.localScale.x / 2,
            MiddlePipeTransform.localPosition.y,
            MiddlePipeTransform.localPosition.z);
    }
}
