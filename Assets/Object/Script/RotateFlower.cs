//---------------------------------------------------------
//担当者：二宮怜
//内容　：背景オブジェクトの花が回転する
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFlower : MonoBehaviour
{
    //----------------------------------------------------------------------------------------------------------
    // - 変数宣言 -

    [Header("回転速度")]
    public float RotateSpeed = 5.0f; // 花の回転速度

    public enum PATTERN
    {
        LEFT,
        RIGHT,
        MIX
    }
    [Header("回転方法が変わる")]
    public PATTERN pattern = PATTERN.LEFT;

    // 外部取得
    private Transform thisTransform; // このオブジェクトのTransformを取得する

    // Start is called before the first frame update
    void Start()
    {
        // Transformを取得する
        thisTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // 一秒間にRotateSpeedの数値分だけ回転する
        switch (pattern)
        {
            case PATTERN.LEFT:

                // 反時計回り
                thisTransform.Rotate(new Vector3(0, 0, 10 * RotateSpeed * Time.deltaTime));
                break;

            case PATTERN.RIGHT:

                // 時計回り
                thisTransform.Rotate(new Vector3(0, 0, -10 * RotateSpeed * Time.deltaTime));
                break;

            case PATTERN.MIX:

                // 時計回り→反時計回り→時計周り...繰り返し
                thisTransform.Rotate(new Vector3(0, 0, 10 * Mathf.Cos(Time.time) * RotateSpeed * Time.deltaTime));
                break;
        }
    }
}
