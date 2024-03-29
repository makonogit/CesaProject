//-------------------------------------
// 担当：菅眞心
// 内容：ひびの分岐
//-------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchCreater : MonoBehaviour
{
    //--------------------------
    //　変数宣言
    [SerializeField,Header("生成する分岐ひび")]
    private GameObject BranchObj;       //生成する分岐ひび
    private CrackCreater branchcreater; //分岐ひびのCrackCreater
    private CrackCreater creater;       //このオブジェクトのCrackCreater

    [SerializeField, Header("ひびのTexture")]
    private Material Crack;

    [SerializeField, Header("ひびの先端Material")]
    private Material Crackend;

    [SerializeField, Header("ひびの先端Material(発光)")]
    private Material CrackendFlash;

    private Hammer hammer;      //Hammerスクリプト

    private bool Branch = false;    //1回判定用

    private List<Vector2> PointList;    //ひびのPointList

    private int StartBranch = 1;    //分岐ひびの生成開始値

    // Start is called before the first frame update
    void Start()
    {
        //Createrの取得
        creater = GetComponent<CrackCreater>();
        branchcreater = BranchObj.GetComponent<CrackCreater>();

        //Hammerスクリプトの取得
        hammer = GameObject.Find("player").GetComponent<Hammer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATE)
        {
           
        }

        if (creater.GetState() == CrackCreater.CrackCreaterState.ADD_CREATING)
        {
            //Debug.Log("aaaaaaaaaaaaaaaa");
            if (Branch)
            {

                //　先端のスプライトを変更
                if (transform.childCount > 0)
                {
                    for (int i = transform.childCount - 1; i<transform.childCount; i--)
                    {

                        if (transform.GetChild(i).name == "Point(Clone)")
                        {
                            transform.GetChild(i).GetComponent<PointMatControl>().NomalCrack();
                            break;
                        }
                    }
                }

                StartBranch++;
                Branch = false;
            }
        }

        //　生成が終了していたら
        if (creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        {

            if (!Branch)
            {
                //　先端のスプライトを変更
                if (transform.childCount > 1)
                {
                    for (int i = transform.childCount - 1; i < transform.childCount; i--)
                    {
                        if (transform.GetChild(i).name == "Point(Clone)")
                        {
                            transform.GetChild(i).GetComponent<PointMatControl>().NormalCrackEnd();
                            break;
                        }
                    }

                    //　分岐ひび生成
                    StartBranch = hammer.CreateBranch(BranchObj, gameObject, branchcreater, StartBranch);
                }
                Branch = true;
            }
        }
    }

    public void SetPointList(List<Vector2> pointlist)
    {
        PointList = pointlist;
    }
}
