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
    //private HaveCrystal crystal; // HaveCrystal��ێ�

    private PlayerStatas status;

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �v���C���[������
        player = GameObject.Find("player");

        // HaveCrystal���擾
        //crystal = player.GetComponent<HaveCrystal>();
        status = player.GetComponent<PlayerStatas>();

        TextCrystalNum = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // �O�t���[���̃N���X�^���̐�����ϓ��������
        if (oldCrystalNum != status.GetCrystal())
        {
            //---------------------------------------------------------
            // �v���C���[����������N���X�^���̐����ĕ`��
            TextCrystalNum.text = string.Format("�~{0:00}", status.GetCrystal());
        }

        // �Â��N���X�^���X�V
        oldCrystalNum = status.GetCrystal();
    }
}
