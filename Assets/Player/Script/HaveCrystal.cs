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
    private int CrystalNum = 0; // �����Ă���B�̐�

    // �O���擾
    private GameObject player;
    private PlayerStatas status;

    private CrystalNum Crystal;

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("player");
        status = player.GetComponent<PlayerStatas>();
    }

    //�����Ă���N���X�^���ɐG���ƃN���X�^����������������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //---------------------------------------------------------
        // �G�ꂽ�̂��A�C�e���Ƃ��ẴN���X�^���Ȃ�N���X�^��������������

        // �^�O���N���X�^���Ȃ�
        if (collision.tag == CrystalTag)
        {
            Crystal = collision.GetComponent<CrystalNum>();
            //Debug.Log(Crystal.Get);

            // �擾�ς݂łȂ��Ȃ�
            if (Crystal.Get == false)
            {
                // �N���X�^���������𑝂₷
                status.SetCrystal(status.GetCrystal() + Crystal.crystalNum);
                // �擾�ς݃t���O
                collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;

                Crystal.Get = true;
            }
        }
    }
}
