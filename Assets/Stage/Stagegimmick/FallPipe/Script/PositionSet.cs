//---------------------------------------------------------
//担当者：二宮怜
//内容　：一番左のパイプを基準にパイプとユニオンクリスタルの座標をセット
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSet : MonoBehaviour
{
    // 変数宣言

    // 外部取得

    // 左のパイプ情報
    private GameObject LeftPipe;
    private Transform LeftPipeTransform;

    // 中央のパイプ情報
    private GameObject MiddlePipe;
    private Transform MiddlePipeTransform;

    // 右のパイプ情報
    private GameObject RightPipe;
    private Transform RightPipeTransform;

    // 接合部クリスタルマネージャー
    private GameObject CrystalManager;

    // 左の接合部クリスタル情報
    private GameObject LeftUnionCrystal;
    private Transform LeftUnionCrystalTransform;

    // 右の接合部クリスタル情報
    private GameObject RightUnionCrystal;
    private Transform RightUnionCrystalTransform;

    // Start is called before the first frame update
    void Start()
    {
        // 子オブジェクトを順番に取得

        // 左パイプ
        LeftPipe = transform.GetChild(0).gameObject;
        LeftPipeTransform = LeftPipe.GetComponent<Transform>();
        //Debug.Log(LeftPipe);

        // 右パイプ
        RightPipe = transform.GetChild(1).gameObject;
        RightPipeTransform = RightPipe.GetComponent<Transform>();
        //Debug.Log(RightPipe);

        // 中央パイプ
        MiddlePipe = transform.GetChild(2).gameObject;
        MiddlePipeTransform = MiddlePipe.GetComponent<Transform>();
        //Debug.Log(MiddlePipe);

        // クリスタルマネージャー
        CrystalManager = transform.GetChild(3).gameObject;
        //Debug.Log(CrystalManager);

        // 接合部クリスタル（左）
        LeftUnionCrystal = CrystalManager.transform.GetChild(0).gameObject;
        LeftUnionCrystalTransform = LeftUnionCrystal.GetComponent<Transform>();
        //Debug.Log(LeftUnionCrystal);

        // 接合部クリスタル（右）
        RightUnionCrystal = CrystalManager.transform.GetChild(1).gameObject;
        RightUnionCrystalTransform = RightUnionCrystal.GetComponent<Transform>();
        //Debug.Log(RightUnionCrystal);

        // 座標セット
        Init_Position();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init_Position()
    {
        // 一番左のパイプを基準に配置していく

        // 中央のパイプ配置
        MiddlePipeTransform.localPosition = new Vector3(
            LeftPipeTransform.localPosition.x + LeftPipeTransform.localScale.x,
            LeftPipeTransform.localPosition.y,
            LeftPipeTransform.localPosition.z);

        // 右のパイプ配置
        RightPipeTransform.localPosition = new Vector3(
            MiddlePipeTransform.localPosition.x + MiddlePipeTransform.localScale.x,
            MiddlePipeTransform.localPosition.y,
            MiddlePipeTransform.localPosition.z);

        // 左のクリスタル配置
        LeftUnionCrystalTransform.localPosition = new Vector3(
            LeftPipeTransform.localPosition.x + LeftPipeTransform.localScale.x / 2,
            LeftPipeTransform.localPosition.y,
            LeftPipeTransform.localPosition.z);

        // 右のクリスタル配置
        RightUnionCrystalTransform.localPosition = new Vector3(
            MiddlePipeTransform.localPosition.x + MiddlePipeTransform.localScale.x / 2,
            MiddlePipeTransform.localPosition.y,
            MiddlePipeTransform.localPosition.z);
    }
}
