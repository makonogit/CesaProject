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

    [SerializeField, Header("TitleLogo")]
    private SpriteRenderer TitleLogo;
    [SerializeField] private Transform _logoTransform;

    [SerializeField, Header("Button")]
    private SpriteRenderer _Button;

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

    GameObject player;      // 演出用主人公
    Animator anim;          // アニメーション

    // 二宮追加
    public AudioClip se_start1; // 一回目
    public AudioClip se_start2; // 二回目
    public AudioClip se_startcrush; // 割れる

    [SerializeField] private List<Sprite> SpriteList = new List<Sprite>();
    [SerializeField] private GameObject _CreackEffect; // 割れるエフェクト

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
        // 演出用Animatorを取得
        player = GameObject.Find("UIPlayerWalk");
        anim = player.GetComponent<Animator>();
        anim.SetInteger("Select", -1);

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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))
        {

            if (FallNum > 0)
            {
                //Sesource.Play();
                Sesource.PlayOneShot(se_crush[FallNum - 1]);
            }
            //-------------------------------------------------
            //スプライトを変更
            if (FallNum > 0)
            {
                CrystalAlpha += (1.0f / 255.0f) * 20;
                CrystalRenderer.color = new Color(1.0f, 1.0f, 1.0f, CrystalAlpha);
                //―追加担当者：中川直登―//
                Instantiate(_particle);
                //――――――――――――//
                CrystalRenderer.sprite = TitleBackSprite[FallNum - 1];

                if(FallNum == 3)
                {
                    // スプライト変更
                    TitleLogo.sprite = SpriteList[1];
                }
                else if(FallNum == 2)
                {
                    // スプライト変更
                    TitleLogo.sprite = SpriteList[0];

                    // エフェクト生成
                    var Obj = Instantiate(_CreackEffect, _logoTransform.transform);
                    var Pos = Obj.transform.position;
                    // Tの位置らへんにパーティクル生成
                    Obj.transform.position = new Vector3(Pos.x + 2.83f, Pos.y - 0.86f, Pos.z);
                }else if(FallNum == 1)
                {
                    // 残ったタイトルロゴ消す
                    TitleLogo.enabled = false;
                    // Press Keyを消す
                    _Button.enabled = false;

                    // エフェクト一つ目生成
                    var Obj1 = Instantiate(_CreackEffect, _logoTransform.transform);
                    var Pos1 = Obj1.transform.position;
                    // Kの位置らへんにパーティクル生成
                    Obj1.transform.position = new Vector3(Pos1.x, Pos1.y - 0.86f, Pos1.z);

                    // エフェクト二つ目生成
                    var Obj2 = Instantiate(_CreackEffect, _logoTransform.transform);
                    var Pos2 = Obj2.transform.position;
                    // Pの位置らへんにパーティクル生成
                    Obj2.transform.position = new Vector3(Pos2.x - 2.4f, Pos2.y - 0.86f, Pos2.z);

                    // エフェクト三つ目生成
                    var Obj3 = Instantiate(_CreackEffect, _logoTransform.transform);
                    var Pos3 = Obj3.transform.position;
                    // Keyの位置らへんにパーティクル生成
                    Obj3.transform.position = new Vector3(Pos3.x, Pos3.y - 3.82f, Pos3.z);
                }
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
            anim.SetInteger("Select", 4);
            // プレイヤーが右に移動していく
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                new Vector3(11.0f, player.transform.position.y, player.transform.position.z), 6.0f * Time.deltaTime);

            if (player.transform.position.x > 10.7f)
            {
                BreakTime += Time.deltaTime;

                // BGMをフェードアウトさせる
                Bgmsource.volume -= 0.5f * Time.deltaTime;
                //Bgmsource.volume = 0;

                GameObject.Find("SceneManager").GetComponent<SceneChange>().LoadScene("newSelectScene");

                ////3秒経過したらscene移動
                //if (BreakTime > 3.0f)
                //{
                //    SceneManager.LoadScene("newSelectScene");//―変更担当者：中川直登―//SelectSceneからnewSelectSceneに変更
                //}
            }
        }

       
    }

}
        