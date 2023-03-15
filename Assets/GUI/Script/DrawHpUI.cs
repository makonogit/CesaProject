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
    private GameObject GUI; // HP�������Ă���Q�[���I�u�W�F�N�g
    private GameOver gameover; // HP�������Ă���X�N���v�g
    private GameObject parent; // �e�ƂȂ�Q�[���I�u�W�F�N�g
    private RectTransform parentTransform; // �e�ƂȂ�Q�[���I�u�W�F�N�g�̍��W
    public GameObject chirdren; // ��������I�u�W�F�N�g
    private Image img; // �摜��ύX���邽�߂̕ϐ�
    [SerializeField] Sprite[] sprites; // �摜�������Ă���

    GameObject[] objs;

    private GameOver.SPRITESTATUS oldSpriteStatus; // ��Ԃ��Ƃ��Ă��Ă���ɉ����ăX�v���C�g��ύX

    // Start is called before the first frame update
    void Start()
    {
        //---------------------------------------------------------
        // �Q�[���I�u�W�F�N�g�T��
        parent = GameObject.Find("Health");
        // �e�̍��W
        parentTransform = parent.GetComponent<RectTransform>();

        // GUI�T��
        GUI = GameObject.Find("GUI");
        // �X�N���v�g�擾
        gameover = GUI.GetComponent<GameOver>();

        objs = new GameObject[gameover.maxHp];

        // �ő�HP��hpUI�I�u�W�F�N�g���
        for (int i = 0; i < gameover.maxHp; i++)
        {
            objs[i] = Instantiate(chirdren, new Vector3(i * 1.0f, 0.0f, 0.0f), Quaternion.identity,parentTransform);
            objs[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(100 * i, 0);
        }

        // �\������Hp�p�ϐ��ɍő�HP����ď�����
        NowHp = gameover.maxHp;

        //��ԉE�[�ɂ���HP��Image�R���|�[�l���g���擾
        img = objs[gameover.maxHp - 1].GetComponent<Image>();

        // ������
        oldSpriteStatus = GameOver.SPRITESTATUS.HIGH;
    }

    // Update is called once per frame
    void Update()
    {
        // ���݂̏�Ԃ��擾
        GameOver.SPRITESTATUS SS = gameover.GetSpriteStatus();

        //--------------------------------------------------------
        // �O�̏�Ԃƌ��݂̏�Ԃ��������X�v���C�g��ύX����
        if (oldSpriteStatus != SS)
        {
            // ����if�̒��ɓ��� = UI�̌���������
            // �X�v���C�g��������HPUI��Image�R���|�[�l���g���Ď擾����
            if (SS == GameOver.SPRITESTATUS.HIGH)
            {
                img = objs[gameover.HP - 1].GetComponent<Image>();
                //Debug.Log(img);
            }
            else
            {
                // �z��sprites�� 2 or 3 �ڂ̃X�v���C�g�ɕύX
                img.sprite = sprites[(int)SS];
            }
        }

        // ��r�ϐ��X�V
        oldSpriteStatus = SS;

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

    
}
