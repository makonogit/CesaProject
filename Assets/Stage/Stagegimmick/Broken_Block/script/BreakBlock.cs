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

    [SerializeField,Header("���ɓ����Ă���N���X�^���̐�")]
    private int CrystalNum;

    // �O���擾
    private GameObject Player;
    private PlayerStatas statas;
    private CrackCreater order = null;
    private ParticleSystem BreakParticle;   //����G�t�F�N�g
    private ParticleSystem CrystalParticle; //�N���X�^�����Q�b�g�����p�[�e�B�N��
    private GameObject CrystalPoint;        //�N���X�^�����W�܂���W

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;

        Player = GameObject.Find("player");
        statas = Player.GetComponent<PlayerStatas>();
        BreakParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        CrystalParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        CrystalPoint = transform.GetChild(2).gameObject;
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

            // �Ђѐ������Ȃ�
            if (order.State == CrackCreater.CrackCreaterState.CREATING ||
                order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
            {
                //Destroy(collision.gameObject);
                //����p�[�e�B�N���̍Đ�
                BreakParticle.Play();
                if(CrystalNum > 0)
                {
                    CrystalPoint.transform.position = Player.transform.position;
                    //Debug.Log(CrystalPoint);
                    CrystalParticle.Play();
                }
                // ����u���b�N�̏����p�֐��Ăяo��
                Func_BreakBlock();

                //�N���X�^����t�^
                statas.SetCrystal(statas.GetCrystal() + CrystalNum);
              
            }
        }
    }
    
    public void Func_BreakBlock()
    {
        // �����ɂ���
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);

        // �����蔻�������
        //GetComponent<BoxCollider2D>().enabled = false;
        Destroy(GetComponent<BoxCollider2D>());
    }
}
