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
    [SerializeField] private PlayerStatas status;

    private CrystalNum Crystal;

    [SerializeField] private Animator anim1;
    [SerializeField] private Animator anim2;

    // Update is called once per frame
    void Update()
    {
        //player = GameObject.Find("player");
        //status = player.GetComponent<PlayerStatas>();
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

                // �N���X�^���擾���̃A�j���[�V����
                anim1.SetBool("get", true);
                anim2.SetBool("get", true);

                // ���ɃA�j���[�V�������Ă�����
                if (anim1.GetBool("get"))
                {
                    // �n�߂���
                    anim1.Play("AccentNumber", 0, 0);
                    anim2.Play("AccentNumber", 0, 0);
                }
            }
        }
    }
}
