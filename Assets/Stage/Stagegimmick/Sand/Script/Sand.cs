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
    public bool SandHit = false;   //砂が当たっているか   
    private float Line = 0.0f;
    private PolygonCollider2D thiscoll; //このオブジェクトのcollider

    [SerializeField, Header("つもるスピード")]
    private float accumulatespeed;


    // Start is called before the first frame update
    void Start()
    {
        thismat = GetComponent<SpriteRenderer>();
        thiscoll = GetComponent<PolygonCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //SandHit = false;
        //砂が当たっていたら徐々に見えるようにする
        if (SandHit)
        {
            if (Line <= 1.0f)
            {
                Line += accumulatespeed * Time.deltaTime;
                //Vector2[] points = thiscoll.points;
                //points[0].y += Line;
                //points[1].y += Line;
                //thiscoll.SetPath(0, points);
            }
            else
            {
                gameObject.layer = 10;
            }

        }
        else
        {
            if (Line > 0.0f)
            {
                Line -= accumulatespeed * Time.deltaTime;
                //Vector2[] points = thiscoll.points;
                //points[0].y -= Line;
                //points[1].y -= Line;
                //thiscoll.SetPath(0, points);

            }
        }

        

        thismat.material.SetFloat("_Border", Line);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //砂が当たっていたら
        if (collision.gameObject.tag == "Sand")
        {
            SandHit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //砂が当たらなくなったら
        if (collision.gameObject.tag == "Sand")
        {
            SandHit = false;
        }
    }


}