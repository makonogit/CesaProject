//----------------------------------------------------------------
// 担当者：藤原昂祐
// 内容　：入力のトリガー処理
//----------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTrigger : MonoBehaviour
{
    //================================================================
    // - 変数 -

    // インプットマネージャー
    private GameObject PlayerInputMana;         // ゲームオブジェクトPlayerInputManagerを取得する変数
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数

    // キーの入力がされている時間を持つ変数
    int jumpPushTime   = 0; // ジャンプ
    int nailPushTime   = 0; // 釘の打ち込み
    int hammerPushTime = 0; // ハンマー

    //================================================================
    // - 初期化処理 -

    void Start()
    {
        //------------------------------------------------------------
        // インプットマネージャーを取得する

        PlayerInputMana = GameObject.Find("PlayerInputManager");
        ScriptPIManager = PlayerInputMana.GetComponent<PlayerInputManager>();

        //------------------------------------------------------------
    }

    //================================================================
    // - ジャンプ入力のトリガー処理 -
    //
    // 引数　：無し
    // 戻り値：キーが押されている時間をカウントして0秒の時のみtrueを返す

    public bool GetJumpTrigger()
    {
        //------------------------------------------------------------
        // キーが押されている時間をカウントして0秒の時のみtrueを返す

        // 戻り値用変数
        bool isReturn;

        // 押されている場合の処理
        if (ScriptPIManager.GetJump() == true)
        {

            // 0秒の時のみtrueを返す
            if (jumpPushTime == 0)
            {
                isReturn = true;
            }
            else
            {
                isReturn = false;
            }

            // カウントを進める
            jumpPushTime++;
        }
        // 押されていない場合の処理
        else
        {
            // カウントをリセット
            jumpPushTime = 0;
            isReturng = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }

    //================================================================
    // - 釘打ち込み入力のトリガー処理 -
    //
    // 引数　：無し
    // 戻り値：キーが押されている時間をカウントして0秒の時のみtrueを返す

    public bool GetNailTrigger()
    {
        //------------------------------------------------------------
        // キーが押されている時間をカウントして0秒の時のみtrueを返す

        // 戻り値用変数
        bool isReturn;

        // 押されている場合の処理
        if (ScriptPIManager.GetNail() == true)
        {
            // 0秒の時のみtrueを返す
            if (nailPushTime == 0)
            {
                isReturn = true;
            }
            else
            {
                isReturn = false;
            }

            // カウントを進める
            nailPushTime++;
        }
        // 押されていない場合の処理
        else
        {
            // カウントをリセット
            nailPushTime = 0;
            isReturng = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }


    //================================================================
    // - ハンマーのトリガー処理 -
    //
    // 引数　：無し
    // 戻り値：キーが押されている時間をカウントして0秒の時のみtrueを返す

    public bool GetHammerTrigger()
    {
        //------------------------------------------------------------
        // キーが押されている時間をカウントして0秒の時のみtrueを返す

        // 戻り値用変数
        bool isReturn;

        // 押されている場合の処理
        if (ScriptPIManager.GetHammer() == true)
        {
            // 0秒の時のみtrueを返す
            if (hammerPushTime == 0)
            {
                isReturn = true;
            }
            else
            {
                isReturn = false;
            }

            // カウントを進める
            hammerPushTime++;
        }
        // 押されていない場合の処理
        else
        {
            // カウントをリセット
            hammerPushTime = 0;
            isReturng = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }
    //================================================================
}
