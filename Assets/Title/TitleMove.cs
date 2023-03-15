using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMove : MonoBehaviour
{
    public int FallNum; // タイトル画面が崩壊するまでの数

    string TitleBackName;   //　背景のオブジェクト名

    SpriteRenderer MainSpriteRenderer;

    public List<Sprite> TitleBackSprite;

    // Start is called before the first frame update
    void Start()
    {
        //タイトルのSptiteRenderereを取得
        MainSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

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
                MainSpriteRenderer.sprite = TitleBackSprite[FallNum - 1];
            }
            //-------------------------------------------------

            //---------------------------------------------------
            // 崩壊までのカウントダウンが0になったらシーン移動
            if (FallNum == 0)
            {
                SceneManager.LoadScene("SelectScene");
            }

        }

    }
}
