//---------------------------------------------------------
//担当者：二宮怜
//内容　：指定範囲内にランダムに星を生成
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomPosition : MonoBehaviour
{
    // 変数宣言

    // 生成する間隔
    public float CreateTime = 2.0f;
    // 経過時間
    private float time = 0.0f;
    // 生成されたオブジェクト
    private GameObject Obj;

    // 外部取得
    [SerializeField]
    [Tooltip("生成するGameObject")]
    public GameObject createPrefab;

    // 親オブジェクト
    private GameObject parent;

    // ゲームオブジェクト
    private GameObject Area_A;
    private GameObject Area_B;

    // 上のゲームオブジェクトのTransform
    private Transform rangeA;
    private Transform rangeB;

    private void Start()
    {
        // オブジェクト探す
        Area_A = GameObject.Find("CreateArea_A");
        Area_B = GameObject.Find("CreateArea_B");

        rangeA = Area_A.GetComponent<Transform>();
        rangeB = Area_B.GetComponent<Transform>();

        parent = GameObject.Find("Stars");
    }

    // Update is called once per frame
    void Update()
    {
        // 前フレームからの時間を加算していく
        time = time + Time.deltaTime;

        // 指定時間置きにランダムに生成されるようにする。
        if (time > CreateTime)
        {
            // rangeAとrangeBのx座標の範囲内でランダムな数値を作成
            float x = Random.Range(rangeA.position.x, rangeB.position.x);
            // rangeAとrangeBのy座標の範囲内でランダムな数値を作成
            float y = Random.Range(rangeA.position.y, rangeB.position.y);

            // GameObjectを上記で決まったランダムな場所に生成
            Obj = Instantiate(createPrefab, new Vector3(x, y, 0), createPrefab.transform.rotation);
            // Starsの子オブジェクトとして生成
            Obj.transform.parent = parent.transform;

            // 経過時間リセット
            time = 0f;
        }
    }
}
