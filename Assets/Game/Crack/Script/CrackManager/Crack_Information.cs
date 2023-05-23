//---------------------------------------------------------
//
//担当者：中川直登
//
//内容　：CrackManagerに情報を渡すスクリプト
//
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crack_Information : MonoBehaviour
{
    //-----------------------------------------------------
    // private variable
    //-----------------------------------------------------
    private CrackManager _manager;
    private CrackInfo _info;
    //-----------------------------------------------------
    // private method
    //-----------------------------------------------------

    // Use this for initialization
    void Start()
    {
        _manager = GetComponentInParent<CrackManager>();
        if (_manager == null) Debug.LogError("CrackManagerのコンポーネントを取得できませんでした。");
        _info.gameObject = gameObject;
        _info.creater = GetComponent<CrackCreater>();
        _info.NullCheck();
        _manager.AddCrackInfokList(_info);
    }

    private void OnDestroy()
    {
        _manager.RemoveCrackInfokList(_info);
    }


}