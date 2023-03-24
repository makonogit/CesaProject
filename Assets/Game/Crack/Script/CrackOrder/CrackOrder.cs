//------------------------------------------------------------------------------
// 担当者：中川直登
// 内容  ：ひびを呼び出す
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackOrder : MonoBehaviour
{
    //------------------------------------------------------------------------------
    //―変数宣言―
    [Header("呼び出すオブジェ")]
    public GameObject PrefabObject;
    [Header("呼び出す数")]
    public float numSummon;
    [Header("1つを呼び出す時間")]
    public float WaitTime = 0.05f;
    [Header("ひびのサイズと減衰率と次の範囲")]
    public Vector3 _Scale;// サイズ
    public Vector3 _decreaseRate = new Vector3(0.9f, 1.0f, 1.0f);// サイズの減少率
    public Vector2 _nextAngleRange;// 次の角度を決める範囲
    [Header("触らないでください。")]
    public List<Vector2> Points;
    public List<GameObject> _crackObjects;// ひびのリスト
    [System.NonSerialized]
    public EdgeCollider2D EC2D;

    Transform Trans;
    bool SetPointFlg = false;

    public bool CrackFlg = false;
    public float CrackAngle;
    public Vector3 CrackPos;

    public Vector3 RayDirection;    //レイの向き
    public float RayAngle;

    public float Raylength = 1.0f;         //レイの長さ

    public int CreateNum = 0; // そのひびの生成順に割り当てられる

    
    float CreateTime = 0.0f;      //ひびが生成完了してから時間を計測する(敵をたおせるまでの時間)
    [SerializeField,Header("敵との当たり判定を取得する時間")]
    float EnemyHitTime = 0.5f;    //敵の当たり判定を取る時間

     //ひびの生成状態
    public enum CrackState
    {
        NoneCreate, //生成していない
        NowCreate,  //生成中
        OldCreate,  //生成終了
    }

    [SerializeField]
    public CrackState crackState = CrackState.NoneCreate;

    float _creatTime;// ひびを生成する時間

    float _nextAngle;// 次のひびの角度

    //------------------------------------------------------------------------------
    //―初期化処理―
    void Start()
    {
        EC2D = GetComponent<EdgeCollider2D>();
        Trans = GetComponent<Transform>();
        RayDirection = new Vector3(0,0,0);
        RayAngle = 0;

        crackState = CrackState.NowCreate;
        // オフセット
        EC2D.offset = new Vector2(this.transform.position.x * -1, this.transform.position.y * -1);

        // 座標リスト追加
        Points.Add(this.transform.position);
       
        // 入力された方向を入れる
        _nextAngle = CrackAngle - 90;
        RayAngle = (_nextAngle + 90) * Mathf.PI / 180;
        
        
        numSummon--;
    }
    //------------------------------------------------------------------------------
    //―更新処理―
    void Update()
    {
        _creatTime -= Time.deltaTime;

        //---------------------------------------------------------
        // 呼び出せるなら
        if (numSummon > 0 )
        {
            //---------------------------------------------------------
            //Rayを飛ばして何かとぶつかったら生成を止める

            //Rayの向きを設定
            RayDirection = new Vector3(Mathf.Cos(RayAngle) , Mathf.Sin(RayAngle) , 0);
            Vector3 origin = new Vector3(Points[Points.Count - 1].x,Points[Points.Count - 1].y + 0.0001f, 0.0f);
            Vector3 Distance = RayDirection * _Scale.y;
            
            RaycastHit2D hit = Physics2D.Raycast(origin, RayDirection, Raylength, 3);

            Debug.DrawRay(origin, Distance, Color.red,1000,false);

            bool noHit = true;// 当たっていないか
            //---------------------------------------------------
            //タグと同一の衝突したら最後のpoint座標を衝突した座標に合わせる
            if (hit)
            {
                
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag == "Crack" || hit.collider.gameObject.tag == "Ground")
                {
                    //Debug.Log(Points.Count);//何個目で当たったか
                    
                    if(hit.distance >= 0.2f) 
                    {
                        // 呼び出すオブジェの位置を設定
                        Vector3 pos = new Vector3(Points[Points.Count - 1].x, Points[Points.Count - 1].y, 0);
                        pos += new Vector3(Mathf.Cos(hit.distance) * 0.5f, Mathf.Sin(hit.distance) * 0.5f,0) ;
                        _crackObjects.Add(Instantiate(PrefabObject, hit.point, new Quaternion(0, 0, 0, 0), Trans));
                        // 角度設定
                        _crackObjects[_crackObjects.Count - 1].transform.localEulerAngles = new Vector3(0, 0, _nextAngle);
                        // サイズ設定
                        _Scale = new Vector3(_Scale.x, hit.distance / 2, _Scale.z);
                        _crackObjects[_crackObjects.Count - 1].transform.localScale = _Scale;
                        // 位置の更新
                        Points.Add(hit.point);
                        // ポイントセット
                        EC2D.SetPoints(Points);
                    }
                    numSummon = 0;
                    noHit = false;
                }
            }

            //---------------------------------------------------
            //衝突していないなら呼び出す
            if (noHit && _creatTime <= 0) 
            {
                //---------------------------------------------------
                // 呼び出すオブジェの位置を設定
                Vector3 pos = new Vector3(Points[Points.Count - 1].x, Points[Points.Count - 1].y, 0);
                pos += new Vector3(RayDirection.x * (_Scale.y / 2), RayDirection.y * (_Scale.y / 2), 0);
                _crackObjects.Add(Instantiate(PrefabObject, pos, new Quaternion(0, 0, 0, 0), Trans));
                // 角度設定
                _crackObjects[_crackObjects.Count - 1].transform.localEulerAngles = new Vector3(0, 0, _nextAngle);
                // サイズ設定
                _crackObjects[_crackObjects.Count - 1].transform.localScale = _Scale;
                // サイズを減少させる
                _Scale = new Vector3(_Scale.x * _decreaseRate.x, _Scale.y * _decreaseRate.y, _Scale.z * _decreaseRate.z);
               
                //---------------------------------------------------
                // 位置の更新
                Vector2 nextPos = Points[Points.Count - 1];
                nextPos += new Vector2(RayDirection.x * _Scale.y, RayDirection.y * _Scale.y);
                Points.Add(nextPos);
                // ポイントセット
                EC2D.SetPoints(Points);

                // 次の角度を決める
                _nextAngle += Random.Range(_nextAngleRange.x, _nextAngleRange.y);
                RayAngle = (_nextAngle + 90) * Mathf.PI / 180;// 更新

                numSummon--;
                _creatTime = WaitTime;// リセット
            }
            
        }
        //---------------------------------------------------------
        // 呼びきったら、ポイントをセットしてなければ
        else if (SetPointFlg == false) 
        {
            //--------------------------------
            //生成後時間計測、ひびを生成済みにする
            if(CreateTime < EnemyHitTime)
            {
                CreateTime += Time.deltaTime;
            }
            else
            {
                // 状態を生成済みにする
                crackState = CrackState.OldCreate;
                SetPointFlg = true;
            }
        }
        
    }
}
