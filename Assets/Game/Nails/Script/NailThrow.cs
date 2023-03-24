//-------------------------------------
//担当：菅眞心
//内容：釘を投げるアニメーション
//---------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailThrow : MonoBehaviour
{

    GameObject Player;      // プレイヤーのオブジェクト
    HammerNail hammerNail;  // 釘を打つスクリプト

    Vector3 TargetPos;      // 釘の目的地
    Vector3 NowNailPos;     // 現在の釘の位置

    [Header("釘を投げるスピード")]
    public float ThrowSpeed; 

    [Header("釘の目的地との距離")]
    public float NailDistance;


    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------------
        // 釘を打つスクリプトの取得
        Player = GameObject.Find("player");
        hammerNail = Player.GetComponent<HammerNail>();

        //------------------------------------------
        // 釘の座標を設定
        TargetPos = hammerNail.NailsTrans.position;
        NowNailPos = Player.transform.position;

        NailDistance = 100000;
        transform.position = NowNailPos;

    }

    // Update is called once per frame
    void Update()
    {

        //-----------------------------------------------
        //目的地に行くまで座標移動
        NailDistance = Vector3.Magnitude(NowNailPos - TargetPos);
        if (NailDistance > 0)
        {
            //Debug.Log("移動")
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, ThrowSpeed * Time.deltaTime);
            NowNailPos = transform.position;
        }
       
    }
}
