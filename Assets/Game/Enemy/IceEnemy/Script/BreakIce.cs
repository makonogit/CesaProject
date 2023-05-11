//----------------------------------------------------------
// 担当者：中川直登
// 内容  ：プレイヤーが範囲に入ったかどうか
//----------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BreakIce : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ice") 
        {
            BreakBlock Ice = collision.GetComponent<BreakBlock>();
            if (Ice != null) Ice.Func_BreakBlock();
        }
    }
}