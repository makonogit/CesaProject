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

    // Start is called before the first frame update
    void Start()
    {
        //�^�C�g����SptiteRenderere���擾
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        // ScreenBreak���擾
        BreakObj = GameObject.Find("ScreenBreak");
        _ScreenBreak = BreakObj.GetComponent<ScreenBreak>();

        BreakTime = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        //  �L�[����
        if (Input.GetKeyDown("joystick button 0"))
        {
            FallNum--;

            //-------------------------------------------------
            //�X�v���C�g��ύX
            if (FallNum > 0)
            {
                MainSpriteRenderer.sprite = TitleBackSprite[FallNum - 1];
            }
            //-------------------------------------------------

            //---------------------------------------------------
            // ����܂ł̃J�E���g�_�E����0�ɂȂ�����V�[���ړ�
            if (FallNum == 0)
            {
                _ScreenBreak.enabled = true;
            }

        }

        if (_ScreenBreak.enabled)
        {
            BreakTime += Time.deltaTime;
            //3�b�o�߂�����scene�ړ�
            if (BreakTime > 3.0f)
            {
                SceneManager.LoadScene("SelectScene");
            }
        }
    }

}
        