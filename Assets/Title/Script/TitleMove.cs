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

    [SerializeField, Header("Button")]
    private SpriteRenderer _Button;

    private float a = 1.0f;

    SpriteRenderer MainSpriteRenderer;

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

            //Sesource.Play();
            Sesource.PlayOneShot(se_crush[FallNum - 1]);
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
            a -= 1.0f * Time.deltaTime;

            _Button.color = new Color(1.0f, 1.0f, 1.0f, a);
            TitleLogo.color = new Color(1.0f, 1.0f, 1.0f, a);

            if (a < 0.0f)
            {
                anim.SetInteger("Select", 4);
               
                player.transform.position = Vector3.MoveTowards(player.transform.position,
                    new Vector3(11.0f, player.transform.position.y, player.transform.position.z), 6.0f * Time.deltaTime);
            }

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
        