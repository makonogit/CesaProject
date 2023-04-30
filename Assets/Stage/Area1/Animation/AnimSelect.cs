//----------------------------------------------------
//　内容：チュートリアル用のアニメーションを選択する
//　担当：菅眞心
//----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSelect : MonoBehaviour
{
    //------------------------------
    //　変数宣言

    // 再生するアニメーションの種類
    public enum PlayAnim{
        Move,
        Hammer = 5,
        Jump = 4
    }

    [SerializeField,Header("再生するアニメーション")]
    private PlayAnim playanim;

    //--------------------------------
    //　外部取得
    private Animator anim;  //　チュートリアル用のAnimator


    // Start is called before the first frame update
    void Start()
    {
        //　このオブジェクトのAnimatorを取得
        anim = GetComponent<Animator>();

        //---------------------------
        //　再生するアニメーションを選択
        anim.SetInteger("Select", (int)(playanim));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
