//---------------------------------------------
//�S���ҁF���Ԑ^���q
//���e�F�����蔻��i�S�[���j
//---------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hit : MonoBehaviour
{
    //�\�ǉ��S���ҁF���쒼�o�\

    // ���͊֌W
    private PlayerInputManager _playerInputManager;
    private InputTrigger _inputTrigger;
    [SerializeField]
    private bool _isHitGoal = false;
    //�\�\�\�\�\�\�\�\�\�\�\�\

    //---------------------------------------------------------
    //* ���������� *
    //---------------------------------------------------------
    private void Start()
    {
        //�\�ǉ��S���ҁF���쒼�o�\

        // ������
        _isHitGoal = false;
        //--------------------------------------
        // PlayerInputManager�̎擾

        // PlayerInputManager��T��
        GameObject _inputManager = GameObject.Find("PlayerInputManager");
        // �G���[��
        if (_inputManager == null) Debug.LogError("PlayerInputManager�������邱�Ƃ��o���܂���ł����B");

        // �R���|�[�l���g�̎擾
        _playerInputManager = _inputManager.GetComponent<PlayerInputManager>();
        // �G���[��
        if (_playerInputManager == null) Debug.LogError("PlayerInputManager�̃R���|�[�l���g���擾�ł��܂���ł����B");

        // �R���|�[�l���g�̎擾
        _inputTrigger = _inputManager.GetComponent<InputTrigger>();
        // �G���[��
        if (_inputTrigger == null) Debug.LogError("InputTrigger�̃R���|�[�l���g���擾�ł��܂���ł����B");
        //�\�\�\�\�\�\�\�\�\�\�\�\
    }


    //---------------------------------------------------------
    //* �X�V���� *
    //---------------------------------------------------------
    void Update()
    {
        //�\�ǉ��S���ҁF���쒼�o�\
        if(_isHitGoal == true) 
        {
            // GetHammerTrigger�����g���Ȃ��̂�GetHammer�ő�p���Ă��܂�
            if (_playerInputManager.GetHammer() )
            {
                //Debug.Log("�����܂���");
                // �S�[���I�u�W�F�N�g�ɓ���������N���A��ʂ�`�悷��
                SceneManager.LoadScene("SelectScene");
            }
        }
        //�\�\�\�\�\�\�\�\�\�\�\�\
    }


    //----------------------------------------------------------
    // * �����蔻��̏��� *
    //----------------------------------------------------------
    void OnTriggerEnter2D(Collider2D collider)
    {
      
        if (collider.gameObject.CompareTag("Goal"))
        {
            //collider.gameObject.SetActive(false);

            // �S�[���I�u�W�F�N�g�ɓ���������N���A��ʂ�`�悷��
            //SceneManager.LoadScene("ClearScene");
            //�\�ǉ��S���ҁF���쒼�o�\
            _isHitGoal = true;
        }
    }

}
