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

    private bool _BreakBlock = false;
    private float timer = 0f;

    // �O���擾
    private GameObject Player;
    private PlayerStatas statas;
    private CrackCreater order = null;
    private ParticleSystem BreakParticle;   //����G�t�F�N�g
    //private ParticleSystem CrystalParticle; //�N���X�^�����Q�b�g�����p�[�e�B�N��
    //private GameObject CrystalPoint;        //�N���X�^�����W�܂���W

    [SerializeField,Header("�Ȃ��Ȃ炢��Ȃ�")] private BoxCollider2D _boxCollider; // �X�ɂ��Ă�
    [SerializeField,Header("�Ȃ��Ȃ炢��Ȃ�")] private PolygonCollider2D _polygonCollider; // ��ɂ��Ă�

    private GameObject SEObj;               //SE�Đ��p�I�u�W�F�N�g
    private GimmickPlay_2 PlaySound;     //SE�Đ��p�X�N���v�g

    [SerializeField] private SpriteRenderer _spriteRenderer; // �X�v���C�g�����_���[

    [SerializeField] private Animator anim; // �A�j���[�^�[

    [SerializeField,Header("�X�u���b�N�̂ݕK�v")] private Material defaultMat;

    private GetCrystal getCrystal; // �N���X�^���擾���̃p�[�e�B�N���o���p

    private void Start()
    {
        SEObj = GameObject.Find("BlockSE");
        PlaySound = SEObj.GetComponent<GimmickPlay_2>();

        mat = GetComponent<SpriteRenderer>().material;

        Player = GameObject.Find("player");
        statas = Player.GetComponent<PlayerStatas>();
        BreakParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        //CrystalParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        //CrystalPoint = transform.GetChild(2).gameObject;

        _boxCollider = GetComponent<BoxCollider2D>();
        _polygonCollider = GetComponent<PolygonCollider2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (transform.childCount == 4)
        {
            Crystal = transform.GetChild(3).gameObject;
        }

        // �N���X�^�������ł������o�p
        getCrystal = GameObject.Find("GetCrystal").GetComponent<GetCrystal>();
        if (getCrystal == null) Debug.LogError("GetCrystal�R���|�[�l���g���擾�ł��܂���ł����B");
    }

    private void Update()
    {
        //�Đ���
        //if (CrystalParticle.isPlaying)
        //{
        //    CrystalPoint.transform.position = Player.transform.position;
        //}
        //else
        //{
        //    CrystalPoint.transform.position = new Vector3(-1000.0f, -1000.0f);
        //}

        // Func_BreakBlock���Ă΂ꂽ��true
        if(_BreakBlock == true)
        {
            if (CrystalNum > 0)
            {
                if (timer > 0.5f)
                {
                    // �N���X�^���̌����G�t�F�N�g����
                    getCrystal.Creat();

                    // ������
                    timer = 0f;

                    CrystalNum--;
                }

                timer += Time.deltaTime;
            }
            else
            {
                _BreakBlock = false;
            }
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
                if (order.State != CrackCreater.CrackCreaterState.CRAETED)
                {
                    if (Break == false)
                    {
                        // �Ђт�����
                        Destroy(collision.gameObject);

                        //if (CrystalNum > 0)
                        //{
                        //    CrystalPoint.transform.position = Player.transform.position;
                        //    CrystalParticle.Play();
                        //}

                        // ����u���b�N�̏����p�֐��Ăяo��
                        Func_BreakBlock();
                    }
                }
            }
        }
    }
    
    public void Func_BreakBlock()
    {
        //����p�[�e�B�N���̍Đ�
        BreakParticle.Play();

        if (tag == "Ice")
        {
            PlaySound.PlayerGimmickSE(GimmickPlay_2.GimmickSE2List.ICEBLOCK);

            // ����A�j���[�V�����J�n
            anim.SetBool("breakIce", true);
            // �f�t�H���g�̃}�e���A���ɖ߂�(�A�j���[�^�[�ŃA�j���[�V��������Ƃ��������Ȃ邽��)
            _spriteRenderer.material = defaultMat; // �f�t�H���g�}�e���A�����Z�b�g
        }
        else
        {
            PlaySound.PlayerGimmickSE(GimmickPlay_2.GimmickSE2List.ROCKBLOCK);
            // ����A�j���[�V�����J�n
            anim.SetBool("breakRock", true);
        }

        // �N���X�^���擾�G�t�F�N�g�p�t���Os
        _BreakBlock = true;

        if (Crystal != null)
        {
            Destroy(Crystal);
        }

        // �����蔻����I�t
        if (_boxCollider != null)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if (_polygonCollider != null)
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }

        Break = true;
    }

    public void Invisible()
    {
        // �s����
        _spriteRenderer.enabled = false;

        // �A�j���[�V�����I��
        anim.SetBool("breakIce", false);
        anim.SetBool("breakRock", false);
    }
}
