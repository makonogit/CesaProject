//---------------------------------------
//　担当：菅眞心
//  内容：ボスが倒れた後の処理
//---------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeat : MonoBehaviour
{
    [SerializeField, Header("生成するひび")]
    private GameObject CoreCrack;

    [SerializeField, Header("ひびのTransform")]
    private Transform CrackTrans;

    private GameObject CoreManager;  // ひびを管理しているオブジェクト

    private StageStatas statas;     //　ステージステータス用

    private bool Create = false;    //生成フラグ 

    // Start is called before the first frame update
    void Start()
    {
        CoreManager = GameObject.Find("Core");
        statas = transform.root.transform.GetChild(0).GetComponent<StageStatas>();
    }

    // Update is called once per frame
    void Update()
    {
        // ボスが消えたら
        if(transform.childCount == 0 && CoreManager.transform.childCount == 0 && !Create)
        {
            //　ひび生成
            GameObject obj = Instantiate(CoreCrack, CrackTrans);
            obj.GetComponent<SpriteRenderer>().sortingOrder = 2;
            obj.transform.parent = CoreManager.transform;   // Coreオブジェクトの子オブジェクトにする
            statas.SetStageCrystal(1);
            Create = true;
        }
        
    }
}
