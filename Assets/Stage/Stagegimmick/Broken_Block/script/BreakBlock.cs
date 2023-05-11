//-----------------------------
//�S���F��{��
//���e�F�ЂтƐڐG����Ɖ���u���b�N
//-----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    // �ϐ��錾

    // �ЂуI�u�W�F�N�g�̃^�O��
    private string CrackTag = "Crack";

    // ���̃Q�[���I�u�W�F�N�g�̃}�e���A����ێ�����ϐ�
    private Material mat;

    private GameObject Crystal;

    [SerializeField,Header("���ɓ����Ă���N���X�^���̐�")]
    private int CrystalNum;

    public bool Break = false;

    // �O���擾
    private GameObject Player;
    private PlayerStatas statas;
    private CrackCreater order = null;
    private ParticleSystem BreakParticle;   //����G�t�F�N�g
    private ParticleSystem CrystalParticle; //�N���X�^�����Q�b�g�����p�[�e�B�N��
    private GameObject CrystalPoint;        //�N���X�^�����W�܂���W

    private GameObject SEObj;               //SE�Đ��p�I�u�W�F�N�g
    private GimmickPlay_2 PlaySound;     //SE�Đ��p�X�N���v�g

    private void Start()
    {
        SEObj = GameObject.Find("BlockSE");
        PlaySound = SEObj.GetComponent<GimmickPlay_2>();

        mat = GetComponent<SpriteRenderer>().material;

        Player = GameObject.Find("player");
        statas = Player.GetComponent<PlayerStatas>();
        BreakParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        CrystalParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        CrystalPoint = transform.GetChild(2).gameObject;

        if (transform.childCount == 4)
        {
            Crystal = transform.GetChild(3).gameObject;
        }
    }

    private void Update()
    {
        //�Đ���
        if (CrystalParticle.isPlaying)
        {
            CrystalPoint.transform.position = Player.transform.position;
        }
        else
        {
            CrystalPoint.transform.position = new Vector3(-1000.0f, -1000.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �Ђтɂ���������
        if (collision.gameObject.tag == CrackTag)
        {
            // ���������Ђт�CrackOrder���擾
            order = collision.gameObject.GetComponent<CrackCreater>();

            if (order != null)
            {
                // �Ђѐ������Ȃ�
                if (order.State == CrackCreater.CrackCreaterState.CREATING ||
                    order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
                {
                    if (Break == false)
                    {
                        // �Ђт�����
                        Destroy(collision.gameObject);

                        //����p�[�e�B�N���̍Đ�
                        BreakParticle.Play();
                        if (CrystalNum > 0)
                        {
                            CrystalPoint.transform.position = Player.transform.position;
                            CrystalParticle.Play();
                        }

                        // ����u���b�N�̏����p�֐��Ăяo��
                        Func_BreakBlock();

                        //�N���X�^����t�^
                        statas.SetCrystal(statas.GetCrystal() + CrystalNum);
                    }
                }
            }
        }
    }
    
    public void Func_BreakBlock()
    {
        if(tag == "Ice")
        {
            PlaySound.PlayerGimmickSE(GimmickPlay_2.GimmickSE2List.ICEBLOCK);
        }
        else
        {
            PlaySound.PlayerGimmickSE(GimmickPlay_2.GimmickSE2List.ROCKBLOCK);
        }
        // �����ɂ���
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);
        if (Crystal != null)
        {
            Destroy(Crystal);
        }

        // �����蔻�������
        //GetComponent<BoxCollider2D>().enabled = false;
        Destroy(GetComponent<PolygonCollider2D>());
        Destroy(GetComponent<BoxCollider2D>());

        Break = true;
    }
}
