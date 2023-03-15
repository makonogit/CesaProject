//---------------------------------------
//�S���ҁF��{
//���e�@�F�N���X�^���l������\���A���A���^�C���X�V
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCrystalNum : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    private Text TextCrystalNum; // �Q�[����ʂɕ`�悷��
    public int oldCrystalNum; // �O�t���[���̃N���X�^���̐���ۑ�����

    // �O���擾
    private GameObject player; // �v���C���[�������ĕێ�����
    private HaveCrystal crystal; // HaveCrystal��ێ�

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �v���C���[������
        player = GameObject.Find("player");

        // HaveCrystal���擾
        crystal = player.GetComponent<HaveCrystal>();

        TextCrystalNum = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // �O�t���[���̓B�̐�����ϓ��������
        if (oldCrystalNum != crystal.CrystalNum)
        {
            //---------------------------------------------------------
            // �v���C���[����������B�̐����ĕ`��
            TextCrystalNum.text = string.Format("�~{0:00}", crystal.CrystalNum);
        }

        // �Â��B�X�V
        oldCrystalNum = crystal.CrystalNum;
    }
}
