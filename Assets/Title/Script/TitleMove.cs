using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMove : MonoBehaviour
{

    public int FallNum; // �^�C�g����ʂ����󂷂�܂ł̐�

    string TitleBackName;   //�@�w�i�̃I�u�W�F�N�g��

    //-----------
    // �O���擾
    //-----------
    private GameObject BreakObj;     //ScreenBreak���������Ă���I�u�W�F�N�g
    private ScreenBreak _ScreenBreak; //ScreenBreak���擾����ϐ�

    [SerializeField, Header("TitleLogo")]
    private SpriteRenderer TitleLogo;
    [SerializeField] private Transform _logoTransform;

    [SerializeField, Header("Button")]
    private SpriteRenderer _Button;

    public List<Sprite> TitleBackSprite;

    private float BreakTime;        //��ʂ����󂷂�܂ł̎���

    //----------------------------------------
    //�ǉ��F��
    GameObject CrystalObj;      //�N���X�^���̔w�i�I�u�W�F�N�g
    SpriteRenderer CrystalRenderer;     //�N���X�^����Render
    float CrystalAlpha;                 //�N���X�^���̓����x

    GameObject BGM;          // BGM�p�I�u�W�F�N�g   
    AudioSource Bgmsource;   // BGM

    GameObject SE;          // SE�p�I�u�W�F�N�g   
    AudioSource Sesource;   // SE

    GameObject player;      // ���o�p��l��
    Animator anim;          // �A�j���[�V����

    // ��{�ǉ�
    public AudioClip se_start1; // ����
    public AudioClip se_start2; // ����
    public AudioClip se_startcrush; // �����

    [SerializeField] private List<Sprite> SpriteList = new List<Sprite>();
    [SerializeField] private GameObject _CreackEffect; // �����G�t�F�N�g

    private AudioClip[] se_crush = new AudioClip[4];

    //�\�ǉ��S���ҁF���쒼�o�\//
    [SerializeField, Header("�p�[�e�B�N��")]
    private ParticleSystem _particle;
    //�\�\�\�\�\�\�\�\�\�\�\�\//
    

    // Start is called before the first frame update
    void Start()
    {
        // ScreenBreak���擾
        BreakObj = GameObject.Find("ScreenBreak");
        _ScreenBreak = BreakObj.GetComponent<ScreenBreak>();

        BreakTime = 0.0f;

        //--------------------------------------------
        //�N���X�^���̏�Ԃ��擾
        CrystalObj = GameObject.Find("Crystal");
        CrystalRenderer = CrystalObj.GetComponent<SpriteRenderer>();
        CrystalAlpha = CrystalRenderer.color.a;
        //�^�C�g����SptiteRenderere���擾
        //MainSpriteRenderer = .GetComponent<SpriteRenderer>();

        //--------------------------------------------
        // ���o�pAnimator���擾
        player = GameObject.Find("UIPlayerWalk");
        anim = player.GetComponent<Animator>();
        anim.SetInteger("Select", -1);

        //--------------------------------------------
        //�@SE�̏����擾
        SE = GameObject.Find("SE");
        Sesource = SE.GetComponent<AudioSource>();

        //--------------------------------------------
        //�@BGM�̏����擾
        BGM = GameObject.Find("BGM");
        Bgmsource = BGM.GetComponent<AudioSource>();

        se_crush[3] = se_start1;
        se_crush[2] = se_start2;
        se_crush[1] = se_start1;
        se_crush[0] = se_startcrush;
    }

    // Update is called once per frame
    void Update()
    {
        //  �L�[����
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))
        {

            if (FallNum > 0)
            {
                //Sesource.Play();
                Sesource.PlayOneShot(se_crush[FallNum - 1]);
            }
            //-------------------------------------------------
            //�X�v���C�g��ύX
            if (FallNum > 0)
            {
                CrystalAlpha += (1.0f / 255.0f) * 20;
                CrystalRenderer.color = new Color(1.0f, 1.0f, 1.0f, CrystalAlpha);
                //�\�ǉ��S���ҁF���쒼�o�\//
                Instantiate(_particle);
                //�\�\�\�\�\�\�\�\�\�\�\�\//
                CrystalRenderer.sprite = TitleBackSprite[FallNum - 1];

                if(FallNum == 3)
                {
                    // �X�v���C�g�ύX
                    TitleLogo.sprite = SpriteList[1];
                }
                else if(FallNum == 2)
                {
                    // �X�v���C�g�ύX
                    TitleLogo.sprite = SpriteList[0];

                    // �G�t�F�N�g����
                    var Obj = Instantiate(_CreackEffect, _logoTransform.transform);
                    var Pos = Obj.transform.position;
                    // T�̈ʒu��ւ�Ƀp�[�e�B�N������
                    Obj.transform.position = new Vector3(Pos.x + 2.83f, Pos.y - 0.86f, Pos.z);
                }else if(FallNum == 1)
                {
                    // �c�����^�C�g�����S����
                    TitleLogo.enabled = false;
                    // Press Key������
                    _Button.enabled = false;

                    // �G�t�F�N�g��ڐ���
                    var Obj1 = Instantiate(_CreackEffect, _logoTransform.transform);
                    var Pos1 = Obj1.transform.position;
                    // K�̈ʒu��ւ�Ƀp�[�e�B�N������
                    Obj1.transform.position = new Vector3(Pos1.x, Pos1.y - 0.86f, Pos1.z);

                    // �G�t�F�N�g��ڐ���
                    var Obj2 = Instantiate(_CreackEffect, _logoTransform.transform);
                    var Pos2 = Obj2.transform.position;
                    // P�̈ʒu��ւ�Ƀp�[�e�B�N������
                    Obj2.transform.position = new Vector3(Pos2.x - 2.4f, Pos2.y - 0.86f, Pos2.z);

                    // �G�t�F�N�g�O�ڐ���
                    var Obj3 = Instantiate(_CreackEffect, _logoTransform.transform);
                    var Pos3 = Obj3.transform.position;
                    // Key�̈ʒu��ւ�Ƀp�[�e�B�N������
                    Obj3.transform.position = new Vector3(Pos3.x, Pos3.y - 3.82f, Pos3.z);
                }
            }
            //-------------------------------------------------
            FallNum--;

        }

        //---------------------------------------------------
        // ����܂ł̃J�E���g�_�E����0�ɂȂ�����V�[���ړ�
        if (FallNum == 0)
        {
            //CrystalRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            _ScreenBreak.enabled = true;
        }

        if (_ScreenBreak.enabled)
        {
            anim.SetInteger("Select", 4);
            // �v���C���[���E�Ɉړ����Ă���
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                new Vector3(11.0f, player.transform.position.y, player.transform.position.z), 6.0f * Time.deltaTime);

            if (player.transform.position.x > 10.7f)
            {
                BreakTime += Time.deltaTime;

                // BGM���t�F�[�h�A�E�g������
                Bgmsource.volume -= 0.5f * Time.deltaTime;
                //Bgmsource.volume = 0;

                GameObject.Find("SceneManager").GetComponent<SceneChange>().LoadScene("newSelectScene");

                ////3�b�o�߂�����scene�ړ�
                //if (BreakTime > 3.0f)
                //{
                //    SceneManager.LoadScene("newSelectScene");//�\�ύX�S���ҁF���쒼�o�\//SelectScene����newSelectScene�ɕύX
                //}
            }
        }

       
    }

}
        