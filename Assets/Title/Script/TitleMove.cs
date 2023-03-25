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

    // Start is called before the first frame update
    void Start()
    {
        //�^�C�g����SptiteRenderere���擾
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        // ScreenBreak���擾
        BreakObj = GameObject.Find("ScreenBreak");
        _ScreenBreak = BreakObj.GetComponent<ScreenBreak>();

        BreakTime = 0.0f;

        //--------------------------------------------
        //�N���X�^���̏�Ԃ��擾
        CrystalObj = GameObject.Find("Crystal");
        CrystalRenderer = CrystalObj.GetComponent<SpriteRenderer>();
        CrystalAlpha = CrystalRenderer.color.a;

        //--------------------------------------------
        //�@SE�̏����擾
        SE = GameObject.Find("SE");
        Sesource = SE.GetComponent<AudioSource>();

        //--------------------------------------------
        //�@BGM�̏����擾
        BGM = GameObject.Find("BGM");
        Bgmsource = BGM.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //  �L�[����
        if (Input.GetKeyDown("joystick button 0"))
        {
            Sesource.Play();
            FallNum--;

            //-------------------------------------------------
            //�X�v���C�g��ύX
            if (FallNum > 0)
            {
                CrystalAlpha += (1.0f / 255.0f) * 20;
                CrystalRenderer.color = new Color(1.0f, 1.0f, 1.0f, CrystalAlpha);
               // MainSpriteRenderer.sprite = TitleBackSprite[FallNum - 1];
            }
            //-------------------------------------------------

            //---------------------------------------------------
            // ����܂ł̃J�E���g�_�E����0�ɂȂ�����V�[���ړ�
            if (FallNum == 0)
            {
                CrystalRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                _ScreenBreak.enabled = true;
            }

        }

        if (_ScreenBreak.enabled)
        {
            BreakTime += Time.deltaTime;

            // BGM���t�F�[�h�A�E�g������
            //Bgmsource.volume -= 0.5f * Time.deltaTime;
            Bgmsource.volume = 0;
            
            //3�b�o�߂�����scene�ړ�
            if (BreakTime > 3.0f)
            {
                SceneManager.LoadScene("SelectScene");
            }
        }
    }

}
        