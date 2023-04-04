//-----------------------------------------
//　担当：菅眞心
//　内容：針の移動
//-----------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleMove : MonoBehaviour
{
    //---------------------------------------
    //　変数宣言
    
    //---------------------------------------
    // 外部取得
    GameOver hpsystem;  //HPシステムのスクリプト

    Transform ThisTrans;    // 自身のTransform

    [SerializeField, Header("移動スピード")]
    private float MoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ThisTrans = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //----------------------------------------------------------------
        //　向いている方向に移動
        Vector3 verocity = ThisTrans.rotation * new Vector3(MoveSpeed, 0);
        ThisTrans.position += verocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //---------------------------------------------------------
        //　プレイヤーと衝突したらHPを減らす
        if (collision.gameObject.tag == "Player")
        {
            hpsystem = collision.gameObject.GetComponent<GameOver>();
            hpsystem.DecreaseHP(1);
        }

        //--------------------------------
        //　サボテン以外とぶつかったら消去
        if(collision.gameObject.name != "cactus")
        {
            Destroy(this.gameObject);
        }
    }
  

}
