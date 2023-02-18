//------------------------------------------------------------------------------
// 担当者：中川直登
// 内容  ：ひびを作る
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCrack : MonoBehaviour
{
    //------------------------------------------------------------------------------
    //―変数宣言―
    [Header("次オブジェの最小ｘ最大ｙ角度")]
    public Vector2 nextAngleRange;
    [Header("次オブジェのサイズ減少率")]
    public Vector3 decreaseRate = new Vector3(0.9f, 1.0f, 1.0f);
    [Header("次オブジェの呼び出し時間")]
    public float waitTime = 0.01f;
    [Header("呼び出すオブジェ")]
    public GameObject PrefabObject;
    Transform Trans;
    bool nextSummon = false;
    CrackOrder Order;
    

    //------------------------------------------------------------------------------
    //―初期化処理―

    void Start()
    {
        Trans = GetComponent<Transform>();
        Order = GetComponentInParent<CrackOrder>();
        // EdgeCollider2Dにこのオブジェの位置を設定する
        Order.Points.Add(this.transform.position);
        //毎回Pointを更新する
        Order.EC2D.SetPoints(Order.Points);

    }

    //------------------------------------------------------------------------------
    //―更新処理―

    void Update()
    {
        // 待ち時間の処理
        waitTime -= Time.deltaTime;
        //---------------------------------------------------------
        // 待ち時間が0以下の時、オーダーの数がまだある時、次のオブジェクトを呼び出してない時
        if (waitTime <= 0 && Order.numSummon > 0 && nextSummon == false)
        {
            //---------------------------------------------------------
            // 子のオブジェクトの角度を決める（x～yの間をランダムで）
            float angle =Random.Range(nextAngleRange.x, nextAngleRange.y);
            // 子オブジェのｙサイズの半分
            float radius = decreaseRate.y / 2;
            // ラジアンを求める
            float radian = ((angle + 90.0f) / 180.0f) * Mathf.PI;

            //---------------------------------------------------------
            // 子オブジェを作る
            GameObject obj = Instantiate(PrefabObject, Trans);

            // 子オブジェの位置（親から見た）
            obj.transform.localPosition = new Vector3(radius * Mathf.Cos(radian), 1.0f + radius * Mathf.Sin(radian), 0);
            // 子オブジェの角度（親から見た）
            obj.transform.localEulerAngles = new Vector3(0, 0, angle);
            // 子オブジェのサイズ（親から見た）
            obj.transform.localScale = decreaseRate;
            

            Order.RayAngle += angle;
            //Order.RayDirection = Vector3.up;
            //---------------------------------------------------------
            nextSummon = true;
            Order.numSummon--;
        }
     
    }

}
