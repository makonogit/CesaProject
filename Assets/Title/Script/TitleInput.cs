//---------------------------------------------------------
//�S���ҁF��{��
//���e�@�F�^�C�g����ʂł̓���
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleInput : MonoBehaviour
{
    // �ϐ��錾

    [SerializeField] private TitleDirection _titleDirection;

    public void OnSkipDirection(InputAction.CallbackContext context)
    {
        // �{�^���������ꂽ��
        if(context.phase == InputActionPhase.Started)
        {
            _titleDirection.PushStartButton = true;
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            _titleDirection.PushStartButton = false;
        }
    }

}
