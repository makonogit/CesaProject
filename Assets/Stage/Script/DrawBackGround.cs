//---------------------------------------------------------
//担当者：二宮怜
//内容　：背景を体力によって変化させる
//---------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBackGround : MonoBehaviour
{
    //---------------------------------------------------------
    // - 変数宣言 -
    //[Header("画像が差し変わるHpの参照値")]
    //public float[] HpLine =
    //{
    //    0.9f,
    //    0.6f,
    //    0.3f,
    //};

    //public enum BACKGROUNDSTATUS
    //{
    //    UN_DAMAGED,      // 無傷
    //    LITTLE_DAMAGED,  // すこしひび
    //    HALF_DAMAGED,    // 半分くらいひび
    //    ALMOST_DAMAGED,  // ほぼひび
    //}

    //// 初めは無傷
    //public BACKGROUNDSTATUS BackGroundStatus = BACKGROUNDSTATUS.UN_DAMAGED;

    //// 外部取得
    //private GameObject WallGauge;
    //private Wall_HP_System_Script wallHP;

    //private SpriteRenderer Sprite; // 画像を変更するための変数
    //[SerializeField] Sprite[] sprites; // 画像名を入れておく

    //// Start is called before the first frame update
    //void Start()
    //{
    //    WallGauge = GameObject.Find("Wall_Hp_Gauge");
    //    wallHP = WallGauge.GetComponent<Wall_HP_System_Script>();

    //    Sprite = GetComponent<SpriteRenderer>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    // 画像を変更するか
    //    bool ChangeSprite = false;

    //    // それぞれの状態で指定のHP以下になったら
    //    switch (BackGroundStatus)
    //    {
    //        case BACKGROUNDSTATUS.UN_DAMAGED:

    //            if (wallHP._nowHP <= HpLine[(int)BackGroundStatus])
    //            {
    //                // 状態を変化
    //                BackGroundStatus = BACKGROUNDSTATUS.LITTLE_DAMAGED;
    //                ChangeSprite = true;
    //            }
    //            break;

    //        case BACKGROUNDSTATUS.LITTLE_DAMAGED:
    //            if (wallHP._nowHP < HpLine[(int)BackGroundStatus])
    //            {
    //                // 状態を変化
    //                BackGroundStatus = BACKGROUNDSTATUS.HALF_DAMAGED;
    //                ChangeSprite = true;
    //            }
    //            break;

    //        case BACKGROUNDSTATUS.HALF_DAMAGED:
    //            if (wallHP._nowHP < HpLine[(int)BackGroundStatus])
    //            {
    //                // 状態を変化
    //                BackGroundStatus = BACKGROUNDSTATUS.ALMOST_DAMAGED;
    //                ChangeSprite = true;
    //            }
    //            break;
    //    }

    //    // スプライト変更命令がでていたら
    //    if (ChangeSprite)
    //    {
    //        // 少しひびの入った背景画像に差し替え
    //        Sprite.sprite = sprites[(int)BackGroundStatus];
    //    }
    //}
}
