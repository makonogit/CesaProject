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
    int nailPushTimeLeft   = 0;   // 釘の打ち込み(左)
    int nailPushTimeRight  = 0;   // 釘の打ち込み(右)
    float hammerPushTime = 0; // ハンマー

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
        bool isReturn = false;

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
            isReturn = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }

    //================================================================
    // - 釘打ち込み入力のトリガー処理(左右) -
    //
    // 引数　：無し
    // 戻り値：キーが押されている時間をカウントして0秒の時のみtrueを返す

    public bool GetNailTrigger_Left()
    {
        //------------------------------------------------------------
        // キーが押されている時間をカウントして0秒の時のみtrueを返す

        // 戻り値用変数
        bool isReturn = false;

        // 押されている場合の処理
        if (ScriptPIManager.GetNail_Left() == true)
        {
            // 0秒の時のみtrueを返す
            if (nailPushTimeLeft == 0)
            {
                isReturn = true;
            }
            else
            {
                isReturn = false;
            }

            // カウントを進める
            nailPushTimeLeft++;
        }
        // 押されていない場合の処理
        else
        {
            // カウントをリセット
            nailPushTimeLeft = 0;
            isReturn = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }

    public bool GetNailTrigger_Right()
    {
        //------------------------------------------------------------
        // キーが押されている時間をカウントして0秒の時のみtrueを返す

        // 戻り値用変数
        bool isReturn = false;

        // 押されている場合の処理
        if (ScriptPIManager.GetNail_Right() == true)
        {
            // 0秒の時のみtrueを返す
            if (nailPushTimeRight == 0)
            {
                isReturn = true;
            }
            else
            {
                isReturn = false;
            }

            // カウントを進める
            nailPushTimeRight++;
        }
        // 押されていない場合の処理
        else
        {
            // カウントをリセット
            nailPushTimeRight = 0;
            isReturn = false;
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
        bool isReturn = false;

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
            hammerPushTime+=Time.deltaTime;
        }
        // 押されていない場合の処理
        else
        {
            // カウントをリセット
            hammerPushTime = 0;
            isReturn = false;
        }

        return isReturn;

        //------------------------------------------------------------
    }
    //================================================================
}
