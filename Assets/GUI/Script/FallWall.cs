//-----------------------------
//担当：菅眞心
//内容：壁が崩れていく処理(HP)
//-----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallWall : MonoBehaviour
{
    //----------------------------
    //変数宣言

    [System.NonSerialized]
    public GameObject NowCrackObj;     //最新のひびオブジェクト
    private CrackCreater Creater;      //ひびの生成システム
    private EdgeCollider2D CrackEdge;  //ひびのコライダーを保存
  
    [System.NonSerialized]
    public bool CreateCrackFlg = false;     //ひびが生成されているか

    private float Cracklength;          //ひびの長さ
    [SerializeField,Header("壁の耐久値")]
    private float WallEndress;          //壁の耐久値

    private RectTransform thisrect;     //このオブジェクトのRectTrans
    private float UIsizeraito;          //UIのサイズ減少率

    // Start is called before the first frame update
    void Start()
    {
        Cracklength = 0.0f;     //長さを初期化
        WallEndress = 50.0f;   //壁の耐久最大値
       
        thisrect = GetComponent<RectTransform>();

        UIsizeraito = thisrect.sizeDelta.x / WallEndress;   //UIのサイズ減少率

    }

    // Update is called once per frame
    void Update()
    {
        //--------------------------
        //ひびが生成された瞬間
        if (CreateCrackFlg)
        {
            //-----------------------------
            //最新のひびの情報を取得
            Creater = NowCrackObj.GetComponent<CrackCreater>();
            CreateCrackFlg = false;
    
        }

        //---------------------------------------
        //ひびの生成が終了したら長さを求める
        if (Creater != null && Creater.GetState() == CrackCreater.CrackCreaterState.CRAETED)
        {
            CracklengthAdd();

            //--------------------------------
            //長さから耐久値ゲージを縮小させる
            WallEndress -= Cracklength;
            thisrect.sizeDelta = new Vector2(thisrect.sizeDelta.x - (UIsizeraito * Cracklength), thisrect.sizeDelta.y);
            thisrect.position = new Vector3(thisrect.position.x - (UIsizeraito * Cracklength) / 2, thisrect.position.y);
            Cracklength = 0;

        }

    }

    //------------------------------
    //最新のひびの長さを求める
    private void CracklengthAdd()
    {
         //計算用にエッジコライダーの情報を取得
         CrackEdge = Creater.Edge2D;

         //----------------------------------------
         //ひびの長さをコライダーの長さから求める
         if (CrackEdge != null)
         {
             //-------------------------------
             //ひびのポイントの数だけHP計算
             for (int i = 0; i < Creater.Edge2D.pointCount - 1; i++)
             {
                 //ひびの長さを求める
                 Cracklength += Vector3.SqrMagnitude(CrackEdge.points[i] - CrackEdge.points[i + 1]);
             }

         }
         Creater = null;
        
    }

}
