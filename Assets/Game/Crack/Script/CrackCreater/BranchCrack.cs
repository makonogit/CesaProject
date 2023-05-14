//---------------------------------
//　担当:菅眞心
//　内容：分岐ひびの生成後処理
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchCrack : MonoBehaviour
{
    GameObject ParentCrack; //本筋のひび
    CrackCreater ParentCrackCreater;    //本筋のひびのCarckCreater
    CrackCreater creater;   //このオブジェクトのCrackCreater

    [SerializeField, Header("生成する分岐ひび")]
    private GameObject BranchObj;
    private CrackCreater branchcreater;

    [SerializeField, Header("ひびのTexture")]
    private Texture Crack;

    [SerializeField, Header("ひびの先端Texture")]
    private Texture Crackend;

    private Hammer hammer;  //Hammerスクリプト、ひび生成用

    private bool Create = false;    //生成フラグ
    private int RandomCrack = 0;

    int StartBranch = 0;

    // Start is called before the first frame update
    void Start()
    {
        ParentCrack = transform.parent.gameObject;  //本筋のヒビのオブジェクトを取得
        ParentCrackCreater = ParentCrack.GetComponent<CrackCreater>();

        //Hammerスクリプトの取得
        hammer = GameObject.Find("player").GetComponent<Hammer>();

        creater = GetComponent<CrackCreater>();
        branchcreater = BranchObj.GetComponent<CrackCreater>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        //{
        //    if (Create)
        //    {
        //        RandomCrack = Random.Range(0, 100); //0～100の乱数取得
        //        //生成終了したらコライダーを無効化
        //        GetComponent<EdgeCollider2D>().enabled = false;
        //        Create = false;
        //    }
        //}

        ////　親のひびが追加生成されたら一定の確率で分岐ひびが伸びる
        //if (ParentCrackCreater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING)
        //{
        //    if (!Create)
        //    {
        //        if (RandomCrack < 2)
        //        {
        //            if (creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        //            {
        //                GetComponent<EdgeCollider2D>().enabled = true;
        //                //StartBranch = hammer.CreateBranch(BranchObj, gameObject, branchcreater, StartBranch);
        //                //creater.SetState(CrackCreater.CrackCreaterState.ADD_CREATEBACK);
        //            }
        //        }
        //        Create = true;
        //    }
        //}

        // 生成終了したらスプライトの変更
        if (creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATE)
        {
            transform.GetChild(transform.childCount - 1).GetComponent<PointMatControl>().
                NormalMat.SetTexture("_MainTexture", Crack);
            GetComponent<EdgeCollider2D>().enabled = true;
        }

        // 生成終了したらスプライトの変更
        if (creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        {
            transform.GetChild(transform.childCount - 1).GetComponent<PointMatControl>().
                NormalMat.SetTexture("_MainTexture",Crackend);
            GetComponent<EdgeCollider2D>().enabled = false;
        }
    }
}
