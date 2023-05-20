//----------------------------------
//  担当：菅眞心
//　砂が積み重なっていく
//----------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    //-------------------------------
    //　変数宣言
    private SpriteRenderer thismat;  //このオブジェクトのRender
    private Transform thistrans;
    private float MaxScale;
    public bool SandHit = false;   //砂が当たっているか   
    [SerializeField]private float Line = 0.0f;
    
    private PolygonCollider2D thiscoll; //このオブジェクトのcollider
    private float MaxSand;

    private float Wait = 0.0f; //時間計測用

    [SerializeField, Header("つもるスピード")]
    private float accumulatespeed;


    // Start is called before the first frame update
    void Start()
    {
        thismat = GetComponent<SpriteRenderer>();
        thiscoll = GetComponent<PolygonCollider2D>();
        thistrans = transform;
        MaxScale = thistrans.localScale.y;  //縦の長さを取得
        MaxSand = thiscoll.points[0].y;
    }

    // Update is called once per frame
    void Update()
    {
        //砂が当たっていたら徐々に見えるようにする
        if (SandHit)
        {
            {
                //if (Line< 1.0f)
                //{
                //    Line += accumulatespeed * Time.deltaTime;
                //    Vector2[] points = thiscoll.points;

                //    if (points[0].y < MaxSand)
                //    {
                //        points[0].y += Line;
                //        points[1].y += Line;
                //    }

                //    thiscoll.SetPath(0, points);
                //}
                //else
                //{
                //    gameObject.layer = 10;
                //}
            }

            if(thistrans.localScale.y < MaxScale)
            {
                thistrans.localScale = new Vector3(thistrans.localScale.x, thistrans.localScale.y + accumulatespeed * Time.deltaTime, thistrans.localScale.z);
                thistrans.localPosition = new Vector3(thistrans.localPosition.x, thistrans.localPosition.y + (accumulatespeed / 2) * Time.deltaTime, thistrans.localPosition.z);

                //// コライダーの設定
                //Vector2[] points = thiscoll.points;

                //if (points[0].y < MaxSand)
                //{
                //    points[0].y += accumulatespeed * Time.deltaTime;
                //    points[1].y += accumulatespeed * Time.deltaTime;
                //}

                //thiscoll.SetPath(0, points);

            }
            else
            {
                gameObject.layer = 10;
            }

        }
        else
        {
            {
                //if (Line > 0.0f)
                //{
                //    Line -= accumulatespeed * Time.deltaTime;
                //    Vector2[] points = thiscoll.points;
                //    if (points[0].y > points[2].y)
                //    {
                //        points[0].y -= Line;
                //        points[1].y -= Line;
                //    }
                //    thiscoll.SetPath(0, points);

                //}
                //else
                //{
                //    gameObject.layer = 15;
                //}
            }

            if (thistrans.localScale.y > 0.1f)
            {
                thistrans.localScale = new Vector3(thistrans.localScale.x, thistrans.localScale.y - accumulatespeed * Time.deltaTime, thistrans.localScale.z);
                thistrans.localPosition = new Vector3(thistrans.localPosition.x, thistrans.localPosition.y - (accumulatespeed / 2) * Time.deltaTime, thistrans.localPosition.z);

                //// コライダーの設定
                //Vector2[] points = thiscoll.points;

                //if (points[0].y > points[2].y + 0.1f)
                //{
                //    points[0].y -= (accumulatespeed / 2) * Time.deltaTime;
                //    points[1].y -= (accumulatespeed / 2) * Time.deltaTime;
                //}

                //thiscoll.SetPath(0, points);

            }
            else
            {
                gameObject.layer = 15;
            }

        }

        // 0.5秒待って当たり判定を無効に
        if (SandHit)
        {
            Wait += Time.deltaTime;
            if (Wait > 0.3f)
            {
                SandHit = false;
                Wait = 0.0f;
            }
        }
        else
        {
            SandHit = false;
        }

        //thismat.material.SetFloat("_Border", Line);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //砂が当たっていたら
        if (collision.gameObject.tag == "Sand" && !SandHit)
        {
            SandHit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //砂が当たらなくなったら
        if (collision.gameObject.tag == "Sand")
        {
            Debug.Log("当たらなかった");
            SandHit = false;
        }
    }


}