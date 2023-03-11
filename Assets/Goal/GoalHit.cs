//--------------------------------
//�S���F���Ԑ^���q
//���e�F�S�[���C�x���g�̎���
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalHit : MonoBehaviour
{
    //---------------------------------------------------------------
    // * �O���擾 *
    //---------------------------------------------------------------
    private GameObject PlayerInputManager; // �Q�[���I�u�W�F�N�gPlayerInputManager���擾����ϐ�
    private PlayerInputManager ScriptPIManager; // PlayerInputManager���擾����ϐ�

    CameraZoom CamZoom; // �J�����Y�[���X�N���v�g���擾
    private GameObject GoalArea; // �Q�[���I�u�W�F�N�gGoalArea���擾����ϐ�

    //---------------------------------------------------------------
    // * ���������� *
    //---------------------------------------------------------------
    void Start()
    {
        // PlayerInputManager��T��
        PlayerInputManager = GameObject.Find("PlayerInputManager");
        // �Q�[���I�u�W�F�N�gPlayerInputManager������PlayerInputManager�X�N���v�g���擾
        ScriptPIManager = PlayerInputManager.GetComponent<PlayerInputManager>();

        //�@GoalArea��T��
        GoalArea = GameObject.Find("GoalArea");
        // �J�����X�N���v�g���擾
        CamZoom = GoalArea.GetComponent<CameraZoom>();


    }


    //---------------------------------------------------------------
    // * �X�V���� *
    //---------------------------------------------------------------
    void Update()
    {
        // �S�[���G���A�ɓ����Ă�����C�x���g����
        if (CamZoom.InArea)
        {
            // �͂��ő�l�ɒB������{�^���ŃV�[���ړ�
            if (ScriptPIManager.GetHammer() == true) 
            {
                //�N���A��ʂ̕`��
                SceneManager.LoadScene("ClearScene");
            }
        }

    }
}
