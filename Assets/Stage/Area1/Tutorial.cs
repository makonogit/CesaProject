//-----------------------------------
//担当：菅眞心
//内容：チュートリアル用UIの表示
//-----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
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
    private GameObject Abuttton;           //AボタンUI
    Animator anim;                         //Animator


    // Start is called before the first frame update
    void Start()
    {
        //------------------------------------
        // プレイヤーのオブジェクトを取得
        Player = GameObject.Find("player");
        PlayerTrans = Player.transform;

        //　自身のTransform
        thisTrans = transform;

        //　アニメーションするUIの取得(最後の子オブジェクト)
        buttonUI = transform.GetChild(transform.childCount - 1).gameObject;

        //　AボタンUI
        Abuttton = transform.Find("Abutton").gameObject;

        //　UIにAnimatorがあれば取得
        if (buttonUI.GetComponent<Animator>())
        {
            anim = buttonUI.GetComponent<Animator>();
        }

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
            //　拡大終了したらUIをアニメーションさせる
            if (OpenAnim())
            {
                //　移動用UI
                if (buttonUI.name == "Lstick_front")
                {
                    if (buttonUI.transform.localPosition.x < 1.5f)
                    {
                        buttonUI.transform.localPosition = new Vector3(buttonUI.transform.localPosition.x + (MoveSpeed / 4) * Time.deltaTime, buttonUI.transform.localPosition.y, 0.0f);
                    }
                    else
                    {
                        //　一定時間止まってから初期位置に移動
                        waitUItime += Time.deltaTime;
                        
                        if (waitUItime > 0.5f)
                        {
                            waitUItime = 0.0f;
                            buttonUI.transform.localPosition = new Vector3(0.92f, buttonUI.transform.localPosition.y, 0.0f);
                        }
                    }
                }

                // ジャンプ用UI
                if(buttonUI.name == "UIPlayerJump")
                {
                    //　ジャンプアニメーションを再生
                    anim.SetInteger("Select", 1);

                    if (buttonUI.transform.localPosition.x < 0.3f)
                    {
                        if (buttonUI.transform.localPosition.x > 0.0f)
                        {
                            gravity = -1.0f;
                        }

                        Abuttton.transform.localScale = new Vector3(Abuttton.transform.localScale.x + (gravity * -1.0f) * Time.deltaTime, Abuttton.transform.localScale.y + (gravity * -1.0f) * Time.deltaTime,1.0f);
                        buttonUI.transform.localPosition = new Vector3(buttonUI.transform.localPosition.x + (MoveSpeed / 2) * Time.deltaTime, buttonUI.transform.localPosition.y + gravity * Time.deltaTime, 0.0f);
                    }
                    else
                    {
                        //　一定時間止まってから初期位置に移動
                        waitUItime += Time.deltaTime;

                        if (waitUItime > 0.5f)
                        {
                            anim.Play("TutorialJump", 0, 0);
                            gravity = 1.5f;
                            waitUItime = 0.0f;
                            buttonUI.transform.localPosition = new Vector3(-0.86f, -0.4f, 0.0f);
                            Abuttton.transform.localScale = new Vector3(1.0f,1.1f,1.0f);
                        }
                    }
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
    }
}
