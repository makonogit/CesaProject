//---------------------------------------------------------
//担当者：二宮怜
//内容　：InputSystemによる入力
//　　　：PlayerInputManagerにそれぞれの入力の状態渡す
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    // 移動用
    public Vector2 L_move;     // 左スティックの入力量を取得する変数
    // ひび用
    public Vector2 R_Push;     // 右スティックの入力量を取得する変数(Press)
    public Vector2 R_move;     // 右スティックの入力量を取得する変数

    // カーソル用
    public Vector2 CL_move; // カーソル用左スティック入力量を取得する変数

    // マウスの座標
    [SerializeField]
    private Vector2 MousePos;  //マウス座標を保持する変数

    // 外部取得
    private GameObject PlayerInputMane; // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数

    private GameObject pause;
    private PauseGame pausegame;

    //----------------------------------------------------------------------------------------------------------
    // - 初期化処理 -
    void Start()
    {
        //----------------------------------------------------------------------------------------------------------
        // PlayerInputManagerを探す
        PlayerInputMane = GameObject.Find("PlayerInputManager");

        //----------------------------------------------------------------------------------------------------------
        // ゲームオブジェクトPlayerInputManagerが持つPlayerInputManagerスクリプトを取得
        ScriptPIManager = PlayerInputMane.GetComponent<PlayerInputManager>();

        pause = GameObject.Find("PausePanel");
        pausegame = pause.GetComponent<PauseGame>();
    }

    //----------------------------------------------------------------------------------------------------------
    // - 更新処理 -

    //----------------------------------------------------------------------------------------------------------
    // 引数context：使用例
    //if (context.phase == InputActionPhase.Started) 設定した入力があった瞬間の状態
    //if (context.phase == InputActionPhase.Performed) 設定した入力が続いている状態
    //if (context.phase == InputActionPhase.Canceled) 設定した入力がおわった瞬間の状態

    public void OnCursorMove(InputAction.CallbackContext context)
    {
        // ポーズ状態の時の入力
        if (pausegame.IsPause == true)
        {
            if (context.phase == InputActionPhase.Started)
            {
                CL_move = context.ReadValue<Vector2>();

                // 入力が一度なくなったら次の入力をとる
                if (ScriptPIManager.GetCursorMoveFlg() == false)
                {
                    ScriptPIManager.SetCursorMove(CL_move);

                    // y入力が0になるまで次の入力をとらない
                    ScriptPIManager.SetCursorMoveFlg(true);
                }
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                ScriptPIManager.SetCursorMoveFlg(false);
            }
        }
    }

    //----------------------------------------------------------------
    //戻り値：無し
    //引数　：入力の様々なパラメーターを持つ変数
    public void OnMove(InputAction.CallbackContext context)
    {
        // ポーズ状態じゃない時の入力
        if (pausegame.IsPause == false)
        {
            //---------------------------------------------------------------
            // 入力量を取得
            L_move = context.ReadValue<Vector2>();

            //---------------------------------------------------------------
            //PlayerInputManagerに入力量をセット
            ScriptPIManager.SetMovement(L_move);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // ポーズ状態じゃない時のAボタン入力
        if (pausegame.IsPause == false)
        {
            if (context.phase == InputActionPhase.Started)
            {
                //---------------------------------------------------------------
                //長押し時の二回目以降のジャンプを封じる
                ScriptPIManager.SetJumpTrigger(true);

                //---------------------------------------------------------------
                //ジャンプが入力されている状態にセットする
                ScriptPIManager.SetJump(true);
            }

            //---------------------------------------------------------------
            //ジャンプ入力が終了した瞬間
            if (context.phase == InputActionPhase.Canceled)
            {
                //---------------------------------------------------------------
                //PlayerInputManagerのメンバ変数IsJumpがtrueなら
                if (ScriptPIManager.GetJump() == true)
                {
                    //---------------------------------------------------------------
                    //ジャンプが入力されていない状態にセットする
                    ScriptPIManager.SetJump(false);
                }
            }
        }
        // ポーズ状態の時
        else
        {
            // おされた最初のフレーム
            if (context.phase == InputActionPhase.Started)
            {
                // 決定ボタンの入力があった
                ScriptPIManager.SetPressA(true);
            }
        }
    }

    //------------------------------------------------------
    //右スティックの入力量を取得する　担当：菅眞心
    public void OnCreateCrack(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        // 入力量を取得
        R_Push = context.ReadValue<Vector2>();

        //入力があれば
        if (R_Push.x < 0 || R_Push.x > 0 || R_Push.y < 0 || R_Push.y > 0)
        {
            // プレイヤーが移動モードなら
            if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.MOVE)
            {
                // 照準モードに切り替える
                ScriptPIManager.SetPlayerMode(PlayerInputManager.PLAYERMODE.AIM);
            }

        }
        else
        {
            // プレイヤーが照準モードなら
            if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.AIM)
            {
                // 移動モードに切り替える
                ScriptPIManager.SetPlayerMode(PlayerInputManager.PLAYERMODE.MOVE);
            }
        }

        // Debug.Log(R_move);

        ScriptPIManager.SetCrackPower(R_Push);
    }


    //------------------------------------------------------
    //Bボタン入力判定(今は中身なし)　担当：菅眞心
    public void OnCrackMove(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        // 押された時だけセット
        if (context.phase == InputActionPhase.Started)
        {
            //// プレイヤーが移動モードなら
            //if(ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.MOVE)
            //{
            //    // 照準モードに切り替える
            //    ScriptPIManager.SetPlayerMode(PlayerInputManager.PLAYERMODE.AIM);
            //}
            //// プレイヤーが照準モードなら
            //else if (ScriptPIManager.GetPlayerMode() == PlayerInputManager.PLAYERMODE.AIM)
            //{
            //    // 移動モードに切り替える
            //    ScriptPIManager.SetPlayerMode(PlayerInputManager.PLAYERMODE.MOVE);
            //}

            Debug.Log(ScriptPIManager.GetPlayerMode());

            if (pausegame.IsPause == true)
            {
                // ポーズ状態じゃない時のBボタン入力
                if (ScriptPIManager.GetPressB() == false)
                {
                    // キャンセルボタン入力セット
                    ScriptPIManager.SetPressB(true);
                }
            }

            //↓↓↓ver.ひびにはいる時の処理
            //if (context.phase == InputActionPhase.Started)
            //{

            //    if (ScriptPIManager.GetCrackMove() == false)
            //    {
            //        ScriptPIManager.SetCrackMove(true);
            //    }
            //}

            //if(context.phase == InputActionPhase.Performed)
            //{

            //    if (ScriptPIManager.GetCrackMove() == false)
            //    {
            //        ScriptPIManager.SetCrackMove(true);
            //    }
            //}

            //if(context.phase == InputActionPhase.Canceled)
            //{
            //    if (ScriptPIManager.GetCrackMove() == true)
            //    {
            //        ScriptPIManager.SetCrackMove(false);
            //    }
            //}
        }
    }


    //------------------------------------------------------
    //右スティックの入力量を取得する(ボタン化)　担当：菅眞心
    public void OnRightMove(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        // 入力量を取得
        R_move = context.ReadValue<Vector2>();

        ScriptPIManager.SetRmove(R_move);
    }


    //------------------------------------------------------
    //マウス座標取得　担当：菅眞心
    public void OnMousePos(InputAction.CallbackContext context)
    {
        //---------------------------------------------------------------
        //マウスの座標を取得
        MousePos = context.ReadValue<Vector2>();

        ScriptPIManager.SetMousePos(MousePos);

    }

    //------------------------------------------------------
    //釘打ち処理　担当：菅眞心
    public void OnHammerNailLeft(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started)
        {
            ScriptPIManager.SetNail_Left(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            if (ScriptPIManager.GetNail_Left() == true)
            {
                ScriptPIManager.SetNail_Left(false);
            }
        }

    }

    public void OnHammerNailRight(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started)
        {
            ScriptPIManager.SetNail_Right(true);
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            if (ScriptPIManager.GetNail_Right() == true)
            {
                ScriptPIManager.SetNail_Right(false);
            }
        }

    }



    //------------------------------------------------------
    //ハンマー処理（同時押し）担当：菅眞心
    public void OnHammer(InputAction.CallbackContext context)
    {

        //--------------------------------------------
        //これやと押されてる間ずっとTrueかも...
        if (context.phase == InputActionPhase.Started)
        {
            ScriptPIManager.SetHammer(true);
        }

        if(context.phase == InputActionPhase.Canceled)
        {
            if(ScriptPIManager.GetHammer() == true)
            {
                ScriptPIManager.SetHammer(false);
            }
        }

    }

    public void OnPause(InputAction.CallbackContext context)
    {
        // ポーズボタンが押された最初のフレームでtrue
        if(context.phase == InputActionPhase.Started)
        {
            ScriptPIManager.SetPause(true);
        }
    }

}
