//---------------------------------
//担当：菅眞心
//役割：範囲内にある釘を探す
//---------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailSearch : MonoBehaviour
{
    NailStateManager manager;   //釘の状態を取得するスクリプト
    HammerNail hammer;          //打ち込み状態を取得するスクリプト

    // Start is called before the first frame update
    void Start()
    {
        //親オブジェクトのHammerNailを取得
        hammer = GetComponentInParent<HammerNail>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //-------------------------------------------------
        //範囲内に釘があって構え中なら釘の範囲を可視化
        if(collision.gameObject.tag == "UsedNail")
        {
            //当たった釘の状態スクリプトを取得
            manager = collision.gameObject.GetComponent<NailStateManager>();

            //打ち込み状態によって釘の状態を変更
            if (hammer._HammerState == HammerNail.HammerState.NAILSET)
            {
                manager.SetState(NailStateManager.NailState.AREAVISUAL);
            }
            if(hammer._HammerState == HammerNail.HammerState.NONE ||
                hammer._HammerState == HammerNail.HammerState.HAMMER)
            {
                manager.SetState(NailStateManager.NailState.NOMAL);
            }

        }


    }

}
