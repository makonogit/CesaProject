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

        if (OpenFlg)
        {
            OpenAnim();
        }

    }

    //-------------------------------
    // UIの表示アニメーション
    private void OpenAnim()
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
    }

}
