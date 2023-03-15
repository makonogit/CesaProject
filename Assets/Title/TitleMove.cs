using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMove : MonoBehaviour
{
    public int FallNum; // �^�C�g����ʂ����󂷂�܂ł̐�

    string TitleBackName;   //�@�w�i�̃I�u�W�F�N�g��

    SpriteRenderer MainSpriteRenderer;

    public List<Sprite> TitleBackSprite;

    // Start is called before the first frame update
    void Start()
    {
        //�^�C�g����SptiteRenderere���擾
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

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
                SceneManager.LoadScene("SelectScene");
            }

        }

    }
}
