//--------------------------------
//　担当:菅眞心
//　内容：エリア4でのLight設定
//---------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSet : MonoBehaviour
{
    private SpriteRenderer thisrenderer;
    private Light2D thislight;

    // Start is called before the first frame update
    void Start()
    {
        thislight = GetComponent<Light2D>();

        SetStage stage = new SetStage();
        if(stage.GetAreaNum() == 3)
        {
            thislight.intensity = 1.0f;
        }

        thisrenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //　オブジェクトが消えたらライトも消す
        if(thisrenderer.color.a == 0.0f)
        {
            thislight.intensity = 0.0f;
        }
        else
        {
            thislight.intensity = 1.0f;
        }
    }

}
