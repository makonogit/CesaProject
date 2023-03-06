//---------------------------------------------------------
//担当者：二宮怜
//内容　：playerの動作に必要なステータスを管理
//　　　：唯一PlayerInputとやり取りできる
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    // 移動用
    private Vector2 movement; // 入力量を取得する変数

    //ジャンプ用
    [Header("ジャンプ中")]
    public bool IsJump = false; // 現在ジャンプボタンが押されているか
    [Header("")]
    [SerializeField] bool JumpTrigger = false; // ジャンプ入力離さないと二回目以降ジャンプしないようにする

    //ひび生成用
    private Vector2 CrackPower; //ひびを入れる強さ

    //ひびの移動用
    public bool CrackMoveflg = false; //ひびの移動開始フラグ

    //右スティック
    private Vector2 R_move;

    //マウス座標
    private Vector2 MousePos;

    //釘打ち込み判定
    bool Nail_Push = false;

    //ハンマーの同時押し判定
    bool Hammer_Push = false;

    //-----------------------------------------------------
    // ポーズ中の入力取得用変数
    // ポーズボタンが押されたか
    private bool Pause = false;

    // 決定ボタンが押されたか
    private bool PressA = false;

    // キャンセルボタンが押されたか
    private bool PressB = false;

    // 移動完了したか
    private bool CursorMoveFlg = false;
    private Vector2 CursorMove; // ポーズ時の入力量

    // 移動モードと照準モードを持つ
    public enum PLAYERMODE
    {
        MOVE,  //移動
        AIM,   //照準
    }

    private PLAYERMODE Mode = PLAYERMODE.MOVE; // プレイヤーの現在のモードを持つ変数

    //----------------------------------------------------------------------------------------------------------
    // - ゲッター ・ セッター -

    //----------------------------------------------------------------------------------------------------------
    // 移動
    //--------------------------------------------
        //戻り値：Vector2型（デバイス入力による入力量）
        //引数　：無し
        public Vector2 GetMovement()
        {
            return movement;
        }

        //--------------------------------------------
        //戻り値：無し
        //引数　：Vector2型（デバイス入力による入力量）
        public void SetMovement(Vector2 _move)
        {
            movement = _move;
        }

    //----------------------------------------------------------------------------------------------------------
    // ジャンプ
    //----------------------------------------------------------------------------------------------------------
    //戻り値：bool型（ジャンプの入力がされているか）
    //引数　：なし
    public bool GetJump()
    {
        return IsJump;
    }

    //戻り値：無し
    //引数　：bool型（ジャンプの入力がされているか）
    public void SetJump(bool _TRUEorFalse)
    {
        IsJump = _TRUEorFalse;
    }

    //----------------------------------------------------------------------------------------------------------
    //戻り値：bool型（ジャンプの入力がされているか）
    //引数　：なし
    public bool GetJumpTrigger()
    {
        return JumpTrigger;
    }

    public void SetJumpTrigger(bool _TRUEorFALSE)
    {
        JumpTrigger = _TRUEorFALSE;
    }

    //----------------------------------------------------------------------------------------------------------
    // 右スティックの入力量
    //----------------------------------------------------------------------------------------------------------

    //戻り値：Vector2型（デバイス入力による入力量）
    //引数　：無し
    public Vector2 GetRmove()
    {
        return R_move;
    }

    //戻り値：無し
    //引数　：Vector2型（デバイス入力による入力量）
    public void SetRmove(Vector2 _R_move)
    {
        R_move = _R_move;
    }

    //----------------------------------------------------------------------------------------------------------
    // ひび
    //----------------------------------------------------------------------------------------------------------

    //戻り値：Vector2型（デバイス入力による入力量）
    //引数　：無し
    public Vector2 GetCarackPower()
    {
        return CrackPower;
    }

    //戻り値：無し
    //引数　：Vector2型（デバイス入力による入力量）
    public void SetCrackPower(Vector2 _CrackPower)
    {
        CrackPower = _CrackPower;
    }

    //----------------------------------------------------------------------------------------------------------
    // ひびの移動
    //----------------------------------------------------------------------------------------------------------
    //戻り値：bool型(ひびに入っているか)
    //引数　：なし
    public bool GetCrackMove()
    {
        return CrackMoveflg;
    }

    //戻り値：無し
    //引数　：bool型(ひびに入っているか)
    public void SetCrackMove(bool _CrackMoveflg)
    {
        CrackMoveflg = _CrackMoveflg;
    }


    //----------------------------------------------------------------------------------------------------------
    // マウス
    //----------------------------------------------------------------------------------------------------------

    //戻り値：Vector2型（マウスの座標）
    //引数　：無し
    public Vector2 GetMousePos()
    {
        return MousePos;
    }

    //戻り値：無し
    //引数　：Vector2型（マウスの座標）
    public void SetMousePos(Vector2 _MousePos)
    {
        MousePos = _MousePos;
    }

    //----------------------------------------------------------------------------------------------------------
    // プレイヤーのモード変数
    //----------------------------------------------------------------------------------------------------------
    
    //戻り値：PLAYERMODE型
    //引数　：無し
    public PLAYERMODE GetPlayerMode()
    {
        return Mode;
    }

    //戻り値：無し
    //引数　：PLAYERMODE型
    public void SetPlayerMode(PLAYERMODE _mode)
    {
        Mode = _mode;
    }

    //----------------------------------------------------------------------------------------------------------
    // 釘打ち込み判定
    //----------------------------------------------------------------------------------------------------------

    //戻り値：bool(釘打ち込みがされたか)
    //引数　：なし
    public bool GetNail()
    {
        return Nail_Push;
    }


    //戻り値：無し
    //引数　：bool(釘打ち込みがされたか)
    public void SetNail(bool _NailFlg)
    {
        Nail_Push = _NailFlg;
    }

    //----------------------------------------------------------------------------------------------------------
    // ハンマーの同時押し処理
    //----------------------------------------------------------------------------------------------------------

    //戻り値：bool(ハンマーの同時がされたか)
    //引数　：なし
    public bool GetHammer()
    {
        return Hammer_Push;
    }


    //戻り値：無し
    //引数　：bool(ハンマーの同時がされたか)
    public void SetHammer(bool _HammerFlg)
    {
        Hammer_Push = _HammerFlg;
    }

    public bool GetPause()
    {
        return Pause;
    }

    //-----------------------------------------------------
    // ポーズ系gettersetter
    public void SetPause(bool _pause)
    {
        Pause = _pause;
    }

    public bool GetPressA()
    {
        return PressA;
    }

    public void SetPressA(bool _pressA)
    {
        PressA = _pressA;
    }

    public bool GetPressB()
    {
        return PressB;
    }

    public void SetPressB(bool _pressB)
    {
        Pause = _pressB;
    }

    public bool GetCursorMoveFlg()
    {
        return CursorMoveFlg;
    }

    public void SetCursorMoveFlg(bool _cursorMoveFlg)
    {
        CursorMoveFlg = _cursorMoveFlg;
    }

    public Vector2 GetCursorMove()
    {
        return CursorMove;
    }

    public void SetCursorMove(Vector2 Value)
    {
        CursorMove = Value;
    }

    //-----------------------------------------------------

}
