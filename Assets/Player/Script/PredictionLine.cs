//------------------------------------------------------------------------------
// 担当者：中川直登
// 内容  ：ひびの予測線
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionLine : MonoBehaviour
{
    //------------------------------------------------------------------------------
    //～変数宣言～
    private float _rate =0.8f;//予測線のサイズ調整
    private Crack _crack;
    private PlayerInputManager ScriptPIManager; // PlayerInputManagerを取得する変数

    //--------菅担当-------------
    private Vector3 CrackPower; //ひびを入れる力を保持する変数
    private GameObject Player;  //プレイヤーオブジェクト

    private bool CrackLine = false;

    //------------------------------------------------------------------------------
    //～初期化処理～
    void Start()
    {
        Player = GameObject.Find("player");
        CrackPower = Vector3.zero;
        _crack = Player.GetComponent<Crack>();
        ScriptPIManager =_crack.PlayerInputManager.GetComponent<PlayerInputManager>();
    }

    //------------------------------------------------------------------------------
    //～更新処理～
    void Update()
    {
        //--------------------------------------------------------
        //右スティック入力があるかの判定
        if (ScriptPIManager.GetCarackPower().x == 1 || ScriptPIManager.GetCarackPower().x == -1 ||
            ScriptPIManager.GetCarackPower().y == 1 || ScriptPIManager.GetCarackPower().y == -1 )
        {
            CrackPower = ScriptPIManager.GetRmove();
            CrackLine = true;
        }
        else
        {
            CrackLine = false;
        }

        //---------------------------------------------------------
        //オブジェクトのY軸のサイズを変更して表示する処理

        // 入力の角度
        float angle = Mathf.Atan2(CrackPower.y, CrackPower.x) * Mathf.Rad2Deg;
        float radius = _crack.CrackPower / 2 * _rate;
        float radian = ((angle) / 180.0f) * Mathf.PI;

        this.transform.localPosition = new Vector3(Player.transform.localPosition.x + radius * Mathf.Cos(radian), Player.transform.localPosition.y + radius * Mathf.Sin(radian), 0);
        this.transform.localEulerAngles = new Vector3(0, 0, angle - 90);
        this.transform.localScale = new Vector3(this.transform.localScale.x, _crack.CrackPower * _rate, this.transform.localScale.z);
        
        //--------------------------------------------
        //入力外で移動したらキャンセルする
        if (!CrackLine)
        {
            if(ScriptPIManager.GetMovement().x > 0 || ScriptPIManager.GetMovement().x < 0)
            {
                CrackPower = Vector3.zero;
                _crack.CrackPower = 0;
            }
        }



    }

}
