//--------------------------------
//担当：菅眞心
//内容：クリスタルの破壊
//--------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BraekCrystal : MonoBehaviour
{
    StageStatas stageStatas;    //ステージのステータス管理

    // Start is called before the first frame update
    void Start()
    {
        //-------------------------------------
        //ステージのステータスを取得
        stageStatas = transform.root.gameObject.GetComponent<StageStatas>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //--------------------------------------------------
        // 釘かひびが衝突したら自身を破壊
        if(collision.gameObject.tag == "UsedNail" || collision.gameObject.tag == "Crack")
        {
            //----------------------------------------------
            //　ステージのクリスタル数を1つ消去
            stageStatas.SetStageCrystal(stageStatas.GetStageCrystal() - 1);
            Destroy(this.gameObject);
        }
    }

}
