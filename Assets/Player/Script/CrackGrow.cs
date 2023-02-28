//-------------------------------------
//担当：菅眞心
//役割：範囲にあるひびを成長刺せる
//-------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackGrow : MonoBehaviour
{

    private CrackCreater Creater;       //CrackCreaterを保持する変数
    private CircleCollider2D thiscol;   //このオブジェクトのコライダー

    // Start is called before the first frame update
    void Start()
    {
        //このオブジェクトのコライダーを取得
        thiscol = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //------------------------------
        //ひびの生成情報を取得
        if (collision.gameObject.tag == "Crack")
        {
            Creater = collision.GetComponent<CrackCreater>();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Crack")
        {
            //--------------------------------------
            //生成終了していたらひびを追加
            if (Creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
            {
                Creater.SetState(CrackCreater.CrackCreaterState.ADD_CREATE);
            }
        }

    }

}
