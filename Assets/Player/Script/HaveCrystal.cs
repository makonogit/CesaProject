//---------------------------------------
//�S���ҁF��{
//���e�@�F�v���C���[���擾�����N���X�^���̐����Ǘ�
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveCrystal : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -
    private string CrystalTag = "Crystal"; //�^�O��

    [Header("�N���X�^��������")]
    public int CrystalNum = 0; // �����Ă���B�̐�

    // Update is called once per frame
    void Update()
    {
        
    }

    //�����Ă���N���X�^���ɐG���ƃN���X�^����������������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // �G�ꂽ�̂��A�C�e���Ƃ��ẴN���X�^���Ȃ�N���X�^��������������

        // �^�O���B�Ȃ�
        if (collision.tag == CrystalTag)
        {
            CrystalNum++;
            // �A�C�e���Ƃ��ẴN���X�^���͏���
            Destroy(collision.gameObject);
        }

    }
}
