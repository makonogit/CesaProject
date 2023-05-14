//---------------------------------
// 担当：菅
// 内容：マテリアルの管理
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMatControl : MonoBehaviour
{
    public Material NormalMat; //　通常のMaterial
    public Material FlashMat;  //  発光Material

    [SerializeField, Header("SpeiteRenderer")]
    private SpriteRenderer thisrenderer;

    // Start is called before the first frame update
    void Start()
    {
        // 通常マテリアルを設定
        //NormalMat = GetComponent<SpriteRenderer>().material;
    }

    // ひびを発光させる関数
    public void FlashCrack()
    {
        thisrenderer.material = FlashMat;
    }

    public void NomalCrack()
    {
        thisrenderer.material = NormalMat;
    }

}
