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

    [SerializeField,Header("中に入っているクリスタルの数")]
    private int CrystalNum;

    // 外部取得
    private GameObject Player;
    private PlayerStatas statas;
    private CrackCreater order = null;
    private ParticleSystem BreakParticle;   //壊れるエフェクト
    private ParticleSystem CrystalParticle; //クリスタルをゲットしたパーティクル
    private GameObject CrystalPoint;        //クリスタルが集まる座標

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;

        Player = GameObject.Find("player");
        statas = Player.GetComponent<PlayerStatas>();
        BreakParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        CrystalParticle = transform.GetChild(1).GetComponent<ParticleSystem>();
        CrystalPoint = transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        //再生中
        if (CrystalParticle.isPlaying)
        {
            CrystalPoint.transform.position = Player.transform.position;
        }
        else
        {
            CrystalPoint.transform.position = new Vector3(-1000.0f, -1000.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ひびにあたったら
        if (collision.gameObject.tag == CrackTag)
        {
            // 当たったひびのCrackOrderを取得
            order = collision.gameObject.GetComponent<CrackCreater>();

            // ひび生成中なら
            if (order.State == CrackCreater.CrackCreaterState.CREATING ||
                order.State == CrackCreater.CrackCreaterState.ADD_CREATING)
            {
                //Destroy(collision.gameObject);
                //壊れるパーティクルの再生
                BreakParticle.Play();
                if(CrystalNum > 0)
                {
                    CrystalPoint.transform.position = Player.transform.position;
                    //Debug.Log(CrystalPoint);
                    CrystalParticle.Play();
                }
                // 壊れるブロックの処理用関数呼び出し
                Func_BreakBlock();

                //クリスタルを付与
                statas.SetCrystal(statas.GetCrystal() + CrystalNum);
              
            }
        }
    }
    
    public void Func_BreakBlock()
    {
        // 透明にする
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);

        // 当たり判定を消す
        //GetComponent<BoxCollider2D>().enabled = false;
        Destroy(GetComponent<BoxCollider2D>());
    }
}
