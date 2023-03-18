using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMove : MonoBehaviour
{

    public int FallNum; // タイトル画面が崩壊するまでの数

    string TitleBackName;   //　背景のオブジェクト名

    //-----------
    // 外部取得
    //-----------
    private GameObject BreakObj;     //ScreenBreakを所持しているオブジェクト
    private ScreenBreak _ScreenBreak; //ScreenBreakを取得する変数

    SpriteRenderer MainSpriteRenderer;

    public List<Sprite> TitleBackSprite;

    private float BreakTime;        //画面が崩壊するまでの時間


    //----------------------------------------
    //追加：菅
    GameObject CrystalObj;      //クリスタルの背景オブジェクト
    SpriteRenderer CrystalRenderer;     //クリスタルのRender
    float CrystalAlpha;                 //クリスタルの透明度
    
    // Start is called before the first frame update
    void Start()
    {
        //タイトルのSptiteRenderereを取得
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        // ScreenBreakを取得
        BreakObj = GameObject.Find("ScreenBreak");
        _ScreenBreak = BreakObj.GetComponent<ScreenBreak>();

        BreakTime = 0.0f;

        //--------------------------------------------
        //クリスタルの状態を取得
        CrystalObj = GameObject.Find("Crystal");
        CrystalRenderer = CrystalObj.GetComponent<SpriteRenderer>();
        CrystalAlpha = CrystalRenderer.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        //  キー入力
        if (Input.GetKeyDown("joystick button 0"))
        {
            FallNum--;

            //-------------------------------------------------
            //スプライトを変更
            if (FallNum > 0)
            {
                CrystalAlpha += (1.0f / 255.0f) * 20;
                CrystalRenderer.color = new Color(1.0f, 1.0f, 1.0f, CrystalAlpha);
               // MainSpriteRenderer.sprite = TitleBackSprite[FallNum - 1];
            }
            //-------------------------------------------------

            //---------------------------------------------------
            // 崩壊までのカウントダウンが0になったらシーン移動
            if (FallNum == 0)
            {
                _ScreenBreak.enabled = true;
            }

        }

        if (_ScreenBreak.enabled)
        {
            BreakTime += Time.deltaTime;
            //3秒経過したらscene移動
            if (BreakTime > 3.0f)
            {
                SceneManager.LoadScene("SelectScene");
            }
        }
    }

}
        