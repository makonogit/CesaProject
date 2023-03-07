//----------------------------------------
//担当：菅眞心
//内容：ひびの生成リストを追加
//----------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SetNailList : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> HitList;   //衝突したオブジェクトのリスト 

    public float NearDistance;         //1番近い距離
    public int NearNailNum;            //1番近い釘の番号

    public int ThisPointNum = -1;   //自身のポイント番号(設定されていなかったら-1)  

    GetCrackPoint _getCrackPoint;   //ひびのポイントを作成するスクリプト

    GameObject CrackCreateArea;     //ひびを作成する用のオブジェクト
    SetNailList _nextSet;           //次のPointセット用

    CircleCollider2D thiscol;       //このオブジェクトのコライダー

    public int OldNailNum;          //1個前の釘番号

    public bool ChainFlg = true;   //つながったらあたり判定を取る

    bool AddPointFlg = false;       //ポイント追加フラグ

    public bool Crackend = false;          //この釘を使ってひびが作成されたか

    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------------------------
        //ひびのポイント情報を取得
        CrackCreateArea = GameObject.Find("CrackCreateArea");
        _getCrackPoint = CrackCreateArea.GetComponent<GetCrackPoint>();

        //このオブジェクトのコライダーを取得
        thiscol = GetComponent<CircleCollider2D>();

        NearDistance = 10000;

    }

    // Update is called once per frame
    void Update()
    { 

        if (ThisPointNum != -1)
        {
            if (_nextSet != null)
            {
               // _nextSet.ChainFlg = true;
            }

            //if (!GetComponent<CircleCollider2D>())
            //{
            //    gameObject.AddComponent<CircleCollider2D>();
            //    thiscol = GetComponent<CircleCollider2D>();
            //    thiscol.isTrigger = true;
            //}

            thiscol.radius = 0.5f;
            //thiscol.enabled = true;

        }
        else
        { 
            if (_nextSet != null)
            {
                _nextSet.ChainFlg = false;
                _nextSet.ThisPointNum = -1;
                _nextSet.OldNailNum = -1;

            }

            //if (GetComponent<CircleCollider2D>() && OldNailNum == -1)
            //{
            //    Destroy(thiscol);
            //}

            thiscol.radius = 0.0f;

            //thiscol.enabled = false;

        }

        //当たっている釘があれば
        if (HitList.Count > 0 && AddPointFlg)
        {

            for (int i = 0; i < HitList.Count; i++)
            {
                //------------------------------------------------------
                //リストに存在してない釘の中から1番近い釘を取得
                //距離を求める
                if (!_getCrackPoint.GetPointLest().Contains(HitList[i].transform.position))
                {
                    float Distance = Vector3.Magnitude(transform.position - HitList[i].transform.position);

                    //1番近い距離より近かったら情報更新
                    if (NearDistance > Distance)
                    {
                        NearDistance = Distance;
                        NearNailNum = i;
                    }
                }

                Debug.Log(NearDistance);
                //Debug.Log(NearNailNum);
            }

            //同じ釘が存在していなかったらポイントを追加
            if (!_getCrackPoint.GetPointLest().Contains(HitList[NearNailNum].transform.position))
            {
                _getCrackPoint.objectList.Add(HitList[NearNailNum]);
                _getCrackPoint.SetPoint(HitList[NearNailNum].transform.position);
            }


            _nextSet = HitList[NearNailNum].GetComponentInChildren<SetNailList>();


            //1番近いPointセットスクリプトを呼んで繋げる    
            //_nextSet.ChainFlg = true;
           
            if (ThisPointNum != -1)
            {
                //Debug.Log();
                _nextSet.OldNailNum = ThisPointNum;
                _nextSet.ThisPointNum = ThisPointNum + 1;
                _nextSet.ChainFlg = true;
            }

            Debug.Log(ThisPointNum);

            AddPointFlg = false;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.gameObject + "Enter");

        if (collision.gameObject.tag == "NailArea")
        {
            Debug.Log(collision.gameObject + "Enter");

            //リストに同じデータがなければ
            if (!HitList.Contains(collision.gameObject.transform.root.gameObject))
            {
                //-------------------------------------------
                //範囲内に釘があったらHitListを追加
                HitList.Add(collision.gameObject.transform.root.gameObject);

            }

            if (HitList.Contains(collision.gameObject.transform.root.gameObject))
            {
                AddPointFlg = true;   //PointListの追加を許可
                Debug.Log("AddPoint");
            }


        }

    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //   // Debug.Log(collision.gameObject);
    //    //if(collision.gameObject.tag == "UsedNail" && ChainFlg)
    //    //{
    //    //    for(int i = 0; i < HitList.Count; i++)
    //    //    {
    //    //        //HitListに同じゲームオブジェクトがない
    //    //        if(HitList[i].transform.position != collision.gameObject.transform.position)
    //    //        {
    //    //            //-------------------------------------------
    //    //            //範囲内に釘があったらHitListを追加
    //    //            HitList.Add(collision.gameObject);
    //    //        }
    //    //    }

    //    //    AddPointFlg = true;   //PointListの追加を許可
    //    //    ChainFlg = false;     //1回だけ当たり判定を取る
    //    //}
        
    //}

}
