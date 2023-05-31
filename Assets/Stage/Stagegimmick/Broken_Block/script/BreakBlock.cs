//-----------------------------
//担当：二宮怜
//内容：ひびと接触すると壊れるブロック
//-----------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    // 変数宣言

    // ひびオブジェクトのタグ名
    private string CrackTag = "Crack";

    // このゲームオブジェクトのマテリアルを保持する変数
    private Material mat;

    private GameObject Crystal;

    [SerializeField,Header("中に入っているクリスタルの数")]
    private int CrystalNum;

    public bool Break = false;

    private bool _BreakBlock = false;
    private float timer = 0f;

    // 外部取得
    private GameObject Player;
    private PlayerStatas statas;
    private CrackCreater order = null;
    private ParticleSystem BreakParticle;   //壊れるエフェクト
    //private ParticleSystem CrystalParticle; //クリスタルをゲットしたパーティクル
    //private GameObject CrystalPoint;        //クリスタルが集まる座標

    [SerializeField,Header("ないならいらない")] private BoxCollider2D _boxCollider; // 氷についてる
    [SerializeField,Header("ないならいらない")] private PolygonCollider2D _polygonCollider; // 岩についてる

    private GameObject SEObj;               //SE再生用オブジェクト
    private GimmickPlay_2 PlaySound;     //SE再生用スクリプト

    [SerializeField] private SpriteRenderer _spriteRenderer; // スプライトレンダラー

    [SerializeField] private Animator anim; // アニメーター

    [SerializeField,Header("氷ブロックのみ必要")] private Material defaultMat;

    private GetCrystal getCrystal; // クリスタル取得時のパーティクル出現用

    private void Start()
    {
        SEObj = GameObject.Find("BlockSE");
        PlaySound = SEObj.GetComponent<GimmickPlay_2>();

        mat = GetComponent<SpriteRenderer>().material;

        Player = GameObject.Find("player");
        statas = Player.GetComponent<PlayerStatas>();
        BreakParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        //CrystalParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        //CrystalPoint = transform.GetChild(2).gameObject;

        _boxCollider = GetComponent<BoxCollider2D>();
        _polygonCollider = GetComponent<PolygonCollider2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (transform.childCount == 4)
        {
            Crystal = transform.GetChild(3).gameObject;
        }

        // クリスタルが飛んでいく演出用
        getCrystal = GameObject.Find("GetCrystal").GetComponent<GetCrystal>();
        if (getCrystal == null) Debug.LogError("GetCrystalコンポーネントを取得できませんでした。");
    }

    private void Update()
    {
        //再生中
        //if (CrystalParticle.isPlaying)
        //{
        //    CrystalPoint.transform.position = Player.transform.position;
        //}
        //else
        //{
        //    CrystalPoint.transform.position = new Vector3(-1000.0f, -1000.0f);
        //}

        // Func_BreakBlockが呼ばれたらtrue
        if(_BreakBlock == true)
        {
            if (CrystalNum > 0)
            {
                if (timer > 0.5f)
                {
                    // クリスタルの個数分エフェクト生成
                    getCrystal.Creat();

                    // 初期化
                    timer = 0f;

                    CrystalNum--;
                }

                timer += Time.deltaTime;
            }
            else
            {
                _BreakBlock = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ひびにあたったら
        if (collision.gameObject.tag == CrackTag)
        {
            // 当たったひびのCrackOrderを取得
            order = collision.gameObject.GetComponent<CrackCreater>();

            if (order != null)
            {
                // ひび生成中なら
                if (order.State != CrackCreater.CrackCreaterState.CRAETED)
                {
                    if (Break == false)
                    {
                        // ひびを消す
                        Destroy(collision.gameObject);

                        //if (CrystalNum > 0)
                        //{
                        //    CrystalPoint.transform.position = Player.transform.position;
                        //    CrystalParticle.Play();
                        //}

                        // 壊れるブロックの処理用関数呼び出し
                        Func_BreakBlock();
                    }
                }
            }
        }
    }
    
    public void Func_BreakBlock()
    {
        //壊れるパーティクルの再生
        BreakParticle.Play();

        if (tag == "Ice")
        {
            PlaySound.PlayerGimmickSE(GimmickPlay_2.GimmickSE2List.ICEBLOCK);

            // 壊れるアニメーション開始
            anim.SetBool("breakIce", true);
            // デフォルトのマテリアルに戻す(アニメーターでアニメーションするとおかしくなるため)
            _spriteRenderer.material = defaultMat; // デフォルトマテリアルをセット
        }
        else
        {
            PlaySound.PlayerGimmickSE(GimmickPlay_2.GimmickSE2List.ROCKBLOCK);
            // 壊れるアニメーション開始
            anim.SetBool("breakRock", true);
        }

        // クリスタル取得エフェクト用フラグs
        _BreakBlock = true;

        if (Crystal != null)
        {
            Destroy(Crystal);
        }

        // 当たり判定をオフ
        if (_boxCollider != null)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if (_polygonCollider != null)
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }

        Break = true;
    }

    public void Invisible()
    {
        // 不可視化
        _spriteRenderer.enabled = false;

        // アニメーション終了
        anim.SetBool("breakIce", false);
        anim.SetBool("breakRock", false);
    }
}
