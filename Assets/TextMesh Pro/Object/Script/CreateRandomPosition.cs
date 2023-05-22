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

    [Header("総生成数"),SerializeField] private int CreateNumAll = 20;
    private int CreateNumNow = 0;

    // 生成されたオブジェクト
    private GameObject Obj;

    // 外部取得
    [SerializeField]
    [Tooltip("生成するGameObject")]
    public GameObject createPrefab;

    // 上のゲームオブジェクトのTransform
    [SerializeField] private Transform rangeA;
    [SerializeField] private Transform rangeB;

    [SerializeField] private Transform _thisTransform;

    private void Start()
    {
        // オブジェクト探す
        //Area_A = GameObject.Find("CreateArea_A");
        //Area_B = GameObject.Find("CreateArea_B");

        //rangeA = Area_A.GetComponent<Transform>();
        //rangeB = Area_B.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // 生成予定総数より少なければ追加生成
        if (CreateNumNow < CreateNumAll)
        {
            // 前フレームからの時間を加算していく
            time = time + Time.deltaTime;

            // 指定時間置きにランダムに生成されるようにする。
            if (time > CreateTime)
            {
                // 特定の範囲内の座標をランダムで取得
                var pos = GetSpawnPos();

                // GameObjectを上記で決まったランダムな場所に生成
                Obj = Instantiate(createPrefab, new Vector3(pos.x, pos.y, 0), createPrefab.transform.rotation);
                // Starsの子オブジェクトとして生成
                Obj.transform.parent = _thisTransform;

                // 経過時間リセット
                time = 0f;

                // 生成数カウント
                CreateNumNow++;
            }
        }
    }

    public Vector2 GetSpawnPos()
    {
        // 戻り値用変数
        Vector2 pos;

        // rangeAとrangeBのx座標の範囲内でランダムな数値を作成
        pos.x = Random.Range(rangeA.position.x, rangeB.position.x);
        // rangeAとrangeBのy座標の範囲内でランダムな数値を作成
        pos.y = Random.Range(rangeA.position.y, rangeB.position.y);

        return pos;
    }
}
