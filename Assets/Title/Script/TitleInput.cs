//---------------------------------------------------------
//担当者：二宮怜
//内容　：タイトル画面での入力
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleInput : MonoBehaviour
{
    // 変数宣言

    [SerializeField] private TitleDirection _titleDirection;

    public void OnSkipDirection(InputAction.CallbackContext context)
    {
        // ボタンが押されたら
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
