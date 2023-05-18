//---------------------------------
//担当：二宮怜
//内容：アイテムを浮遊させる
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyItem : MonoBehaviour
{
    // 変数宣言

    [Header("どのくらいの速さで")]
    public float UpDownSpeed = 1.0f;
    [Header("どのくらい上下させるか")]
    public float Difference = 0.3f;

    [Header("何秒で一回転するか")]
    private float rotateSecond = 10f;

    private float initTransformY;

    // Start is called before the first frame update
    void Start()
    {
        // Yの初期座標を保持
        initTransformY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        var rot = 360f * Time.deltaTime / rotateSecond;

        // Mathf.PingPong(時間,上下差)
        transform.position = new Vector3(transform.position.x, initTransformY + Mathf.PingPong(Time.time * UpDownSpeed, Difference), transform.position.z);
        transform.Rotate(0f, rot, 0f);
    }
}