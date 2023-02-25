//---------------------------------------
//�S���ҁF��{
//���e�@�F�B��������UI�Ƃ��ĕ\��
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawNailNum : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    private Text TextNailNum; // �Q�[����ʂɕ`�悷��
    public int oldNailsNum; // �O�t���[���̓B�̐���ۑ�����

    // �O���擾
    private GameObject player; // �v���C���[�������ĕێ�����
    private HaveNails nails; // HaveNails��ێ�

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �v���C���[������
        player = GameObject.Find("player");

        // HaveNails���擾
        nails = player.GetComponent<HaveNails>();

        TextNailNum = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // �O�t���[���̓B�̐�����ϓ��������
        if(oldNailsNum != nails.NailsNum)
        {
            
        }

        //---------------------------------------------------------
        // �v���C���[����������B�̐����ĕ`��
        TextNailNum.text = string.Format("�~{0:00}", nails.NailsNum);

        // �Â��B�X�V
        oldNailsNum = nails.NailsNum;
    }
}