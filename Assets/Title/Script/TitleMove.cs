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

    GameObject BGM;          // BGM用オブジェクト   
    AudioSource Bgmsource;   // BGM

    GameObject SE;          // SE用オブジェクト   
    AudioSource Sesource;   // SE

    // 二宮追加
    public AudioClip se_start1; // 一回目
    public AudioClip se_start2; // 二回目
    public AudioClip se_startcrush; // 割れる

    private AudioClip[] se_crush = new AudioClip[4];

    //―追加担当者：中川直登―//
    [SerializeField, Header("パーティクル")]
    private ParticleSystem _particle;
    //――――――――――――//
    

    // Start is called before the first frame update
    void Start()
    {
        // ScreenBreakを取得
        BreakObj = GameObject.Find("ScreenBreak");
        _ScreenBreak = BreakObj.GetComponent<ScreenBreak>();

        BreakTime = 0.0f;

        //--------------------------------------------
        //クリスタルの状態を取得
        CrystalObj = GameObject.Find("Crystal");
        CrystalRenderer = CrystalObj.GetComponent<SpriteRenderer>();
        CrystalAlpha = CrystalRenderer.color.a;
        //タイトルのSptiteRenderereを取得
        //MainSpriteRenderer = .GetComponent<SpriteRenderer>();

        //--------------------------------------------
        //　SEの情報を取得
        SE = GameObject.Find("SE");
        Sesource = SE.GetComponent<AudioSource>();

        //--------------------------------------------
        //　BGMの情報を取得
        BGM = GameObject.Find("BGM");
        Bgmsource = BGM.GetComponent<AudioSource>();

        se_crush[3] = se_start1;
        se_crush[2] = se_start2;
        se_crush[1] = se_start1;
        se_crush[0] = se_startcrush;
    }

    // Update is called once per frame
    void Update()
    {
        //  キー入力
        if (Input.GetKeyDown(KeyCode.Space)/*("joystick button 0")*/)
        {

            //Sesource.Play();
            Sesource.PlayOneShot(se_crush[FallNum - 1]);
            //-------------------------------------------------
            //スプライトを変更
            if (FallNum > 0)
            {
                //CrystalAlpha += (1.0f / 255.0f) * 20;
                //CrystalRenderer.color = new Color(1.0f, 1.0f, 1.0f, CrystalAlpha);
                //―追加担当者：中川直登―//
                Instantiate(_particle);
                //――――――――――――//
                CrystalRenderer.sprite = TitleBackSprite[FallNum - 1];
            }
            //-------------------------------------------------
            FallNum--;

        }

        //---------------------------------------------------
        // 崩壊までのカウントダウンが0になったらシーン移動
        if (FallNum == 0)
        {
            //CrystalRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            _ScreenBreak.enabled = true;
        }

        if (_ScreenBreak.enabled)
        {
            BreakTime += Time.deltaTime;

            // BGMをフェードアウトさせる
            //Bgmsource.volume -= 0.5f * Time.deltaTime;
            Bgmsource.volume = 0;
            
            //3秒経過したらscene移動
            if (BreakTime > 3.0f)
            {
                SceneManager.LoadScene("newSelectScene");//―変更担当者：中川直登―//SelectSceneからnewSelectSceneに変更
            }
        }

       
    }

}
        