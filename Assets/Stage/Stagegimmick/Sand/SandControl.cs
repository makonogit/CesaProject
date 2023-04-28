//-------------------------------
//　担当：菅眞心
//　内容：砂のギミック
//-------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandControl : MonoBehaviour
{

    //---------------------------------
    //　変数宣言
    private SandManager sandmanager;    //　このオブジェクトの管理スクリプト
    private GameObject sandmanagerobj;  //　親オブジェクト
    private bool HitTrigger = false;    //　1回当たり判定用
    private Vector3 ReleasePos;         //　放出する座標
    [SerializeField,Header("放出される距離")]
    private float ReleaseLength;

    [SerializeField,Header("放出用砂オブジェクト")]
    private GameObject SandObj;
    private GameObject ReleasedSand;        //　生成した砂オブジェクト保存用
    private GameObject HitCrack;            //　当たったひびのオブジェクト
    private CrackCreater HitCrackCreater;   //　当たったひびのCreater
    private EdgeCollider2D Edge;            //　衝突したEdgeを保存

    private void Start()
    {
        //　砂の管理スクリプト
        sandmanagerobj = GameObject.Find("SandManager");
        sandmanager = sandmanagerobj.GetComponent<SandManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // 砂が生成されたら放出したようにアニメーションする
        if(ReleasedSand != null)
        {
            //　保留   
        }

        // 対象のひびが存在していたら処理
        if (HitCrack != null)
        {
            //　ひびがのばされたら破棄して再度生成
            if (HitCrackCreater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATEBACK)
            {
                Debug.Log("のびた");
                Destroy(ReleasedSand);
                HitTrigger = false;
            }
        }
        else
        {
            // 移動などで無くなれば再度設定状態に
            Destroy(ReleasedSand);
            HitTrigger = false;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        // ひびに当たったら1回処理
        if (other.tag == "Crack" && !HitTrigger)
        {
            Debug.Log("hibi");
            //　オブジェクトの情報を保存
            HitCrack = other;
            HitCrackCreater = other.transform.parent.gameObject.GetComponent<CrackCreater>();

            // EdgeColliderの情報を取得
            Edge = other.GetComponent<EdgeCollider2D>();

            // EdgeColliderの終点からパーティクルを放出
            var EdgePos = Edge.points[Edge.pointCount - 1];

            // ひびの向きによって座標を指定
            ReleasePos = Edge.points[Edge.pointCount - 1].x > transform.position.x ?
                new Vector3(EdgePos.x + ReleaseLength / 4, EdgePos.y - 0.1f, 1.0f) :
                new Vector3(EdgePos.x - ReleaseLength / 4, EdgePos.y - 0.1f, 1.0f);
                
            //　同じ座標に砂がなければ生成
            if (!sandmanager.GetSand().Contains(ReleasePos))
            {
                ReleasedSand = Instantiate(SandObj, ReleasePos, Quaternion.identity);
                ReleasedSand.transform.localScale = new Vector3(ReleaseLength, 1.0f, 1.0f);
                ReleasedSand.transform.parent = sandmanagerobj.transform;
                sandmanager.SetSand(ReleasePos);
            }

            HitTrigger = true;
        }


        //------------------------------------------------
        //　プレイヤーと衝突したら押し返す
        if(other.tag == "Player")
        {
            PlayerMove move = other.GetComponent<PlayerMove>();
            other.transform.Translate(-6.0f * (move.GetMovement().x * move.BaseSpeed * Time.deltaTime), 0.0f, 0.0f);
        }
     

    }

}
