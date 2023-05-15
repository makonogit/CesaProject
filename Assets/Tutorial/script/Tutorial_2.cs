//-----------------------------------
//担当：菅眞心
//内容：チュートリアル用UIの表示
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_2 : MonoBehaviour
{
    //--------------------------------------
    //　変数宣言
    
    //--------------------------------------
    //　外部取得
    private GameObject Player;      // プレイヤーのオブジェクト
    private Transform PlayerTrans;  // プレイヤーのTransform

    private Transform thisTrans;    // 自身のTransform

    [SerializeField,Header("どのくらいの距離で表示されるか")]
    private float OpenDistance;

    [SerializeField,Header("UIのサイズ")]
    private Vector3 UIsize;

    [SerializeField, Header("拡大縮小スピード")]
    private float MoveSpeed;

    private bool OpenFlg = false;   //表示フラグ

    GameObject buttonUI;                   //アニメーションするUI
    private float waitUItime = 0.0f;       //アニメーション終了から一定時間停止
    private float gravity = 1.5f;          //アニメーション用重力加速度

    [SerializeField] private GameObject Abuttton;           //AボタンUI
    [SerializeField] private GameObject RTbutton;           //RTボタンUI
    [SerializeField] private GameObject Lstick;             //LスティックUI
    [SerializeField] private GameObject Crack;              //ひびのオブジェクト
    [SerializeField] private GameObject BButton;            //BボタンUI
    [SerializeField] private Animator anim;                         //Animator


    public enum TutorialType
    {
        CrackMove,  //ひびの移動
        AddCrack    //ひびを伸ばす
    }

    public TutorialType type;

    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------
        // プレイヤーのオブジェクトを取得
        Player = GameObject.Find("player");
        PlayerTrans = Player.transform;

        //　自身のTransform
        thisTrans = transform;

      
    }

    // Update is called once per frame
    void Update()
    {

        // プレイヤーとUIの距離を求める
        float Distance = Vector3.Magnitude(PlayerTrans.position - thisTrans.position);
        
        //--------------------------------------------
        //　表示距離まで近づいたらUIの表示アニメーションを再生
        if(Distance < OpenDistance)
        {
            OpenFlg = true;
        }
        else
        {
            //----------------------------------------
            //　離れたら閉じる
            OpenFlg = false;
        }

        if (OpenFlg)
        {
            //--------------------------------------
            // パネルが開いたらアニメーションする
            if (OpenAnim())
            {
                switch (type)
                {
                    case TutorialType.CrackMove:
                        anim.SetBool("CrackMove",true);
                        break;
                    case TutorialType.AddCrack:
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            CloseAnim();
        }

    }

    //-------------------------------
    // UIの表示アニメーション
    private bool OpenAnim()
    {
        //---------------------------------------------------
        // UIを拡大する
        if (thisTrans.localScale.x < UIsize.x)
        {
            thisTrans.localScale = new Vector3(thisTrans.localScale.x + (MoveSpeed + 1) * Time.deltaTime, thisTrans.localScale.y, 1.0f);
        }
        if (thisTrans.localScale.y < UIsize.y)
        {
            thisTrans.localScale = new Vector3(thisTrans.localScale.x, thisTrans.localScale.y + MoveSpeed * Time.deltaTime, 1.0f);
        }

        //　拡大終了したらtrueを返す
        return thisTrans.localScale.x >= UIsize.x && thisTrans.localScale.y >= UIsize.y;
    }

    //---------------------------------
    //　UIを非表示するアニメーション
    private void CloseAnim()
    {
        //---------------------------------------------------
        // UIを縮小する
        if (thisTrans.localScale.x > 0.0f)
        {
            thisTrans.localScale = new Vector3(thisTrans.localScale.x - (MoveSpeed + 1) * Time.deltaTime, thisTrans.localScale.y, 1.0f);
        }
        if (thisTrans.localScale.y > 0.0f)
        {
            thisTrans.localScale = new Vector3(thisTrans.localScale.x, thisTrans.localScale.y - MoveSpeed * Time.deltaTime, 1.0f);
        }

        switch (type)
        {
            case TutorialType.CrackMove:
                anim.SetBool("CrackMove", false);
                break;
            case TutorialType.AddCrack:
                break;
            default:
                break;
        }
    }
}
