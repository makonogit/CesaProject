//---------------------------------
//　担当:菅眞心
//　ボードの当たり判定
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBord : MonoBehaviour
{
    
    Transform playertrans;      //PlayerのTransform
    PolygonCollider2D thiscol;  //このオブジェクトのCollider
    EdgeCollider2D walkedge;    //歩く範囲用のEdgeCollider

    // Start is called before the first frame update
    void Start()
    {
        // playerのTransform取得
        playertrans = GameObject.Find("player").transform;
        // このオブジェクトのcolliderを取得
        thiscol = GetComponent<PolygonCollider2D>();
        // 歩く用のEdgeCollider
        walkedge = transform.GetChild(0).GetComponent<EdgeCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //　プレイヤーが衝突したら当たり判定をONにする
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("当たり");
             // PlayerのPositionによってTriggerをONにする
            walkedge.isTrigger = playertrans.position.y > transform.GetChild(0).transform.position.y ? false : true;
        }
    }



}
