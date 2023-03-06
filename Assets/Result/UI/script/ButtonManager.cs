//----------------------------------------------------------------
// �S���ҁF�����V�S
// ���e�@�F���U���g��ʂ̃{�^��
//----------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    //=================================================
    // - �ϐ� -

    [Header("�\������{�^��")]
    public GameObject[] button = new GameObject[3];// �\������I�u�W�F�N�g

    // �{�^���p
    Vector3 scale;       // �{�^���̑傫��
    int selectButton = 0;// ���ݑI������Ă���{�^���̔ԍ�

    // ���͗p
    private GameObject PlayerInputMana;         // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�
    bool stickTrigger;                          // �X�e�b�N�̓��͏��
    int buttonTim = 0;                          // �{�^���̓��͎���

    //=================================================
    // - ���������� -

    void Start()
    {
        //--------------------------------------
        // �C���v�b�g�}�l�[�W���[���擾����

        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        //--------------------------------------
    }

    //=================================================
    // - �X�V���� -

    void Update()
    {
        //-----------------------------------------------
        // L�X�e�b�N�̍��E���͂Ń{�^����I������

        Vector2 inputStick = ScriptPIManager.GetMovement();

        if(stickTrigger == false)
        {
            // �E����
            if (inputStick.x > 0)
            {
                selectButton++;

                if (selectButton > 2)
                    selectButton = 0;

                stickTrigger = true;
            }

            // ������
            if (inputStick.x < 0)
            {
                selectButton--;

                if (selectButton < 0)
                    selectButton = 2;

                stickTrigger = true;
            }
        }
        else
        {
            // �E�̔����
            if (inputStick.x == 0)
            {

                stickTrigger = false;
            }

            // ���̔����
            if (inputStick.x == 0)
            {

                stickTrigger = false;
            }
        }

        //-----------------------------------------------
        // �{�^���̃T�C�Y�𓝈ꂷ��

        for (int i = 0; i < 3; i++)
        {
            scale.x = 1.0f;
            scale.y = 0.5f;
            button[i].transform.localScale = scale;
        }

        //-----------------------------------------------
        // �I������Ă���{�^�������傫������

        scale.x = 2.0f;
        scale.y = 1.0f;
        button[selectButton].transform.localScale = scale;

        //-----------------------------------------------

        //-----------------------------------------------
        // B�{�^���̓��͂Ō��肷��

        if (ScriptPIManager.GetJump())
        {
            if (buttonTim == 0)
            {
                switch (selectButton)
                {
                    //-----------------------------------
                    // ���̃X�e�[�W�ɐi�ރ{�^���̏���
                    case 0:
                        Debug.Log("next");
                        break;
                    //-----------------------------------
                    // ���̃X�e�[�W��������x�v���C����{�^���̏���
                    case 1:
                        Debug.Log("retry");
                        break;
                    //-----------------------------------
                    // �}�b�v�I����ʂɖ߂�{�^���̏���
                    case 2:
                        Debug.Log("select");
                        break;
                    //-----------------------------------

                }
            }

            buttonTim++;
        }
        else
        {
            buttonTim = 0;
        }
        //-----------------------------------------------
    }
    //=================================================
}
