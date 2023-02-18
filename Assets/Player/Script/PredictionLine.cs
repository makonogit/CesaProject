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
    

    //------------------------------------------------------------------------------
    //～初期化処理～
    void Start()
    {
        _crack = GetComponentInParent<Crack>();
        ScriptPIManager =_crack.PlayerInputManager.GetComponent<PlayerInputManager>();
    }

    //------------------------------------------------------------------------------
    //～更新処理～
    void Update()
    {
        
        //---------------------------------------------------------
        //オブジェクトのY軸のサイズを変更して表示する処理

        // 入力の角度
        float angle = Mathf.Atan2(ScriptPIManager.GetRmove().y, ScriptPIManager.GetRmove().x) * Mathf.Rad2Deg;
        float radius = _crack.CrackPower / 2 * _rate;
        float radian = ((angle) / 180.0f) * Mathf.PI;

        this.transform.localPosition = new Vector3(radius * Mathf.Cos(radian), radius * Mathf.Sin(radian), 0);
        this.transform.localEulerAngles = new Vector3(0, 0, angle - 90);
        this.transform.localScale = new Vector3(this.transform.localScale.x, _crack.CrackPower * _rate, this.transform.localScale.z);
    }
}
