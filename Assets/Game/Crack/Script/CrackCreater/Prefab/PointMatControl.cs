//---------------------------------
// 担当：菅
// 内容：マテリアルの管理
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMatControl : MonoBehaviour
{
    public Material NormalMat;      //　通常のMaterial
    public Material NormalEndMat;   //　通常の先端Material
    public Material FlashMat;       //  発光Material
    public Material FlashEndMat;    //  発光先端Material

    [SerializeField, Header("SpeiteRenderer")]
    private SpriteRenderer thisrenderer;

    //float Wait = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 通常マテリアルを設定
        NormalMat = GetComponent<SpriteRenderer>().material;
        //FirstCheck = true;
    }

    private void Update()
    {
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

    public void NormalCrackEnd()
    {
        thisrenderer.material = NormalEndMat;
    }

    public void FlashCrackEnd()
    {
        thisrenderer.material = FlashEndMat;
    }

}
