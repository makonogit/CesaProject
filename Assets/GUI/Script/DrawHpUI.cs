//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�FGUI�I�u�W�F�N�g����HP���Ƃ��Ă��āA�`�悷��
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    GameObject[] objs;

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
    }

    // Update is called once per frame
    void Update()
    {
        //---------------------------------------------------------
        // Hp�ɕϓ������������ɕ`�揈���̍X�V������
        //// HP���ւ���
        //if (NowHp > gameover.maxHp)
        //{
        //    // �ϓ�������ꏊ���\��
        //    objs[gameover.HP].SetActive(false);
        //}
        //// HP��������
        //else if(NowHp < gameover.maxHp)
        //{
        //    // �ϓ�������ꏊ��\��
        //    objs[gameover.HP].SetActive(true);
        //}

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

        // HP���ŐV�̒l�ɂ���
        //NowHp = gameover.maxHp;

        //Debug.Log(parentTransform.anchoredPosition);
    }

    
}
