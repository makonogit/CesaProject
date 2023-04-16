//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�FGUI�I�u�W�F�N�g����HP���Ƃ��Ă��āA�`�悷��
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawHpUI : MonoBehaviour
{
    //---------------------------------------------------------
    // - �ϐ��錾 -

    private int NowHp; // ���݂�HP����

    // �O���擾
    private GameObject player; // HP�������Ă���Q�[���I�u�W�F�N�g
    private GameOver gameover; // HP�������Ă���X�N���v�g
    private GameObject parent; // �e�ƂȂ�Q�[���I�u�W�F�N�g
    private RectTransform parentTransform; // �e�ƂȂ�Q�[���I�u�W�F�N�g�̍��W
    public GameObject chirdren; // ��������I�u�W�F�N�g

    // �A�j���[�V�����p
    private Image img; // �摜��ύX���邽�߂̕ϐ�
    [SerializeField] Sprite[] sprites; // �摜�������Ă���
    private int NowHPAnimationNumber = 0; // sprites�̓Y�����p�ϐ�
    private bool isHPUIAnimation = false; // HPUI�̃A�j���[�V���������邩
    public float NextImageTime = 0.1f; // ���̉摜�ɕς��܂ł̃^�C�}�[
    private float ChangeImageTimer = 0f; // �O�̉摜�ɕς���Ă���̌o�ߎ���

    GameObject[] objs;

    //private GameOver.SPRITESTATUS oldSpriteStatus; // ��Ԃ��Ƃ��Ă��Ă���ɉ����ăX�v���C�g��ύX

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �Q�[���I�u�W�F�N�g�T��
        parent = GameObject.Find("Health");
        // �e�̍��W
        parentTransform = parent.GetComponent<RectTransform>();

        // GUI�T��
        player = GameObject.Find("player");
        // �X�N���v�g�擾
        gameover = player.GetComponent<GameOver>();

        objs = new GameObject[gameover.maxHp];

        // �ő�HP��hpUI�I�u�W�F�N�g���
        for (int i = 0; i < gameover.maxHp; i++)
        {
            objs[i] = Instantiate(chirdren, new Vector3(i * 1.0f, 0.0f, 0.0f), Quaternion.identity,parentTransform);
            objs[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(100 * i, 0);
        }

        // �\������Hp�p�ϐ��ɍő�HP����ď�����
        //NowHp = gameover.maxHp;

        // ��ԉE�[�ɂ���HP��Image�R���|�[�l���g���擾
        img = objs[gameover.maxHp - 1].GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        ////--------------------------------------------------------
        //// �O�̏�Ԃƌ��݂̏�Ԃ��������X�v���C�g��ύX����
        //if (oldSpriteStatus != SS)
        //{
        //    // ����if�̒��ɓ��� = UI�̌���������
        //    // �X�v���C�g��������HPUI��Image�R���|�[�l���g���Ď擾����
        //    if (SS == GameOver.SPRITESTATUS.HIGH)
        //    {
        //        img = objs[gameover.HP - 1].GetComponent<Image>();
        //        //Debug.Log(img);
        //    }
        //    else
        //    {
        //        // �z��sprites�� 2 or 3 �ڂ̃X�v���C�g�ɕύX
        //        img.sprite = sprites[(int)SS];
        //    }
        //}

        //// ��r�ϐ��X�V
        //oldSpriteStatus = SS;

        // �A�j���[�V�����w�����o�Ă�����
        if(isHPUIAnimation == true)
        {
            HPUI_Animation();
        }

        //---------------------------------------------------------
        // ���݂�HP���n�[�g��\������
        
        for(int i = 0;i< gameover.maxHp; i++)
        {
            if (gameover.HP >= i + 1)
            {
                objs[i].SetActive(true);
            }
            else
            {
                objs[i].SetActive(false);
            }
        }
    }

    // HPUI�̃A�j���[�V�����p�֐�
    public void HPUI_Animation()
    {
        // ��莞�Ԃ��Ƃɉ摜��ύX���ăA�j���[�V����������

        // ��莞�Ԍo�߂�����
        if(ChangeImageTimer >= NextImageTime)
        {
            // ���̉摜
            NowHPAnimationNumber++;
            if (NowHPAnimationNumber <= 4)
            {
                // ���̉摜�ɐ؂�ւ�
                img.sprite = sprites[NowHPAnimationNumber];
            }

            // ������
            ChangeImageTimer = 0f;
        }

        // �Ō�̃A�j���[�V�����摜�ɂȂ��Ă����莞�Ԍo�߂�����
        if (NowHPAnimationNumber >= 5) 
        {
            // HP���炷
            gameover.DecreaseHP(1f);

            // ���̃A�j���[�V�����Ώېݒ�
            img = objs[gameover.HP - 1].GetComponent<Image>();

            // �A�j���[�V�����I��
            isHPUIAnimation = false;

            // ������
            NowHPAnimationNumber = 0;
        }

        ChangeImageTimer += Time.deltaTime;
    }

    public void Set_HPAnim(bool _set)
    {
        isHPUIAnimation = _set;
    }
}


