//---------------------------------------
//�S���ҁF��{
//���e�@�F�v���C���[�����B�̐����Ǘ�
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveNails : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -
    private string NailTag = "Nail"; //�^�O��

    [Header("�B������")]
    public int NailsNum = 0; // �����Ă���B�̐�

    //�����Ă���B�ɐG���ƓB��������������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // �G�ꂽ�̂��A�C�e���Ƃ��Ă̓B�Ȃ�B������������

        // �^�O���B�Ȃ�
        if (collision.tag == NailTag)
        {
            NailsNum++;
            // �A�C�e���Ƃ��Ă̓B�͏���
            Destroy(collision.gameObject);
        }

    }
}
