//---------------------------------------------------------
//担当者：二宮怜
//内容　：背景オブジェクトの星が降る
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStar : MonoBehaviour
{
    //-変数宣言-

    [Header("星が降る速度")]
    public float FallSpeed = 10.0f;
    [Header("消滅までの時間")]
    public float DestroyTime = 3.0f;
    // 経過時間
    public float time = 0.0f;

    // 外部取得
    private Transform thisTransform;

    private void Start()
    {
        // Transform取得
        thisTransform = GetComponent<Transform>();

        // 星の降る速度をランダムに
        // 10～20
        FallSpeed = Random.Range(10.0f, 20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // 時間経過
        time += Time.deltaTime;

        // 生まれてから一定時間経過したら消滅
        if(time > DestroyTime)
        {
            Destroy(this.gameObject);
        }

        // 落ちる方向
        Vector3 fall = new Vector3(-0.5f, -1.0f, 0.0f);

        // 落ちる方向に現在地から移動させる
        thisTransform.Translate(fall * FallSpeed * Time.deltaTime);
    }
}
