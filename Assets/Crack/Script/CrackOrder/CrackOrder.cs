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

    [Header("触らないでください。")]
    public List<Vector2> Points;

    [System.NonSerialized]
    public EdgeCollider2D EC2D;

    Transform Trans;
    bool SetPointFlg = false;

    public bool CrackFlg = false;
    public float CrackAngle;
    public Vector3 CrackPos;

    public Vector3 RayDirection;    //レイの向き
    public float RayAngle;

    public float Raylength = 0.5f;         //レイの長さ

    //------------------------------------------------------------------------------
    //―初期化処理―
    void Start()
    {
        EC2D = GetComponent<EdgeCollider2D>();
        Trans = GetComponent<Transform>();
        RayDirection = new Vector3(0,0,0);
        RayAngle = 0;
    }
    //------------------------------------------------------------------------------
    //―更新処理―
    void Update()
    {
        //---------------------------------------------------------
        //Rayを飛ばして何かとぶつかったら生成を止める

        //Rayの向きを設定
        RayDirection = new Vector3(Mathf.Cos((RayAngle + 90) * Mathf.PI / 180) , Mathf.Sin((RayAngle + 90) * Mathf.PI / 180) , 0);
        Vector3 origin = new Vector3(EC2D.points[EC2D.pointCount - 1].x, EC2D.points[EC2D.pointCount - 1].y, 0.0f);
        Vector3 Distance = RayDirection * Raylength;

        RaycastHit2D hit = Physics2D.Raycast(origin, RayDirection, Raylength, -1);

        Debug.DrawRay(origin, Distance, Color.red);


        //---------------------------------------------------
        //衝突したら最後のpoint座標を衝突した座標に合わせる
        if (hit)
        {
            EC2D.points[EC2D.pointCount - 1] = hit.point;
            numSummon = 0;
        }


        if (CrackFlg)
        {
            //---------------------------------------------------------
            // 呼び出すオブジェの位置を設定
            GameObject obj = Instantiate(PrefabObject, CrackPos, Quaternion.Euler(new Vector3(0.0f, 0.0f, CrackAngle)), Trans);
            // 角度設定
            obj.transform.localEulerAngles = new Vector3(0, 0, CrackAngle - 90);
            float radian = (CrackAngle - 90) * Mathf.PI / 180;
            RayAngle += CrackAngle - 90; 
            // 呼んだので呼び数を減らす。
            numSummon--;
            CrackFlg = false;
      
        }

        //---------------------------------------------------------
        // 呼びきったら、ポイントをセットしてなければ
        if (numSummon <= 0 && SetPointFlg == false)
        {
            SetPointFlg = true;
            // ポイントセット
            //EC2D.SetPoints(Points);
            EC2D.offset = new Vector2(this.transform.position.x * -1, this.transform.position.y * -1);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    // Debug.Log("Hit");
    //    if (collision.CompareTag("Ground"))
    //    {
    //        numSummon = 0;
    //    }
    //    if (collision.CompareTag("Crack"))
    //    {
    //        Debug.Log("Hit");
    //    }
    //}

}
