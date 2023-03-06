//-----------------------------------
//担当：菅眞心
//内容：ひびの情報を作成する
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCrackPoint : MonoBehaviour
{
    public List<GameObject> HitList;   //当たったコライダーの情報のリスト

    float NearDistance;         //1番近い距離
    public int NearNailNum;            //1番近い釘の番号

    Transform Playertransform;      //プレイヤーの座標
    CrackAutoMove _crackAutoMove;   //ひびの移動中か
  
    bool Removeflg = false;

    Vector2 OldFirstPoint;  //1個前の始点釘

    SetNailList _Setlist;           //釘の情報を取得
    public List<Vector2> PointList; //ひびの生成用ポイントリスト

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーの座標を取得
        Playertransform = GetComponentInParent<Transform>();

        _crackAutoMove = GetComponentInParent<CrackAutoMove>();
    
        NearDistance = 10000;
       
        /*
        PointList = new List<Vector2>(2);   //リストをサイズ確保して初期化したいけどなんかできへん
        */

        PointList.Add(transform.position);
      //  PointList.Add(transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        PointList[0] = new Vector2(Playertransform.position.x, Playertransform.position.y);  //ひびの始点は常にプレイヤーの座標に設定
      
        //-----------------------------------------
        //衝突しているオブジェクトが1つ以上あれば
        if (HitList.Count > 0)
        {
            OldFirstPoint = HitList[NearNailNum].transform.position;

            //----------------------------------
            //1番近い釘を取得
            for (int i = 0;i<HitList.Count; i++)
            {
                //距離を求める
                float Distance = Vector3.Magnitude(transform.position - HitList[i].transform.position);

                //1番近い距離より近かったら情報更新
                if(NearDistance > Distance)
                {
                    NearDistance = Distance;
                    NearNailNum = i;
                }

            }

            //前回の始点と違う始点になったらリストを初期化
            if (PointList.Count > 1)
            {
                if (OldFirstPoint != (Vector2)HitList[NearNailNum].transform.position)
                {
                     Debug.Log("始点の移動");
                    _Setlist.ChainFlg = false;
                    _Setlist.ThisPointNum = -1;
                    _Setlist.OldNailNum = -1;
                    for (int i = 1; i < PointList.Count; i++)
                    {
                        PointList.RemoveAt(i);
                    }

                }
            }
            
            //次のPointを1番近い釘に設定
            //PointList[1] = HitList[NearNailNum].transform.position;
            if (!PointList.Contains(HitList[NearNailNum].transform.position) &&  PointList.Count == 1)
            {
                PointList.Add(HitList[NearNailNum].transform.position);
            }

            //1番近い釘をつなげる
            _Setlist = HitList[NearNailNum].GetComponent<SetNailList>();
            _Setlist.ThisPointNum = 1;
            _Setlist.OldNailNum = 0;
            _Setlist.ChainFlg = true;

            //距離を初期化(アクセス違反が起こる、他に方法あるんやろうけど思いつかん1:30のまこより)
            NearDistance = 10000;
            NearNailNum = 0;   

        }

    }

    //-------------------------------------------
    //当たった瞬間HitListを更新
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "UsedNail")
        {
            //当たったコライダーをリスト化
            HitList.Add(collision.gameObject);

        }
    }

    //----------------------------------------
    //範囲から出たらオブジェクトを消去
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "UsedNail")
        {
            for (int i = 0; i < HitList.Count; i++)
            {
                //HitListに同じゲームオブジェクトがあったら
                if (HitList[i].transform.position == collision.gameObject.transform.position)
                {
                    //オブジェクトをリストから消去
                    HitList.RemoveAt(i);
                }
            }
        }
    }


    //----------------------------------
    //ひびのポイント作成用関数
    //引数：ポイント座標
    //戻り値：なし
    //----------------------------------
    public void SetPoint(Vector2 point)
    {
        //ポイントを追加
        PointList.Add(point);
    }

    //----------------------------------
    //ひびのポイントリスト取得関数
    //引数：なし
    //戻り値：PointList
    //----------------------------------
    public List<Vector2> GetPointLest()
    {
        return PointList;
    }


    //----------------------------------
    //ひびのポイントリスト削除関数
    //引数：削除するポイント番号
    //戻り値：なし
    //----------------------------------
    public void RemovePoint(int Pointnum)
    {
        PointList.RemoveAt(Pointnum);
    }

    public void AcsessPoint(int Pointnum)
    {
        
    }

}
